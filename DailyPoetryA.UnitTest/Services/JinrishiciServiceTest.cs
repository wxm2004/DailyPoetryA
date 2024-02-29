using DailyPoetryA.Library.Services;
using DailyPoetryA.UnitTest.Helpers;
using Moq;

namespace DailyPoetryA.UnitTest.Services;

public class JinrishiciServiceTest {
    [Fact(Skip = "依赖远程服务的测试")]
    public async Task GetTokenAsync_ReturnIsNotNullOrWhiteSpace() {
        var alertServiceMock = new Mock<IAlertService>();
        var mockAlertService = alertServiceMock.Object;

        var preferenceServiceMock = new Mock<IPreferenceStorage>();
        var mockPreferenceService = preferenceServiceMock.Object;

        var poetryStorageMock = new Mock<IPoetryStorage>();
        var mockPoetryStorage = poetryStorageMock.Object;

        var jinrishiciService = new JinrishiciService(mockAlertService,
            mockPreferenceService, mockPoetryStorage);
        var token = await jinrishiciService.GetTokenAsync();

        Assert.False(string.IsNullOrWhiteSpace(token));
        alertServiceMock.Verify(
            p => p.AlertAsync(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
        preferenceServiceMock.Verify(
            p => p.Get(JinrishiciService.JinrishiciTokenKey, string.Empty),
            Times.Once);
        preferenceServiceMock.Verify(
            p => p.Set(JinrishiciService.JinrishiciTokenKey, token),
            Times.Once);
    }

    [Fact(Skip = "依赖远程服务的测试")]
    public async Task GetTodayPoetryAsync_ReturnFromJinrishici() {
        var alertServiceMock = new Mock<IAlertService>();
        var mockAlertService = alertServiceMock.Object;

        var preferenceServiceMock = new Mock<IPreferenceStorage>();
        var mockPreferenceService = preferenceServiceMock.Object;

        var poetryStorageMock = new Mock<IPoetryStorage>();
        var mockPoetryStorage = poetryStorageMock.Object;

        var jinrishiciService = new JinrishiciService(mockAlertService,
            mockPreferenceService, mockPoetryStorage);
        var todayPoetry = await jinrishiciService.GetTodayPoetryAsync();

        Assert.Equal(TodayPoetrySources.Jinrishici, todayPoetry.Source);
        Assert.False(string.IsNullOrWhiteSpace(todayPoetry.Snippet));
        alertServiceMock.Verify(
            p => p.AlertAsync(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
        preferenceServiceMock.Verify(
            p => p.Get(JinrishiciService.JinrishiciTokenKey, string.Empty),
            Times.Once);
        preferenceServiceMock.Verify(
            p => p.Set(JinrishiciService.JinrishiciTokenKey,
                It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetRandomPoetryAsync_Default() {
        var poetryStorage =
            await PoetryStorageHelper.GetInitializedPoetryStorage();

        var jinrishiciService =
            new JinrishiciService(null, null, poetryStorage);
        var randomPoetry = await jinrishiciService.GetRandomPoetryAsync();
        Assert.NotNull(randomPoetry);
        Assert.False(string.IsNullOrWhiteSpace(randomPoetry.Name));

        await poetryStorage.CloseAsync();
        PoetryStorageHelper.RemoveDatabaseFile();
    }
}