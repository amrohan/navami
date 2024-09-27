using AutoMapper;
using Microsoft.EntityFrameworkCore;
using navami.Dto;
using navami.Models;

namespace navami
{

    public class CategoryMasterService
    {
        private readonly NavamiDevContext dbContext;
        private readonly IMapper _mapper;
        public CategoryMasterService(NavamiDevContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }


        // get all

        // public ApiResponse<List<CategoryDto>> GetCategoryMaster()
        // {
        //     try
        //     {
        //         var categoryMaster = dbContext.CategoryMasters.Where(u => u.IsActive != true).ToList();
        //         // return the list of CategoryMasters as CategoryDto objects use mapper
        //         return new ApiResponse<List<CategoryDto>>(_mapper.Map<List<CategoryDto>>(categoryMaster));
        //     }
        //     catch (Exception ex)
        //     {
        //         return new ApiResponse<List<CategoryDto>>(ex.Message);
        //     }
        // }

        public async Task<ApiResponse<List<CategoryDto>>> GetCategoryMasterAsync()
        {
            try
            {
                var categoryMasters = await dbContext.Categories
                    .Where(u => !u.IsActive)
                    .ToListAsync();
                // Map the list of CategoryMasters to CategoryDto objects
                var CategoryDtos = _mapper.Map<List<CategoryDto>>(categoryMasters);

                return new ApiResponse<List<CategoryDto>>(CategoryDtos);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine(ex); // Replace with proper logging
                return new ApiResponse<List<CategoryDto>>(ex.Message);
            }
        }

        // get by id
        public ApiResponse<CategoryDto> GetCategoryMasterById(int id)
        {
            try
            {
                var categoryMaster = dbContext.Categories.FirstOrDefault(u => u.CategoryId == id);
                if (categoryMaster == null)
                {
                    return new ApiResponse<CategoryDto>("CategoryMaster not found");
                }
                return new ApiResponse<CategoryDto>(_mapper.Map<CategoryDto>(categoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryDto>(ex.Message);
            }
        }

        // add
        public ApiResponse<CategoryDto> AddCategoryMaster(CategoryDto model)
        {
            try
            {
                var categoryMaster = _mapper.Map<Category>(model);
                dbContext.Categories.Add(categoryMaster);
                dbContext.SaveChanges();
                return new ApiResponse<CategoryDto>(_mapper.Map<CategoryDto>(categoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryDto>(ex.Message);
            }
        }

        // update
        public ApiResponse<CategoryDto> UpdateCategoryMaster(CategoryDto model)
        {
            try
            {
                var categoryMaster = dbContext.Categories.FirstOrDefault(u => u.CategoryId == model.CategoryId);
                if (categoryMaster == null)
                {
                    return new ApiResponse<CategoryDto>("CategoryMaster not found");
                }
                categoryMaster.CategoryName = model.CategoryName;
                categoryMaster.IsActive = model.IsActive;
                dbContext.SaveChanges();
                return new ApiResponse<CategoryDto>(_mapper.Map<CategoryDto>(categoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryDto>(ex.Message);
            }
        }

        // delete
        public ApiResponse<CategoryDto> DeleteCategoryMaster(int id)
        {
            try
            {
                var categoryMaster = dbContext.Categories.FirstOrDefault(u => u.CategoryId == id);
                if (categoryMaster == null)
                {
                    return new ApiResponse<CategoryDto>("CategoryMaster not found");
                }
                categoryMaster.IsActive = true;
                dbContext.SaveChanges();
                return new ApiResponse<CategoryDto>(_mapper.Map<CategoryDto>(categoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryDto>(ex.Message);
            }
        }




    }

}