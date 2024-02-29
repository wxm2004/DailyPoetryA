using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;
using Moq;

namespace DailyPoetryA.UnitTest.Services;

public class BingImageServiceTest {
    [Fact(Skip = "依赖远程服务的测试")]
    public async Task CheckUpdateAsync_TodayImageNotExpired() {
        var todayImageToReturn = new TodayImage {
            FullStartDate = "197001010000",
            ExpiresAt = DateTime.Now + TimeSpan.FromHours(1),
            Copyright = "Copyright",
            CopyrightLink = "http://no.such.url"
        };

        var todayImageStorageMock = new Mock<ITodayImageStorage>();
        todayImageStorageMock.Setup(p => p.GetTodayImageAsync(false))
            .ReturnsAsync(todayImageToReturn);
        var mockTodayImageStorage = todayImageStorageMock.Object;

        var alertServiceMock = new Mock<IAlertService>();
        var mockAlertService = alertServiceMock.Object;

        var bingImageService =
            new BingImageService(mockTodayImageStorage, mockAlertService);
        var checkUpdateResult = await bingImageService.CheckUpdateAsync();

        Assert.False(checkUpdateResult.HasUpdate);
        todayImageStorageMock.Verify(p => p.GetTodayImageAsync(false),
            Times.Once);
        todayImageStorageMock.Verify(
            p => p.SaveTodayImageAsync(It.IsAny<TodayImage>(),
                It.IsAny<bool>()), Times.Never);
        alertServiceMock.Verify(
            p => p.AlertAsync(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact(Skip = "依赖远程服务的测试")]
    public async Task CheckUpdateAsync_TodayImageExpired() {
        var todayImageToReturn = new TodayImage {
            FullStartDate = "197001010000",
            ExpiresAt = DateTime.Now - TimeSpan.FromHours(1),
            Copyright = "Copyright",
            CopyrightLink = "http://no.such.url"
        };

        var todayImageStorageMock = new Mock<ITodayImageStorage>();
        todayImageStorageMock.Setup(p => p.GetTodayImageAsync(false))
            .ReturnsAsync(todayImageToReturn);
        var mockTodayImageStorage = todayImageStorageMock.Object;

        var alertServiceMock = new Mock<IAlertService>();
        var mockAlertService = alertServiceMock.Object;

        var bingImageService =
            new BingImageService(mockTodayImageStorage, null);
        var checkUpdateResult = await bingImageService.CheckUpdateAsync();

        Assert.True(checkUpdateResult.HasUpdate);
        todayImageStorageMock.Verify(p => p.GetTodayImageAsync(false),
            Times.Once);
        todayImageStorageMock.Verify(
            p => p.SaveTodayImageAsync(checkUpdateResult.TodayImage, false),
            Times.Once);
        alertServiceMock.Verify(
            p => p.AlertAsync(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }
}