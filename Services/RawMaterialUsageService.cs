using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using navami.Dto;
using navami.Models;

namespace navami
{
    public class RawMaterialUsageService
    {

        private readonly NavamiContext dbContext;
        private readonly IMapper _mapper;
        public RawMaterialUsageService(NavamiContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }

        // GetRawMaterialUsage

        public ApiResponse<List<RawMaterialUsageDto>> GetRawMaterialUsage()
        {
            try
            {
                var rawMaterialUsage = dbContext.RawMaterialUsages.ToList();
                return new ApiResponse<List<RawMaterialUsageDto>>(_mapper.Map<List<RawMaterialUsageDto>>(rawMaterialUsage));
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<RawMaterialUsageDto>>(ex.Message);
            }
        }

        // GetRawMaterialUsageById
        public ApiResponse<RawMaterialUsageDto> GetRawMaterialUsageById(int id)
        {
            try
            {
                var rawMaterialUsage = dbContext.RawMaterialUsages.FirstOrDefault(u => u.RmusageId == id);
                if (rawMaterialUsage == null)
                {
                    return new ApiResponse<RawMaterialUsageDto>("RawMaterialUsage not found");
                }

                return new ApiResponse<RawMaterialUsageDto>(_mapper.Map<RawMaterialUsageDto>(rawMaterialUsage));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RawMaterialUsageDto>(ex.Message);
            }
        }

        // AddRawMaterialUsage
        public ApiResponse<RawMaterialUsageDto> AddRawMaterialUsage(RawMaterialUsageDto model)
        {
            try
            {
                var rawMaterialUsage = _mapper.Map<RawMaterialUsage>(model);
                dbContext.RawMaterialUsages.Add(rawMaterialUsage);
                dbContext.SaveChanges();
                return new ApiResponse<RawMaterialUsageDto>(_mapper.Map<RawMaterialUsageDto>(rawMaterialUsage));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RawMaterialUsageDto>(ex.Message);
            }
        }

        // UpdateRawMaterialUsage

        public ApiResponse<RawMaterialUsageDto> UpdateRawMaterialUsage(RawMaterialUsageDto model)
        {
            try
            {
                var rawMaterialUsage = dbContext.RawMaterialUsages.FirstOrDefault(u => u.RmusageId == model.RmusageId);
                if (rawMaterialUsage == null)
                {
                    return new ApiResponse<RawMaterialUsageDto>("RawMaterialUsage not found");
                }

                rawMaterialUsage = _mapper.Map<RawMaterialUsage>(model);
                dbContext.SaveChanges();
                return new ApiResponse<RawMaterialUsageDto>(_mapper.Map<RawMaterialUsageDto>(rawMaterialUsage));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RawMaterialUsageDto>(ex.Message);
            }
        }

        // DeleteRawMaterialUsage
        public async Task<ApiResponse<RawMaterialUsageDto>> DeleteRawMaterialUsage(int id)
        {
            try
            {
                var rawMaterialUsage = dbContext.RawMaterialUsages.FirstOrDefault(u => u.RmusageId == id);
                if (rawMaterialUsage == null)
                {
                    return new ApiResponse<RawMaterialUsageDto>("RawMaterialUsage not found");
                }
                dbContext.RawMaterialUsages.Remove(rawMaterialUsage);
                // rawMaterialUsage.IsActive = true;
                dbContext.SaveChanges();
                await UpdateRecipeCostAsync(rawMaterialUsage.RecipeId);

                return new ApiResponse<RawMaterialUsageDto>(_mapper.Map<RawMaterialUsageDto>(rawMaterialUsage));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RawMaterialUsageDto>(ex.Message);
            }
        }



        // Sp

        public async Task UpdateRecipeCostAsync(int recipeId)
        {
            await dbContext.Database.ExecuteSqlRawAsync("EXEC UpdateRecipeCost @RecipeID", new SqlParameter("@RecipeID", recipeId));
        }

    }
}