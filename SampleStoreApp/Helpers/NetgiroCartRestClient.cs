using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using SampleStoreApp.Models;

namespace SampleStoreApp.Helpers
{
    public class NetgiroCartRestClient : INetgiroCart
    {
        private string _apiURL;
        private string _secretKey;
        private string _applicationId;

        public NetgiroCartRestClient(string apiURL, string secretKey, string applicationId)
        {
            _apiURL = apiURL;
            _secretKey = secretKey;
            _applicationId = applicationId;
        }

        public string InsertCart(InsertCartModel insertCartModel)
        {
            var client = new RestClient(_apiURL);
            var request = new RestRequest("Checkout/InsertCart", Method.POST, DataFormat.Json);

            var nonce = "nonce";
            var url = client.BaseUrl + request.Resource;
            var jsonModel = JsonConvert.SerializeObject(insertCartModel, new JsonSerializerSettings
            {
                Error = (object sender, ErrorEventArgs args) =>
                {
                    args.ErrorContext.Handled = true;
                }
            });

            var signature = Signature.CalculateSignature(_secretKey, nonce + url + jsonModel);

            request.AddJsonBody(insertCartModel);
            request.AddHeader(Constants.Netgiro_AppKey, _applicationId);
            request.AddHeader(Constants.Netgiro_Signature, signature);
            request.AddHeader(Constants.Netgiro_Nonce, nonce);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }

            throw response.ErrorException;
        }

        public string CheckCart(string transactionId)
        {
            var client = new RestClient(_apiURL);
            var request = new RestRequest("Checkout/CheckCart", Method.POST, DataFormat.Json);

            var nonce = "nonce";
            var url = client.BaseUrl + request.Resource;
            var model = new CheckCartRequest
            {
                TransactionId = transactionId
            };
            var jsonModel = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                Error = (object sender, ErrorEventArgs args) =>
                {
                    args.ErrorContext.Handled = true;
                }
            });

            var signature = Signature.CalculateSignature(_secretKey, nonce + url + jsonModel);

            request.AddJsonBody(model);
            request.AddHeader(Constants.Netgiro_AppKey, _applicationId);
            request.AddHeader(Constants.Netgiro_Signature, signature);
            request.AddHeader(Constants.Netgiro_Nonce, nonce);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }

            throw response.ErrorException;
        }
    }
}
