using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using Moq;

namespace DailyPoetryA.UnitTest.ViewModels;

public class TodayViewModelTest {
    [Fact]
    public async Task LoadedCommandFunction_ImageUpdated() {
        var oldTodayImageToReturn = new TodayImage();
        var newTodayImageToReturn = new TodayImage();
        var updateResultToReturn = new TodayImageServiceCheckUpdateResult {
            HasUpdate = true, TodayImage = newTodayImageToReturn
        };

        var todayImageServiceMock = new Mock<ITodayImageService>();
        todayImageServiceMock.Setup(p => p.GetTodayImageAsync())
            .ReturnsAsync(oldTodayImageToReturn);
        todayImageServiceMock.Setup(p => p.CheckUpdateAsync())
            .ReturnsAsync(updateResultToReturn);
        var mockImageService = todayImageServiceMock.Object;

        var todayPoetryToReturn = new TodayPoetry();
        var todayPoetryServiceMock = new Mock<ITodayPoetryService>();
        todayPoetryServiceMock.Setup(p => p.GetTodayPoetryAsync())
            .ReturnsAsync(todayPoetryToReturn);
        var mockTodayPoetryService = todayPoetryServiceMock.Object;

        var todayPageViewModel = new TodayViewModel(mockImageService,
            mockTodayPoetryService, null);
        var todayImageList = new List<TodayImage>();
        var isLoadingList = new List<bool>();
        todayPageViewModel.PropertyChanged += (sender, args) => {
            switch (args.PropertyName) {
                case nameof(TodayViewModel.TodayImage):
                    todayImageList.Add(todayPageViewModel.TodayImage);
                    break;
                case nameof(TodayViewModel.IsLoading):
                    isLoadingList.Add(todayPageViewModel.IsLoading);
                    break;
            }
        };

        todayPageViewModel.OnLoaded();
        while (todayImageList.Count != 2 || isLoadingList.Count != 2) {
            await Task.Delay(100);
        }

        Assert.Same(oldTodayImageToReturn, todayImageList[0]);
        Assert.Same(newTodayImageToReturn, todayImageList[1]);
        Assert.True(isLoadingList[0]);
        Assert.False(isLoadingList[1]);
        Assert.Same(todayPoetryToReturn, todayPageViewModel.TodayPoetry);

        todayImageServiceMock.Verify(p => p.GetTodayImageAsync(), Times.Once);
        todayImageServiceMock.Verify(p => p.CheckUpdateAsync(), Times.Once);
        todayPoetryServiceMock.Verify(p => p.GetTodayPoetryAsync(), Times.Once);
    }

    [Fact]
    public async Task LoadedCommandFunction_ImageNotUpdated() {
        var oldTodayImageToReturn = new TodayImage();
        var updateResultToReturn = new TodayImageServiceCheckUpdateResult {
            HasUpdate = false
        };

        var todayImageServiceMock = new Mock<ITodayImageService>();
        todayImageServiceMock.Setup(p => p.GetTodayImageAsync())
            .ReturnsAsync(oldTodayImageToReturn);
        todayImageServiceMock.Setup(p => p.CheckUpdateAsync())
            .ReturnsAsync(updateResultToReturn);
        var mockImageService = todayImageServiceMock.Object;

        var todayPoetryServiceMock = new Mock<ITodayPoetryService>();
        var mockTodayPoetryService = todayPoetryServiceMock.Object;

        var todayPageViewModel = new TodayViewModel(mockImageService,
            mockTodayPoetryService, null);
        var todayImageList = new List<TodayImage>();
        todayPageViewModel.PropertyChanged += (sender, args) => {
            switch (args.PropertyName) {
                case nameof(TodayViewModel.TodayImage):
                    todayImageList.Add(todayPageViewModel.TodayImage);
                    break;
            }
        };

        todayPageViewModel.OnLoaded();
        while (todayImageList.Count != 1) {
            await Task.Delay(100);
        }

        Assert.Same(oldTodayImageToReturn, todayImageList[0]);

        todayImageServiceMock.Verify(p => p.GetTodayImageAsync(), Times.Once);
        todayImageServiceMock.Verify(p => p.CheckUpdateAsync(), Times.Once);
        todayPoetryServiceMock.Verify(p => p.GetTodayPoetryAsync(), Times.Once);
    }

    [Fact]
    public async Task ShowDetailCommandFunction_Default() {
        var menuNavigationServiceMock = new Mock<IMenuNavigationService>();
        var mockMenuNavigationService = menuNavigationServiceMock.Object;

        var todayPageViewModel = new TodayViewModel(null, null,
            mockMenuNavigationService);
        todayPageViewModel.ShowDetail();
        menuNavigationServiceMock.Verify(
            p => p.NavigateTo(ContentNavigationConstant.TodayDetailView, null),
            Times.Once);
    }

    public async Task QueryCommandFunction_Default() {
        object parameter = null;
        var menuNavigationServiceMock = new Mock<IMenuNavigationService>();
        menuNavigationServiceMock
            .Setup(p => p.NavigateTo(MenuNavigationConstant.QueryView,
                It.IsAny<object>()))
            .Callback<string, object>((s, o) => parameter = o);
        var mockMenuNavigationService = menuNavigationServiceMock.Object;

        var todayPoetry = new TodayPoetry { Name = "小重山", Author = "张良能" };

        var todayPageViewModel =
            new TodayViewModel(null, null, mockMenuNavigationService);
        todayPageViewModel.TodayPoetry = todayPoetry;
        todayPageViewModel.Query();

        Assert.IsType<PoetryQuery>(parameter);
        var poetryQuery = (PoetryQuery)parameter;
        Assert.Equal(todayPoetry.Name, poetryQuery.Name);
        Assert.Equal(todayPoetry.Author, poetryQuery.Author);
    }
}