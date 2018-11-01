using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.DAL.Entities;
using System.Collections.Generic;

namespace Inventory.BLL.Infrastructure
{
    public class BLLRoleMapper
    {
        public static RoleDTO EntityToDto(ApplicationRole role)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationRole, RoleDTO>());

            return config.CreateMapper().Map<RoleDTO>(role);
        }

        public static IEnumerable<RoleDTO> EntityToDto(IEnumerable<ApplicationRole> roles)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationRole, RoleDTO>());

            return config.CreateMapper().Map<IEnumerable<RoleDTO>>(roles);
        }
    }
}
