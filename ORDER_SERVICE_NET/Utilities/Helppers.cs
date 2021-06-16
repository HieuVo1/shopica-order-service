using ORDER_SERVICE_NET.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.Utilities
{
    public static class Helppers
    {
        public static string GenerateQrString(OrderCreateRequest request)
        {
            var total = request.OrderOneStores.Sum(x => x.Total - x.Discount);
            string qrString = String.Format("CustomerName: {0}\nAddress: {1}\nPhone:{2}\nEmail: {3}\nTotal: {4}\nDate: {5}\nState: {6}\nDetails:\n",
                request.CustomerName,
                request.Address.AddressName,
                request.Phone,
                request.Email,
                total,
                DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"),
                Constant.PENDING);
            foreach (var orderStore in request.OrderOneStores)
            {
                foreach (var item in orderStore.OrderDetails)
                {
                    qrString += String.Format("ProductName: {0}\nQuantity: {1}\nTotal Price: {2}", item.ProductName, item.Quantity, item.TotalPriceProduct);
                }
            }
            return qrString;
        }
    }
}
