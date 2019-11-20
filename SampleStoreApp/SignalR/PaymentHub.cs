using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SampleStoreApp.SignalR
{
    public class PaymentHub : Hub
    {
        private readonly IClientManager _clientManager;

        public PaymentHub(IClientManager clientManager)
        {
            _clientManager = clientManager;
        }

        public void RegisterTransactionId(string transactionId)
        {
            _clientManager.InsertNewClient(transactionId, Context.ConnectionId);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
