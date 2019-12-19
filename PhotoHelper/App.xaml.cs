using PhotoHelper.ViewModels.Main;
using PhotoHelper.Views.MainView;
using System.Windows;

namespace PhotoHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainView _mainView;
        private MainViewModel _mainViewModel;


        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _mainViewModel = new MainViewModel();
            _mainView = new MainView { DataContext = _mainViewModel };

            var window = new MainWindow { Content = _mainView };

            MainWindow = window;
            MainWindow.Closing += OnMainWindowClosing;

            window.ShowDialog();
        }

        private void OnMainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _mainViewModel.Terminate();
        }
    }
}
