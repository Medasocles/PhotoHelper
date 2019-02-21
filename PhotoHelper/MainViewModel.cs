using MediaDevices;
using Ookii.Dialogs.Wpf;
using PhotoHelper.Core.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PhotoHelper
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string _selectedPath;

        public MainViewModel()
        {
            MtpDevices = new ObservableCollection<string>();

            OpenSourceFolderCommand = new RelayCommand<object>(OnOpenSourceFolderCommandExecute);
            ListMtpDevicesCommand = new RelayCommand<object>(OnListMtpDevicesCommandExecute);
        }

        public ICommand OpenSourceFolderCommand { get; }
        public ICommand ListMtpDevicesCommand { get; }

        public string SelectedPath
        {
            get => _selectedPath;
            set
            {
                _selectedPath = value; 
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> MtpDevices { get; }

        private void OnListMtpDevicesCommandExecute(object obj)
        {
            var devices = MediaDevice.GetDevices();
            foreach (var mediaDevice in devices)
            {
                if (mediaDevice.FriendlyName.Contains("G4") && mediaDevice.Manufacturer.Contains("LGE"))
                {
                    mediaDevice.Connect();

                    var functionalCategories = mediaDevice.FunctionalCategories();
                    foreach (var functionalCategory in functionalCategories)
                    {
                        var functionalObjects = mediaDevice.FunctionalObjects(functionalCategory);
                    }

                    var fuid = mediaDevice.FunctionalUniqueId;
                    var root= mediaDevice.GetRootDirectory();
                    foreach (var enumerateFileSystemInfo in root.EnumerateFileSystemInfos())
                    {
                        foreach (var fileSystemEntry in mediaDevice.GetFileSystemEntries(enumerateFileSystemInfo.FullName))
                        {
                            var fi = mediaDevice.GetFileInfo(Path.Combine(enumerateFileSystemInfo.FullName,
                                fileSystemEntry));
                        }
                    }

                    mediaDevice.Disconnect();
                }

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
    }
}
