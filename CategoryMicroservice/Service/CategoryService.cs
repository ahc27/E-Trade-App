using AutoMapper;
using CategoryMicroservice.Data;
using CategoryMicroservice.Data.Repositories;
using CategoryMicroservice.Infrastructures.Messaging;
using CategoryMicroservice.Service.Dtos;
using classLib.LogDtos;
using classLib;

namespace CategoryMicroservice.Service
{

        public class CategoryService : ICategoryService
        {
            private readonly IMapper _mapper;
            private readonly CategoryRepository _categoryRepository;
            private readonly RabbitMqProducer _rabbitMqProducer;

        public CategoryService(CategoryRepository categoryRepository, IMapper mapper,RabbitMqProducer rabbitMqProducer)
            {
                _categoryRepository = categoryRepository;
                _mapper = mapper;
                _rabbitMqProducer = rabbitMqProducer;
            }

            public async Task<IEnumerable<GetCategoryDto>> GetAllAsync()
            {
                var categorys = await _categoryRepository.GetAll();
                return _mapper.Map<IEnumerable<GetCategoryDto>>(categorys);
            }


            public async Task<GetCategoryDto> GetByIdAsync(int id)
            {

                return _mapper.Map<GetCategoryDto>(await _categoryRepository.GetById(id));
            }


            public async Task<Category> AddAsync(CreateCategoryDto categoryDto)
            {

            var categoryEntity = _mapper.Map<Category>(categoryDto);

            if (categoryEntity != null) categoryEntity.ParentCategoryId = await _categoryRepository.GetByName(categoryDto.ParentCategoryName);

            var addedCategory = await _categoryRepository.Add(categoryEntity);

            if (addedCategory == null)
            {
                await LogCategory(null, false, "Add Category", "Category could not be added.", null);
                return null;
            }
            
            await LogCategory(addedCategory.Id.ToString(), true, "Add Category", "Category added successfully.", null);
            return addedCategory;
            }


            public async Task<bool> UpdateAsync(int id, UpdateCategoryDto categoryDto)
            {
                var existingCategory = await _categoryRepository.GetById(id);
                if (existingCategory == null)
                {
                    return false;
                }
                _mapper.Map(categoryDto, existingCategory);

            if (existingCategory != null) existingCategory.ParentCategoryId = await _categoryRepository.GetByName(categoryDto.ParentCategoryName);

            var updatedCategory = await _categoryRepository.Update(existingCategory);

            if (updatedCategory == null)
            {
                await LogCategory(id.ToString(), false, "Update Category", "Category could not be updated.", null);
                return false;
            }
                
            await LogCategory(id.ToString(), true, "Update Category", "Category updated successfully.", null);
            return true;
            }


            public async Task<bool> DeleteAsync(int id)
            {
                return await _categoryRepository.Delete(id);
            }

        public async Task<bool> LogCategory(string? entityId, bool success, string action, string message, Exception? exception)
        {
            try
            {

                var CategoryLog = new Log
                {
                    IsSuccess = success,
                    Action = action,
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    EntityId = entityId,
                    ServiceName = "Category",
                    Level = success ? "Information" : "Error",
                    Exception = exception
                };

                await _rabbitMqProducer.SendLogAsync(CategoryLog);
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
