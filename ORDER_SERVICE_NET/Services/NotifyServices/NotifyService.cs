using Microsoft.EntityFrameworkCore;
using ORDER_SERVICE_NET.Models;
using ORDER_SERVICE_NET.ViewModels.Commons;
using ORDER_SERVICE_NET.ViewModels.Commons.Pagging;
using ORDER_SERVICE_NET.ViewModels.Notifys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.Services.NotifyServices
{
    public class NotifyService : INotifyService
    {
        private readonly ShopicaContext _context;

        public NotifyService(ShopicaContext context)
        {
            _context = context;
        }

        public async Task<APIResult<bool>> UpdateNumUnRead(int storeId)
        {
            var listNotify = await _context.Notify.Where(x => x.StoreId == storeId).ToListAsync();

            listNotify.ForEach(x => {
                    x.IsRead = 1;
                    x.Updated_at = DateTime.Now;
                });

            await _context.SaveChangesAsync();

            return new APIResultSuccess<bool>();
        }

        public async Task<APIResult<PaggingView<NotifyView>>> GetByStoreId(PaggingRequest request, int storeId)
        {
            var listNotify = await _context.Notify.Where(x=> x.StoreId == storeId).ToListAsync();

            int totalRow = listNotify.Count();

            if (request.sortOrder == "descend" && request.sortField =="id")
            {
                listNotify = listNotify.OrderByDescending(r => r.Id).ToList();
            }

            var data = listNotify
                .Skip(request.pageSize * (request.pageIndex - 1))
                .Take(request.pageSize)
                .Select(x => new NotifyView()
                {
                    Id = x.Id,
                    Content = x.Content,
                    Created_at = x.Created_at,
                    Type = x.Type,
                    StoreId = x.StoreId,
                    IsRead = x.IsRead,
                    OrderId = x.OrderId
                }).ToList();

            var result = new PaggingView<NotifyView>()
            {
                Pageindex = request.pageIndex,
                PageSize = request.pageSize,
                Content = data,
                TotalElements = totalRow
            };

            return new APIResultSuccess<PaggingView<NotifyView>>(result);
        }
    }
}
