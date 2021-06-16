using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using ORDER_SERVICE_NET.Hubs;
using ORDER_SERVICE_NET.Models;
using ORDER_SERVICE_NET.Services.CartServices;
using ORDER_SERVICE_NET.Services.ProductServices;
using ORDER_SERVICE_NET.Utilities;
using ORDER_SERVICE_NET.ViewModels.Address;
using ORDER_SERVICE_NET.ViewModels.Carts;
using ORDER_SERVICE_NET.ViewModels.Commons;
using ORDER_SERVICE_NET.ViewModels.Commons.Pagging;
using ORDER_SERVICE_NET.ViewModels.OrderDetails;
using ORDER_SERVICE_NET.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly ShopicaContext _context;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IHubContext<NotificationHub> _hubContext;
        public OrderService(
            ShopicaContext context,
            IProductService productService,
            IHubContext<NotificationHub> hubContext,
            ICartService cartService
         )
        {
            _context = context;
            _productService = productService;
            _hubContext = hubContext;
            _cartService = cartService;
        }
        public async Task<APIResult<string>> Create(OrderCreateRequest request)
        {
            try
            {
                var qrString = Helppers.GenerateQrString(request);
                var qrCodeData = await _productService.GetOrderQrCode(qrString);
                string address = JsonConvert.SerializeObject(request.Address);
                List<Orders> listOrders = new List<Orders>();
                var cartItems = new List<UpdateProductDetailQuantityRequest>();

                foreach (var orderStore in request.OrderOneStores)
                {
                    var order = new Orders()
                    {
                        CustomerName = request.CustomerName,
                        Address = address,
                        Email = request.Email,
                        Phone = request.Phone,
                        State = Constant.PENDING,
                        Notes = orderStore.Notes,
                        QrCode = qrCodeData.Message == "OK" ? qrCodeData.Data : null,
                        Total = orderStore.Total,
                        Discount = orderStore.Discount,
                        ShippingCost = orderStore.ShippingCost,
                        PaymentMethod = request.PaymentMethod,
                        TransactionId = request.TransactionId,
                        Created_at = DateTime.Now,
                        StoreId = orderStore.StoreId
                    };

                    foreach (var item in orderStore.OrderDetails)
                    {
                        var orderDetail = new OrderDetail
                        {
                            ProductDetailId = item.ProductDetailId,
                            Quantity = item.Quantity,
                            TotalPriceProduct = item.TotalPriceProduct,
                            Created_at = DateTime.Now,
                        };
                        var cartItem = new UpdateProductDetailQuantityRequest
                        {
                            Quantity = item.Quantity,
                            Price = item.TotalPriceProduct / item.Quantity,
                            ProductDetailID = item.ProductDetailId
                        };

                        cartItems.Add(cartItem);
                        order.OrderDetail.Add(orderDetail);
                    }

                    listOrders.Add(order);

                    _context.Orders.Add(order);

                    if (orderStore.PromotionId != 0)
                    {
                        order.PromotionId = orderStore.PromotionId;

                        var promotion = new CustomerPromo()
                        {
                            Used_at = DateTime.Now,
                            CustomerPhone = request.Phone,
                            PromotionId = orderStore.PromotionId
                        };

                        _context.CustomerPromo.Add(promotion);
                    }
                }

                // update cary
                if (request.AccountId != 0)
                {
                    _cartService.DeleteItems(cartItems, request.AccountId);
                }

                //update product detail quantity

                _productService.UpdateQuantityProductDetail(cartItems);

                await _context.SaveChangesAsync();


                foreach (var item in listOrders)
                {
                    var notify = new Notify()
                    {
                        Content = "You have a new order with orderId - " + item.Id,
                        StoreId = item.StoreId,
                        OrderId = item.Id,
                        Type = "Info",
                        IsRead = 0,
                        Created_at = DateTime.Now
                    };

                    _context.Notify.Add(notify);
                    await _hubContext.Clients.User(item.StoreId.ToString()).SendAsync("NewOrderNotify", notify);
                }

                await _context.SaveChangesAsync();

                return new APIResultSuccess<string>("Order successfully");
            }
            catch (Exception ex)
            {
                return new APIResultErrors<string>(ex.Message);
            }
        }

        public async Task<APIResult<bool>> Delete(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if(order == null)
            {
                return new APIResultErrors<bool>("Not found");
            }
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();

            return new APIResultSuccess<bool>();
        }

        public async Task<APIResult<PaggingView<OrderView>>> GetAll(PaggingRequest request, int storeId, string customerName, string state)
        {
            var listOrder = await _context.Orders.Where(o => o.StoreId == storeId).ToListAsync();

            if(customerName != null)
            {
                listOrder = listOrder.Where(o => o.CustomerName.Contains(customerName)).ToList();
            }

            if (state != null)
            {
                var listState = state.Split(",");
                listOrder = listOrder.Where(o => listState.FirstOrDefault(ls => ls == o.State) !=null).ToList();
            }


            if (request.sortField =="id" && request.sortOrder== Constant.SortDESC)
            {
                listOrder = listOrder.OrderByDescending(o => o.Id).ToList();
            }

            int totalRow = listOrder.Count();

            var data = listOrder
                .Skip(request.pageSize * (request.pageIndex -1))
                .Take(request.pageSize)
                .Select(x => new OrderView()
                {
                    Id = x.Id,
                    CustomerName = x.CustomerName,
                    State = x.State,
                    Total = x.Total,
                    Created_at = x.Created_at,
                    Notes = x.Notes,
                    QrCode = x.QrCode,
                    Discount = x.Discount,
                    ShippingCost = x.ShippingCost
                }).ToList();

            var result = new PaggingView<OrderView>()
            {
                Pageindex = request.pageIndex,
                PageSize = request.pageSize,
                Content = data,
                TotalElements = totalRow
            };

            return new APIResultSuccess<PaggingView<OrderView>>(result);
        }

        public async Task<APIResult<List<OrderView>>> GetAllByUser(string email)
        {
            var query = from p in _context.Orders
                        where p.Email == email
                        select p;

            var data = await query
                .OrderByDescending(o => o.Created_at)
                .Select(x => new OrderView()
                {
                    Id = x.Id,
                    Created_at = x.Created_at,
                    Notes = x.Notes,
                    Email = x.Email,
                    Phone = x.Phone,
                    Address = JsonConvert.DeserializeObject<AddressInfo>(x.Address),
                    CustomerName = x.CustomerName,
                    QrCode = x.QrCode,
                    State = x.State,
                    Total = x.Total,
                    Discount = x.Discount,
                }).ToListAsync();

            return new APIResultSuccess<List<OrderView>>(data);
        }

        public async Task<APIResult<BestSeller>> GetBestSeller(int storeId)
        {
            var listOrder = storeId != 0 ? from od in _context.OrderDetail.Include(o => o.Order)
                                           where od.Order.StoreId == storeId
                                           group od by od.ProductDetailId into r
                                           select new { produceDetailId = r.Key, quanity = r.Sum(x => x.Quantity) }
                                        : from od in _context.OrderDetail.Include(o => o.Order)
                                          group od by od.ProductDetailId into r
                                          select new { produceDetailId = r.Key, quanity = r.Sum(x => x.Quantity) };
            var orderState = from o in _context.Orders
                             group o by o.State into r
                             select new { state = r.Key, quantity = r.Count() };


            var listProductDetail = await listOrder
                .OrderByDescending(x => x.quanity)
                .Select(x => new ProductDetailQuantity()
                {
                    ProductDetailId = x.produceDetailId,
                    Quantity = x.quanity
                }).ToListAsync();


            var listAllState = new List<OrderState>()
            {
                new OrderState() {State = Constant.PENDING, Quantity = 0},
                new OrderState() {State = Constant.DELIVER, Quantity = 0},
                new OrderState() {State = Constant.COMPLETE, Quantity = 0},
                new OrderState() {State = Constant.CANCEL, Quantity = 0},
            };

            var listState = await orderState
                .Select(x => new OrderState()
                {
                    State = x.state,
                    Quantity = x.quantity

                }).ToListAsync();


            listAllState.ForEach(item =>
            {
                var state = listState.FirstOrDefault(x => x.State == item.State);
                item.Quantity = state == null ? 0 : state.Quantity;
            });

            var result = new BestSeller()
            {
                OrderStates = listAllState,
                ProductDetails = listProductDetail

            };

            return new APIResultSuccess<BestSeller>(result);

        }

        public async Task<APIResult<List<OrderView>>> GetByDate(OrderRequestByDate request)
        {
            var listOrder = request.IsAdmin 
                ? from o in _context.Orders.Include(o => o.OrderDetail)
                                              where  o.Created_at.CompareTo(request.FromDate) > 0 && o.Created_at.CompareTo(request.ToDate) < 0
                                              select o
                : from o in _context.Orders.Include(o => o.OrderDetail)
                            where o.StoreId == request.StoreId && o.Created_at.CompareTo(request.FromDate) > 0 && o.Created_at.CompareTo(request.ToDate) < 0
                            select o;

            var data = await listOrder
                .OrderByDescending(x => x.Created_at)
                .Take(request.Top)
                .Select(x => new OrderView()
            {
                Id = x.Id,
                CustomerName = x.CustomerName,
                State = x.State,
                Total = x.Total,
                Created_at = x.Created_at,
                Notes = x.Notes,
                Discount = x.Discount,
                OrderDetails = x.OrderDetail.Select(od =>  new OrderDetailView
                {
                    Id = od.Id,
                    ProductDetailId = od.ProductDetailId,
                    TotalPriceProduct = od.TotalPriceProduct,
                    Quantity =od.Quantity,

                }).ToList(),
                ShippingCost = x.ShippingCost
            }).ToListAsync();

            if(request.SortOrder == Constant.SortASC)
            {
                data = data.OrderBy(x => x.Created_at).ToList();
            }

            return new APIResultSuccess<List<OrderView>>(data);
        }

        public async Task<APIResult<OrderView>> GetById(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if(order == null)
            {
                return new APIResultErrors<OrderView>("Not found");
            }

            var orderView = new OrderView()
            {
                Id = order.Id,
                Created_at = order.Created_at,
                Notes = order.Notes,
                Email = order.Email,
                Phone = order.Phone,
                Address = order.Address!=null? JsonConvert.DeserializeObject<AddressInfo>(order.Address):null,
                CustomerName = order.CustomerName,
                QrCode = order.QrCode,
                State = order.State,
                Total = order.Total,
                Discount = order.Discount,
                ShippingCost = order.ShippingCost
            };

            return new APIResultSuccess<OrderView>(orderView);
        }

        public async Task<APIResult<OrderView>> GetOrderDetails(int orderId)
        {
            var order = await _context.Orders.Include(o => o.OrderDetail)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if(order == null)
            {
                return new APIResultErrors<OrderView>("not found");
            }
            var listProductDetailId = order.OrderDetail.Select(x => x.ProductDetailId).ToList();

            var listProductDetailInfo = await _productService.GetListProduct(listProductDetailId);

            var listData = from q in order.OrderDetail
                           join p in listProductDetailInfo.Data on q.ProductDetailId equals p.ProductDetailId
                           select new { q, p };

            var data = new OrderView()
            {
                Id = order.Id,
                Created_at = order.Created_at,
                Notes = order.Notes,
                Email = order.Email,
                Phone = order.Phone,
                Address = order.Address != null ? JsonConvert.DeserializeObject<AddressInfo>(order.Address) : null,
                CustomerName = order.CustomerName,
                QrCode = order.QrCode,
                State = order.State,
                Total = order.Total,
                Discount = order.Discount,
                ShippingCost = order.ShippingCost
            };

            data.OrderDetails = listData
             .Select(x => new OrderDetailView()
             {
                 ProductName = x.p.ProductName,
                 Image = x.p.Image,
                 Price = x.p.Price,
                 Id = x.q.Id,
                 SizeName = x.p.SizeName,
                 ColorName =x.p.ColorName,
                 ProductId = x.p.ProductId,
                 ProductDetailId = x.p.ProductDetailId,
                 Quantity = x.q.Quantity,
                 TotalPriceProduct = x.q.TotalPriceProduct,
             }).ToList();

            return new APIResultSuccess<OrderView>(data);
        }

        public async Task<APIResult<bool>> UpdateStatus(string state, int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return new APIResultErrors<bool>("Not found");
            }

            order.State = state;
            order.Updated_at = DateTime.Now;

            await _context.SaveChangesAsync();

            return new APIResultSuccess<bool>();
        }
    }
}
