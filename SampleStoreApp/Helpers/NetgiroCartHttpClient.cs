using Newtonsoft.Json;
using SampleStoreApp.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SampleStoreApp.Helpers
{
    public class NetgiroCartHttpClient : INetgiroCart
    {
        private string _apiURL;
        private string _secretKey;
        private string _applicationId;

        public NetgiroCartHttpClient(string apiURL, string secretKey, string applicationId)
        {
            _apiURL = apiURL;
            _secretKey = secretKey;
            _applicationId = applicationId;
        }

        public string InsertCart(InsertCartModel insertCartModel)
        {
            Task<string> insertCartTask = Task.Run(() => InsertCartAsync(insertCartModel));
            insertCartTask.Wait();

            return insertCartTask.Result;
        }

        public async Task<string> InsertCartAsync(InsertCartModel insertCartModel)
        {
            HttpRequestMessage httpRequestMessage = GenerateHttpRequestMessage(insertCartModel, "/Checkout/InsertCart");
            return await DoPost(httpRequestMessage);
        }

        public string CheckCart(string transactionId)
        {
            Task<string> checkCartTask = Task.Run(() => CheckCartAsync(transactionId));
            checkCartTask.Wait();

            return checkCartTask.Result;
        }

        public async Task<string> CheckCartAsync(string transactionId)
        {
            HttpRequestMessage httpRequestMessage = GenerateHttpRequestMessage(new CheckCartRequest { TransactionId = transactionId }, "/Checkout/CheckCart");
            return await DoPost(httpRequestMessage);
        }

        private HttpRequestMessage GenerateHttpRequestMessage(object model, string apiAction, string nonce = "nonce")
        {
            string url = _apiURL.TrimEnd('/') + apiAction;
            string jsonModel = JsonConvert.SerializeObject(model);
            string signature = Signature.CalculateSignature(_secretKey, nonce + url + jsonModel);

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequestMessage.Headers.Add(Constants.Netgiro_AppKey, _applicationId);
            httpRequestMessage.Headers.Add(Constants.Netgiro_Signature, signature);
            httpRequestMessage.Headers.Add(Constants.Netgiro_Nonce, nonce);
            httpRequestMessage.Content = new StringContent(jsonModel, Encoding.UTF8, "application/json");

            return httpRequestMessage;
        }

        private async Task<string> DoPost(HttpRequestMessage httpRequestMessage)
        {
            HttpResponseMessage httpResponseMessage = await (new HttpClient()).SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await httpResponseMessage.Content.ReadAsStringAsync();
            }

            throw new Exception(await httpResponseMessage.Content.ReadAsStringAsync());
        }
    }
}