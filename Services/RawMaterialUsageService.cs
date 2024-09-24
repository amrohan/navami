using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using navami.Dto;
using navami.Models;

namespace navami
{
    public class RawMaterialUsageService
    {

        private readonly NavamiDevContext dbContext;
        private readonly IMapper _mapper;
        public RawMaterialUsageService(NavamiDevContext context, IMapper mapper)
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
        public ApiResponse<RawMaterialUsageDto> GetRawMaterialUsageById(Guid id)
        {
            try
            {
                var rawMaterialUsage = dbContext.RawMaterialUsages.FirstOrDefault(u => u.RawMaterialUsageId == id);
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
                var rawMaterialUsage = dbContext.RawMaterialUsages.FirstOrDefault(u => u.RawMaterialUsageId == model.RawMaterialUsageId);
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
        public async Task<ApiResponse<RawMaterialUsageDto>> DeleteRawMaterialUsage(Guid id)
        {
            try
            {
                var rawMaterialUsage = dbContext.RawMaterialUsages.FirstOrDefault(u => u.RawMaterialUsageId == id);
                if (rawMaterialUsage == null)
                {
                    return new ApiResponse<RawMaterialUsageDto>("RawMaterialUsage not found");
                }
                dbContext.RawMaterialUsages.Remove(rawMaterialUsage);
                 //rawMaterialUsage.isActive = true;
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

        public async Task UpdateRecipeCostAsync(Guid recipeId)
        {
            await dbContext.Database.ExecuteSqlRawAsync("EXEC UpdateRecipeCost @RecipeId", new SqlParameter("@RecipeId", recipeId));
        }

    }
}