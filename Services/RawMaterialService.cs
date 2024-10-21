

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using navami.Dto;
using navami.Models;

namespace navami
{

    public class RawMaterialService
    {

        private readonly NavamiDevContext dbContext;
        private readonly IMapper _mapper;
        public RawMaterialService(NavamiDevContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }

        public ApiResponse<List<RawMaterialsDto>> GetRawMaterial()
        {
            try
            {
                var rawMaterial = dbContext.RawMaterials.Where(u => u.IsActive != true).ToList();
                return new ApiResponse<List<RawMaterialsDto>>(_mapper.Map<List<RawMaterialsDto>>(rawMaterial));
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<RawMaterialsDto>>(ex.Message);
            }
        }


        public async Task<ApiResponse<RawMaterialsDto>> GetRawMaterialByIdAsync(int id)
        {
            try
            {
                var rawMaterial = await dbContext.RawMaterials
                    .Where(u => u.RawMaterialId == id && !u.IsActive) 
                    .Include(u => u.Category)
                    .Include(u => u.SubCategory)
                    .Select(rm => new
                    {
                        Rm = rm,
                        PriceInfo = dbContext.RawMaterials
                            .Where(pm => pm.RawMaterialId == rm.RawMaterialId)
                            .OrderByDescending(pm => pm.AddedOn)
                            .FirstOrDefault()
                    })
                    .FirstOrDefaultAsync();

                if (rawMaterial == null)
                {
                    return new ApiResponse<RawMaterialsDto>("RawMaterial not found");
                }

                var rawMaterialDto = _mapper.Map<RawMaterialsDto>(rawMaterial.Rm);
                rawMaterialDto.CategoryName = rawMaterial.Rm.Category?.CategoryName;
                rawMaterialDto.SubCategoryName = rawMaterial.Rm.SubCategory?.SubCategoryName;
                rawMaterialDto.Party = rawMaterial.PriceInfo.Party;

                if (rawMaterial.PriceInfo != null)
                {
                    rawMaterialDto.Price = rawMaterial.PriceInfo.Price ?? 0m;
                    rawMaterialDto.PriceDate = rawMaterial.PriceInfo.AddedOn;
                }

                return new ApiResponse<RawMaterialsDto>(rawMaterialDto);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine(ex); // Replace with proper logging
                return new ApiResponse<RawMaterialsDto>(ex.Message);
            }
        }
        //public async Task<ApiResponse<List<RawMaterialsDto>>> GetAllRawMaterials()
        //{
        //    try
        //    {
        //        var rawMaterials = await dbContext.RawMaterials
        //            .Where(rm => !rm.IsActive)
        //            .Include(rm => rm.Category)
        //            .Include(rm => rm.SubCategory)
        //            .Select(rm => new
        //            {
        //                Rm = rm,
        //                PriceInfo = dbContext.RawMaterialPrices
        //                    .Where(pm => pm.RawMaterialId == rm.RawMaterialId)
        //                    .OrderByDescending(pm => pm.CreatedAt)
        //                    .FirstOrDefault()
        //            })
        //            .Select(result => new RawMaterialsDto
        //            {
        //                RawMaterialId = result.Rm.RawMaterialId,
        //                RawMaterialCode = result.Rm.RawMaterialCode,
        //                RawMaterialName = result.Rm.RawMaterialName,
        //                IsNew = result.Rm.IsNew,
        //                CategoryId = result.Rm.CategoryId,
        //                CategoryName = result.Rm.Category.CategoryName,
        //                SubCategoryId = result.Rm.SubCategoryId,
        //                SubCategoryName = result.Rm.SubCategory.SubCategoryName,
        //                SpecificationNo = result.Rm.SpecificationNo,
        //                Description = result.Rm.Description,
        //                IsDiscontinued = result.Rm.IsDiscontinued,
        //                AddedOn = result.Rm.AddedOn,
        //                AddedBy = result.Rm.AddedBy,
        //                LastModifiedOn = result.Rm.LastModifiedOn,
        //                LastModifiedBy = result.Rm.LastModifiedBy,
        //                Price = result.PriceInfo.Price,
        //                PriceDate = result.PriceInfo.CreatedAt,
        //                IsActive = result.Rm.IsActive
        //            })
        //            .ToListAsync();

        //        return new ApiResponse<List<RawMaterialsDto>>(rawMaterials);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return new ApiResponse<List<RawMaterialsDto>>(ex.Message);
        //    }
        //}

