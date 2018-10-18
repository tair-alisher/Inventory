using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.DAL.Entities;
using System.Collections.Generic;

namespace Inventory.BLL.Infrastructure
{
    public class BLLEquipmentTypeMapper
    {
        public static EquipmentTypeDTO EntityToDto(EquipmentType equipmentType)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentType, EquipmentTypeDTO>());

            return config.CreateMapper().Map<EquipmentTypeDTO>(equipmentType);
        }

        public static IEnumerable<EquipmentTypeDTO> EntityToDto(IEnumerable<EquipmentType> equipmentTypes)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentType, EquipmentTypeDTO>());

            return config.CreateMapper().Map<IEnumerable<EquipmentTypeDTO>>(equipmentTypes);
        }

        public static EquipmentType DtoToEntity(EquipmentTypeDTO equipmentTypeDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentTypeDTO, EquipmentType>());

            return config.CreateMapper().Map<EquipmentType>(equipmentTypeDTO);
        }
    }
}
