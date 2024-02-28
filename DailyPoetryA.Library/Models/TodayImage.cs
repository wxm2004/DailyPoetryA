namespace DailyPoetryA.Library.Models;

public class TodayImage {
    public string FullStartDate { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public string Copyright { get; set; } = string.Empty;

    public string CopyrightLink { get; set; } = string.Empty;

    public byte[] ImageBytes { get; set; }
}