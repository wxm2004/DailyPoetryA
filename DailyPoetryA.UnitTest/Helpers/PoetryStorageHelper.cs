using DailyPoetryA.Library.Services;
using Moq;

namespace DailyPoetryA.UnitTest.Helpers;

public class PoetryStorageHelper {
    public static void RemoveDatabaseFile() =>
        File.Delete(PoetryStorage.PoetryDbPath);

    public static async Task<PoetryStorage> GetInitializedPoetryStorage() {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        preferenceStorageMock.Setup(p =>
            p.Get(PoetryStorageConstant.VersionKey, -1)).Returns(-1);
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        await poetryStorage.InitializeAsync();
        return poetryStorage;
    }
}