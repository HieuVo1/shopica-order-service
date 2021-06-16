using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ORDER_SERVICE_NET.Services.CartServices;
using ORDER_SERVICE_NET.ViewModels.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("GetCart")]
        public async Task<IActionResult> GetCart()
        {

            var accountId = Convert.ToInt32(HttpContext.User.FindFirstValue("accountId"));

            var result = await _cartService.GetById(accountId);

            if (!result.IsSuccessed) return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("DeleteCartItem")]

        public async Task<IActionResult> DeleteCartItem(CartItemCreateRequest request)
        {
            request.AccountId = Convert.ToInt32(HttpContext.User.FindFirstValue("accountId"));

            var result = await _cartService.DeleteItem(request);

            if (!result.IsSuccessed) return BadRequest(result);

            var cart = await _cartService.GetById(request.AccountId);

            if (!cart.IsSuccessed) return BadRequest(cart);

            return Ok(cart);
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(CartItemCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            request.AccountId = Convert.ToInt32(HttpContext.User.FindFirstValue("accountId"));

            var result = await _cartService.AddToCart(request);

            if (!result.IsSuccessed) return BadRequest(result);

            var cart = await _cartService.GetById(request.AccountId);

            if (!cart.IsSuccessed) return BadRequest(cart);

            return Ok(cart);
        }

        [HttpPost("UpdateCart")]
        public async Task<IActionResult> UpdateCart(CartItemUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            request.AccountId = Convert.ToInt32(HttpContext.User.FindFirstValue("accountId"));

            var result = await _cartService.Update(request);

            if (!result.IsSuccessed) return BadRequest(result);

            var cart = await _cartService.GetById(request.AccountId);

            if (!cart.IsSuccessed) return BadRequest(cart);

            return Ok(cart);
        }


        [HttpPost("ChangeQuantity")]
        public async Task<IActionResult> ChangeQuantity(CartItemCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            request.AccountId = Convert.ToInt32(HttpContext.User.FindFirstValue("accountId"));

            var result = await _cartService.ChangeQuantity(request);

            if (!result.IsSuccessed) return BadRequest(result);

            var cart = await _cartService.GetById(request.AccountId);

            if (!cart.IsSuccessed) return BadRequest(cart);

            return Ok(cart);
        }
    }
}
