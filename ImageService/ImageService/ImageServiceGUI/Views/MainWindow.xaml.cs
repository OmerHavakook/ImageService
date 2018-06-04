using ImageServiceGUI.ViewModels;
using System.Windows;

namespace ImageServiceGUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowVm();
        }

    }
}
