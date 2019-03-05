using System.Net;
using pc_remote_server.Server;

namespace pc_remote_server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var endpoint = new IPEndPoint(IPAddress.Any, 8005);
            var server = new RemoteServer(endpoint);
        }
    }
}