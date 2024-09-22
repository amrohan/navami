

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using navami.Dto;
using navami.Models;

namespace navami
{

    public class RawMaterialService
    {

        private readonly NavamiContext dbContext;
        private readonly IMapper _mapper;
        public RawMaterialService(NavamiContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }

        public ApiResponse<List<RmmasterDto>> GetRawMaterial()
        {
            try
            {
                var rawMaterial = dbContext.Rmmasters.Where(u => u.IsActive != true).ToList();
                return new ApiResponse<List<RmmasterDto>>(_mapper.Map<List<RmmasterDto>>(rawMaterial));
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<RmmasterDto>>(ex.Message);
            }
        }


        public async Task<ApiResponse<RmmasterDto>> GetRawMaterialByIdAsync(int id)
        {
            try
            {
                var rawMaterial = await dbContext.Rmmasters
                    .Where(u => u.Rmid == id && !u.IsActive) // Use !u.IsActive for clarity
                    .Include(u => u.Category)
                    .Include(u => u.SubCategory)
                    .Select(rm => new
                    {
                        Rm = rm,
                        PriceInfo = dbContext.RmpriceMasters
                            .Where(pm => pm.Rmid == rm.Rmid)
                            .OrderByDescending(pm => pm.CreatedAt)
                            .FirstOrDefault()
                    })
                    .FirstOrDefaultAsync(); // Use FirstOrDefaultAsync for asynchronous execution

                if (rawMaterial == null)
                {
                    return new ApiResponse<RmmasterDto>("RawMaterial not found");
                }

                var rawMaterialDto = _mapper.Map<RmmasterDto>(rawMaterial.Rm);
                rawMaterialDto.CategoryName = rawMaterial.Rm.Category?.CategoryName;
                rawMaterialDto.SubCategoryName = rawMaterial.Rm.SubCategory?.SubCategoryName;

                if (rawMaterial.PriceInfo != null)
                {
                    rawMaterialDto.Price = rawMaterial.PriceInfo.Price;
                    rawMaterialDto.PriceDate = rawMaterial.PriceInfo.CreatedAt;
                }

                return new ApiResponse<RmmasterDto>(rawMaterialDto);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine(ex); // Replace with proper logging
                return new ApiResponse<RmmasterDto>(ex.Message);
            }
        }
        public async Task<ApiResponse<List<RmmasterDto>>> GetAllRawMaterials()
        {
            try
            {
                var rawMaterials = await dbContext.Rmmasters
                    .Where(rm => !rm.IsActive)
                    .Include(rm => rm.Category)
                    .Include(rm => rm.SubCategory)
                    .Select(rm => new
                    {
                        Rm = rm,
                        PriceInfo = dbContext.RmpriceMasters
                            .Where(pm => pm.Rmid == rm.Rmid)
                            .OrderByDescending(pm => pm.CreatedAt)
                            .FirstOrDefault()
                    })
                    .Select(result => new RmmasterDto
                    {
                        Rmid = result.Rm.Rmid,
                        Rmcode = result.Rm.Rmcode,
                        Rmname = result.Rm.Rmname,
                        IsNewRm = result.Rm.IsNewRm,
                        CategoryId = result.Rm.CategoryId,
                        CategoryName = result.Rm.Category.CategoryName,
                        SubCategoryId = result.Rm.SubCategoryId,
                        SubCategoryName = result.Rm.SubCategory.SubCategoryName,
                        SpecificationNo = result.Rm.SpecificationNo,
                        Description = result.Rm.Description,
                        IsDiscontinued = result.Rm.IsDiscontinued,
                        AddedOn = result.Rm.AddedOn,
                        AddedBy = result.Rm.AddedBy,
                        LastModifiedOn = result.Rm.LastModifiedOn,
                        LastModifiedBy = result.Rm.LastModifiedBy,
                        Party = result.Rm.Party,
                        VendorId = result.Rm.VendorId,
                        Price = result.PriceInfo.Price,
                        PriceDate = result.PriceInfo.CreatedAt,
                        IsActive = result.Rm.IsActive
                    })
                    .ToListAsync();

                return new ApiResponse<List<RmmasterDto>>(rawMaterials);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResponse<List<RmmasterDto>>(ex.Message);
            }
        }

