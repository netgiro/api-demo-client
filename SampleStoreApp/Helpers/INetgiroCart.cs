using SampleStoreApp.Models;

namespace SampleStoreApp.Helpers
{
    public interface INetgiroCart
    {
        public string InsertCart(InsertCartModel insertCartModel);
        public string CheckCart(string transactionId);
    }
}