using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ORDER_SERVICE_NET.Services.OrderServices;
using ORDER_SERVICE_NET.ViewModels.Commons.Pagging;
using ORDER_SERVICE_NET.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(OrderCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            request.AccountId = Convert.ToInt32(HttpContext.User.FindFirstValue("accountId"));

            var result = await _orderService.Create(request);

            if(!result.IsSuccessed) return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] PaggingRequest request, string customerName, string state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storeId = Convert.ToInt32(HttpContext.User.FindFirstValue("storeId"));

            var result = await _orderService.GetAll(request, storeId, customerName, state);

            if (!result.IsSuccessed) return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("GetByDate")]
        [Authorize]
        public async Task<IActionResult> GetByDate([FromQuery] OrderRequestByDate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storeId = Convert.ToInt32(HttpContext.User.FindFirstValue("storeId"));

            request.IsAdmin = HttpContext.User.FindFirstValue(ClaimTypes.Role) == "Admin";

            request.StoreId = storeId;

            var result = await _orderService.GetByDate(request);

            if (!result.IsSuccessed) return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("GetAllByUser/{email}")]
        [Authorize]
        public async Task<IActionResult> GetAllByUser(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.GetAllByUser(email);

            if (!result.IsSuccessed) return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("GetById/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetById(int orderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.GetById(orderId);

            if (!result.IsSuccessed) return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("GetOrderDetails/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.GetOrderDetails(orderId);

            if(!result.IsSuccessed) return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("UpdateState/{orderId}")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int orderId,string state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.UpdateStatus(state, orderId);

            if (!result.IsSuccessed) return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("GetBestSeller")]
        public async Task<IActionResult> GetBestSeller(int storeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.GetBestSeller(storeId);

            if (!result.IsSuccessed) return BadRequest(result);

            return Ok(result);
        }
    }
}
