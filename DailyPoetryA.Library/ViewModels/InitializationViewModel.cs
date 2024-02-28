using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class InitializationViewModel : ViewModelBase {
    private readonly IPoetryStorage _poetryStorage;

    public InitializationViewModel(IPoetryStorage poetryStorage) {
        _poetryStorage = poetryStorage;
    }
}