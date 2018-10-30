using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory.Web.Util
{
    public class WebHistoryMapper
    {
        public static HistoryVM DtoToVm(HistoryDTO historyDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<HistoryDTO, HistoryVM>());

            return config.CreateMapper().Map<HistoryVM>(historyDTO);
        }

        public static IEnumerable<HistoryVM> DtoToVm(IEnumerable<HistoryDTO> historyDTOs)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<HistoryDTO, HistoryVM>());

            return config.CreateMapper().Map<IEnumerable<HistoryVM>>(historyDTOs);
        }

        public static HistoryDTO VmToDto(HistoryVM historyVM)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<HistoryVM, HistoryDTO>());

            return config.CreateMapper().Map<HistoryDTO>(historyVM);
        }
    }
}