using ORDER_SERVICE_NET.ViewModels.Carts;
using ORDER_SERVICE_NET.ViewModels.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.Services.CartServices
{
    public interface ICartService
    {
        Task<APIResult<CartView>> GetById(int customerId);
        Task<APIResult<bool>> Update(CartItemUpdateRequest request);
        Task<APIResult<bool>> Delete(int cartId);
        Task<APIResult<bool>> AddToCart(CartItemCreateRequest request);
        Task<APIResult<bool>> ChangeQuantity(CartItemCreateRequest request);
        Task<APIResult<bool>> DeleteItem(CartItemCreateRequest request);
        Task<APIResult<bool>> DeleteAll(int accountId);
        Task<APIResult<bool>> DeleteItems(List<UpdateProductDetailQuantityRequest> request, int accountId);
    }
}
