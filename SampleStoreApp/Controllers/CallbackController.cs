using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using NetgiroClient;
using NetgiroClient.Helpers;
using NetgiroClient.Models;
using Newtonsoft.Json;
using SampleStoreApp.SignalR;

namespace SampleStoreApp.Controllers
{
    public class CallbackController : Controller
    {
        private readonly IHubContext<PaymentHub> _hubcontext;
        private readonly IClientManager _clientManager;
        private readonly AppSettings _appSettings;

        public CallbackController(IHubContext<PaymentHub> hubcontext, IClientManager clientManager, IOptions<AppSettings> appSettings)
        {
            _hubcontext = hubcontext;
            _clientManager = clientManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        public async Task<ActionResult> Callback([FromBody] PaymentResponse paymentResponse)
        {
            var signature = this.Request.Headers[Constants.Netgiro_Signature];
            var nonce = this.Request.Headers[Constants.Netgiro_Nonce];
            var transactionId = paymentResponse.PaymentInfo.TransactionId;
            var jsonModel = JsonConvert.SerializeObject(paymentResponse);
            var callbackUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}{Url.Action(nameof(CallbackController.Callback), "Callback")}";

            var calculatedSignature = Signature.CalculateSignature(_appSettings.SecretKey, nonce, callbackUrl, jsonModel);

            if (signature != calculatedSignature)
            {
                return BadRequest();
            }

            await this._hubcontext.Clients.Client(_clientManager.GetClientByTransactionId(transactionId)).SendAsync("ReceiveMessage", "user", "Callback received");

            return Ok();
        }
    }
}