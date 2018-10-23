using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.Web.Models;
using System.Collections.Generic;

namespace Inventory.Web.Util
{
    public class WebComponentTypeMapper
    {
        public static ComponentTypeVM DtoToVm(ComponentTypeDTO componentTypeDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentTypeDTO, ComponentTypeVM>());

            return config.CreateMapper().Map<ComponentTypeVM>(componentTypeDTO);
        }

        public static IEnumerable<ComponentTypeVM> DtoToVm(IEnumerable<ComponentTypeDTO> componentTypeDTOs)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentTypeDTO, ComponentTypeVM>());

            return config.CreateMapper().Map<IEnumerable<ComponentTypeVM>>(componentTypeDTOs);
        }

        public static ComponentTypeDTO VmToDto(ComponentTypeVM componentTypeVM)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentTypeVM, ComponentTypeDTO>());

            return config.CreateMapper().Map<ComponentTypeDTO>(componentTypeVM);
        }
    }
}