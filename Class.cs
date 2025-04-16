using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;

namespace classwork;

public class MainWindow : Window
{
    const int LEN = 5;
    Canvas c;
    List<Label> squares;
    Shape? piece;
    (int, int)? piece_square;
    public MainWindow()
    {
        Canvas c = new Canvas();
        c.HorizontalAlignment = HorizontalAlignment.Stretch;
        c.VerticalAlignment = VerticalAlignment.Stretch;
        c.Background = Brushes.LightBlue;
        c.EffectiveViewportChanged += OnDimensionsChange;
        c.PointerReleased += OnClick;
        c.KeyDown += OnKey;

        this.Content = c;
        this.c = c;
        this.squares = new List<Label>();
    }

    public void OnDimensionsChange(object? sender, EffectiveViewportChangedEventArgs e)
    {
        foreach (Label l in this.squares)
        {
            this.c.Children.Remove(l);
        }
        this.squares.RemoveAll((_) => true);

        for (int i = 0; i < LEN; i++)
            for (int j = 0; j < LEN; j++)
            {
                Label l = new Label();
                double w = e.EffectiveViewport.Width;
                double h = e.EffectiveViewport.Height;
                l.Width = w;
                l.Height = h;
                l.Background = (i * LEN + j) % 2 == 0 ? Brushes.Beige : Brushes.SaddleBrown;
                Canvas.SetLeft(l, j * e.EffectiveViewport.Width / LEN);
                Canvas.SetTop(l, i * e.EffectiveViewport.Height / LEN);
                this.c.Children.Add(l);
                this.squares.Add(l);
            }
    }

    public void OnClick(object? sender, PointerReleasedEventArgs e)
    {
        if (this.piece is not null)
            return;

        Avalonia.Point p = e.GetPosition(this.c);
        Console.WriteLine($"{p}, {GetCell(p.X, p.Y, LEN, this.c.Bounds.Width, this.c.Bounds.Height)}");

        Shape s = new Ellipse();
        s.Fill = Brushes.LightCoral;
        double cellWidth = this.c.Bounds.Width / LEN;
        double cellHeight = this.c.Bounds.Height / LEN;
        const double factor = 0.75;
        s.Height = this.c.Bounds.Height / (2*LEN) * factor;
        s.Width = this.c.Bounds.Width / (2*LEN) * factor;

        (int i, int j) = GetCell(p.X, p.Y, LEN, this.c.Bounds.Width, this.Bounds.Height);
        double x_coord = cellWidth * (i + 0.5) - s.Width*0.5;
        double y_coord = cellHeight * (j + 0.5) - s.Height*0.5;

        Canvas.SetTop(s, y_coord);
        Canvas.SetLeft(s, x_coord);


        c.Children.Add(s);
        this.piece = s;
        this.piece_square = (i, j);
    }

    public void OnKey(object? sender, KeyEventArgs e)
    {

        if (this.piece_square is null || this.piece is null)
            return;

        Console.WriteLine($"{e.Key} {this.piece_square.Value}");

        (int curr_i, int curr_j) = this.piece_square.Value;
        (int i, int j) = e.Key switch
        {
            Key.Left => (curr_i, curr_j - 1),
            Key.Right => (curr_i, curr_j + 1),
            Key.Up => (curr_i - 1, curr_j),
            Key.Down => (curr_i + 1, curr_j),
            _ => (-1,-1)
        };

        if (i < 0 || i >= LEN || j < 0 || j >= LEN)
            return;

        double cellWidth = this.c.Bounds.Width / LEN;
        double cellHeight = this.c.Bounds.Height / LEN;

        double x_coord = cellWidth * (i + 0.5) - this.piece.Width*0.5;
        double y_coord = cellHeight * (j + 0.5) - this.piece.Height*0.5;

        Console.WriteLine($"{e.Key} {x_coord} {y_coord}");

        Canvas.SetTop(this.piece, y_coord);
        Canvas.SetLeft(this.piece, x_coord);
    }

    public static (int,int) GetCell(double x, double y, int len, double width, double height)
    {
        double cellWidth = width/ len;
        double cellHeight = height/len;
        int i = 0;
        int j = 0;

        while(y >= cellHeight)
        {
            y -= cellHeight;
            j++;
        }

        while(x >= cellWidth)
        {
            x -=cellWidth;
            i++;
        }
        
        return (i,j);
    }
    
}
