using AutoMapper;
using Carts.Data;
using Carts.Data.Repositories;
using classLib.ProductDtos;
using classLib.LogDtos;
using classLib.CartDtos;
using Carts.Infrastructures.Messaging;
using System.Text.Json;

namespace Carts.Service
{

        public class CartService : ICartService
        {
            private readonly IMapper _mapper;
            private readonly CartRepository _CartRepository;
            private readonly CartItemRepository _CartItemRepository;
            private readonly RabbitMqProducer _rabbitMqProducer;
            private readonly HttpClient _httpClient;


        public CartService(CartRepository CartRepository,CartItemRepository cartItemRepository, IMapper mapper,RabbitMqProducer rabbitMqProducer,IHttpClientFactory httpClient)
            {
                _CartRepository = CartRepository;
                _CartItemRepository = cartItemRepository;
                _mapper = mapper;
                _rabbitMqProducer = rabbitMqProducer;
                _httpClient = httpClient.CreateClient();
            }

            public async Task<IEnumerable<Cart>> GetAllAsync()
            {
                var Carts = await _CartRepository.GetAll();
                return _mapper.Map<IEnumerable<Cart>>(Carts);
            }


            public async Task<Cart> GetByIdAsync(int id)
            {

                return await _CartRepository.GetById(id);
            }

            public async Task<Cart> GetByUserIdAsync(int userId)
            {
                return await _CartRepository.GetByUserID(userId);
            }

        public async Task<Cart> AddAsync(CartDto CartDto)
            {

            var CartEntity = _mapper.Map<Cart>(CartDto);

            if (CartEntity == null) return null;

            var addedCart = await _CartRepository.Add(CartEntity);

            if (addedCart == null)
            {
                await LogCart(null, false, "Add Cart", "Cart could not be added.", null);
                return null;
            }
            
            await LogCart(addedCart.Id.ToString(), true, "Add Cart", "Cart added successfully.", null);
            return addedCart;
            }

            
            public async Task<Cart> AddItemToCartAsync(CartDto cartDto)
            {
            if (cartDto == null || cartDto.userId <= 0 || cartDto.productId <= 0 || cartDto.quantity <= 0)
            {
                await LogCart(null, false, "Add Item to Cart", "Invalid CartDto provided.", null);
                return null;
            }
            var userCart =await _CartRepository.GetByUserID(cartDto.userId);

            if(userCart ==null) userCart =await AddAsync(cartDto);
            if(userCart ==null) return null;


            var product = await GetProductAsync(cartDto.productId);
            if(product== null)
            {
                await LogCart(null, false, "Add Item to Cart", "Product not found.", null);
                return null;
            }
            var cartItem =  _mapper.Map<CartItem>(product);
            cartItem.quantity = cartDto.quantity;
            cartItem.cartId= userCart.Id;


            var itemInList = userCart.Items.Find(x => x.productId == cartItem.productId);

            if (itemInList == null) userCart.Items.Add(cartItem);
            else
            {
                itemInList.quantity += cartItem.quantity;
                await _CartItemRepository.Update(itemInList);
            }
                userCart.updatedAt = DateTime.UtcNow;
            await _CartRepository.Update(userCart);

            return userCart;
            }


            public async Task<bool> UpdateAsync(int id, CartDto CartDto)
            {
                var existingCart = await _CartRepository.GetById(id);
                if (existingCart == null)
                {
                    return false;
                }
                _mapper.Map(CartDto, existingCart);

            if (existingCart == null) return false ;

            var updatedCart = await _CartRepository.Update(existingCart);

            if (updatedCart == null)
            {
                await LogCart(id.ToString(), false, "Update Cart", "Cart could not be updated.", null);
                return false;
            }
                
            await LogCart(id.ToString(), true, "Update Cart", "Cart updated successfully.", null);
            return true;
            }


            public async Task<bool> DeleteAsync(int id)
            {
                return await _CartRepository.Delete(id);
            }

        private async Task<ProductDto> GetProductAsync(int productId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://product_api:8080/api/Products/{productId}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return  JsonSerializer.Deserialize<ProductDto>(content);
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> LogCart(string? entityId, bool success, string action, string message, Exception? exception)
        {
            try
            {

                var CartLog = new Log
                {
                    IsSuccess = success,
                    Action = action,
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    EntityId = entityId,
                    ServiceName = "Cart",
                    Level = success ? "Information" : "Error",
                    Exception = exception
                };

                await _rabbitMqProducer.SendLogAsync(CartLog);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging failed: {ex.Message}");
                return false;
            }
        }
    }
    
}
