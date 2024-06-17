using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class DetailViewModel : ViewModelBase {
    private IFavoriteStorage _favoriteStorage;

    public DetailViewModel(IFavoriteStorage favoriteStorage) {
        _favoriteStorage = favoriteStorage;

        OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync);
    }

    public override void SetParameter(object parameter) {
        if (parameter is not Models.Poetry poetry) {
            return;
        }

        Poetry = poetry;
    }

    public Poetry Poetry {
        get => _poetry;
        set => SetProperty(ref _poetry, value);
    }

    private Poetry _poetry;

    public Favorite Favorite {
        get => _favorite;
        set => SetProperty(ref _favorite, value);
    }

    private Favorite _favorite;

    public ICommand OnInitializedCommand { get; }

    public async Task OnInitializedAsync() { }
}