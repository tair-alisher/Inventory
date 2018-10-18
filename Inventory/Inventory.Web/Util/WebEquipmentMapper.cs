using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.Web.Models;
using System.Collections.Generic;

namespace Inventory.Web.Util
{
    public class WebEquipmentMapper
    {
        public static EquipmentVM DtoToVm(EquipmentDTO equipmentDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentDTO, EquipmentVM>());

            return config.CreateMapper().Map<EquipmentVM>(equipmentDTO);
        }

        public static IEnumerable<EquipmentVM> DtoToVm(IEnumerable<EquipmentDTO> equipmentDTOs)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentDTO, EquipmentVM>());

            return config.CreateMapper().Map<IEnumerable<EquipmentVM>>(equipmentDTOs);
        }

        public static EquipmentDTO VmToDto(EquipmentVM equipmentVM)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentVM, EquipmentDTO>());

            return config.CreateMapper().Map<EquipmentDTO>(equipmentVM);
        }
    }
}