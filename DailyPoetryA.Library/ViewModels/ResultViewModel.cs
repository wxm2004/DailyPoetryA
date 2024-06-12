using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class ResultViewModel : ViewModelBase {
    private IContentNavigationService _contentNavigationService;

    private Expression<Func<Poetry, bool>> _where;

    public ResultViewModel(IContentNavigationService contentNavigationService) {
        _contentNavigationService = contentNavigationService;
        AppendCommand = new AsyncRelayCommand(AppendAsync);
    }

    public override void SetParameter(object parameter) {
        if (parameter is not Expression<Func<Poetry, bool>> where) {
            return;
        }

        _where = where;
    }

    public ObservableCollection<Poetry> PoetryCollection { get; } = [];

    public ICommand AppendCommand { get; }

    public async Task AppendAsync() { }
}