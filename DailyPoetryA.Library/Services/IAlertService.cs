namespace DailyPoetryA.Library.Services;

public interface IAlertService {
    Task AlertAsync(string title, string message);
}