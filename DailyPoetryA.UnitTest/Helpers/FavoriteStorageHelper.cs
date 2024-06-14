using DailyPoetryA.Library.Services;

namespace DailyPoetryA.UnitTest.Helpers;

public class FavoriteStorageHelper {
    public static void RemoveDatabaseFile() =>
        File.Delete(FavoriteStorage.PoetryDbPath);
}