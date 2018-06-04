using ImageServiceGUI.ViewModels;
using System.Windows.Controls;

namespace ImageServiceGUI.Views
{
    /// <summary>
    /// Interaction logic for Logs.xaml
    /// </summary>
    public partial class Logs : UserControl
    {
        public Logs()
        {
            InitializeComponent();
            this.DataContext = new LogsVM();
        }

    }
}
