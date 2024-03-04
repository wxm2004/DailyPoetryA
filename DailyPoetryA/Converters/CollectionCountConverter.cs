using System;
using System.Collections;
using System.Globalization;
using Avalonia.Data.Converters;

namespace DailyPoetryA.Converters;

public class CollectionCountConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture) =>
        value is ICollection collection && parameter is string countString &&
        int.TryParse(countString, out var count)
            ? collection.Count > count
            : null;

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture) {
        throw new InvalidOperationException();
    }
}