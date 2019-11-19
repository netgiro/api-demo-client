using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
        public async Task<ActionResult> Callback([FromBody] DummyClass data)
        {
            await this._hubcontext.Clients.Client(_clientManager.GetClientByTransactionId(data.TransactionId)).SendAsync("ReceiveMessage", "user", "Callback received");

            return Ok();
        }
    }

    public class DummyClass
    {
        public string TransactionId { get; set; }
    }
}