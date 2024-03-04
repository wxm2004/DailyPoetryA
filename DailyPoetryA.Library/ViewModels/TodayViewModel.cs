using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class TodayViewModel : ViewModelBase {
    private ITodayImageService _todayImageService;

    private ITodayPoetryService _todayPoetryService;

    private IMenuNavigationService _menuNavigationService;

    private TodayImage? _todayImage;

    private TodayPoetry? _todayPoetry;

    private bool _isLoading;

    public TodayViewModel(ITodayImageService todayImageService,
        ITodayPoetryService todayPoetryService,
        IMenuNavigationService menuNavigationService) {
        _todayImageService = todayImageService;
        _todayPoetryService = todayPoetryService;
        _menuNavigationService = menuNavigationService;

        OnLoadedCommand = new RelayCommand(OnLoaded);
        ShowDetailCommand = new RelayCommand(ShowDetail);
        QueryCommand = new RelayCommand(Query);
    }

    public TodayImage? TodayImage {
        get => _todayImage;
        set => SetProperty(ref _todayImage, value);
    }

    public TodayPoetry? TodayPoetry {
        get => _todayPoetry;
        set => SetProperty(ref _todayPoetry, value);
    }

    public bool IsLoading {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand OnLoadedCommand { get; }

    public void OnLoaded() {
        Task.Run(async () => {
            TodayImage = await _todayImageService.GetTodayImageAsync();

            var updateResult = await _todayImageService.CheckUpdateAsync();
            if (updateResult.HasUpdate) {
                TodayImage = updateResult.TodayImage;
            }
        });

        Task.Run(async () => {
            IsLoading = true;
            TodayPoetry = await _todayPoetryService.GetTodayPoetryAsync();
            IsLoading = false;
        });
    }

    public ICommand ShowDetailCommand { get; }

    public void ShowDetail() {
        _menuNavigationService.NavigateTo(ContentNavigationConstant
            .TodayDetailView);
    }

    public ICommand QueryCommand { get; }

    public void Query() =>
        _menuNavigationService.NavigateTo(MenuNavigationConstant.QueryView,
            new PoetryQuery {
                Author = TodayPoetry.Author, Name = TodayPoetry.Name
            });

    // TODO
}