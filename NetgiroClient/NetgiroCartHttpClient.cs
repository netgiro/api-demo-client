using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NetgiroClient.Helpers;
using NetgiroClient.Models;
using Newtonsoft.Json;

namespace NetgiroClient
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
            Task<string> insertCartTask = InsertCartAsync(insertCartModel);
            insertCartTask.Wait();

            return insertCartTask.Result;
        }

        public async Task<string> InsertCartAsync(InsertCartModel insertCartModel)
        {
            HttpRequestMessage httpRequestMessage = GenerateHttpRequestMessage(insertCartModel, Constants.Netgiro_Api_InsertCartURL, RandomString.Generate());
            return await DoPost(httpRequestMessage);
        }

        public string CheckCart(string transactionId)
        {
            Task<string> checkCartTask = CheckCartAsync(transactionId);
            checkCartTask.Wait();

            return checkCartTask.Result;
        }

        public async Task<string> CheckCartAsync(string transactionId)
        {
            HttpRequestMessage httpRequestMessage = GenerateHttpRequestMessage(new CheckCartRequestModel { TransactionId = transactionId }, Constants.Netgiro_Api_CheckCartURL, RandomString.Generate());
            return await DoPost(httpRequestMessage);
        }

        public async Task<bool> CancelCartAsync(string transactionId)
        {
            HttpRequestMessage httpRequestMessage = GenerateHttpRequestMessage(new CheckCartRequestModel { TransactionId = transactionId }, Constants.Netgiro_Api_CancelCartURL, RandomString.Generate());
            return await DoPostBool(httpRequestMessage);
        }

        private HttpRequestMessage GenerateHttpRequestMessage(object model, string apiAction, string nonce)
        {
            // we don't know if an URL in config has a slash at the end of it, so we remove it from URL, and always include it at the start of path
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

        private async Task<bool> DoPostBool(HttpRequestMessage httpRequestMessage)
        {
            HttpResponseMessage httpResponseMessage = await (new HttpClient()).SendAsync(httpRequestMessage);

            return httpResponseMessage.IsSuccessStatusCode;
        }
    }
}