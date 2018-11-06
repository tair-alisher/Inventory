using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Inventory.BLL.Infrastructure
{
    public class BLLUserMapper
    {
        public static UserDTO EntityToDto(ApplicationUser user)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, UserDTO>());

            return config.CreateMapper().Map<UserDTO>(user);
        }

        public static IEnumerable<UserDTO> EntityToDto(IEnumerable<ApplicationUser> users)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, UserDTO>());

            return config.CreateMapper().Map<IEnumerable<UserDTO>>(users);
        }

        public static ApplicationUser DtoToEntity(UserDTO userDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, ApplicationUser>());

            return config.CreateMapper().Map<ApplicationUser>(userDTO);
        }
    }
}
