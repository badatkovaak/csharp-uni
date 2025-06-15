using Avalonia.Controls.Shapes;

namespace Lagrange;

public class Plot
{
    public readonly int id;
    public Func<double, double> function;
    public List<Line> plotLines;
    public double step;

    public Plot(int id, Func<double, double> function, List<Line> plotLines, double step)
    {
        this.id = id;
        this.function = function;
        this.plotLines = plotLines;
        this.step = step;
    }
}
