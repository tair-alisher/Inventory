using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.Web.Models;
using System.Collections.Generic;

namespace Inventory.Web.Util
{
    public class WebEquipmentTypeMapper
    {
        public static IEnumerable<EquipmentTypeVM> DtoToVm(IEnumerable<EquipmentTypeDTO> equipmentTypeDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentTypeDTO, EquipmentTypeVM>());

            return config.CreateMapper().Map<IEnumerable<EquipmentTypeVM>>(equipmentTypeDTO);
        }

        public static EquipmentTypeVM DtoToVm(EquipmentTypeDTO equipmentTypeDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentTypeDTO, EquipmentTypeVM>());

            return config.CreateMapper().Map<EquipmentTypeVM>(equipmentTypeDTO);
        }

        public static EquipmentTypeDTO VmToDto(EquipmentTypeVM equipmentTypeVM)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentTypeVM, EquipmentTypeDTO>());

            return config.CreateMapper().Map<EquipmentTypeDTO>(equipmentTypeVM);
        }
    }
}