using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class QueryViewModel : ViewModelBase {
    private readonly IContentNavigationService _contentNavigationService;

    public QueryViewModel(IContentNavigationService contentNavigationService) {
        _contentNavigationService = contentNavigationService;
        FilterViewModelCollection = [new FilterViewModel(this)];
        QueryCommand = new RelayCommand(Query);
    }

    public override void SetParameter(object parameter) {
        if (parameter is not PoetryQuery poetryQuery) {
            return;
        }

        FilterViewModelCollection.Clear();
        FilterViewModelCollection.Add(new FilterViewModel(this) {
            Type = FilterType.NameFilter, Content = poetryQuery.Name
        });
        FilterViewModelCollection.Add(new FilterViewModel(this) {
            Type = FilterType.AuthorNameFilter, Content = poetryQuery.Author
        });
    }

    public ObservableCollection<FilterViewModel> FilterViewModelCollection { get; }

    public virtual void AddFilterViewModel(FilterViewModel filterViewModel) =>
        FilterViewModelCollection.Insert(
            FilterViewModelCollection.IndexOf(filterViewModel) + 1,
            new FilterViewModel(this));

    public virtual void RemoveFilterViewModel(FilterViewModel filterViewModel) {
        FilterViewModelCollection.Remove(filterViewModel);
        if (FilterViewModelCollection.Count == 0) {
            FilterViewModelCollection.Add(new FilterViewModel(this));
        }
    }

    public ICommand QueryCommand { get; }

    public void Query() {
        // Connection.Table<Poetry>().Where(p => p.Name.Contains("something")
        //                                       && p.AuthorName.Contains("something")
        //                                       && p.Content.Contains("something")
        //                                 ).ToList();

        // p => p.Name.Contains("something")
        //      && p.AuthorName.Contains("something")
        //      && p.Content.Contains("something")

        // p
        var parameter = Expression.Parameter(typeof(Poetry), "p");

        var aggregatedExpression = FilterViewModelCollection
            // Those ViewModels who do have a content.
            .Where(p => !string.IsNullOrWhiteSpace(p.Content))
            // Translate a FilterViewModel to a condition
            // e.g. FilterViewModel {
            //     FileType = {
            //         Name = "作者",
            //         PropertyName = "AuthorName"
            //     },
            //     Content = "苏轼"
            // } => p.AuthorName.Contains("苏轼")
            .Select(p => GetExpression(parameter, p))
            // Put all the conditions together
            // e.g. true && p.AuthorName.Contains("苏轼") && p.Content.Contains("老夫")
            .Aggregate(Expression.Constant(true) as Expression,
                Expression.AndAlso);

        // Turning the expression into a lambda expression
        var where =
            Expression.Lambda<Func<Poetry, bool>>(aggregatedExpression,
                parameter);

        _contentNavigationService.NavigateTo(ContentNavigationConstant.ResultView, where);
    }

    private static Expression GetExpression(ParameterExpression parameter,
        FilterViewModel filterViewModel) {
        // parameter => p

        // p.Name or p.AuthorName or p.Content
        var property = Expression.Property(parameter,
            filterViewModel.Type.PropertyName);

        // .Contains()
        var method =
            typeof(string).GetMethod("Contains", new[] {
                typeof(string)
            });

        // "something"
        var condition =
            Expression.Constant(filterViewModel.Content, typeof(string));

        // p.Name.Contains("something")
        // or p.AuthorName.Contains("something")
        // or p.Content.Contains("something")
        return Expression.Call(property, method, condition);
    }
}

public class FilterViewModel : ObservableObject {
    private QueryViewModel _queryViewModel;

    public FilterViewModel(QueryViewModel queryViewModel) {
        _queryViewModel = queryViewModel;
        AddCommand = new RelayCommand(Add);
        RemoveCommand = new RelayCommand(Remove);
    }

    public FilterType Type {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    private FilterType _type = FilterType.NameFilter;

    public string Content {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    private string _content;

    public ICommand AddCommand { get; }

    public void Add() => _queryViewModel.AddFilterViewModel(this);

    public ICommand RemoveCommand { get; }

    public void Remove() => _queryViewModel.RemoveFilterViewModel(this);
}

public class FilterType {
    public static readonly FilterType NameFilter =
        new("标题", nameof(Poetry.Name));

    public static readonly FilterType AuthorNameFilter =
        new("作者", nameof(Poetry.Author));

    public static readonly FilterType ContentFilter =
        new("内容", nameof(Poetry.Content));

    public static List<FilterType> FilterTypes { get; } =
        [NameFilter, AuthorNameFilter, ContentFilter];

    private FilterType(string name, string propertyName) {
        Name = name;
        PropertyName = propertyName;
    }

    public string Name { get; }

    public string PropertyName { get; }
}