using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SampleStoreApp.Hubs;
using System.Threading.Tasks;

namespace SampleStoreApp.Controllers
{
    public class CallbackController : Controller
    {
        private readonly IHubContext<PaymentHub> _hubcontext;

        public CallbackController(IHubContext<PaymentHub> hubcontext)
        {
            _hubcontext = hubcontext;
        }

        [HttpPost]
        public async Task<ActionResult> Callback(object obj)
        {
            var jsonModel = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                Error = (object sender, ErrorEventArgs args) =>
                {
                    args.ErrorContext.Handled = true;
                }
            });

            await this._hubcontext.Clients.All.SendAsync("ReceiveMessage", "user", "Callback received");

            return Ok();
        }
    }
}