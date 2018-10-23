using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.DAL.Entities;
using System.Collections.Generic;

namespace Inventory.BLL.Infrastructure
{
    public class BLLEquipmentMapper
    {
        public static EquipmentDTO EntityToDto(Equipment equipment)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<Equipment, EquipmentDTO>());

            return config.CreateMapper().Map<EquipmentDTO>(equipment);
        }

        public static IEnumerable<EquipmentDTO> EntityToDto(IEnumerable<Equipment> equipments)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<Equipment, EquipmentDTO>());

            return config.CreateMapper().Map<IEnumerable<EquipmentDTO>>(equipments);
        }

        public static Equipment DtoToEntity(EquipmentDTO equipmentDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentDTO, Equipment>());

            return config.CreateMapper().Map<Equipment>(equipmentDTO);
        }
    }
}
