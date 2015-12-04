using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using System.Xml.Linq;
using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;
using Microsoft.Owin.Hosting;
using NAudio.Wave;

namespace Sampler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private readonly UsbListener _listener;

        private bool _communicationEstablished = false;

        private Configuration _config;

        private int _lastKeyCode = -1;

        private ICommand _taskBarClickCommand;
        private ObservableCollection<DirectSoundDeviceInfo> _devices;
        private DirectSoundDeviceInfo _device;

        private IDisposable _webServer;

        public ICommand TaskbarClickCommand
        {
            get
            {
                return (_taskBarClickCommand ?? (_taskBarClickCommand = new TaskbarClickCommand()));
            }
        }

        

        public UsbListener UsbListener
        {
            get
            {
                return _listener;
            }
        }

        public int LastKeyCode
        {
            get
            {
                return _lastKeyCode;
            }
            private set
            {
                if (_lastKeyCode != value)
                {
                    _lastKeyCode = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<DirectSoundDeviceInfo> Devices
        {
            get
            {
                return _devices;
            }
            private set
            {
                if (_devices != value)
                {
                    _devices = value;
                    OnPropertyChanged();
                }
            }
        }

        public DirectSoundDeviceInfo CurrentDevice
        {
            get
            {
                return _device;
            }
            private set
            {
                if (_device != value)
                {
                    _device = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainWindow()
        {
            _listener = new UsbListener();
            InitializeComponent();

            string baseAddress = "http://+:9000/";

            // Start OWIN host 
            _webServer = WebApp.Start<Startup>(url: baseAddress);
            SoundController.SoundRequested += SoundControllerSoundRequested;

            Loaded += MainWindow_Loaded;
            Application.Current.Exit += OnApplicationExit;
        }

        void SoundControllerSoundRequested(object sender, int e)
        {
            Player sound = _config.GetSound(e);
            if (sound != null)
            {
                if (!sound.IsLooping)
                {
                    Dispatcher.Invoke(() => sound.Play(false));
                }
                else
                {
                    Dispatcher.Invoke(() => sound.Stop());
                }
            }
        }

        void OnApplicationExit(object sender, ExitEventArgs e)
        {
            StopListener();
            if (_webServer != null)
            {
                _webServer.Dispose();
            }
        }

        void OnUsbKeyDown(object sender, KeyDownEventArgs e)
        {
            LastKeyCode = e.KeyCode;
            Player sound = _config.GetSound(LastKeyCode);
            if (sound != null)
            {
                if (!sound.IsLooping)
                {
                    Dispatcher.Invoke(() => sound.Play(e.IsShiftPressed));
                }
                else
                {
                    Dispatcher.Invoke(() => sound.Stop());
                }
            }
            
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Devices = new ObservableCollection<DirectSoundDeviceInfo>(DirectSoundOut.Devices);
            this.PropertyChanged += MainWindow_PropertyChanged;
            LoadConfiguration();
            ResetListener();
        }

        void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentDevice")
            {
                foreach (var player in _config.GetPlayers())
                {
                    player.SetDevice(CurrentDevice.Guid);
                }
            }
        }

        private void ResetListener()
        {
            StopListener();
            _listener.KeyDown += OnUsbKeyDown;
            _listener.Start();
        }

        private void StopListener()
        {
            _listener.KeyDown -= OnUsbKeyDown;
            _listener.Stop();
        }

        private void LoadConfiguration()
        {
            _config = Configuration.Parse(XDocument.Load("Configuration.xml").Root.Elements().First());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            ResetListener();
        }
    }
}
