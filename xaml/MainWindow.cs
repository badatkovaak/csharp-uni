using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

/*using Avalonia.Media;*/



public class MainWindow : Window
{
    public MainWindow()
    {
        this.Title = "calculator";

        TextBlock result = new TextBlock();
        result.Text = "0";
        result.FontSize = 40;
        result.HorizontalAlignment = HorizontalAlignment.Right;

        Grid key_grid = new Grid();
        key_grid.HorizontalAlignment = HorizontalAlignment.Stretch;
        key_grid.VerticalAlignment = VerticalAlignment.Stretch;
        key_grid.ShowGridLines = true;

        for (int i = 0; i < 4; i++)
        {
            key_grid.ColumnDefinitions.Add(new ColumnDefinition(0.25, GridUnitType.Star));
            key_grid.RowDefinitions.Add(new RowDefinition(0.25, GridUnitType.Star));
        }

        for (int i = 1; i < 11; i++)
        {
            Button b = new Button();
            b.FontSize = 40;
            b.HorizontalAlignment = HorizontalAlignment.Stretch;
            b.VerticalAlignment = VerticalAlignment.Stretch;

            Label l = new Label();
            l.Content = (i % 10).ToString();
            l.HorizontalAlignment = HorizontalAlignment.Center;
            l.VerticalAlignment = VerticalAlignment.Center;
            b.Content = l;

            Grid.SetRow(b, (i - 1) / 3 + i / 10);
            Grid.SetColumn(b, (i - 1) % 3 + i / 10);

            key_grid.Children.Add(b);
        }

        Grid main_grid = new Grid();
        /*main_grid.Background = Brushes.LightCyan;*/
        main_grid.VerticalAlignment = VerticalAlignment.Stretch;
        main_grid.HorizontalAlignment = HorizontalAlignment.Stretch;

        main_grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));
        main_grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));

        Grid.SetRow(result, 0);
        main_grid.Children.Add(result);

        Grid.SetRow(key_grid, 1);
        main_grid.Children.Add(key_grid);

        this.Content = main_grid;
    }

    public void OnClick(object sender, RoutedEventArgs e) { }

    /*static Button create1(string c) =>*/
    /*    Utils.create_button_with_label(*/
    /*        c,*/
    /*        HorizontalAlignment.Stretch,*/
    /*        VerticalAlignment.Stretch,*/
    /*        HorizontalAlignment.Center,*/
    /*        VerticalAlignment.Center,*/
    /*        40*/
    /*    );*/
}
