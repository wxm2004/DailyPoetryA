using AvaloniaInfiniteScrolling;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;

namespace DailyPoetryA.DesignViewModels;

public class ResultDesignViewModel : ResultViewModel {
    public ResultDesignViewModel(IPoetryStorage poetryStorage,
        IContentNavigationService contentNavigationService) :
        base(poetryStorage, contentNavigationService) { }

    public new AvaloniaInfiniteScrollCollection<Poetry> PoetryCollection { get; } = [
        new Poetry {
            Name = "Name 1", Author = "Author 1", Content = "Content 1", Dynasty = "Dynasty 1"
        },
        new Poetry {
            Name = "Name 2", Author = "Author 2", Content = "Content 2", Dynasty = "Dynasty 2"
        },
        new Poetry {
            Name = "Name 3", Author = "Author 3", Content = "Content 3", Dynasty = "Dynasty 3"
        },
        new Poetry {
            Name = "Name 4", Author = "Author 4", Content = "Content 4", Dynasty = "Dynasty 4"
        },
        new Poetry {
            Name = "Name 5", Author = "Author 5", Content = "Content 5", Dynasty = "Dynasty 5"
        },
    ];

    public new string Status { get; } = "Loading...";
}