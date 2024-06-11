using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class QueryViewModel : ViewModelBase {
    private IContentNavigationService contentNavigationService;

    public QueryViewModel(IContentNavigationService contentNavigationService) {
        this.contentNavigationService = contentNavigationService;
        FilterViewModelCollection = [new FilterViewModel(this)];
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

    public ObservableCollection<FilterViewModel> FilterViewModelCollection {
        get;
    }

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