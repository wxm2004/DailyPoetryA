using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Services;

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
        private set => SetProperty(ref _content, value);
    }

    public ICommand OpenPaneCommand { get; }

    public void OpenNavigationPane() => IsPaneOpen = true;

    public ICommand ClosePaneCommand { get; }

    public void CloseNavigationPane() => IsPaneOpen = false;

    public void PushContent(ViewModelBase content) =>
        ContentStack.Insert(0, Content = _content);

    public void SetMenuAndContent(string view, ViewModelBase content) {
        // 如果view是当前view则return
        throw new NotImplementedException();
        // TODO 设置Menu, 清空ContentStack, 设置Content, PushContent
    }

    private MenuItem _selectedMenuItem;

    public MenuItem SelectedMenuItem {
        get => _selectedMenuItem;
        set => SetProperty(ref _selectedMenuItem, value);
    }

    public ICommand OnSelectedMenuItemChangedCommand { get; }

    public void OnSelectedMenuItemChanged() {
        if (SelectedMenuItem is null) {
            return;
        }

        // todo call menu navigation service
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

public class MenuItem {
    public string View { get; private init; }
    public string Name { get; private init; }

    private MenuItem() { }

    private static MenuItem TodayView =>
        new() { Name = "今日推荐", View = MenuNavigationConstants.TodayView };

    private static MenuItem QueryView =>
        new() { Name = "诗词搜索", View = MenuNavigationConstants.QueryView };

    private static MenuItem FavoriteView =>
        new() { Name = "诗词收藏", View = MenuNavigationConstants.FavoriteView };

    public static IEnumerable<MenuItem> MenuItems => [
        TodayView, QueryView, FavoriteView
    ];
}