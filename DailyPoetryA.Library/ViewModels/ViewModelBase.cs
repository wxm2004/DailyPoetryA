using CommunityToolkit.Mvvm.ComponentModel;

namespace DailyPoetryA.Library.ViewModels;

public class ViewModelBase : ObservableObject {
    public virtual void SetParameter(object parameter) { }
}