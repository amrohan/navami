using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        // get all
        // public ApiResponse<List<SubCategoryDto>> GetSubCategoryMaster()
        // {
        //     try
        //     {
        //         // var subCategoryMaster = dbContext.SubCategoryMasters.Where(u => u.IsActive != true).ToList();
        //         //  join table use linq and return the list of SubCategoryMasters as SubCategoryDto objects use mapper
        //         var subCategoryMaster = (from subCategory in dbContext.SubCategoryMasters
        //                                  join category in dbContext.CategoryMasters on subCategory.CategoryId equals category.CategoryId
        //                                  select new SubCategoryDto
        //                                  {
        //                                      SubCategoryId = subCategory.SubCategoryId,
        //                                      SubCategoryName = subCategory.SubCategoryName,
        //                                      IsActive = subCategory.IsActive,
        //                                      CategoryId = subCategory.CategoryId,
        //                                      CategoryName = category.CategoryName,
        //                                      CreatedBy = subCategory.CreatedBy.ToString(),
        //                                      UpdatedBy = subCategory.UpdatedBy.ToString(),
        //                                      CreatedDate = subCategory.CreatedDate.ToString("dd/MM/yyyy"),
        //                                  }).ToList();


        //         // return the list of SubCategoryMasters as SubCategoryDto objects use mapper
        //         return new ApiResponse<List<SubCategoryDto>>(_mapper.Map<List<SubCategoryDto>>(subCategoryMaster));
        //     }
        //     catch (Exception ex)
        //     {
        //         return new ApiResponse<List<SubCategoryDto>>(ex.Message);
        //     }
        // }
        public async Task<ApiResponse<List<SubCategoryDto>>> GetSubCategoryMasterAsync()
        {
            try
            {
                var subCategoryMasters = await (from subCategory in dbContext.SubCategories
                                                join category in dbContext.SubCategories on subCategory.CategoryId equals category.CategoryId
                                                where !subCategory.IsActive
                                                select new SubCategoryDto
                                                {
                                                    SubCategoryId = subCategory.SubCategoryId,
                                                    SubCategoryName = subCategory.SubCategoryName,
                                                    IsActive = subCategory.IsActive,
                                                    CategoryId = subCategory.CategoryId,
                                                    CategoryName = category.CategoryName,
                                                    CreatedBy = subCategory.CreatedBy,
                                                    CreatedAt = subCategory.CreatedAt,
                                                }).ToListAsync();

                return new ApiResponse<List<SubCategoryDto>>(subCategoryMasters);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine(ex); // Replace with proper logging
                return new ApiResponse<List<SubCategoryDto>>(ex.Message);
            }
        }

        // get by id
        public ApiResponse<SubCategoryDto> GetSubCategoryMasterById(Guid id)
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
        public ApiResponse<List<SubCategoryDto>> GetSubCategoryMasterByCategory(Guid categoryId)
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
        public ApiResponse<SubCategoryDto> DeleteSubCategoryMaster(Guid id)
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