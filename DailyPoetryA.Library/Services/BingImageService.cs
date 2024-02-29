using System.Globalization;
using System.Text.Json;
using DailyPoetryA.Library.Helpers;
using DailyPoetryA.Library.Models;

namespace DailyPoetryA.Library.Services;

public class BingImageService : ITodayImageService {
    private readonly ITodayImageStorage _todayImageStorage;

    private readonly IAlertService _alertService;

    private static HttpClient _httpClient = new();

    private const string Server = "必应每日图片服务器";

    public BingImageService(ITodayImageStorage todayImageStorage,
        IAlertService alertService) {
        _todayImageStorage = todayImageStorage;
        _alertService = alertService;
    }

    public async Task<TodayImage> GetTodayImageAsync() =>
        await _todayImageStorage.GetTodayImageAsync(true);

    public async Task<TodayImageServiceCheckUpdateResult> CheckUpdateAsync() {
        var todayImage = await _todayImageStorage.GetTodayImageAsync(false);
        if (todayImage.ExpiresAt > DateTime.Now) {
            return new TodayImageServiceCheckUpdateResult { HasUpdate = false };
        }

        HttpResponseMessage response;
        try {
            response = await _httpClient.GetAsync(
                "https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=zh-CN");
            response.EnsureSuccessStatusCode();
        } catch (Exception e) {
            await _alertService.AlertAsync(
                ErrorMessageHelper.HttpClientErrorTitle,
                ErrorMessageHelper.GetHttpClientError(Server, e.Message));
            return new TodayImageServiceCheckUpdateResult { HasUpdate = false };
        }

        var json = await response.Content.ReadAsStringAsync();
        string bingImageUrl;
        try {
            var bingImage = JsonSerializer.Deserialize<BingImageOfTheDay>(json,
                new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                })?.Images?.FirstOrDefault() ?? throw new JsonException();

            var bingImageFullStartDate = DateTime.ParseExact(
                bingImage.FullStartDate ?? throw new JsonException(),
                "yyyyMMddHHmm", CultureInfo.InvariantCulture);
            var todayImageFullStartDate = DateTime.ParseExact(
                todayImage.FullStartDate, "yyyyMMddHHmm",
                CultureInfo.InvariantCulture);

            if (bingImageFullStartDate <= todayImageFullStartDate) {
                todayImage.ExpiresAt = DateTime.Now.AddHours(2);
                await _todayImageStorage.SaveTodayImageAsync(todayImage, true);
                return new TodayImageServiceCheckUpdateResult {
                    HasUpdate = false
                };
            }

            todayImage = new TodayImage {
                FullStartDate = bingImage.FullStartDate,
                ExpiresAt = bingImageFullStartDate.AddDays(1),
                Copyright = bingImage.Copyright ?? throw new JsonException(),
                CopyrightLink = bingImage.CopyrightLink ??
                    throw new JsonException()
            };

            bingImageUrl = bingImage.Url ?? throw new JsonException();
        } catch (Exception e) {
            await _alertService.AlertAsync(
                ErrorMessageHelper.JsonDeserializationErrorTitle,
                ErrorMessageHelper.GetJsonDeserializationError(Server,
                    e.Message));
            return new TodayImageServiceCheckUpdateResult { HasUpdate = false };
        }

        try {
            response =
                await _httpClient.GetAsync(
                    "https://www.bing.com" + bingImageUrl);
            response.EnsureSuccessStatusCode();
        } catch (Exception e) {
            await _alertService.AlertAsync(
                ErrorMessageHelper.HttpClientErrorTitle,
                ErrorMessageHelper.GetHttpClientError(Server, e.Message));
            return new TodayImageServiceCheckUpdateResult { HasUpdate = false };
        }

        todayImage.ImageBytes = await response.Content.ReadAsByteArrayAsync();
        await _todayImageStorage.SaveTodayImageAsync(todayImage, false);
        return new TodayImageServiceCheckUpdateResult {
            HasUpdate = true, TodayImage = todayImage
        };
    }
}

public class BingImageOfTheDay {
    public List<BingImageOfTheDayImage> Images { get; set; }
}

public class BingImageOfTheDayImage {
    public string StartDate { get; set; }

    public string FullStartDate { get; set; }

    public string EndDate { get; set; }

    public string Url { get; set; }

    public string Copyright { get; set; }

    public string CopyrightLink { get; set; }
}