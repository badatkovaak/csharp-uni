using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

class Utils
{
    public static Button create_button(
        object content,
        HorizontalAlignment h,
        VerticalAlignment v,
        int FontSize
    )
    {
        Button b = new Button();
        b.VerticalAlignment = v;
        b.HorizontalAlignment = h;
        b.Content = content;
        b.FontSize = FontSize;
        return b;
    }

    public static Button create_button_with_label(
        object content,
        HorizontalAlignment button_h,
        VerticalAlignment button_v,
        HorizontalAlignment label_h,
        VerticalAlignment label_v,
        int FontSize,
        EventHandler<RoutedEventArgs> event_handler
    )
    {
        Button b = new Button();
        b.VerticalAlignment = button_v;
        b.HorizontalAlignment = button_h;

        Label l = new Label();
        l.VerticalAlignment = label_v;
        l.HorizontalAlignment = label_h;
        l.FontSize = FontSize;
        l.Content = content;
        b.Content = l;
        b.Click += event_handler;

        return b;
    }
}
