using DailyPoetryA.Library.Models;

namespace DailyPoetryA.Library.Services;

public interface ITodayImageStorage {
    Task<TodayImage> GetTodayImageAsync(bool isIncludingImageStream);

    Task SaveTodayImageAsync(TodayImage todayImage, bool isSavingExpiresAtOnly);
}