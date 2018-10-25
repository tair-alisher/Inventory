using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BLL.Infrastructure
{
    public class BLLHistoryMapper
    {
        public static HistoryDTO EntityToDto(History history)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<History, HistoryDTO>());

            return config.CreateMapper().Map<HistoryDTO>(history);
        }

        public static IEnumerable<HistoryDTO> EntityToDto(IEnumerable<History> histories)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<History, HistoryDTO>());

            return config.CreateMapper().Map<IEnumerable<HistoryDTO>>(histories);
        }

        public static History DtoToEntity(HistoryDTO historyDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<HistoryDTO, History>());

            return config.CreateMapper().Map<History>(historyDTO);
        }
    }
}
