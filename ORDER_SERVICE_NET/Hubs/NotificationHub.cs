using Microsoft.AspNetCore.SignalR;
using ORDER_SERVICE_NET.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.Hubs
{
    public class NotificationHub:Hub
    {
        public async Task SendNotify(string storeId, string data)
        {
            await Clients.User(storeId).SendAsync("NewOrderNotify", data);
        }
    }
}
