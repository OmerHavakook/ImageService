using ImageServiceGUI.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImageServiceGUI.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            this.DataContext = new SettingsVM();
        }

    }
}
