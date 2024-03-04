using System;
using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;

namespace DailyPoetryA.Services;

public class MenuNavigationService : IMenuNavigationService {
    public void NavigateTo(string view, object parameter = null) {
        ViewModelBase viewModel = view switch {
            MenuNavigationConstant.TodayView => ServiceLocator.Current
                .TodayViewModel,
            MenuNavigationConstant.QueryView => ServiceLocator.Current
                .QueryViewModel,
            MenuNavigationConstant.FavoriteView => ServiceLocator.Current
                .FavoriteViewModel,
            _ => throw new Exception("未知的视图。")
        };

        if (parameter is not null) {
            viewModel.SetParameter(parameter);
        }

        ServiceLocator.Current.MainViewModel.SetMenuAndContent(view, viewModel);
    }
}