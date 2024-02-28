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

        OnInitializedCommand = new RelayCommand(OnInitialized);
    }

    private ViewModelBase _content;

    public ViewModelBase Content {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    public ICommand OnInitializedCommand { get; }

    public void OnInitialized() {
        if (!_poetryStorage.IsInitialized) {
            _rootNavigationService.NavigateTo(RootNavigationConstant
                .InitializationView);
        } else {
            _rootNavigationService.NavigateTo(RootNavigationConstant.MainView);
        }
    }
}