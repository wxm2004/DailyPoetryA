namespace DailyPoetryA.Library.ViewModels;

public class MainWindowViewModel : ViewModelBase {
    public ViewModelBase Content { get; set; }

    public MainWindowViewModel(ResultViewModel resultViewModel) {
        Content = resultViewModel;
    }
}