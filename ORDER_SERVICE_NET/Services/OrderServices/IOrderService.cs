using ORDER_SERVICE_NET.ViewModels.Commons;
using ORDER_SERVICE_NET.ViewModels.Commons.Pagging;
using ORDER_SERVICE_NET.ViewModels.OrderDetails;
using ORDER_SERVICE_NET.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.Services.OrderServices
{
    public interface IOrderService
    {
        Task<APIResult<OrderView>> GetOrderDetails(int orderId);
        Task<APIResult<OrderView>> GetById(int orderId);
        Task<APIResult<List<OrderView>>> GetAllByUser(string email);
        Task<APIResult<PaggingView<OrderView>>> GetAll(PaggingRequest request,int storeId, string customerName, string state);
        Task<APIResult<List<OrderView>>> GetByDate(OrderRequestByDate request);
        Task<APIResult<string>> Create(OrderCreateRequest request);
        Task<APIResult<bool>> UpdateStatus(string state, int orderId);
        Task<APIResult<bool>> Delete(int orderId);
        Task<APIResult<BestSeller>> GetBestSeller(int storeId);

    }
}
