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
    public class BLLStatusTypeMapper
    {
        public static StatusTypeDTO EntityToDto(StatusType statusTypeDto)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<StatusType, StatusTypeDTO>());

            return config.CreateMapper().Map<StatusTypeDTO>(statusTypeDto);
        }

        public static IEnumerable<StatusTypeDTO> EntityToDto(IEnumerable<StatusType> statusTypes)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<StatusType,StatusTypeDTO>());

            return config.CreateMapper().Map<IEnumerable<StatusTypeDTO>>(statusTypes);
        }

        public static StatusType DtoToEntity(StatusTypeDTO statusTypeDTO)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<StatusTypeDTO,StatusType>());

            return config.CreateMapper().Map<StatusType>(statusTypeDTO);
        }
    }
}
