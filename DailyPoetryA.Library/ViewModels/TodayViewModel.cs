using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class TodayViewModel : ViewModelBase {
    private ITodayImageService _todayImageService;
    private ITodayPoetryService _todayPoetryService;
    private IContentNavigationService _contentNavigationService;

    public TodayViewModel(ITodayImageService todayImageService,
        ITodayPoetryService todayPoetryService,
        IContentNavigationService contentNavigationService) {
        _todayImageService = todayImageService;
        _todayPoetryService = todayPoetryService;
        _contentNavigationService = contentNavigationService;

        OnInitializedCommand = new RelayCommand(OnInitialized);
        ShowDetailCommand = new RelayCommand(ShowDetail);
        QueryCommand = new RelayCommand(Query);
    }

    private TodayImage? _todayImage;

    public TodayImage? TodayImage {
        get => _todayImage;
        private set => SetProperty(ref _todayImage, value);
    }

    private TodayPoetry? _todayPoetry;

    public TodayPoetry? TodayPoetry {
        get => _todayPoetry;
        set => SetProperty(ref _todayPoetry, value);
    }

    private bool _isLoading;

    public bool IsLoading {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    public ICommand OnInitializedCommand { get; }

    public void OnInitialized() {
        Task.Run(async () => {
            TodayImage = await _todayImageService.GetTodayImageAsync();

            var updateResult = await _todayImageService.CheckUpdateAsync();
            if (updateResult.HasUpdate) {
                TodayImage = updateResult.TodayImage;
            }
        });

        Task.Run(async () => {
            IsLoading = true;
            await Task.Delay(1000);
            TodayPoetry = await _todayPoetryService.GetTodayPoetryAsync();
            IsLoading = false;
        });
    }

    public ICommand ShowDetailCommand { get; }

    public void ShowDetail() {
        _contentNavigationService.NavigateTo(
            ContentNavigationConstant.TodayDetailView, TodayPoetry);
    }

    public ICommand QueryCommand { get; }

    public void Query() =>
        _contentNavigationService.NavigateTo(MenuNavigationConstant.QueryView,
            new PoetryQuery {
                Author = TodayPoetry.Author, Name = TodayPoetry.Name
            });
}