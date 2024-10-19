using AutoMapper;
using Microsoft.EntityFrameworkCore;
using navami.Components.Pages.Category;
using navami.Dto;
using navami.Models;

namespace navami
{
    public class SubCategoryMasterService
    {
        private readonly NavamiDevContext dbContext;
        private readonly IMapper _mapper;
        public SubCategoryMasterService(NavamiDevContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }


        public async Task<ApiResponse<List<SubCategoryDto>>> GetSubCategoryMasterAsync()
        {
            try
            {
                var subCategoryMasters = await dbContext.SubCategories.Include(i=>i.Category)
                    .Where(u => !u.IsActive)
                    .Select(subCategory => new SubCategoryDto
                    {
                        SubCategoryId = subCategory.SubCategoryId,
                        SubCategoryName = subCategory.SubCategoryName,
                        IsActive = subCategory.IsActive,
                        CategoryId = subCategory.CategoryId,
                        CategoryName = subCategory.Category.CategoryName,
                        CreatedBy = subCategory.CreatedBy,
                        CreatedAt = subCategory.CreatedAt,
                        UpdatedAt = subCategory.UpdatedAt,
                    }).ToListAsync();

                return new ApiResponse<List<SubCategoryDto>>(subCategoryMasters);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex); // Replace with proper logging
                return new ApiResponse<List<SubCategoryDto>>(ex.Message);
            }
        }

        // get by id
        public ApiResponse<SubCategoryDto> GetSubCategoryMasterById(int id)
        {
            try
            {
                var subCategoryMaster = (from subCategory in dbContext.SubCategories
                                         join category in dbContext.Categories on subCategory.CategoryId equals category.CategoryId
                                         where subCategory.SubCategoryId == id
                                         select new SubCategoryDto
                                         {
                                             SubCategoryId = subCategory.SubCategoryId,
                                             SubCategoryName = subCategory.SubCategoryName,
                                             IsActive = subCategory.IsActive,
                                             CategoryId = subCategory.CategoryId,
                                             CategoryName = category.CategoryName,
                                             CreatedBy = subCategory.CreatedBy,
                                             CreatedAt = subCategory.CreatedAt,
                                             UpdatedAt = subCategory.UpdatedAt,
                                         }).FirstOrDefault();

                if (subCategoryMaster == null)
                {
                    return new ApiResponse<SubCategoryDto>("SubCategoryMaster not found");
                }
                return new ApiResponse<SubCategoryDto>(_mapper.Map<SubCategoryDto>(subCategoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<SubCategoryDto>(ex.Message);
            }
        }

        // GetSubCategoryMasterByCategory
        public ApiResponse<List<SubCategoryDto>> GetSubCategoryMasterByCategory(int categoryId)
        {
            try
            {
                var subCategoryMaster = (from subCategory in dbContext.SubCategories
                                         join category in dbContext.Categories on subCategory.CategoryId equals category.CategoryId
                                         where subCategory.CategoryId == categoryId
                                         select new SubCategoryDto
                                         {
                                             SubCategoryId = subCategory.SubCategoryId,
                                             SubCategoryName = subCategory.SubCategoryName,
                                             IsActive = subCategory.IsActive,
                                             CategoryId = subCategory.CategoryId,
                                             CategoryName = category.CategoryName,
                                             CreatedBy = subCategory.CreatedBy,
                                             CreatedAt = subCategory.CreatedAt,
                                             UpdatedAt = subCategory.UpdatedAt,
                                         }).ToList();

                return new ApiResponse<List<SubCategoryDto>>(_mapper.Map<List<SubCategoryDto>>(subCategoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<SubCategoryDto>>(ex.Message);
            }
        }

        // add
        public ApiResponse<SubCategoryDto> AddSubCategoryMaster(SubCategoryDto model)
        {
            try
            {
                var subCategoryMaster = _mapper.Map<SubCategory>(model);
                dbContext.SubCategories.Add(subCategoryMaster);
                dbContext.SaveChanges();
                return new ApiResponse<SubCategoryDto>(_mapper.Map<SubCategoryDto>(subCategoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<SubCategoryDto>(ex.Message);
            }
        }

        // update

        public ApiResponse<SubCategoryDto> UpdateSubCategoryMaster(SubCategoryDto model)
        {
            try
            {
                var subCategoryMaster = dbContext.SubCategories.FirstOrDefault(u => u.SubCategoryId == model.SubCategoryId);
                if (subCategoryMaster == null)
                {
                    return new ApiResponse<SubCategoryDto>("SubCategoryMaster not found");
                }
                subCategoryMaster.SubCategoryName = model.SubCategoryName;
                subCategoryMaster.CategoryId = model.CategoryId;
                subCategoryMaster.CategoryName = model.CategoryName;
                subCategoryMaster.IsActive = model.IsActive;
                subCategoryMaster.UpdatedAt = DateTime.Now;
                dbContext.SaveChanges();
                return new ApiResponse<SubCategoryDto>(_mapper.Map<SubCategoryDto>(subCategoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<SubCategoryDto>(ex.Message);
            }
        }

        // delete
        public ApiResponse<SubCategoryDto> DeleteSubCategoryMaster(int id)
        {
            try
            {
                var subCategoryMaster = dbContext.SubCategories.FirstOrDefault(u => u.SubCategoryId == id);
                if (subCategoryMaster == null)
                {
                    return new ApiResponse<SubCategoryDto>("SubCategoryMaster not found");
                }
                subCategoryMaster.IsActive = true;
                dbContext.SaveChanges();
                return new ApiResponse<SubCategoryDto>(_mapper.Map<SubCategoryDto>(subCategoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<SubCategoryDto>(ex.Message);
            }
        }
    }
}