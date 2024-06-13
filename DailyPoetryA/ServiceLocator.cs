using System;
using Avalonia;
using Avalonia.Controls;
using DailyPoetryA.DesignViewModels;
using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using DailyPoetryA.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DailyPoetryA;

public class ServiceLocator {
    private readonly IServiceProvider _serviceProvider;

    private static ServiceLocator _current;

    public static ServiceLocator Current {
        get
        {
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

    public InitializationViewModel InitializationViewModel =>
        _serviceProvider.GetService<InitializationViewModel>();

    public MainWindowViewModel MainWindowViewModel =>
        _serviceProvider.GetService<MainWindowViewModel>();

    public MainViewModel MainViewModel =>
        _serviceProvider.GetService<MainViewModel>();

    public TodayViewModel TodayViewModel =>
        _serviceProvider.GetService<TodayViewModel>();

    public QueryViewModel QueryViewModel =>
        _serviceProvider.GetService<QueryViewModel>();

    public FavoriteViewModel FavoriteViewModel =>
        _serviceProvider.GetService<FavoriteViewModel>();

    public TodayDetailViewModel TodayDetailViewModel =>
        _serviceProvider.GetService<TodayDetailViewModel>();

    public ResultViewModel ResultViewModel =>
        _serviceProvider.GetService<ResultViewModel>();

    public ResultDesignViewModel ResultDesignViewModel =>
        _serviceProvider.GetService<ResultDesignViewModel>();

    public DetailViewModel DetailViewModel =>
        _serviceProvider.GetService<DetailViewModel>();


    public ServiceLocator() {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<InitializationViewModel>();
        serviceCollection.AddSingleton<MainWindowViewModel>();
        serviceCollection.AddSingleton<MainViewModel>();
        serviceCollection.AddSingleton<TodayViewModel>();
        serviceCollection.AddSingleton<QueryViewModel>();
        serviceCollection.AddSingleton<FavoriteViewModel>();
        serviceCollection.AddSingleton<TodayDetailViewModel>();
        serviceCollection.AddSingleton<ResultViewModel>();
        serviceCollection.AddSingleton<DetailViewModel>();

        serviceCollection
            .AddSingleton<IRootNavigationService, RootNavigationService>();
        serviceCollection
            .AddSingleton<IMenuNavigationService, MenuNavigationService>();
        serviceCollection
            .AddSingleton<IContentNavigationService,
                ContentNavigationService>();
        serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();
        serviceCollection
            .AddSingleton<IPreferenceStorage, FilePreferenceStorage>();
        serviceCollection.AddSingleton<IAlertService, AlertService>();
        serviceCollection.AddSingleton<ITodayImageService, BingImageService>();
        serviceCollection.AddSingleton<ITodayImageStorage, TodayImageStorage>();
        serviceCollection
            .AddSingleton<ITodayPoetryService, JinrishiciService>();

        serviceCollection.AddSingleton<ResultDesignViewModel>();

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}