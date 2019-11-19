using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UsbMonitor;

namespace PhotoHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UsbMonitorManager _usbMonitorManager;
        public ObservableCollection<string> UsbStrings;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            _usbMonitorManager = new UsbMonitorManager(this, false);
            
            UsbStrings = new ObservableCollection<string>();
           _usbMonitorManager.UsbOem += OnUsb;
           _usbMonitorManager.UsbVolume += OnUsb;
           _usbMonitorManager.UsbPort += OnUsb;
           _usbMonitorManager.UsbDeviceInterface += OnUsb;
           _usbMonitorManager.UsbHandle += OnUsb;
           _usbMonitorManager.UsbChanged += OnUsb;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _usbMonitorManager.Start();
        }

        private void OnUsb(object sender, UsbEventArgs e)
        {
            UsbStrings.Add(e.ToString());
            //this.textBox.Text += e.ToString() + "\r\n";
        }

    }
}
