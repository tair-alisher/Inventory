using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.DAL.Entities;
using System.Collections.Generic;

namespace Inventory.BLL.Infrastructure
{
    public class BLLComponentMapper
    {
        public static ComponentDTO EntityToDto(Component component)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<Component, ComponentDTO>());

            return config.CreateMapper().Map<ComponentDTO>(component);
        }

        public static IEnumerable<ComponentDTO> EntityToDto(IEnumerable<Component> components)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<Component, ComponentDTO>());

            return config.CreateMapper().Map<IEnumerable<ComponentDTO>>(components);
        }

        public static Component DtoToEntity(ComponentDTO componentDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentDTO, Component>());

            return config.CreateMapper().Map<Component>(componentDTO);
        }
    }
}
