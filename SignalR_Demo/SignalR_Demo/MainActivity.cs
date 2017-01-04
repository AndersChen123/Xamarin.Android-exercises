using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.AspNet.SignalR.Client;
using System.Net.Http;

namespace SignalR_Demo
{
    [Activity(Label = "SignalR_Demo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private IHubProxy _hubProxy;
        private HubConnection _connection;
        private string _serverUrl = "http://192.168.0.102:8080";
        private EditText _message, _userName;
        private TextView _info;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _message = FindViewById<EditText>(Resource.Id.Message);
            _userName = FindViewById<EditText>(Resource.Id.UserName);
            _info = FindViewById<TextView>(Resource.Id.Info);

            var connectButton = FindViewById<Button>(Resource.Id.Connect);
            connectButton.Click += ConnectButton_Click;

            var sendButton = FindViewById<Button>(Resource.Id.Send);
            sendButton.Click += SendButton_Click;
        }

        private void SendButton_Click(object sender, System.EventArgs e)
        {
            // 调用服务端的 Send
            _hubProxy.Invoke("Send", _userName.Text, _message.Text);

            _message.Text = "";
        }

        private void ConnectButton_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_userName.Text))
            {
                _userName.RequestFocus();
                Toast.MakeText(this, "请输入用户名。", ToastLength.Short);
                return;
            }

            _userName.Enabled = false;

            _connection = new HubConnection(_serverUrl, true);
            _connection.Closed += Connection_Closed;
            _hubProxy = _connection.CreateHubProxy("HubDemo");
            // 匿名方法 Send
            _hubProxy.On<string, string>("Send", (n, s) =>
            {
                Application.SynchronizationContext.Post(_ =>
                {
                    AppendText(n + ": " + s);
                }, null);
            });

            try
            {
                _connection.Start().Wait();
            }
            catch (HttpRequestException ex)
            {
                AppendText(ex.Message);
            }

            AppendText("已连接到：" + _serverUrl + "\r");
        }

        private void Connection_Closed()
        {
            AppendText("连接已断开。\r");
        }

        private void AppendText(string txt)
        {
            _info.Text += txt + "\n";
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_connection == null) return;

            _connection.Stop();
            _connection.Dispose();
        }
    }
}

