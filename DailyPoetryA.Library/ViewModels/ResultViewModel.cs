using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class ResultViewModel : ViewModelBase {
    private IPoetryStorage _poetryStorage;

    private IContentNavigationService _contentNavigationService;

    private Expression<Func<Poetry, bool>> _where;

    public ResultViewModel(IPoetryStorage poetryStorage, IContentNavigationService contentNavigationService) {
        _poetryStorage = poetryStorage;
        _contentNavigationService = contentNavigationService;
        AppendCommand = new AsyncRelayCommand(AppendAsync);
    }

    private bool _canLoadMore;

    public override void SetParameter(object parameter) {
        if (parameter is not Expression<Func<Poetry, bool>> where) {
            return;
        }

        _where = where;
        PoetryCollection.Clear();
        _canLoadMore = true;
    }

    public ObservableCollection<Poetry> PoetryCollection { get; } = [];

    private string _status;

    public string Status {
        get => _status;
        private set => SetProperty(ref _status, value);
    }

    public const string Loading = "正在载入";

    public const string NoResult = "没有满足条件的结果";

    public const string NoMoreResult = "没有更多结果";

    public const int PageSize = 20;

    public ICommand AppendCommand { get; }

    public async Task AppendAsync() {
        if (!_canLoadMore) {
            return;
        }

        Status = Loading;
        await Task.Delay(3000);
        var poetries = await _poetryStorage.GetPoetriesAsync(_where, PoetryCollection.Count, PageSize);
        Status = string.Empty;

        if (poetries.Count < PageSize) {
            _canLoadMore = false;
            Status = NoMoreResult;
        }

        if (PoetryCollection.Count == 0 && poetries.Count == 0) {
            Status = NoResult;
        }

        foreach (var poetry in poetries) {
            PoetryCollection.Add(poetry);
        }
    }
}