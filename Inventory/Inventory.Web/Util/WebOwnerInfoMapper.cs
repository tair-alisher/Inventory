using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.Web.Models;
using System.Collections.Generic;

namespace Inventory.Web.Util
{
    public class WebOwnerInfoMapper
    {
        public static OwnerInfoVM DtoToVm(OwnerInfoDTO ownerInfoDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<OwnerInfoDTO, OwnerInfoVM>());

            return config.CreateMapper().Map<OwnerInfoVM>(ownerInfoDTO);
        }

        public static IEnumerable<OwnerInfoVM> DtoToVm(IEnumerable<OwnerInfoDTO> ownerInfoDTOs)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<OwnerInfoDTO, OwnerInfoVM>());

            return config.CreateMapper().Map<IEnumerable<OwnerInfoVM>>(ownerInfoDTOs);
        }

        public static OwnerInfoDTO VmToDto(OwnerInfoVM ownerInfoVM)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<OwnerInfoVM, OwnerInfoDTO>());

            return config.CreateMapper().Map<OwnerInfoDTO>(ownerInfoVM);
        }
    }
}