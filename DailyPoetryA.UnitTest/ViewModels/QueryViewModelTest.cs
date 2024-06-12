using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.ViewModels;

namespace DailyPoetryA.UnitTest.ViewModels;

public class QueryViewModelTest {
    [Fact]
    public void AddFilterViewModel_RemoveFilterViewModel_Default() {
        var queryPageViewModel = new QueryViewModel(null);
        Assert.Equal(1, queryPageViewModel.FilterViewModelCollection.Count);

        var firstFilterViewModel = queryPageViewModel
            .FilterViewModelCollection.First();
        queryPageViewModel.AddFilterViewModel(queryPageViewModel
            .FilterViewModelCollection.First());
        Assert.Equal(2, queryPageViewModel.FilterViewModelCollection.Count);
        Assert.Same(firstFilterViewModel,
            queryPageViewModel.FilterViewModelCollection.First());

        queryPageViewModel.RemoveFilterViewModel(firstFilterViewModel);
        Assert.Equal(1, queryPageViewModel.FilterViewModelCollection.Count);
        Assert.NotSame(firstFilterViewModel,
            queryPageViewModel.FilterViewModelCollection.First());
    }

    [Fact]
    public async Task SetParameter_Default() {
        var queryPageViewModel = new QueryViewModel(null);
        Assert.Equal(1, queryPageViewModel.FilterViewModelCollection.Count);
        Assert.Equal(FilterType.NameFilter,
            queryPageViewModel.FilterViewModelCollection.First().Type);
        Assert.True(string.IsNullOrWhiteSpace(queryPageViewModel
            .FilterViewModelCollection.First().Content));

        var poetryQuery = new PoetryQuery {
            Name = "小重山", Author = "张良能"
        };
        queryPageViewModel.SetParameter(poetryQuery);

        Assert.Equal(2, queryPageViewModel.FilterViewModelCollection.Count);
        Assert.Equal(FilterType.NameFilter,
            queryPageViewModel.FilterViewModelCollection.First().Type);
        Assert.Equal(poetryQuery.Name,
            queryPageViewModel.FilterViewModelCollection.First().Content);
        Assert.Equal(FilterType.AuthorNameFilter,
            queryPageViewModel.FilterViewModelCollection.Last().Type);
        Assert.Equal(poetryQuery.Author,
            queryPageViewModel.FilterViewModelCollection.Last().Content);
    }
}