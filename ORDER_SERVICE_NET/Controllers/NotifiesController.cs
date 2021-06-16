using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ORDER_SERVICE_NET.Services.NotifyServices;
using ORDER_SERVICE_NET.ViewModels.Commons.Pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifiesController : ControllerBase
    {
        private readonly INotifyService _notifyService;
        public NotifiesController(INotifyService notifyService)
        {
            _notifyService = notifyService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaggingRequest request, int storeId)
        {
            var result = await _notifyService.GetByStoreId(request, storeId);

            if (!result.IsSuccessed) return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("UpdateNumUnRead")]
        public async Task<IActionResult> UpdateNumUnRead(int storeId)
        {
            var result = await _notifyService.UpdateNumUnRead(storeId);

            if (!result.IsSuccessed) return BadRequest(result);

            return Ok(result);
        }
    }
}
