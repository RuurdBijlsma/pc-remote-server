using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using pc_remote_server.Server;
using pc_remote_server.Server.Mouse;
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

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system!");
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
                Text = "IP: "+GetLocalIPAddress(),
                Visible = true
            };
            nIcon.DoubleClick += (sender, args) => Debug.WriteLine("Double clicked icon");
        }
    }
}