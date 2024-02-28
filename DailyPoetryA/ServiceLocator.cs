using System;
using Avalonia;
using Avalonia.Controls;
using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using DailyPoetryA.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DailyPoetryA;

public class ServiceLocator {
    private readonly IServiceProvider _serviceProvider;

    private static ServiceLocator _current;

    public static ServiceLocator Current {
        get {
            if (_current is not null) {
                return _current;
            }

            if (Application.Current.TryGetResource(nameof(ServiceLocator),
                    out var resource) &&
                resource is ServiceLocator serviceLocator) {
                return _current = serviceLocator;
            }

            throw new Exception("理论上来讲不应该发生这种情况。");
        }
    }

    public MainWindowViewModel MainWindowViewModel =>
        _serviceProvider.GetService<MainWindowViewModel>();

    public InitializationViewModel InitializationViewModel =>
        _serviceProvider.GetService<InitializationViewModel>();

    public MainViewModel MainViewModel =>
        _serviceProvider.GetService<MainViewModel>();

    public ServiceLocator() {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<MainWindowViewModel>();
        serviceCollection.AddSingleton<ResultViewModel>();
        serviceCollection.AddSingleton<InitializationViewModel>();
        serviceCollection.AddSingleton<MainViewModel>();

        serviceCollection
            .AddSingleton<IRootNavigationService, RootNavigationService>();
        serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();
        serviceCollection
            .AddSingleton<IPreferenceStorage, FilePreferenceStorage>();

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}