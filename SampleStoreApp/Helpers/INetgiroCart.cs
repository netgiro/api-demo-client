﻿using SampleStoreApp.Models;

namespace SampleStoreApp.Helpers
{
    public interface INetgiroCart
    {
        /// <summary>
        /// This method has to be called when clicking the Buy/Checkout button in your system
        /// </summary>
        /// <param name="insertCartModel"></param>
        /// <returns>JSON data</returns>
        public string InsertCart(InsertCartModel insertCartModel);

        /// <summary>
        /// After successfull response from InsertCart method this method checks cart status (has the user accepted or rejected the request)
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns>JSON data</returns>
        public string CheckCart(string transactionId);
    }
}