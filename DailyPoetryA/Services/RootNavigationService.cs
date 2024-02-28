using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;

namespace DailyPoetryA.Services;

public class RootNavigationService : IRootNavigationService {
    public void NavigateTo(string view) {
        ServiceLocator.Current.MainWindowViewModel.Content = view switch {
            RootNavigationConstant.InitializationView => ServiceLocator.Current
                .InitializationViewModel,
            RootNavigationConstant.MainView => ServiceLocator.Current
                .MainViewModel,
            _ => throw new Exception("未知的视图。")
        };
    }
}