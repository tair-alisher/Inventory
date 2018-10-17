using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.Web.Models;
using System.Collections.Generic;

namespace Inventory.Web.Util
{
    public class WebEquipmentEmployeeMapper
    {
        public static EquipmentEmployeeRelationVM DtoToVm(EquipmentEmployeeRelationDTO relationDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentEmployeeRelationDTO, EquipmentEmployeeRelationVM>());

            return config.CreateMapper().Map<EquipmentEmployeeRelationVM>(relationDTO);
        }

        public static IEnumerable<EquipmentEmployeeRelationVM> DtoToVm(IEnumerable<EquipmentEmployeeRelationDTO> relationDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentEmployeeRelationDTO, EquipmentEmployeeRelationVM>());

            return config.CreateMapper().Map<IEnumerable<EquipmentEmployeeRelationVM>>(relationDTO);
        }

        public static EquipmentEmployeeRelationDTO VmToDto(EquipmentEmployeeRelationVM relationVM)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentEmployeeRelationVM, EquipmentEmployeeRelationDTO>());

            return config.CreateMapper().Map<EquipmentEmployeeRelationDTO>(relationVM);
        }
    }
}