//
// http://designbased.net
//
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDisposable _signalrServer;
        private string _serverUrl = "http://192.168.0.102:8080";

        public MainWindow()
        {
            InitializeComponent();

            UxStop.IsEnabled = false;
        }
                
        private void UxStart_Click(object sender, RoutedEventArgs e)
        {
            StartServer();

            UxStart.IsEnabled = false;
        }

        private void StartServer()
        {
            try
            {
                _signalrServer = WebApp.Start(_serverUrl);

                LogMessage("服务已启动：" + _serverUrl + "\r");
                UxStop.IsEnabled = true;
            }
            catch (TargetInvocationException ex)
            {
                LogMessage(ex.Message);
            }
        }

        private void UxStop_Click(object sender, RoutedEventArgs e)
        {
            _signalrServer.Dispose();
            Close();
        }

        private void UxSend_Click(object sender, RoutedEventArgs e)
        {
            LogMessage("Server: " + UxMessage.Text + "\r");

            var context = GlobalHost.ConnectionManager.GetHubContext<HubDemo>();
            context.Clients.All.send("Server: ", UxMessage.Text);

            UxMessage.Text = "";
        }

        public void LogMessage(string message)
        {
            if (UxInfo.CheckAccess())
            {
                UxInfo.AppendText(message + "\r");
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    UxInfo.AppendText(message + "\r");
                });
            }
        }
    }

    public class MessageRepository : IMessageRepository
    {
        public void Log(string name, string message)
        {
            Application.Current.Dispatcher.Invoke(() => ((MainWindow)Application.Current.MainWindow).LogMessage(name + ": " + message));
        }
    }
}