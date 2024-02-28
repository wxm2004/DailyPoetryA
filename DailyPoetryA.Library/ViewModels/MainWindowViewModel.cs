using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class MainWindowViewModel : ViewModelBase {
    private IPoetryStorage _poetryStorage;
    private IRootNavigationService _rootNavigationService;

    public MainWindowViewModel(IPoetryStorage poetryStorage,
        IRootNavigationService rootNavigationService) {
        _poetryStorage = poetryStorage;
        _rootNavigationService = rootNavigationService;

        OnLoadedCommand = new RelayCommand(OnLoaded);
    }

    private ViewModelBase _content;

    public ViewModelBase Content {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    public ICommand OnLoadedCommand { get; }

    private void OnLoaded() {
        if (!_poetryStorage.IsInitialized) {
            _rootNavigationService.NavigateTo(RootNavigationConstant
                .InitializationView);
        }
    }
}