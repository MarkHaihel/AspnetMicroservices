﻿using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;

        public BasketService(HttpClient client)
        {
            _client = client;
        }

        public async Task<BasketModel> GetBasket(string userName)
        {
            var response = await _client.GetAsync($"/Basket/{userName}");
            return await response.ReadContentAs<BasketModel>();
        }

        public async Task<BasketModel> UpdateBasket(BasketModel model)
        {
            var response = await _client.PostAsJson("/Basket", model);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Something went wrong when calling api");

            return await response.ReadContentAs<BasketModel>();
        }

        public async Task CheckoutBasket(BasketCheckoutModel model)
        {
            var response = await _client.PostAsJson("/Basket/Checkout", model);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Something went wrong when calling api");
        }
    }
}
