using CommunityToolkit.Mvvm.ComponentModel;

namespace DailyPoetryA.Library.ViewModels;

public abstract class ViewModelBase : ObservableObject {
    public virtual void SetParameter(object parameter) { }
}