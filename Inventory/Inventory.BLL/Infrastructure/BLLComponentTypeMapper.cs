using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.DAL.Entities;
using System.Collections.Generic;

namespace Inventory.BLL.Infrastructure
{
    public class BLLComponentTypeMapper
    {
        public static ComponentTypeDTO EntityToDto(ComponentType componentTypeDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentType, ComponentTypeDTO>());

            return config.CreateMapper().Map<ComponentTypeDTO>(componentTypeDTO);
        }

        public static IEnumerable<ComponentTypeDTO> EntityToDto(IEnumerable<ComponentType> componentTypes)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentType, ComponentTypeDTO>());

            return config.CreateMapper().Map<IEnumerable<ComponentTypeDTO>>(componentTypes);
        }

        public static ComponentType DtoToEntity(ComponentTypeDTO componentTypeDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentTypeDTO, ComponentType>());

            return config.CreateMapper().Map<ComponentType>(componentTypeDTO);
        }
    }
}
