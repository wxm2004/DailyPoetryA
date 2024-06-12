using System;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace DailyPoetryA.Controls;

// https://github.com/Coolkeke/LayUI-Avalonia/blob/main/src/LayuiAvaloniaPack/UI/LayUI.Avalonia/Controls/ItemsControl/LayFlowItemsControl.cs
[TemplatePart(Name = nameof(PART_ScrollViewer), Type = typeof(ScrollViewer))]
public class LayFlowItemsControl : ItemsControl {
    private ScrollViewer PART_ScrollViewer;

    /// <summary>
    /// AppendEvent is raise when ....
    /// </summary>
    public static readonly RoutedEvent<RoutedEventArgs> AppendEvent =
        RoutedEvent.Register<LayFlowItemsControl, RoutedEventArgs>(
            nameof(Append), RoutingStrategies.Bubble);

    /// <summary>
    /// 滚动条滑动到底部触发事件
    /// </summary>
    public event EventHandler<RoutedEventArgs> Append {
        add => AddHandler(AppendEvent, value);
        remove => RemoveHandler(AppendEvent, value);
    }

    /// <summary>
    /// 滚动条滑动到底部触发方法
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnAppend(RoutedEventArgs e) {
        RaiseEvent(e);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e) {
        base.OnApplyTemplate(e);
        PART_ScrollViewer = e.NameScope.Find<ScrollViewer>(nameof(PART_ScrollViewer));
        if (PART_ScrollViewer != null) {
            PART_ScrollViewer.ScrollChanged -= PART_ScrollViewer_ScrollChanged;
            PART_ScrollViewer.ScrollChanged += PART_ScrollViewer_ScrollChanged;
        }
    }

    private void PART_ScrollViewer_ScrollChanged(object? sender, ScrollChangedEventArgs e) {
        if (PART_ScrollViewer == null) return;
        if (ItemsSource == null) {
            PART_ScrollViewer.ScrollToHome();
            return;
        }

        ;
        if (PART_ScrollViewer.ScrollBarMaximum.Y != PART_ScrollViewer.Offset.Y) return;
        OnAppend(new RoutedEventArgs(AppendEvent, this));
    }
}