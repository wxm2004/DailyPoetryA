using System.Linq.Expressions;
using DailyPoetryA.Library.Models;

namespace DailyPoetryA.Library.Services;

public interface IPoetryStorage {
    bool IsInitialized { get; }

    Task InitializeAsync();

    Task<Poetry> GetPoetryAsync(int id);

    Task<IEnumerable<Poetry>> GetPoetriesAsync(
        Expression<Func<Poetry, bool>> where, int skip, int take);
}