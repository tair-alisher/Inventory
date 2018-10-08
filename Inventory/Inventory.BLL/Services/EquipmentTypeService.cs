using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;

namespace Inventory.BLL.Services
{
    public class EquipmentTypeService : IEquipmentTypeService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        MapperConfiguration config;
        public EquipmentTypeService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public EquipmentTypeDTO Get(Guid id)
        {
            EquipmentType equipmentType = _unitOfWork.EquipmentTypes.Get(id);
            config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentType, EquipmentTypeDTO>());

            return config.CreateMapper().Map<EquipmentTypeDTO>(equipmentType);
        }

        public IEnumerable<EquipmentTypeDTO> GetAll()
        {
            List<EquipmentType> equipmentTypes = _unitOfWork.EquipmentTypes.GetAll().ToList();
            config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentType, EquipmentTypeDTO>());

            return config.CreateMapper().Map<List<EquipmentType>, List<EquipmentTypeDTO>>(equipmentTypes);
        }
    }
}
