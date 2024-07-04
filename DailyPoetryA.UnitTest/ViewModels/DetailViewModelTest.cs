using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using Moq;

namespace DailyPoetryA.UnitTest.ViewModels;

public class DetailViewModelTest {
    [Fact]
    public async Task OnLoadedAsync_Default() {
        var poetry = new Poetry {
            Id = 0
        };
        var favoriteToReturn =
            new Favorite {
                PoetryId = poetry.Id, IsFavorite = true
            };

        var favoriteStorageMock = new Mock<IFavoriteStorage>();
        favoriteStorageMock
            .Setup(p => p.GetFavoriteAsync(favoriteToReturn.PoetryId))
            .ReturnsAsync(favoriteToReturn);
        var mockFavoriteStorage = favoriteStorageMock.Object;

        var detailViewModel = new DetailViewModel(mockFavoriteStorage);
        detailViewModel.SetParameter(poetry);

        var loadingList = new List<bool>();
        detailViewModel.PropertyChanged += (sender, args) => {
            if (args.PropertyName == nameof(DetailViewModel.IsLoading)) {
                loadingList.Add(detailViewModel.IsLoading);
            }
        };

        await detailViewModel.OnLoadedAsync();

        favoriteStorageMock.Verify(
            p => p.GetFavoriteAsync(favoriteToReturn.PoetryId), Times.Once);
        Assert.Same(favoriteToReturn, detailViewModel.Favorite);
        Assert.Equal(2, loadingList.Count);
        Assert.True(loadingList.First());
        Assert.False(loadingList.Last());

        await detailViewModel.FavoriteSwitchClickedAsync();
        favoriteStorageMock.Verify(
            p => p.GetFavoriteAsync(favoriteToReturn.PoetryId), Times.Once);
        Assert.Same(favoriteToReturn, detailViewModel.Favorite);
        Assert.Equal(4, loadingList.Count);
    }
}