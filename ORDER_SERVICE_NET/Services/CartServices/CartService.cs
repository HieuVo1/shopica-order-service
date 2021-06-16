using Microsoft.EntityFrameworkCore;
using ORDER_SERVICE_NET.Models;
using ORDER_SERVICE_NET.Services.ProductServices;
using ORDER_SERVICE_NET.ViewModels.Carts;
using ORDER_SERVICE_NET.ViewModels.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly ShopicaContext _context;
        private readonly IProductService _productService;
        public CartService(ShopicaContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }
        public async Task<APIResult<bool>> AddToCart(CartItemCreateRequest request)
        {
            var cart = await _context.Carts
                .Include(c => c.CartDetail)
                .FirstOrDefaultAsync(c => c.AccountId == request.AccountId);

            if (cart == null)
            {

                var newCart = new Carts()
                {
                    AccountId = request.AccountId,
                    CartDetail = new List<CartDetail>()
                    {
                        new CartDetail()
                        {
                            ProductDetailId = request.ProductDetailID,
                            Quantity = request.Quantity
                        }
                    },
                    Total = request.Quantity * request.Price 
                };

                _context.Carts.Add(newCart);

                await _context.SaveChangesAsync();

                return new APIResultSuccess<bool>();
            }

            cart.Total += request.Quantity * request.Price;

            var cartDetail = cart.CartDetail
                .FirstOrDefault(c => c.ProductDetailId == request.ProductDetailID);

            if(cartDetail == null)
            {
                var newCartDetail = new CartDetail()
                {
                    ProductDetailId = request.ProductDetailID,
                    Quantity = request.Quantity,
                    CartId = cart.Id,
                };

                cart.CartDetail.Add(newCartDetail);

                await _context.SaveChangesAsync();

                return new APIResultSuccess<bool>();

            }

            cartDetail.Quantity += request.Quantity;

            await _context.SaveChangesAsync();

            return new APIResultSuccess<bool>();
        }

        public Task<APIResult<bool>> Delete(int cartId)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResult<bool>> DeleteAll(int accountId)
        {
            var cart = await _context.Carts
               .Include(c => c.CartDetail)
               .FirstOrDefaultAsync(c => c.AccountId == accountId);

            if(cart == null)
            {
                return new APIResultErrors<bool>("Can not found this cart!");
            }

            foreach(var item in cart.CartDetail)
            {
                _context.CartDetail.Remove(item);
            }

            cart.Total = 0;

            await _context.SaveChangesAsync();

            return new APIResultSuccess<bool>();
        }

        public async Task<APIResult<bool>> DeleteItem(CartItemCreateRequest request)
        {
            var cart = await _context.Carts
                .Include(c => c.CartDetail)
                .FirstOrDefaultAsync(c => c.AccountId == request.AccountId);
            if (cart != null)
            {
                var cartDetail = cart.CartDetail.FirstOrDefault(c => c.ProductDetailId == request.ProductDetailID);

                if (cartDetail != null)
                {
                    cart.Total -= cartDetail.Quantity * request.Price;

                    _context.CartDetail.Remove(cartDetail);

                    await _context.SaveChangesAsync();

                    return new APIResultSuccess<bool>();
                }

                return new APIResultErrors<bool>("Not found item");
            }
            return new APIResultErrors<bool>("Can not find this cart");

        }

        public async Task<APIResult<CartView>> GetById(int accountId)
        {
            var carts = await _context.Carts
                .Include(x => x.CartDetail)
                .FirstOrDefaultAsync(x => x.AccountId == accountId);

            if(carts == null)
            {
                return new APIResultErrors<CartView>("Can not find this cart");
            }

            var listProductDetailId = carts.CartDetail.Select(x => x.ProductDetailId).ToList();

            var listProduct = await _productService.GetListProduct(listProductDetailId);

            var listProductDetail = from cd in carts.CartDetail
                         join l in listProduct.Data on cd.ProductDetailId equals l.ProductDetailId
                         select new { cd , l };

            var cartItemView = listProductDetail.Select(x => new CartItemView()
            {
                Quantity = x.cd.Quantity,
                Price = x.l.Price,
                ProductName = x.l.ProductName,
                ColorName = x.l.ColorName,
                SizeName = x.l.SizeName,
                BrandName = x.l.BrandName,
                ProductDetailId = x.l.ProductDetailId,
                StoreId = x.l.StoreId,
                Available = x.l.Quantity,
                Image = x.l.Image,
                ProductId = x.l.ProductId
            }).ToList();

            var result = new CartView()
            {
                Id = carts.Id,
                Total = cartItemView.Where(x => x.Quantity <= x.Available).Sum(x => x.Price * x.Quantity),
                CartItems = cartItemView
            };

            return new APIResultSuccess<CartView>(result);

        }

        public async Task<APIResult<bool>> Update(CartItemUpdateRequest request)
        {
            var cart = await _context.Carts
                  .Include(c => c.CartDetail)
                  .FirstOrDefaultAsync(c => c.AccountId == request.AccountId);

            if (cart == null)
            {
                return new APIResultErrors<bool>("Can not find this cart!");
            }

            var cartDetail = cart.CartDetail
                .FirstOrDefault(c => c.ProductDetailId == request.OldProductDetailID);

            var newCartDetail = cart.CartDetail
                .FirstOrDefault(c => c.ProductDetailId == request.NewProductDetailID);

            if (cartDetail == null)
            {
                return new APIResultErrors<bool>("This cart item is not exist!");
            }

            cart.Total += (request.Quantity * request.Price - cartDetail.Quantity* request.Price);

            if(newCartDetail != null && request.OldProductDetailID != request.NewProductDetailID)    
            {
                newCartDetail.Quantity += request.Quantity;
                _context.CartDetail.Remove(cartDetail);
                await _context.SaveChangesAsync();

                return new APIResultSuccess<bool>();
            }

            cartDetail.Quantity = request.Quantity;

            cartDetail.ProductDetailId = request.NewProductDetailID;

            await _context.SaveChangesAsync();

            return new APIResultSuccess<bool>();
        }

        public async Task<APIResult<bool>> ChangeQuantity(CartItemCreateRequest request)
        {
            var cart = await _context.Carts
                .Include(c => c.CartDetail)
                .FirstOrDefaultAsync(c => c.AccountId == request.AccountId);
            if (cart != null)
            {
                var cartDetail = cart.CartDetail.FirstOrDefault(cd => cd.ProductDetailId == request.ProductDetailID);

                if (cartDetail != null)
                {
                    cart.Total -= (cartDetail.Quantity - request.Quantity) * request.Price;
                    cartDetail.Quantity = request.Quantity;
                    await _context.SaveChangesAsync();
                    return new APIResultSuccess<bool>();
                }
                return new APIResultErrors<bool>();
            }
            return new APIResultErrors<bool>();
        }

        public async Task<APIResult<bool>> DeleteItems(List<UpdateProductDetailQuantityRequest> cartItemRequests, int accountId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartDetail)
                .FirstOrDefaultAsync(c => c.AccountId == accountId);

            if (cart != null)
            {

                foreach (var cartItemRequest in cartItemRequests)
                {
                    var cartDetail = cart.CartDetail.FirstOrDefault(c => c.ProductDetailId == cartItemRequest.ProductDetailID);

                    if (cartDetail != null)
                    {
                        cart.Total -= cartDetail.Quantity * cartItemRequest.Price;

                        _context.CartDetail.Remove(cartDetail);
                    }

                }

                await _context.SaveChangesAsync();

                return new APIResultSuccess<bool>();
            }
            return new APIResultErrors<bool>("Can not find this cart");
        }
    }
}
