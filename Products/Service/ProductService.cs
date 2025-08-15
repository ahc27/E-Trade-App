using AutoMapper;
using Products.Data;
using Products.Data.Repositories;
using classLib.LogDtos;
using classLib.ProductDtos;
using Products.Infrastructures.Messaging;

namespace Products.Service
{

        public class ProductService : IProductService
        {
            private readonly IMapper _mapper;
            private readonly ProductRepository _ProductRepository;
            private readonly RabbitMqProducer _rabbitMqProducer;

        public ProductService(ProductRepository ProductRepository, IMapper mapper,RabbitMqProducer rabbitMqProducer)
            {
                _ProductRepository = ProductRepository;
                _mapper = mapper;
                _rabbitMqProducer = rabbitMqProducer;
            }

            public async Task<IEnumerable<ProductDto>> GetAllAsync()
            {
                var Products = await _ProductRepository.GetAll();
                return _mapper.Map<IEnumerable<ProductDto>>(Products);
            }


            public async Task<ProductDto> GetByIdAsync(int id)
            {

                return _mapper.Map<ProductDto>(await _ProductRepository.GetById(id));
            }


            public async Task<Data.Product> AddAsync(ProductDto ProductDto)
            {

            var ProductEntity = _mapper.Map<Product>(ProductDto);

            if (ProductEntity == null) return null;

            var addedProduct = await _ProductRepository.Add(ProductEntity);

            if (addedProduct == null)
            {
                await LogProduct(null, false, "Add Product", "Product could not be added.", null);
                return null;
            }
            
            await LogProduct(addedProduct.Id.ToString(), true, "Add Product", "Product added successfully.", null);
            return addedProduct;
            }


            public async Task<bool> UpdateAsync(int id, ProductDto ProductDto)
            {
                var existingProduct = await _ProductRepository.GetById(id);
                if (existingProduct == null)
                {
                    return false;
                }
                _mapper.Map(ProductDto, existingProduct);

            if (existingProduct == null) return false ;

            var updatedProduct = await _ProductRepository.Update(existingProduct);

            if (updatedProduct == null)
            {
                await LogProduct(id.ToString(), false, "Update Product", "Product could not be updated.", null);
                return false;
            }
                
            await LogProduct(id.ToString(), true, "Update Product", "Product updated successfully.", null);
            return true;
            }


            public async Task<bool> DeleteAsync(int id)
            {
                return await _ProductRepository.Delete(id);
            }

        public async Task<bool> LogProduct(string? entityId, bool success, string action, string message, Exception? exception)
        {
            try
            {

                var ProductLog = new Log
                {
                    IsSuccess = success,
                    Action = action,
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    EntityId = entityId,
                    ServiceName = "Product",
                    Level = success ? "Information" : "Error",
                    Exception = exception
                };

                await _rabbitMqProducer.SendLogAsync(ProductLog);
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
