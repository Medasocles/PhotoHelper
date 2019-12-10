using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PhotoHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainViewModel _mainViewModel;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _mainViewModel = new MainViewModel();
            var window = new MainWindow { DataContext = _mainViewModel };

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
