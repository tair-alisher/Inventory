using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.DAL.Entities;
using System.Collections.Generic;

namespace Inventory.BLL.Infrastructure
{
    public class BLLEquipmentEmployeeMapper
    {
        public static EquipmentEmployeeRelationDTO EntityToDto(EquipmentEmployeeRelation relation)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentEmployeeRelation, EquipmentEmployeeRelationDTO>());

            return config.CreateMapper().Map<EquipmentEmployeeRelationDTO>(relation);
        }

        public static IEnumerable<EquipmentEmployeeRelationDTO> EntityToDto(IEnumerable<EquipmentEmployeeRelation> relations)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap <EquipmentEmployeeRelation, EquipmentEmployeeRelationDTO>());

            return config.CreateMapper().Map<IEnumerable<EquipmentEmployeeRelationDTO>>(relations);
        }
        
        public static EquipmentEmployeeRelation DtoToEntity(EquipmentEmployeeRelationDTO relationDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentEmployeeRelationDTO, EquipmentEmployeeRelation>());

            return config.CreateMapper().Map<EquipmentEmployeeRelation>(relationDTO);
        }
    }
}
