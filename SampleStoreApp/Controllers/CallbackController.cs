using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult> Callback()
        {
            var signature = this.Request.Headers[Constants.Netgiro_Signature];
            var nonce = this.Request.Headers[Constants.Netgiro_Nonce];
            var json = await ReadBody(HttpContext);

            var paymentResponseObj = JsonConvert.DeserializeObject<PaymentResponse>(json);
            var transactionId = paymentResponseObj.PaymentInfo.TransactionId;

            var callbackUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}{Url.Action(nameof(CallbackController.Callback), "Callback")}";

            var calculatedSignature = Signature.CalculateSignature(_appSettings.SecretKey, nonce, callbackUrl, json);

            if (signature != calculatedSignature)
            {
                return BadRequest();
            }

            await this._hubcontext.Clients.Client(_clientManager.GetClientByTransactionId(transactionId)).SendAsync("ReceiveMessage", "user", "Callback received");

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> CheckoutCallback([FromBody] CheckoutCallbackModel model)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> CheckoutCallback()
        {
            return Ok();
        }

        // https://devblogs.microsoft.com/aspnet/re-reading-asp-net-core-request-bodies-with-enablebuffering/
        public async Task<string> ReadBody(HttpContext httpContext)
        {
            var result = string.Empty;
            var request = httpContext.Request;

            request.EnableBuffering();

            var stream = request.Body;
            using (var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, 1024, leaveOpen: true))
            {
                var requestBodyAsString = await reader.ReadToEndAsync();

                stream.Seek(0, SeekOrigin.Begin);

                result = requestBodyAsString;
            }

            return result;
        }
    }
}