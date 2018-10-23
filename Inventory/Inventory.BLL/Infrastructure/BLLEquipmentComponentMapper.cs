using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.DAL.Entities;
using System.Collections.Generic;

namespace Inventory.BLL.Infrastructure
{
    public class BLLEquipmentComponentMapper
    {
        public static EquipmentComponentRelationDTO EntityToDto(EquipmentComponentRelation relation)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentComponentRelation, EquipmentComponentRelationDTO>());

            return config.CreateMapper().Map<EquipmentComponentRelationDTO>(relation);
        }

        public static IEnumerable<EquipmentComponentRelationDTO> EntityToDto(IEnumerable<EquipmentComponentRelation> relations)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentComponentRelation, EquipmentComponentRelationDTO>());

            return config.CreateMapper().Map<IEnumerable<EquipmentComponentRelationDTO>>(relations);
        }

        public static EquipmentComponentRelation DtoToEntity(EquipmentComponentRelationDTO relation)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentComponentRelationDTO, EquipmentComponentRelation>());

            return config.CreateMapper().Map<EquipmentComponentRelation>(relation);
        }
    }
}
