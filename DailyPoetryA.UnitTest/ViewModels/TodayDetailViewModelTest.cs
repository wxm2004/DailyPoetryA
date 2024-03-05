using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using Moq;

namespace DailyPoetryA.UnitTest.ViewModels;

public class TodayDetailViewModelTest {
    [Fact]
    public void Query_Default() {
        object parameter = null;
        var menuNavigationService = new Mock<IMenuNavigationService>();
        menuNavigationService
            .Setup(p => p.NavigateTo(MenuNavigationConstant.QueryView,
                It.IsAny<object>()))
            .Callback<string, object>((s, o) => parameter = o);
        var mockRootNavigationService = menuNavigationService.Object;

        var todayPoetry = new TodayPoetry { Name = "小重山", Author = "张良能" };

        var todayDetailViewModel =
            new TodayDetailViewModel(mockRootNavigationService);
        todayDetailViewModel.SetParameter(todayPoetry);
        todayDetailViewModel.Query();

        Assert.IsType<PoetryQuery>(parameter);
        var poetryQuery = (PoetryQuery)parameter;
        Assert.Equal(todayPoetry.Name, poetryQuery.Name);
        Assert.Equal(todayPoetry.Author, poetryQuery.Author);
    }
}