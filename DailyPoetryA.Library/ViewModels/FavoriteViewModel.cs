using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;
using MvvmHelpers;

namespace DailyPoetryA.Library.ViewModels;

public class FavoriteViewModel : ViewModelBase {
    private readonly IFavoriteStorage _favoriteStorage;

    private readonly IPoetryStorage _poetryStorage;

    private readonly IContentNavigationService _contentNavigationService;

    public FavoriteViewModel(IFavoriteStorage favoriteStorage,
        IPoetryStorage poetryStorage,
        IContentNavigationService contentNavigationService) {
        _favoriteStorage = favoriteStorage;
        _poetryStorage = poetryStorage;
        _contentNavigationService = contentNavigationService;

        _favoriteStorage.Updated += FavoriteStorageOnUpdated;

        OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync);
    }

    public ObservableRangeCollection<PoetryFavorite> PoetryFavoriteCollection { get; } = new();

    public bool IsLoading {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private bool _isLoading;

    public ICommand OnInitializedCommand { get; }

    public async Task OnInitializedAsync() {
        IsLoading = true;

        PoetryFavoriteCollection.Clear();
        var favoriteList = await _favoriteStorage.GetFavoritesAsync();

        PoetryFavoriteCollection.AddRange((await Task.WhenAll(
            favoriteList.Select(p => Task.Run(async () => new PoetryFavorite {
                Poetry = await _poetryStorage.GetPoetryAsync(p.PoetryId), Favorite = p
            })))).ToList());

        IsLoading = false;
    }
    
    public IRelayCommand<Poetry> ShowPoetryCommand { get; }

    private async void FavoriteStorageOnUpdated(object? sender,
        FavoriteStorageUpdatedEventArgs e) {
        var favorite = e.UpdatedFavorite;
        PoetryFavoriteCollection.Remove(
            PoetryFavoriteCollection.FirstOrDefault(p =>
                p.Favorite.PoetryId == favorite.PoetryId));

        if (!favorite.IsFavorite) {
            return;
        }

        var poetryFavorite = new PoetryFavorite {
            Poetry = await _poetryStorage.GetPoetryAsync(favorite.PoetryId), Favorite = favorite
        };

        var index = PoetryFavoriteCollection.IndexOf(
            PoetryFavoriteCollection.FirstOrDefault(p =>
                p.Favorite.Timestamp < favorite.Timestamp));
        if (index < 0) {
            index = PoetryFavoriteCollection.Count;
        }

        PoetryFavoriteCollection.Insert(index, poetryFavorite);
    }
}

public class PoetryFavorite {
    public Poetry Poetry { get; set; }

    public Favorite Favorite { get; set; }
}