using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class TodayDetailViewModel : ViewModelBase {
    private readonly IMenuNavigationService _menuNavigationService;

    public TodayDetailViewModel(IMenuNavigationService menuNavigationService) {
        _menuNavigationService = menuNavigationService;
        QueryCommand = new RelayCommand(Query);
    }


    private TodayPoetry _todayPoetry;

    public TodayPoetry TodayPoetry {
        get => _todayPoetry;
        private set => SetProperty(ref _todayPoetry, value);
    }

    public override void SetParameter(object parameter) {
        TodayPoetry = parameter as TodayPoetry;
    }

    public ICommand QueryCommand { get; }

    public void Query() =>
        _menuNavigationService.NavigateTo(MenuNavigationConstant.QueryView,
            new PoetryQuery {
                Author = TodayPoetry.Author, Name = TodayPoetry.Name
            });
}