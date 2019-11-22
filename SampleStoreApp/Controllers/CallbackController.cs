using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NetgiroClient;
using NetgiroClient.Helpers;
using NetgiroClient.Models;
using SampleStoreApp.SignalR;

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
            var appKey = this.Request.Headers[Constants.Netgiro_AppKey];
            var signature = this.Request.Headers[Constants.Netgiro_Signature];
            var nonce = this.Request.Headers[Constants.Netgiro_Nonce];
            var transactionId = paymentResponse.PaymentInfo.TransactionId;
            var calculatedSignature = Signature.CalculateSignature(appKey, transactionId, nonce);

            if (signature != calculatedSignature)
            {
                return Unauthorized("Invalid signature");
            }

            await this._hubcontext.Clients.Client(_clientManager.GetClientByTransactionId(transactionId)).SendAsync("ReceiveMessage", "user", "Callback received");

            return Ok();
        }
    }
}