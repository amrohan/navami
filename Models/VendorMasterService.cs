using AutoMapper;
using Microsoft.EntityFrameworkCore;
using navami.Dto;
using navami.Models;

namespace navami
{
    public class VendorMasterService
    {
        private readonly NavamiContext dbContext;
        private readonly IMapper _mapper;
        public VendorMasterService(NavamiContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }

        // get all
        public async Task<ApiResponse<List<VendorMasterDto>>> GetVendorMasterAsync()
        {
            try
            {
                var vendorMasters = await dbContext.VendorMasters
                    .Where(u => !u.IsActive)
                    .ToListAsync();

                // Map the list of VendorMasters to VendorMasterDto objects
                var vendorMasterDtos = _mapper.Map<List<VendorMasterDto>>(vendorMasters);

                return new ApiResponse<List<VendorMasterDto>>(vendorMasterDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); return new ApiResponse<List<VendorMasterDto>>(ex.Message);
            }
        }
        // get by id
        public ApiResponse<VendorMasterDto> GetVendorMasterById(Guid id)
        {
            try
            {
                var vendorMaster = dbContext.VendorMasters.Where(u => u.VendorId == id).FirstOrDefault();
                return new ApiResponse<VendorMasterDto>(_mapper.Map<VendorMasterDto>(vendorMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<VendorMasterDto>(ex.Message);
            }
        }
        // create
        public ApiResponse<VendorMasterDto> CreateVendorMaster(VendorMasterDto vendorMasterDto)
        {
            try
            {
                var vendorMaster = _mapper.Map<VendorMaster>(vendorMasterDto);
                vendorMaster.VendorId = Guid.NewGuid();
                vendorMaster.CreatedAt = DateTime.Now;
                dbContext.VendorMasters.Add(vendorMaster);
                dbContext.SaveChanges();
                return new ApiResponse<VendorMasterDto>(_mapper.Map<VendorMasterDto>(vendorMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<VendorMasterDto>(ex.Message);
            }
        }
        // update
        public ApiResponse<VendorMasterDto> UpdateVendorMaster(VendorMasterDto vendorMasterDto)
        {
            try
            {
                var vendorMaster = dbContext.VendorMasters.Where(u => u.VendorId == vendorMasterDto.VendorId).FirstOrDefault();
                if (vendorMaster == null)
                {
                    return new ApiResponse<VendorMasterDto>("Vendor not found");
                }
                vendorMaster.VendorName = vendorMasterDto.VendorName;
                vendorMaster.IsActive = vendorMasterDto.IsActive;
                vendorMaster.UpdatedBy = vendorMasterDto.UpdatedBy;
                vendorMaster.UpdatedAt = DateTime.Now;
                dbContext.SaveChanges();
                return new ApiResponse<VendorMasterDto>(_mapper.Map<VendorMasterDto>(vendorMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<VendorMasterDto>(ex.Message);
            }
        }

        // delete
        public ApiResponse<VendorMasterDto> DeleteVendorMaster(Guid id)
        {
            try
            {
                var vendorMaster = dbContext.VendorMasters.Where(u => u.VendorId == id).FirstOrDefault();
                if (vendorMaster == null)
                {
                    return new ApiResponse<VendorMasterDto>("Vendor not found");
                }
                vendorMaster.IsActive = true;
                dbContext.SaveChanges();
                return new ApiResponse<VendorMasterDto>(_mapper.Map<VendorMasterDto>(vendorMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<VendorMasterDto>(ex.Message);
            }
        }

    }
}