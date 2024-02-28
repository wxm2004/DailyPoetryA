using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace DailyPoetryA.Library.ViewModels;

public class MainViewModel : ViewModelBase {
    public MainViewModel() {
        OpenPaneCommand = new RelayCommand(OpenNavigationPane);
        ClosePaneCommand = new RelayCommand(CloseNavigationPane);
    }

    private string _title = "DailyPoetryA";

    public string Title {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private bool _isPaneOpen;

    public bool IsPaneOpen {
        get => _isPaneOpen;
        set => SetProperty(ref _isPaneOpen, value);
    }

    public ICommand OpenPaneCommand { get; }

    public void OpenNavigationPane() => IsPaneOpen = true;

    public ICommand ClosePaneCommand { get; }

    public void CloseNavigationPane() => IsPaneOpen = false;
}