using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using Moq;

namespace DailyPoetryA.UnitTest.ViewModels;

public class MainWindowViewModelTest {
    [Fact]
    public async Task OnInitializedAsync_NotInitialized() {
        var poetryStorageMock = new Mock<IPoetryStorage>();
        poetryStorageMock.Setup(p => p.IsInitialized).Returns(false);
        var mockPoetryStorage = poetryStorageMock.Object;

        // var favoriteStorageMock = new Mock<IFavoriteStorage>();
        // favoriteStorageMock.Setup(p => p.IsInitialized).Returns(false);
        // var mockFavoriteStorage = favoriteStorageMock.Object;

        var rootNavigationServiceMock = new Mock<IRootNavigationService>();
        var mockRootNavigationService = rootNavigationServiceMock.Object;

        var mainWindowViewModel = new MainWindowViewModel(
            mockPoetryStorage, // mockFavoriteStorage,
            mockRootNavigationService);

        mainWindowViewModel.OnInitialized();
        poetryStorageMock.Verify(p => p.IsInitialized, Times.Once);
        // favoriteStorageMock.Verify(p => p.IsInitialized, Times.Once);
        rootNavigationServiceMock.Verify(
            p => p.NavigateTo(RootNavigationConstant.InitializationView),
            Times.Once);
    }

    [Fact]
    public async Task OnInitializedAsync_Initialized() {
        var poetryStorageMock = new Mock<IPoetryStorage>();
        poetryStorageMock.Setup(p => p.IsInitialized).Returns(true);
        var mockPoetryStorage = poetryStorageMock.Object;

        // var favoriteStorageMock = new Mock<IFavoriteStorage>();
        // favoriteStorageMock.Setup(p => p.IsInitialized).Returns(true);
        // var mockFavoriteStorage = favoriteStorageMock.Object;

        var rootNavigationServiceMock = new Mock<IRootNavigationService>();
        var mockRootNavigationService = rootNavigationServiceMock.Object;


        var mainWindowViewModel = new MainWindowViewModel(
            mockPoetryStorage, // mockFavoriteStorage,
            mockRootNavigationService);

        mainWindowViewModel.OnInitialized();
        poetryStorageMock.Verify(p => p.IsInitialized, Times.Once);
        // favoriteStorageMock.Verify(p => p.IsInitialized, Times.Once);
        rootNavigationServiceMock.Verify(
            p => p.NavigateTo(RootNavigationConstant.MainView), Times.Once);
    }
}