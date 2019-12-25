using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace ClientMessanger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Task.Run(() => SendToServer());
        }

        private void SendToServer()
        {
            Task.Run(() => SendToServerMethod());
        }
        private Task SendToServerMethod()
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var localIp = IPAddress.Parse(textBoxIpInput.Text);
                var port = int.Parse(textBoxPortInput.Text);
                var endPoint = new IPEndPoint(localIp, port);
                socket.Bind(endPoint);
                socket.Listen(5);
                while (true)
                {
                    var incomingSocket = socket.Accept(); 
                    while (incomingSocket.Available > 0)
                    {
                        var buffer = new byte[incomingSocket.Available];
                        incomingSocket.Receive(buffer);
                        Console.WriteLine(System.Text.Encoding.UTF8.GetString(buffer));
                    }
                    incomingSocket.Close();
                }
            }
        }

        private void Send(object sender, RoutedEventArgs e)
        {
            Task.Run(() => SendMethodAsync());
        }

        private async Task SendMethodAsync()
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var localIp = IPAddress.Parse("");
                var port = 3231;
                var endpoint = new IPEndPoint(localIp, port);
                socket.Connect(endpoint);
                await socket.ConnectAsync(endpoint);
                var buffer = System.Text.Encoding.UTF8.GetBytes(textBoxMessage.Text);
                socket.Send(buffer);
                socket.Shutdown(SocketShutdown.Both);
            }
        }
    }
}
