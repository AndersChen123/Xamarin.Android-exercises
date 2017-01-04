//
// http://designbased.net
//
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Server
{
    public class HubDemo : Hub
    {
        private IMessageRepository _repository;

        public HubDemo(IMessageRepository repository)
        {
            _repository = repository;
        }

        public void Send(string name, string message)
        {
            _repository.Log(name, message);

            Clients.All.Send(name, message);
        }

        public override Task OnConnected()
        {
            _repository.Log(Context.ConnectionId, "已连接。");

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _repository.Log(Context.ConnectionId, "已断开。");

            return base.OnDisconnected(stopCalled);
        }
    }
}