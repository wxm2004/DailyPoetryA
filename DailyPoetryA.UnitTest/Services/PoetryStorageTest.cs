using System.Linq.Expressions;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;
using DailyPoetryA.UnitTest.Helpers;
using Moq;
using static DailyPoetryA.Library.Services.PoetryStorageConstant;

namespace DailyPoetryA.UnitTest.Services;

public class PoetryStorageTest : IDisposable {
    public PoetryStorageTest() => PoetryStorageHelper.RemoveDatabaseFile();

    public void Dispose() => PoetryStorageHelper.RemoveDatabaseFile();

    [Fact]
    public void IsInitialized_Default() {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        preferenceStorageMock.Setup(p => p.Get(VersionKey, default(int)))
            .Returns(PoetryStorageConstant.Version);
        var mockPreferenceStorage = preferenceStorageMock.Object;

        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        Assert.True(poetryStorage.IsInitialized);

        preferenceStorageMock.Verify(p => p.Get(VersionKey, default(int)),
            Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_Default() {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;

        var poetryStorage = new PoetryStorage(mockPreferenceStorage);

        Assert.False(File.Exists(PoetryStorage.PoetryDbPath));
        await poetryStorage.InitializeAsync();
        Assert.True(File.Exists(PoetryStorage.PoetryDbPath));

        preferenceStorageMock.Verify(
            p => p.Set(VersionKey, PoetryStorageConstant.Version), Times.Once);
    }

    [Fact]
    public async Task GetPoetryAsync_Default() {
        var poetryStorage = await PoetryStorageHelper
            .GetInitializedPoetryStorage();
        var poetry = await poetryStorage.GetPoetryAsync(10001);
        Assert.Equal("临江仙 · 夜归临皋", poetry.Name);
        await poetryStorage.CloseAsync();
    }

    [Fact]
    public async Task GetPoetriesAsync_Default() {
        var poetryStorage =
            await PoetryStorageHelper.GetInitializedPoetryStorage();
        var poetries = await poetryStorage.GetPoetriesAsync(
            Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
                Expression.Parameter(typeof(Poetry), "p")), 0, int.MaxValue);
        Assert.Equal(PoetryStorage.NumberPoetry, poetries.Count());
        await poetryStorage.CloseAsync();
    }
}