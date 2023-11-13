using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services.Implementations
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            var response = await _client.GetAsync("/Catalog");
            return await response.ReadContentAs<IEnumerable<CatalogModel>>();
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            var response = await _client.GetAsync($"/Catalog/{id}");
            return await response.ReadContentAs<CatalogModel>();
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            var response = await _client.GetAsync($"/Catalog/GetProductByCategory/{category}");
            return await response.ReadContentAs<IEnumerable<CatalogModel>>();
        }

        public async Task<CatalogModel> CreateCatalog(CatalogModel model)
        {
            var response = await _client.PostAsJson("/Catalog", model);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Something went wrong when calling api");

            return await response.ReadContentAs<CatalogModel>();
        }
    }
}
