using Avalonia.Controls;

namespace calc;

/*public partial class MainWindow : Window*/
/*{*/
/*    public MainWindow()*/
/*    {*/
/*        InitializeComponent();*/
/*    }*/
/*}*/

public class MainWindow : Window
{
    public MainWindow()
    {
        Window mainWindow = new Window();
        Grid mainGrid = new Grid();

        mainWindow.Content = mainGrid;
        mainWindow.Show();
    }
}

