using ORDER_SERVICE_NET.ViewModels.Commons;
using ORDER_SERVICE_NET.ViewModels.Commons.Pagging;
using ORDER_SERVICE_NET.ViewModels.Notifys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.Services.NotifyServices
{
    public interface INotifyService
    {
        Task<APIResult<PaggingView<NotifyView>>> GetByStoreId(PaggingRequest request,int storeId);
        Task<APIResult<bool>> UpdateNumUnRead(int storeId);
    }                                                         
}
