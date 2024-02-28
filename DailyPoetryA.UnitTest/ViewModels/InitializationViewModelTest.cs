using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using Moq;

namespace DailyPoetryA.UnitTest.ViewModels;

public class InitializationPageViewModelTest {
    [Fact]
    public async Task LoadedCommandFunction_NotInitialized() {
        var poetryStorageMock = new Mock<IPoetryStorage>();
        poetryStorageMock.Setup(p => p.IsInitialized).Returns(false);
        var mockPoetryStorage = poetryStorageMock.Object;

        // var favoriteStorageMock = new Mock<IFavoriteStorage>();
        // favoriteStorageMock.Setup(p => p.IsInitialized).Returns(false);
        // var mockFavoriteStorage = favoriteStorageMock.Object;

        var rootNavigationServiceMock = new Mock<IRootNavigationService>();
        var mockRootNavigationService = rootNavigationServiceMock.Object;

        var initializationPageViewModel = new InitializationViewModel(
            mockPoetryStorage, // mockFavoriteStorage,
            mockRootNavigationService);

        await initializationPageViewModel.OnInitializedAsync();
        poetryStorageMock.Verify(p => p.IsInitialized, Times.Once);
        poetryStorageMock.Verify(p => p.InitializeAsync(), Times.Once);
        // favoriteStorageMock.Verify(p => p.IsInitialized, Times.Once);
        // favoriteStorageMock.Verify(p => p.InitializeAsync(), Times.Once);
        rootNavigationServiceMock.Verify(
            p => p.NavigateTo(RootNavigationConstant.MainView), Times.Once);
    }

    [Fact]
    public async Task LoadedCommandFunction_Initialized() {
        var poetryStorageMock = new Mock<IPoetryStorage>();
        poetryStorageMock.Setup(p => p.IsInitialized).Returns(true);
        var mockPoetryStorage = poetryStorageMock.Object;

        // var favoriteStorageMock = new Mock<IFavoriteStorage>();
        // favoriteStorageMock.Setup(p => p.IsInitialized).Returns(true);
        // var mockFavoriteStorage = favoriteStorageMock.Object;

        var rootNavigationServiceMock = new Mock<IRootNavigationService>();
        var mockRootNavigationService = rootNavigationServiceMock.Object;


        var initializationPageViewModel = new InitializationViewModel(
            mockPoetryStorage, // mockFavoriteStorage,
            mockRootNavigationService);

        await initializationPageViewModel.OnInitializedAsync();
        poetryStorageMock.Verify(p => p.IsInitialized, Times.Once);
        poetryStorageMock.Verify(p => p.InitializeAsync(), Times.Never);
        // favoriteStorageMock.Verify(p => p.IsInitialized, Times.Once);
        // favoriteStorageMock.Verify(p => p.InitializeAsync(), Times.Never);
        rootNavigationServiceMock.Verify(
            p => p.NavigateTo(RootNavigationConstant.MainView), Times.Once);
    }
}