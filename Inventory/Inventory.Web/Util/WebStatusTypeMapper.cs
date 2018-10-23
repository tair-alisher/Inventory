using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.Web.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Web;

namespace Inventory.Web.Util
{
    public class WebStatusTypeMapper
    {
        public static StatusTypeVM DtoToVm(StatusTypeDTO statusTypeDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<StatusTypeDTO, StatusTypeVM>());

            return config.CreateMapper().Map<StatusTypeVM>(statusTypeDTO);
        }

        public static IEnumerable<StatusTypeVM> DtoToVm(IEnumerable<StatusTypeDTO> statusTypeDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<StatusTypeDTO, StatusTypeVM>());

            return config.CreateMapper().Map<IEnumerable<StatusTypeVM>>(statusTypeDTO);
        }

        public static StatusTypeDTO VmToDto(StatusTypeVM statusTypeVM)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<StatusTypeVM, StatusTypeDTO >());

            return config.CreateMapper().Map<StatusTypeDTO>(statusTypeVM);
        }
    }
}