        public ApiResponse<RmmasterDto> AddRawMaterial(RmmasterDto rawMaterialDto)
        {
            try
            {
                var rawMaterial = _mapper.Map<Rmmaster>(rawMaterialDto);
                dbContext.Rmmasters.Add(rawMaterial);
                dbContext.SaveChanges();

                if (rawMaterial.VendorId.HasValue && rawMaterial.Price.HasValue)
                {
                    var price = new RmpriceMaster
                    {
                        Rmid = rawMaterial.Rmid,
                        VendorId = rawMaterial.VendorId.Value,
                        SupplierName = rawMaterial.Party ?? "Unknown",
                        Price = rawMaterial.Price.Value,
                        CreatedAt = rawMaterial.PriceDate ?? DateTime.Now,
                        CreatedBy = rawMaterial.AddedBy
                    };

                    dbContext.RmpriceMasters.Add(price);

                    dbContext.SaveChanges();
                }

                // Map back to DTO for the response
                var resultDto = _mapper.Map<RmmasterDto>(rawMaterial);
                return new ApiResponse<RmmasterDto>(resultDto);
            }
            catch (Exception ex)
            {
                // Log the inner exception details for better debugging
                var innerException = ex.InnerException?.Message ?? "No additional details";
                return new ApiResponse<RmmasterDto>($"Error: {ex.Message}. Inner Exception: {innerException}");
            }
        }

        //UpdateRawMaterial
        public ApiResponse<RmmasterDto> UpdateRawMaterial(RmmasterDto rawMaterialDto)
        {
            try
            {
                var rawMaterial = dbContext.Rmmasters.FirstOrDefault(u => u.Rmid == rawMaterialDto.Rmid);
                if (rawMaterial == null)
                {
                    return new ApiResponse<RmmasterDto>("RawMaterial not found");
                }
                if (rawMaterial.Price != rawMaterialDto.Price)
                {
                    if (rawMaterial.VendorId.HasValue && rawMaterial.Price.HasValue)
                    {
                        var price = new RmpriceMaster
                        {
                            Rmid = rawMaterial.Rmid,
                            VendorId = rawMaterial.VendorId.Value,
                            SupplierName = rawMaterial.Party ?? "Unknown",
                            Price = rawMaterialDto.Price,
                            CreatedAt = rawMaterial.PriceDate ?? DateTime.Now,
                            CreatedBy = rawMaterial.AddedBy
                        };

                        dbContext.RmpriceMasters.Add(price);

                        dbContext.SaveChanges();
                    }
                }
                _mapper.Map(rawMaterialDto, rawMaterial);
                dbContext.SaveChanges();

                return new ApiResponse<RmmasterDto>(_mapper.Map<RmmasterDto>(rawMaterial));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RmmasterDto>(ex.Message);
            }
        }

        //DeleteRawMaterial
        public ApiResponse<RmmasterDto> DeleteRawMaterial(int id)
        {
            try
            {
                var rawMaterial = dbContext.Rmmasters.FirstOrDefault(u => u.Rmid == id);
                if (rawMaterial == null)
                {
                    return new ApiResponse<RmmasterDto>("RawMaterial not found");
                }
                rawMaterial.IsActive = true;
                dbContext.SaveChanges();
                return new ApiResponse<RmmasterDto>(_mapper.Map<RmmasterDto>(rawMaterial));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RmmasterDto>(ex.Message);
            }
        }


        // GetRawMaterialBySubCategory
        public ApiResponse<List<RmmasterDto>> GetRawMaterialBySubCategory(int subCategoryId)
        {
            try
            {
                var rawMaterial = dbContext.Rmmasters
                    .Where(u => u.SubCategoryId == subCategoryId && !u.IsActive)
                    .ToList();
                return new ApiResponse<List<RmmasterDto>>(_mapper.Map<List<RmmasterDto>>(rawMaterial));
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<RmmasterDto>>(ex.Message);
            }
        }


    }
}