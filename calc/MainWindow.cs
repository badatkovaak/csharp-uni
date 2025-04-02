using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

public class MainWindow : Window
{
    /*Calculator calc;*/
    TextBlock input;
    TextBlock total;

    public MainWindow()
    {
        this.Title = "calculator";

        Grid key_grid = new Grid();
        key_grid.HorizontalAlignment = HorizontalAlignment.Stretch;
        key_grid.VerticalAlignment = VerticalAlignment.Stretch;

        for (int i = 0; i < 4; i++)
        {
            key_grid.RowDefinitions.Add(new RowDefinition(0.25, GridUnitType.Star));
        }

        for (int i = 0; i < 5; i++)
        {
            key_grid.ColumnDefinitions.Add(new ColumnDefinition(0.25, GridUnitType.Star));
        }

        for (int i = 1; i < 11; i++)
        {
            Button b = create((i % 10).ToString(), DigitOnClick);
            /*Button b = new Button();*/
            /*b.FontSize = 40;*/
            /*b.HorizontalAlignment = HorizontalAlignment.Stretch;*/
            /*b.VerticalAlignment = VerticalAlignment.Stretch;*/
            /**/
            /*Label l = new Label();*/
            /*l.Content = (i % 10).ToString();*/
            /*l.HorizontalAlignment = HorizontalAlignment.Center;*/
            /*l.VerticalAlignment = VerticalAlignment.Center;*/
            /*b.Content = l;*/
            /*b.Click += DigitOnClick;*/

            Grid.SetRow(b, (i - 1) / 3 + i / 10);
            Grid.SetColumn(b, (i - 1) % 3 + i / 10);

            key_grid.Children.Add(b);
        }

        Button backspace = create("DEL", BackspaceOnClick);
        Grid.SetRow(backspace, 0);
        Grid.SetColumn(backspace, 4);
        key_grid.Children.Add(backspace);

        Button clear = create("C", ClearOnClick);
        Grid.SetRow(clear, 0);
        Grid.SetColumn(clear, 3);
        key_grid.Children.Add(clear);

        Button plus = create("+", OperationOnClick);
        Grid.SetRow(plus, 1);
        Grid.SetColumn(plus, 3);
        key_grid.Children.Add(plus);

        Button sub = create("-", OperationOnClick);
        Grid.SetRow(sub, 1);
        Grid.SetColumn(sub, 4);
        key_grid.Children.Add(sub);

        Button mult = create("*", OperationOnClick);
        Grid.SetRow(mult, 2);
        Grid.SetColumn(mult, 3);
        key_grid.Children.Add(mult);

        Button div = create("/", OperationOnClick);
        Grid.SetRow(div, 2);
        Grid.SetColumn(div, 4);
        key_grid.Children.Add(div);

        Button eq = create("=", EqualsOnClick);
        Grid.SetRow(eq, 3);
        Grid.SetColumn(eq, 4);
        key_grid.Children.Add(eq);

        Button sign = create("-", DigitOnClick);
        Grid.SetRow(sign, 3);
        Grid.SetColumn(sign, 0);
        key_grid.Children.Add(sign);

        Button left_paren = create("(", DigitOnClick);
        Grid.SetRow(left_paren, 3);
        Grid.SetColumn(left_paren, 2);
        key_grid.Children.Add(left_paren);

        Button right_paren = create(")", DigitOnClick);
        Grid.SetRow(right_paren, 3);
        Grid.SetColumn(right_paren, 3);
        key_grid.Children.Add(right_paren);

        Grid main_grid = new Grid();
        main_grid.VerticalAlignment = VerticalAlignment.Stretch;
        main_grid.HorizontalAlignment = HorizontalAlignment.Stretch;

        main_grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));
        main_grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Auto));
        main_grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));

        TextBlock input = new TextBlock();
        input.Text = "";
        input.FontSize = 50;
        input.HorizontalAlignment = HorizontalAlignment.Right;

        TextBlock total = new TextBlock();
        total.Text = "0";
        total.FontSize = 50;
        total.HorizontalAlignment = HorizontalAlignment.Right;

        Grid.SetRow(input, 0);
        main_grid.Children.Add(input);

        Grid.SetRow(total, 1);
        main_grid.Children.Add(total);

        Grid.SetRow(key_grid, 2);
        main_grid.Children.Add(key_grid);

        this.Content = main_grid;
        this.input = input;
        this.total = total;
    }

    public void DigitOnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is null)
            throw new Exception();

        Button b = (Button)sender;

        if (b.Content is null)
            throw new Exception();

        Label l = (Label)b.Content;

        if (l.Content is null)
            throw new Exception();

        string s = (string)l.Content;
        this.input.Text += s;
    }

    public void EqualsOnClick(object? sender, RoutedEventArgs e)
    {
        Parser p = new Parser(this.input.Text ?? "");
        Expression? res = p.Parse();
        Console.WriteLine(res);

        if (res is null)
            this.total.Text = "Error";
        else
            this.total.Text = res.evaluate().ToString();
    }

    public void OperationOnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is null)
            throw new Exception();

        Button b = (Button)sender;

        if (b.Content is null)
            throw new Exception();

        Label l = (Label)b.Content;

        if (l.Content is null)
            throw new Exception();

        string s = (string)l.Content;
        this.input.Text += " " + s + " ";
    }

    public void ClearOnClick(object? sender, RoutedEventArgs e)
    {
        this.input.Text = "";
        this.total.Text = "0";
    }

    public void BackspaceOnClick(object? sender, RoutedEventArgs e)
    {
        if (this.input.Text?.Length == 0)
            return;

        string? s = this.input.Text;

        if (s is null)
            return;

        if (s[s.Length - 1] == ' ')
        {
            if (s.Length <= 3)
                throw new Exception("too low input len");

            this.input.Text = s.Remove(s.Length - 3);
        }
        else if (s.Length >= 1)
            this.input.Text = s.Remove(s.Length - 1);
    }

    static Button create(object c, EventHandler<RoutedEventArgs> f) =>
        Utils.create_button_with_label(
            c,
            HorizontalAlignment.Stretch,
            VerticalAlignment.Stretch,
            HorizontalAlignment.Center,
            VerticalAlignment.Center,
            50,
            f
        );
}
