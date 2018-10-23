using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory.Web.Util
{
    public class WebRepairPlaceMapper
    {
        public static RepairPlaceVM DtoToVm(RepairPlaceDTO repairPlaceDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<RepairPlaceDTO, RepairPlaceVM>());

            return config.CreateMapper().Map<RepairPlaceVM>(repairPlaceDTO);
        }

        public static IEnumerable<RepairPlaceVM> DtoToVm(IEnumerable<RepairPlaceDTO> repairPlaceDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<RepairPlaceDTO, RepairPlaceVM>());

            return config.CreateMapper().Map<IEnumerable<RepairPlaceVM>>(repairPlaceDTO);
        }

        public static RepairPlaceDTO VmToDto(RepairPlaceVM repairPlaceVM)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<RepairPlaceVM, RepairPlaceDTO>());

            return config.CreateMapper().Map<RepairPlaceDTO>(repairPlaceVM);
        }
    }
}