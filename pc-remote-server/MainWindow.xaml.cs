using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using pc_remote_server.Server;
using Application = System.Windows.Application;

namespace pc_remote_server
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var endpoint = new IPEndPoint(IPAddress.Any, 8005);
            var server = new RemoteServer(endpoint);

            ShowInTaskbar = false;
            Visibility = Visibility.Hidden;
            Icon = new BitmapImage(new Uri("res/remoteicon.ico", UriKind.Relative));

            CreateTrayIcon();
            SetAutoStart(false);
        }

        private void SetAutoStart(bool autoStart = true)
        {
            var registryKey =
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            var name = Assembly.GetEntryAssembly().GetName().Name;

            if (registryKey == null) return;
            if (autoStart)
            {
                Debug.WriteLine("Adding to startup programs");
                registryKey.SetValue(name, Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                Debug.WriteLine("Removing from startup programs");
                if (registryKey.GetValue(name) != null)
                    registryKey.DeleteValue(name);
            }
        }

        private void CreateTrayIcon()
        {
            IContainer components = new Container();
            var contextMenu1 = new ContextMenu();
            var menuItem1 = new MenuItem
            {
                Index = 0,
                Text = "Exit PcRemote"
            };
            menuItem1.Click += (sender, args) => Application.Current.Shutdown();

            contextMenu1.MenuItems.AddRange(
                new[] {menuItem1});


            var nIcon = new NotifyIcon(components)
            {
                Icon = new Icon("res/remoteicon.ico"),
                ContextMenu = contextMenu1,
                Text = "PcRemote Server",
                Visible = true
            };
            nIcon.DoubleClick += (sender, args) => Debug.WriteLine("Double clicked icon");
        }
    }
}