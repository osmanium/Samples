using StartScreen.ViewModels;
using System.Windows;

namespace StartScreen.Views
{
    /// <summary>
    /// Interaction logic for StartScreenView.xaml
    /// </summary>
    public partial class StartScreenView : Window
    {
        public StartScreenView(StartScreenViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
