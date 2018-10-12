using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.Web.Models;
using System.Collections.Generic;

namespace Inventory.Web.Util
{
    public class WebComponentMapper
    {
        public static ComponentVM DtoToVm(ComponentDTO componentDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentDTO, ComponentVM>());

            return config.CreateMapper().Map<ComponentVM>(componentDTO);
        }

        public static IEnumerable<ComponentVM> DtoToVm(IEnumerable<ComponentDTO> componentDTOs)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentDTO, ComponentVM>());

            return config.CreateMapper().Map<IEnumerable<ComponentVM>>(componentDTOs);
        }

        public static ComponentDTO VmToDto(ComponentVM componentVM)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentVM, ComponentDTO>());

            return config.CreateMapper().Map<ComponentDTO>(componentVM);
        }
    }
}