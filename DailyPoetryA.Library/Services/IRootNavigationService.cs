namespace DailyPoetryA.Library.Services;

public interface IRootNavigationService {
    void NavigateTo(string view);
}

public static class RootNavigationConstant {
    public const string InitializationView = nameof(InitializationView);

    public const string MainView = nameof(MainView);
}