using DailyPoetryA.Library.Models;

namespace DailyPoetryA.Library.Services;

public interface IFavoriteStorage {
    bool IsInitialized { get; }

    Task InitializeAsync();

    Task<Favorite> GetFavoriteAsync(int poetryId);

    Task<IEnumerable<Favorite>> GetFavoritesAsync();

    Task SaveFavoriteAsync(Favorite favorite);

    event EventHandler<FavoriteStorageUpdatedEventArgs> Updated;
}

public class FavoriteStorageUpdatedEventArgs : EventArgs {
    public Favorite UpdatedFavorite { get; }

    public FavoriteStorageUpdatedEventArgs(Favorite favorite) {
        UpdatedFavorite = favorite;
    }
}