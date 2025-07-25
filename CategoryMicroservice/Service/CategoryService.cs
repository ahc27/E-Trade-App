using AutoMapper;
using CategoryMicroservice.Data;
using CategoryMicroservice.Data.Repositories;
using CategoryMicroservice.Service.Dtos;

namespace CategoryMicroservice.Service
{

        public class CategoryService : ICategoryService
        {
            private readonly IMapper _mapper;
            private readonly CategoryRepository _categoryRepository;

            public CategoryService(CategoryRepository categoryRepository, IMapper mapper)
            {
                _categoryRepository = categoryRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<CreateCategoryDto>> GetAllAsync()
            {
                var categorys = await _categoryRepository.GetAll();
                return _mapper.Map<IEnumerable<CreateCategoryDto>>(categorys);
            }


            public async Task<CreateCategoryDto> GetByIdAsync(int id)
            {

                return _mapper.Map<CreateCategoryDto>(await _categoryRepository.GetById(id));
            }


            public async Task<Category> AddAsync(CreateCategoryDto categoryDto)
            {

            var categoryEntity = _mapper.Map<Category>(categoryDto);

            if(categoryEntity != null) categoryEntity.ParentCategoryId = await _categoryRepository.GetByName(categoryDto.ParentCategoryName);


            var addedCategory = await _categoryRepository.Add(categoryEntity);
                return addedCategory;
            }


            public async Task<bool> UpdateAsync(int id, UpdateCategoryDto categoryDto)
            {
                var existingCategory = await _categoryRepository.GetById(id);
                if (existingCategory == null)
                {
                    return false;
                }
                _mapper.Map(categoryDto, existingCategory); // direkt olarak mevcut entity'ye map'le

            if (existingCategory != null) existingCategory.ParentCategoryId = await _categoryRepository.GetByName(categoryDto.ParentCategoryName);

            return await _categoryRepository.Update(existingCategory);
            }


            public async Task<bool> DeleteAsync(int id)
            {
                return await _categoryRepository.Delete(id);
            }

    }
    
}