        public async Task<ApiResponse<List<RawMaterialsDto>>> GetAllRawMaterials()
        {
            try
            {
                var rawMaterials = await dbContext.RawMaterials
                    .Where(rm => !rm.IsActive)
                    .Include(rm => rm.Category)
                    .Include(rm => rm.SubCategory)
                    .Select(rm => new
                    {
                        Rm = rm,
                        PriceInfo = dbContext.RawMaterialPrices
                            .Where(pm => pm.RawMaterialId == rm.RawMaterialId)
                            .OrderByDescending(pm => pm.CreatedAt)
                            .FirstOrDefault()
                    })
                    .Select(result => new RawMaterialsDto
                    {
                        RawMaterialId = result.Rm.RawMaterialId,
                        RawMaterialCode = result.Rm.RawMaterialCode,
                        RawMaterialName = result.Rm.RawMaterialName,
                        IsNew = result.Rm.IsNew,
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
                        Price = result.PriceInfo.Price,
                        PriceDate = result.PriceInfo.CreatedAt,
                        IsActive = result.Rm.IsActive
                    })
                    .ToListAsync();

                return new ApiResponse<List<RawMaterialsDto>>(rawMaterials);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResponse<List<RawMaterialsDto>>(ex.Message);
            }
        }

        public ApiResponse<RawMaterialsDto> AddRawMaterial(RawMaterialsDto rawMaterialDto)
        {
            try
            {
                var rawMaterial = _mapper.Map<RawMaterial>(rawMaterialDto);
                dbContext.RawMaterials.Add(rawMaterial);
                dbContext.SaveChanges();

                if ( rawMaterial.Price.HasValue)
                {
                    var price = new RawMaterialPrice
                    {
                        RawMaterialId = rawMaterial.RawMaterialId,
                        VendorId = 0,
                        SupplierName = rawMaterialDto.Party ?? "Unknown",
                        Price = rawMaterial.Price.Value,
                        CreatedAt = rawMaterial.PriceDate ?? DateTime.Now,
                        CreatedBy = rawMaterial.AddedBy
                    };

                    dbContext.RawMaterialPrices.Add(price);

                    dbContext.SaveChanges();
                }

                // Map back to DTO for the response
                var resultDto = _mapper.Map<RawMaterialsDto>(rawMaterial);
                return new ApiResponse<RawMaterialsDto>(resultDto);
            }
            catch (Exception ex)
            {
                // Log the inner exception details for better debugging
                var innerException = ex.InnerException?.Message ?? "No additional details";
                return new ApiResponse<RawMaterialsDto>($"Error: {ex.Message}. Inner Exception: {innerException}");
            }
        }

        //UpdateRawMaterial
        public ApiResponse<RawMaterialsDto> UpdateRawMaterial(RawMaterialsDto rawMaterialDto)
        {
            try
            {
                var rawMaterial = dbContext.RawMaterials.FirstOrDefault(u => u.RawMaterialId == rawMaterialDto.RawMaterialId);
                if (rawMaterial == null)
                {
                    return new ApiResponse<RawMaterialsDto>("RawMaterial not found");
                }
                if (rawMaterial.Price != rawMaterialDto.Price)
                {
                    if (rawMaterial.Price.HasValue)
                    {
                        var price = new RawMaterialPrice
                        {
                            RawMaterialId = rawMaterial.RawMaterialId,
                            SupplierName = rawMaterial.Party ?? "Unknown",
                            Price = rawMaterialDto.Price,
                            CreatedAt = rawMaterial.PriceDate ?? DateTime.Now,
                            CreatedBy = rawMaterial.AddedBy
                        };

                        dbContext.RawMaterialPrices.Add(price);

                        dbContext.SaveChanges();
                    }
                }
                _mapper.Map(rawMaterialDto, rawMaterial);
                dbContext.SaveChanges();

                return new ApiResponse<RawMaterialsDto>(_mapper.Map<RawMaterialsDto>(rawMaterial));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RawMaterialsDto>(ex.Message);
            }
        }

        //DeleteRawMaterial
        public ApiResponse<RawMaterialsDto> DeleteRawMaterial(int id)
        {
            try
            {
                var rawMaterial = dbContext.RawMaterials.FirstOrDefault(u => u.RawMaterialId == id);
                if (rawMaterial == null)
                {
                    return new ApiResponse<RawMaterialsDto>("RawMaterial not found");
                }
                rawMaterial.IsActive = true;
                dbContext.SaveChanges();
                return new ApiResponse<RawMaterialsDto>(_mapper.Map<RawMaterialsDto>(rawMaterial));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RawMaterialsDto>(ex.Message);
            }
        }


        // GetRawMaterialBySubCategory
        public ApiResponse<List<RawMaterialsDto>> GetRawMaterialBySubCategory(int subCategoryId)
        {
            try
            {
                var rawMaterial = dbContext.RawMaterials
                    .Where(u => u.SubCategoryId == subCategoryId && !u.IsActive)
                    .ToList();
                return new ApiResponse<List<RawMaterialsDto>>(_mapper.Map<List<RawMaterialsDto>>(rawMaterial));
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<RawMaterialsDto>>(ex.Message);
            }
        }


    }
}