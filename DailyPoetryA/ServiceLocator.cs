using System;
using DailyPoetryA.Library.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DailyPoetryA;

public class ServiceLocator {
    private IServiceProvider _serviceProvider;

    public MainWindowViewModel MainWindowViewModel =>
        _serviceProvider.GetService<MainWindowViewModel>();

    public ServiceLocator() {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<MainWindowViewModel>();
        serviceCollection.AddSingleton<ResultViewModel>();

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}