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
    public class BLLRepairPlaceMapper
    {
        public static RepairPlaceDTO EntityToDto(RepairPlace repairPlaceDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<RepairPlace, RepairPlaceDTO>());

            return config.CreateMapper().Map<RepairPlaceDTO>(repairPlaceDTO);
        }

        public static IEnumerable<RepairPlaceDTO> EntityToDto(IEnumerable<RepairPlace> repairPlaces)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<RepairPlace, RepairPlaceDTO>());

            return config.CreateMapper().Map<IEnumerable<RepairPlaceDTO>>(repairPlaces);
        }

        public static RepairPlace DtoToEntity(RepairPlaceDTO repairPlaceDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<RepairPlaceDTO, RepairPlace>());

            return config.CreateMapper().Map<RepairPlace>(repairPlaceDTO);
        }
    }
}
