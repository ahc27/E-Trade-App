using CategoryMicroservice.Data;
using CategoryMicroservice.Service.Dtos;

namespace CategoryMicroservice.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<CreateCategoryDto>> GetAllAsync();
        Task<CreateCategoryDto> GetByIdAsync(int id);
        Task<Category> AddAsync(CreateCategoryDto user);
        Task<bool> UpdateAsync(int id, UpdateCategoryDto user);
        Task<bool> DeleteAsync(int id);
    }
}
