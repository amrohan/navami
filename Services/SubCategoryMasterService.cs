using AutoMapper;
using navami.Dto;
using navami.Models;

namespace navami
{
    public class SubCategoryMasterService
    {
        private readonly NavamiContext dbContext;
        private readonly IMapper _mapper;
        public SubCategoryMasterService(NavamiContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }

        // get all
        public ApiResponse<List<SubCategoryMasterDto>> GetSubCategoryMaster()
        {
            try
            {
                // var subCategoryMaster = dbContext.SubCategoryMasters.Where(u => u.IsActive != true).ToList();
                //  join table use linq and return the list of SubCategoryMasters as SubCategoryMasterDto objects use mapper
                var subCategoryMaster = (from subCategory in dbContext.SubCategoryMasters
                                         join category in dbContext.CategoryMasters on subCategory.CategoryId equals category.CategoryId
                                         select new SubCategoryMasterDto
                                         {
                                             SubCategoryId = subCategory.SubCategoryId,
                                             SubCategoryName = subCategory.SubCategoryName,
                                             IsActive = subCategory.IsActive,
                                             CategoryId = subCategory.CategoryId,
                                             CategoryName = category.CategoryName,
                                             CreatedBy = subCategory.CreatedBy.ToString(),
                                             UpdatedBy = subCategory.UpdatedBy.ToString(),
                                             CreatedDate = subCategory.CreatedDate.ToString("dd/MM/yyyy"),
                                         }).ToList();


                // return the list of SubCategoryMasters as SubCategoryMasterDto objects use mapper
                return new ApiResponse<List<SubCategoryMasterDto>>(_mapper.Map<List<SubCategoryMasterDto>>(subCategoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<SubCategoryMasterDto>>(ex.Message);
            }
        }

        // get by id
        public ApiResponse<SubCategoryMasterDto> GetSubCategoryMasterById(int id)
        {
            try
            {
                var subCategoryMaster = (from subCategory in dbContext.SubCategoryMasters
                                         join category in dbContext.CategoryMasters on subCategory.CategoryId equals category.CategoryId
                                         where subCategory.SubCategoryId == id
                                         select new SubCategoryMasterDto
                                         {
                                             SubCategoryId = subCategory.SubCategoryId,
                                             SubCategoryName = subCategory.SubCategoryName,
                                             IsActive = subCategory.IsActive,
                                             CategoryId = subCategory.CategoryId,
                                             CategoryName = category.CategoryName,
                                             CreatedBy = subCategory.CreatedBy.ToString(),
                                             UpdatedBy = subCategory.UpdatedBy.ToString(),
                                             CreatedDate = subCategory.CreatedDate.ToString("dd/MM/yyyy"),
                                         }).FirstOrDefault();

                if (subCategoryMaster == null)
                {
                    return new ApiResponse<SubCategoryMasterDto>("SubCategoryMaster not found");
                }
                return new ApiResponse<SubCategoryMasterDto>(_mapper.Map<SubCategoryMasterDto>(subCategoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<SubCategoryMasterDto>(ex.Message);
            }
        }

        // add
        public ApiResponse<SubCategoryMasterDto> AddSubCategoryMaster(SubCategoryMasterDto model)
        {
            try
            {
                var subCategoryMaster = _mapper.Map<SubCategoryMaster>(model);
                dbContext.SubCategoryMasters.Add(subCategoryMaster);
                dbContext.SaveChanges();
                return new ApiResponse<SubCategoryMasterDto>(_mapper.Map<SubCategoryMasterDto>(subCategoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<SubCategoryMasterDto>(ex.Message);
            }
        }

        // update

        public ApiResponse<SubCategoryMasterDto> UpdateSubCategoryMaster(SubCategoryMasterDto model)
        {
            try
            {
                var subCategoryMaster = dbContext.SubCategoryMasters.FirstOrDefault(u => u.SubCategoryId == model.SubCategoryId);
                if (subCategoryMaster == null)
                {
                    return new ApiResponse<SubCategoryMasterDto>("SubCategoryMaster not found");
                }
                subCategoryMaster.SubCategoryName = model.SubCategoryName;
                subCategoryMaster.CategoryId = model.CategoryId;
                subCategoryMaster.CategoryName = model.CategoryName;
                subCategoryMaster.IsActive = model.IsActive;
                dbContext.SaveChanges();
                return new ApiResponse<SubCategoryMasterDto>(_mapper.Map<SubCategoryMasterDto>(subCategoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<SubCategoryMasterDto>(ex.Message);
            }
        }

        // delete
        public ApiResponse<SubCategoryMasterDto> DeleteSubCategoryMaster(int id)
        {
            try
            {
                var subCategoryMaster = dbContext.SubCategoryMasters.FirstOrDefault(u => u.SubCategoryId == id);
                if (subCategoryMaster == null)
                {
                    return new ApiResponse<SubCategoryMasterDto>("SubCategoryMaster not found");
                }
                subCategoryMaster.IsActive = true;
                dbContext.SaveChanges();
                return new ApiResponse<SubCategoryMasterDto>(_mapper.Map<SubCategoryMasterDto>(subCategoryMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<SubCategoryMasterDto>(ex.Message);
            }
        }
    }
}