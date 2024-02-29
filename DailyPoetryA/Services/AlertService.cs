using System.Threading.Tasks;
using DailyPoetryA.Library.Services;
using Ursa.Controls;

namespace DailyPoetryA.Services;

public class AlertService : IAlertService {
    public async Task AlertAsync(string title, string message) =>
        await MessageBox.ShowAsync(message, title, button: MessageBoxButton.OK);
}