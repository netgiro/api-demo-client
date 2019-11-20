using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NetgiroClient.Models;
using SampleStoreApp.SignalR;
using System.Threading.Tasks;

namespace SampleStoreApp.Controllers
{
    public class CallbackController : Controller
    {
        private readonly IHubContext<PaymentHub> _hubcontext;
        private readonly IClientManager _clientManager;

        public CallbackController(IHubContext<PaymentHub> hubcontext, IClientManager clientManager)
        {
            _hubcontext = hubcontext;
            _clientManager = clientManager;
        }

        [HttpPost]
        public async Task<ActionResult> Callback([FromBody] PaymentResponse paymentResponse)
        {
            await this._hubcontext.Clients.Client(_clientManager.GetClientByTransactionId(paymentResponse.PaymentInfo.TransactionId)).SendAsync("ReceiveMessage", "user", "Callback received");

            return Ok();
        }
    }
}