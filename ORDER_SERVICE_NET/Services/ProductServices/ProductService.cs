using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ORDER_SERVICE_NET.Services.Commons;
using ORDER_SERVICE_NET.ViewModels.Carts;
using ORDER_SERVICE_NET.ViewModels.Commons;
using ORDER_SERVICE_NET.ViewModels.Orders;
using ORDER_SERVICE_NET.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.Services.ProductServices
{
    public class ProductService: IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<APIResult<List<ProductView>>> GetListProduct(List<int> productDetailIdList)
        {
            var response = await _httpClient.PostAsJsonAsyncWithAuth("product/detail", productDetailIdList, _httpContextAccessor);
            if (response.IsSuccessStatusCode)
            {
                using(HttpContent content = response.Content)
                {
                    var data = await content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<APIResultSuccess<List<ProductView>>>(data);
                }
            }
            return JsonConvert.DeserializeObject<APIResultErrors<List<ProductView>>>(null);
        }

        public async Task<APIResult<string>> GetOrderQrCode(string request)
        {
            var response = await _httpClient.PostStringAsyncWithAuth("qr", request, _httpContextAccessor);
            if (response.IsSuccessStatusCode)
            {
                using (HttpContent content = response.Content)
                {
                    var data = await content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<APIResultSuccess<string>>(data);
                }
            }
            return JsonConvert.DeserializeObject<APIResultErrors<string>>(null);
        }

        public async Task<APIResult<string>> UpdateQuantityProductDetail(List<UpdateProductDetailQuantityRequest> request)
        {
            var response = await _httpClient.PostAsJsonAsyncWithAuth("product/update-quantity-product-detail", request, _httpContextAccessor);
            if (response.IsSuccessStatusCode)
            {
                using (HttpContent content = response.Content)
                {
                    var data = await content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<APIResultSuccess<string>>(data);
                }
            }
            return JsonConvert.DeserializeObject<APIResultErrors<string>>(null);
        }
    }
}
