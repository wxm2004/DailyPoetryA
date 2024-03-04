using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class InitializationViewModel : ViewModelBase {
    private readonly IPoetryStorage _poetryStorage;
    private readonly IRootNavigationService _rootNavigationService;

    public InitializationViewModel(IPoetryStorage poetryStorage,
        IRootNavigationService rootNavigationService) {
        _poetryStorage = poetryStorage;
        _rootNavigationService = rootNavigationService;

        OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync);
    }

    private ICommand OnInitializedCommand { get; }

    public async Task OnInitializedAsync() {
        if (!_poetryStorage.IsInitialized) {
            await _poetryStorage.InitializeAsync();
        }

        await Task.Delay(1000);

        _rootNavigationService.NavigateTo(RootNavigationConstant.MainView);
    }
}