using System.Linq.Expressions;
using AvaloniaInfiniteScrolling;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class ResultViewModel : ViewModelBase {
    private readonly IPoetryStorage _poetryStorage;

    private readonly IContentNavigationService _contentNavigationService;

    private Expression<Func<Poetry, bool>> _where;

    public ResultViewModel(IPoetryStorage poetryStorage, IContentNavigationService contentNavigationService) {
        _contentNavigationService = contentNavigationService;
        PoetryCollection = new AvaloniaInfiniteScrollCollection<Poetry> {
            OnCanLoadMore = () => _canLoadMore,
            OnLoadMore = async () => {
                Status = Loading;
                var poetries = await poetryStorage.GetPoetriesAsync(_where,
                    PoetryCollection.Count, PageSize);
                Status = string.Empty;

                if (poetries.Count < PageSize) {
                    _canLoadMore = false;
                    Status = NoMoreResult;
                }

                if (PoetryCollection.Count == 0 && poetries.Count == 0) {
                    Status = NoResult;
                }

                return poetries;

            }
        };
        ShowPoetryCommand = new RelayCommand<Poetry>(ShowPoetry);
    }

    private bool _canLoadMore;

    public override void SetParameter(object parameter) {
        if (parameter is not Expression<Func<Poetry, bool>> where) {
            return;
        }

        _where = where;
        _canLoadMore = true;
        PoetryCollection.Clear();
    }

    public AvaloniaInfiniteScrollCollection<Poetry> PoetryCollection { get; }

    private string _status;

    public string Status {
        get => _status;
        private set => SetProperty(ref _status, value);
    }

    public const string Loading = "正在载入";

    public const string NoResult = "没有满足条件的结果";

    public const string NoMoreResult = "没有更多结果";

    public const int PageSize = 20;

    public IRelayCommand<Poetry> ShowPoetryCommand { get; }

    public void ShowPoetry(Poetry poetry) =>
        _contentNavigationService.NavigateTo(ContentNavigationConstant.DetailView, poetry);
}