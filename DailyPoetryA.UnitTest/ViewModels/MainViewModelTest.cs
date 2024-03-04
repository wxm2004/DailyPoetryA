using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using Moq;

namespace DailyPoetryA.UnitTest.ViewModels;

public class MainViewModelTest {
    [Fact]
    public void OpenPane_Default() {
        var mainViewModel = new MainViewModel(null);
        Assert.False(mainViewModel.IsPaneOpen);
        mainViewModel.OpenPane();
        Assert.True(mainViewModel.IsPaneOpen);
    }

    [Fact]
    public void ClosePane_Default() {
        var mainViewModel = new MainViewModel(null);
        mainViewModel.OpenPane();
        Assert.True(mainViewModel.IsPaneOpen);
        mainViewModel.ClosePane();
        Assert.False(mainViewModel.IsPaneOpen);
    }

    [Fact]
    public void PushContent_Default() {
        var mainViewModel = new MainViewModel(null);
        Assert.Null(mainViewModel.Content);
        Assert.Empty(mainViewModel.ContentStack);

        var content = new Mock<ViewModelBase>().Object;
        mainViewModel.PushContent(content);
        Assert.Same(content, mainViewModel.Content);
        Assert.Single(mainViewModel.ContentStack);
        Assert.Same(content, mainViewModel.ContentStack[0]);
    }

    [Fact]
    public void SetMenuAndContent_Default() {
        var mainViewModel = new MainViewModel(null);
        var menuItem = MenuItem.MenuItems.First(p =>
            p.View == MenuNavigationConstant.TodayView);
        var content = new Mock<ViewModelBase>().Object;
        mainViewModel.SetMenuAndContent(menuItem.View, content);

        Assert.Same(content, mainViewModel.Content);
        Assert.Single(mainViewModel.ContentStack);
        Assert.Same(content, mainViewModel.ContentStack[0]);
        Assert.Same(menuItem, mainViewModel.SelectedMenuItem);
        Assert.Equal(menuItem.Name, mainViewModel.Title);
        Assert.False(mainViewModel.IsPaneOpen);
    }

    [Fact]
    public void OnMenuTapped_SelectedMenuItemIsNull() {
        var mainViewModel = new MainViewModel(null);
        mainViewModel.OnMenuTapped();
    }
    
    [Fact]
    public void OnMenuTapped_SelectedMenuItemIsNotNull() {
        // TODO
    }
}