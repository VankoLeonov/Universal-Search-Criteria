using UniversalSearchCriteria.ViewModels;
using System.Windows;

namespace UniversalSearchCriteria.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            DataContext = viewModel; // Set the MainWindowViewModel as the DataContext
        }
    }
}
