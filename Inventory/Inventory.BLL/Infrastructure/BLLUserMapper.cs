using AutoMapper;
using Inventory.BLL.DTO;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Inventory.BLL.Infrastructure
{
    public class BLLUserMapper
    {
        public static UserDTO EntityToDto(IdentityUser user)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<IdentityUser, UserDTO>());

            return config.CreateMapper().Map<UserDTO>(user);
        }

        public static IEnumerable<UserDTO> EntityToDto(IEnumerable<IdentityUser> users)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<IdentityUser, UserDTO>());

            return config.CreateMapper().Map<IEnumerable<UserDTO>>(users);
        }

        public static IdentityUser DtoToEntity(UserDTO userDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, IdentityUser>());

            return config.CreateMapper().Map<IdentityUser>(userDTO);
        }
    }
}
