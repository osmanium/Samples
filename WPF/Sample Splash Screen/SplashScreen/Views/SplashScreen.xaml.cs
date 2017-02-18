using SplashScreen.ViewModels;
using System.Windows;

namespace SplashScreen.Views
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen(SplashScreenViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
