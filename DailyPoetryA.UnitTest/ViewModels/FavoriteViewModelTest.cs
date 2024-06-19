using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using Moq;

namespace DailyPoetryA.UnitTest.ViewModels;

public class FavoriteViewModelTest {
    [Fact]
    public async Task LoadedCommandFunction_Default() {
        var poetryListToReturn = new List<Poetry>();
        for (var i = 0; i < 5; i++) {
            poetryListToReturn.Add(new Poetry {
                Id = i
            });
        }

        var favoriteListToReturn = new List<Favorite>();
        favoriteListToReturn.AddRange(
            poetryListToReturn.Select(p => new Favorite {
                PoetryId = p.Id
            }));

        var favoriteStorageMock = new Mock<IFavoriteStorage>();
        favoriteStorageMock.Setup(p => p.GetFavoritesAsync())
            .ReturnsAsync(favoriteListToReturn);
        var mockFavoriteStorage = favoriteStorageMock.Object;

        var poetryStorageMock = new Mock<IPoetryStorage>();
        poetryListToReturn.ForEach(p =>
            poetryStorageMock.Setup(m => m.GetPoetryAsync(p.Id))
                .ReturnsAsync(p));
        var mockPoetryStorage = poetryStorageMock.Object;

        var favoritePageViewModel =
            new FavoriteViewModel(mockFavoriteStorage, mockPoetryStorage,
                null);

        var loadingList = new List<bool>();
        favoritePageViewModel.PropertyChanged += (sender, args) => {
            if (args.PropertyName == nameof(FavoriteViewModel.IsLoading)) {
                loadingList.Add(favoritePageViewModel.IsLoading);
            }
        };

        Assert.Equal(0, favoritePageViewModel.PoetryFavoriteCollection.Count);
        await favoritePageViewModel.OnInitializedAsync();
        Assert.Equal(favoriteListToReturn.Count,
            favoritePageViewModel.PoetryFavoriteCollection.Count);
        favoriteStorageMock.Verify(p => p.GetFavoritesAsync(), Times.Once);
        favoriteListToReturn.ForEach(p =>
            poetryStorageMock.Verify(m => m.GetPoetryAsync(p.PoetryId),
                Times.Once));

        for (var i = 0;
             i < favoritePageViewModel.PoetryFavoriteCollection.Count;
             i++) {
            Assert.Same(favoriteListToReturn[i],
                favoritePageViewModel.PoetryFavoriteCollection[i].Favorite);
            Assert.Same(poetryListToReturn[i],
                favoritePageViewModel.PoetryFavoriteCollection[i].Poetry);
        }
    }

    [Fact]
    public async Task PoetryTappedCommandFunction_Default() {
        var contentNavigationServiceMock =
            new Mock<IContentNavigationService>();
        var mockContentNavigationService = contentNavigationServiceMock.Object;

        var favoriteStorageMock = new Mock<IFavoriteStorage>();
        var mockFavoriteStorage = favoriteStorageMock.Object;

        var favoritePageViewModel =
            new FavoriteViewModel(mockFavoriteStorage, null,
                mockContentNavigationService);
        var poetryFavoriteToNavigate =
            new PoetryFavorite {
                Poetry = new Poetry()
            };

        favoritePageViewModel.ShowPoetry(
            poetryFavoriteToNavigate.Poetry);
        contentNavigationServiceMock.Verify(
            p => p.NavigateTo(ContentNavigationConstant.DetailView,
                poetryFavoriteToNavigate.Poetry), Times.Once);
    }

    [Fact]
    public void FavoriteStorageOnUpdated_Default() {
        var favoriteStorageMock = new Mock<IFavoriteStorage>();
        var mockFavoriteStorage = favoriteStorageMock.Object;

        var poetryFavoriteList = new List<PoetryFavorite>();
        for (int i = 0; i < 5; i++) {
            poetryFavoriteList.Add(new PoetryFavorite {
                Favorite = new Favorite {
                    PoetryId = i, IsFavorite = true, Timestamp = int.MaxValue - i
                }
            });
        }

        var favoriteUpdated = new Favorite {
            PoetryId = poetryFavoriteList[2].Favorite.PoetryId,
            IsFavorite = false,
            Timestamp = poetryFavoriteList[2].Favorite.Timestamp
        };
        var poetryToReturn = new Poetry {
            Id = favoriteUpdated.PoetryId
        };

        var poetryStorageMock = new Mock<IPoetryStorage>();
        poetryStorageMock.Setup(p => p.GetPoetryAsync(poetryToReturn.Id))
            .ReturnsAsync(poetryToReturn);
        var mockPoetryStorage = poetryStorageMock.Object;

        var favoritePageViewModel =
            new FavoriteViewModel(mockFavoriteStorage, mockPoetryStorage,
                null);
        Assert.Equal(0, favoritePageViewModel.PoetryFavoriteCollection.Count);
        favoritePageViewModel.PoetryFavoriteCollection.AddRange(
            poetryFavoriteList);

        favoriteStorageMock.Raise(p => p.Updated += null, mockFavoriteStorage,
            new FavoriteStorageUpdatedEventArgs(favoriteUpdated));
        Assert.Equal(poetryFavoriteList.Count - 1,
            favoritePageViewModel.PoetryFavoriteCollection.Count);
        Assert.False(favoritePageViewModel.PoetryFavoriteCollection.Any(p =>
            p.Favorite.PoetryId == favoriteUpdated.PoetryId));

        favoriteUpdated.IsFavorite = true;
        favoriteStorageMock.Raise(p => p.Updated += null, mockFavoriteStorage,
            new FavoriteStorageUpdatedEventArgs(favoriteUpdated));
        Assert.Equal(poetryFavoriteList.Count,
            favoritePageViewModel.PoetryFavoriteCollection.Count);
        Assert.Equal(favoriteUpdated.PoetryId,
            favoritePageViewModel.PoetryFavoriteCollection.IndexOf(
                favoritePageViewModel.PoetryFavoriteCollection.First(p =>
                    p.Favorite.PoetryId == favoriteUpdated.PoetryId)));
    }
}