

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

        //GetRawMaterialById 
        public ApiResponse<RmmasterDto> GetRawMaterialById(int id)
        {
            try
            {
                var rawMaterial = dbContext.Rmmasters
                    .Where(u => u.Rmid == id && u.IsActive != true)
                    .Include(u => u.Category)
                    .Include(u => u.SubCategory)
                    .FirstOrDefault();
                if (rawMaterial == null)
                {
                    return new ApiResponse<RmmasterDto>("RawMaterial not found");
                }
                var rawMaterialDto = _mapper.Map<RmmasterDto>(rawMaterial);
                rawMaterialDto.CategoryName = rawMaterial.Category?.CategoryName;
                rawMaterialDto.SubCategoryName = rawMaterial.SubCategory?.SubCategoryName;
                var price = dbContext.RmpriceMasters.Where(u => u.Rmid == rawMaterial.Rmid).OrderByDescending(u => u.CreatedAt).FirstOrDefault();
                if (price != null)
                {
                    rawMaterialDto.Price = price.Price;
                    rawMaterialDto.PriceDate = price.CreatedAt;
                }
                return new ApiResponse<RmmasterDto>(rawMaterialDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<RmmasterDto>(ex.Message);
            }
        }

        public ApiResponse<List<RmmasterDto>> GetAllRawMaterials()
        {
            try
            {
                // Join Rmmasters with CategoryMasters and SubCategoryMasters
                var rawMaterials = dbContext.Rmmasters
                    .Where(u => u.IsActive != true)
                    .Include(u => u.Category)
                    .Include(u => u.SubCategory)
                    .Select(rm => new RmmasterDto
                    {
                        Rmid = rm.Rmid,
                        Rmcode = rm.Rmcode,
                        Rmname = rm.Rmname,
                        IsNewRm = rm.IsNewRm,
                        CategoryId = rm.CategoryId,
                        CategoryName = rm.Category.CategoryName,
                        SubCategoryId = rm.SubCategoryId,
                        SubCategoryName = rm.SubCategory.SubCategoryName,
                        SpecificationNo = rm.SpecificationNo,
                        Description = rm.Description,
                        IsDiscontinued = rm.IsDiscontinued,
                        AddedOn = rm.AddedOn,
                        AddedBy = rm.AddedBy,
                        LastModifiedOn = rm.LastModifiedOn,
                        LastModifiedBy = rm.LastModifiedBy,
                        Party = rm.Party,
                        VendorId = rm.VendorId,
                        Price = dbContext.RmpriceMasters
                            .Where(pm => pm.Rmid == rm.Rmid)
                            .OrderByDescending(pm => pm.CreatedAt)
                            .Select(pm => pm.Price)
                            .FirstOrDefault(),
                        PriceDate = dbContext.RmpriceMasters
                            .Where(pm => pm.Rmid == rm.Rmid)
                            .OrderByDescending(pm => pm.CreatedAt)
                            .Select(pm => pm.CreatedAt)
                            .FirstOrDefault(),
                        IsActive = rm.IsActive
                    }).ToList();

                return new ApiResponse<List<RmmasterDto>>(rawMaterials);
            }
            catch (Exception ex)
            {
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
                            Price = rawMaterialDto.Price.Value,
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



    }
}