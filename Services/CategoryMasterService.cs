using AutoMapper;
using Microsoft.EntityFrameworkCore;
using navami.Dto;
using navami.Models;

namespace navami
{

    public class CategoryMasterService
    {
        private readonly NavamiContext dbContext;
        private readonly IMapper _mapper;
        public CategoryMasterService(NavamiContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }


        // get all

        // public ApiResponse<List<CategoryMasterDto>> GetCategoryMaster()
        // {
        //     try
        //     {
        //         var categoryMaster = dbContext.CategoryMasters.Where(u => u.IsActive != true).ToList();
        //         // return the list of CategoryMasters as CategoryMasterDto objects use mapper
        //         return new ApiResponse<List<CategoryMasterDto>>(_mapper.Map<List<CategoryMasterDto>>(categoryMaster));
        //     }
        //     catch (Exception ex)
        //     {
        //         return new ApiResponse<List<CategoryMasterDto>>(ex.Message);
        //     }
        // }

        public async Task<ApiResponse<List<CategoryMasterDto>>> GetCategoryMasterAsync()
        {
            try
            {
                var categoryMasters = await dbContext.CategoryMasters
                    .Where(u => !u.IsActive)
                    .ToListAsync();
                // Map the list of CategoryMasters to CategoryMasterDto objects
                var categoryMasterDtos = _mapper.Map<List<CategoryMasterDto>>(categoryMasters);

                return new ApiResponse<List<CategoryMasterDto>>(categoryMasterDtos);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine(ex); // Replace with proper logging
                return new ApiResponse<List<CategoryMasterDto>>(ex.Message);
            }
        }

        // get by id
        public ApiResponse<CategoryMasterDto> GetCategoryMasterById(int id)
        {
            try
            {
                var categoryMaster = dbContext.CategoryMasters.FirstOrDefault(u => u.CategoryId == id);
                if (categoryMaster == null)
                {
                    return new ApiResponse<CategoryMasterDto>("CategoryMaster not found");
                }
                return new ApiResponse<CategoryMasterDto>(_mapper.Map<CategoryMasterDto>(categoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryMasterDto>(ex.Message);
            }
        }

        // add
        public ApiResponse<CategoryMasterDto> AddCategoryMaster(CategoryMasterDto model)
        {
            try
            {
                var categoryMaster = _mapper.Map<CategoryMaster>(model);
                dbContext.CategoryMasters.Add(categoryMaster);
                dbContext.SaveChanges();
                return new ApiResponse<CategoryMasterDto>(_mapper.Map<CategoryMasterDto>(categoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryMasterDto>(ex.Message);
            }
        }

        // update
        public ApiResponse<CategoryMasterDto> UpdateCategoryMaster(CategoryMasterDto model)
        {
            try
            {
                var categoryMaster = dbContext.CategoryMasters.FirstOrDefault(u => u.CategoryId == model.CategoryId);
                if (categoryMaster == null)
                {
                    return new ApiResponse<CategoryMasterDto>("CategoryMaster not found");
                }
                categoryMaster.CategoryName = model.CategoryName;
                categoryMaster.IsActive = model.IsActive;
                dbContext.SaveChanges();
                return new ApiResponse<CategoryMasterDto>(_mapper.Map<CategoryMasterDto>(categoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryMasterDto>(ex.Message);
            }
        }

        // delete
        public ApiResponse<CategoryMasterDto> DeleteCategoryMaster(int id)
        {
            try
            {
                var categoryMaster = dbContext.CategoryMasters.FirstOrDefault(u => u.CategoryId == id);
                if (categoryMaster == null)
                {
                    return new ApiResponse<CategoryMasterDto>("CategoryMaster not found");
                }
                categoryMaster.IsActive = true;
                dbContext.SaveChanges();
                return new ApiResponse<CategoryMasterDto>(_mapper.Map<CategoryMasterDto>(categoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryMasterDto>(ex.Message);
            }
        }




    }

}