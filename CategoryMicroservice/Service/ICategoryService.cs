using CategoryMicroservice.Data;
using CategoryMicroservice.Service.Dtos;
using classLib;

namespace CategoryMicroservice.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<GetCategoryDto>> GetAllAsync();
        Task<GetCategoryDto> GetByIdAsync(int id);
        Task<Category> AddAsync(CreateCategoryDto user);
        Task<bool> UpdateAsync(int id, UpdateCategoryDto user);
        Task<bool> DeleteAsync(int id);
    }
}
