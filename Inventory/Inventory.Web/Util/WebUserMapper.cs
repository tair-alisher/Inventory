using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.Web.Models.Account;
using System.Collections.Generic;

namespace Inventory.Web.Util
{
    public class WebUserMapper
    {
        public static UserVM DtoToVm(UserDTO userDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserVM>());

            return config.CreateMapper().Map<UserVM>(userDTO);
        }

        public static IEnumerable<UserVM> DtoToVm(IEnumerable<UserDTO> userDTOs)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserVM>());

            return config.CreateMapper().Map<IEnumerable<UserVM>>(userDTOs);
        }

        public static UserDTO VmToDto(UserVM userVM)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<UserVM, UserDTO>());

            return config.CreateMapper().Map<UserDTO>(userVM);
        }
    }
}