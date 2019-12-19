using MediaDevices;
using Ookii.Dialogs.Wpf;
using PhotoHelper.Core.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;

namespace PhotoHelper.ViewModels.Main
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string _selectedPath;
        private MediaDevice _selectedDevice;
        private MediaDevice _connectedDevice;

        public MainViewModel()
        {
            MtpDevices = new ObservableCollection<MediaDevice>();
            EntriesList = new ObservableCollection<MediaFileInfo>();

            OpenSourceFolderCommand = new RelayCommand<object>(OnOpenSourceFolderCommandExecute);
            ListMtpDevicesCommand = new RelayCommand<object>(OnListMtpDevicesCommandExecute);
            ConnectDeviceCommand = new RelayCommand<object>(OnConnectDeviceCommandExecute, OnConnectDeviceCommandCanExecute);
            DisconnectDeviceCommand = new RelayCommand<object>(OnDisconnectDeviceCommandExecute, OnDisconnectDeviceCommandCanExecute);
        }

        private bool OnDisconnectDeviceCommandCanExecute(object obj)
        {
            return SelectedDevice?.IsConnected == true;
        }

        private void OnDisconnectDeviceCommandExecute(object obj)
        {
            SelectedDevice.Disconnect();
            ConnectedDevice = null;
        }

        private bool OnConnectDeviceCommandCanExecute(object obj)
        {
            return SelectedDevice?.IsConnected == false;
        }

        private void OnConnectDeviceCommandExecute(object obj)
        {
            SelectedDevice.Connect();
            ConnectedDevice = SelectedDevice;

            //var functionalCategories = SelectedDevice.FunctionalCategories();
            //foreach (var functionalCategory in functionalCategories)
            //{
            //    var functionalObjects = SelectedDevice.FunctionalObjects(functionalCategory);
            //}

            var fuid = ConnectedDevice.FunctionalUniqueId;
            var root = ConnectedDevice.GetRootDirectory();

            var images = ConnectedDevice.GetContentLocations(ContentType.Image);
            var dirs = ConnectedDevice.GetDirectories(ConnectedDevice.GetRootDirectory().FullName);
            //var test = ConnectedDevice.GetFiles(@"\\SD-Karte von Transcend\DCIM\Camera");

            if (dirs.Length == 0)
                return;

            var thread = new Thread(
                () =>
                {
                    var camPath = @"\\SD-Karte von Transcend\DCIM\Camera";
                    var directories = ConnectedDevice.GetDirectories(camPath);
                    var files = ConnectedDevice.GetFiles(camPath);

                for (int i = 0; i < files.Length; i++)
                {
                        var s = files[i];
                        var fileName = Path.GetFileName(s);
                        var filePath = Path.Combine(camPath, s);
                        var fi = ConnectedDevice.GetFileInfo(filePath);

                        try
                        {
                            using (var stream = new MemoryStream())
                            {
                                ConnectedDevice.DownloadFile(s, stream);
                                using (var fs = new FileStream($"C:\\Dev\\#Test\\DownloadedFromG4\\{fileName}", FileMode.Create))
                                {
                                    stream.Seek(0, SeekOrigin.Begin);
                                    stream.WriteTo(fs);
                                }
                            }

                        }
                        catch (System.Exception e)
                        {

                            throw;
                        }
                    }
                });

            thread.Start();


            foreach (var enumerateFileSystemInfo in root.EnumerateFileSystemInfos())
            {
                foreach (var fileSystemEntry in ConnectedDevice.GetFileSystemEntries(enumerateFileSystemInfo.FullName))
                {
                    var path = Path.Combine(enumerateFileSystemInfo.FullName, fileSystemEntry);
                    var fi = ConnectedDevice.GetFileInfo(path);

                    if (fi.FullName.Contains("DCIM"))
                    {




                        foreach (var dir in fi.Directory.EnumerateDirectories())
                        {
                            if (dir.FullName.Contains("DCIM"))
                            {
                                foreach (var enumerateDirectory in dir.EnumerateDirectories())
                                {
                                    if (enumerateDirectory.FullName.Contains("Camera"))
                                    {
                                        var files = enumerateDirectory.EnumerateFiles();
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        public ICommand OpenSourceFolderCommand { get; }
        public ICommand ListMtpDevicesCommand { get; }
        public ICommand ConnectDeviceCommand { get; }
        public ICommand DisconnectDeviceCommand { get; }

        public ObservableCollection<MediaDevice> MtpDevices { get; }
        public ObservableCollection<MediaFileInfo> EntriesList { get; }

        public MediaDevice SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                _selectedDevice = value;
                OnPropertyChanged();
            }
        }

        public MediaDevice ConnectedDevice
        {
            get => _connectedDevice;
            set
            {
                _connectedDevice = value;
                OnPropertyChanged();
            }
        }

        public string SelectedPath
        {
            get => _selectedPath;
            set
            {
                _selectedPath = value;
                OnPropertyChanged();
            }
        }



        private void OnListMtpDevicesCommandExecute(object obj)
        {
            var devices = MediaDevice.GetDevices();

            foreach (var mediaDevice in devices)
            {
                MtpDevices.Add(mediaDevice);
            }
        }

        private void OnOpenSourceFolderCommandExecute(object obj)
        {
            var ofd = new VistaFolderBrowserDialog();
            ofd.ShowDialog();

            SelectedPath = ofd.SelectedPath;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Terminate()
        {
            // disconnect device
            if (ConnectedDevice?.IsConnected == true)
                ConnectedDevice.Disconnect();
        }
    }
}
