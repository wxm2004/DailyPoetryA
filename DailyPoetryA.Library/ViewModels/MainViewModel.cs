using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace DailyPoetryA.Library.ViewModels;

public class MainViewModel : ViewModelBase {
    public MainViewModel() {
        OpenPaneCommand = new RelayCommand(OpenNavigationPane);
        ClosePaneCommand = new RelayCommand(CloseNavigationPane);
        GoBackCommand = new RelayCommand(GoBack);
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

    private ViewModelBase _content;

    public ViewModelBase Content {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    public ICommand OpenPaneCommand { get; }

    public void OpenNavigationPane() => IsPaneOpen = true;

    public ICommand ClosePaneCommand { get; }

    public void CloseNavigationPane() => IsPaneOpen = false;

    public void PushContent(ViewModelBase content) =>
        ContentStack.Insert(0, Content = _content);

    public void SetMenuAndContent(string view, ViewModelBase content) {
        throw new NotImplementedException();
        // TODO 设置Menu, 清空ContentStack, 设置Content, PushContent
    }

    public ObservableCollection<ViewModelBase> ContentStack { get; } = new();

    public ICommand GoBackCommand { get; }

    public void GoBack() {
        if (ContentStack.Count <= 1) {
            return;
        }

        Content = ContentStack[0];
        ContentStack.RemoveAt(0);
    }
}