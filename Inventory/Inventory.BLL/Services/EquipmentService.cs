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
    public class EquipmentService : IEquipmentService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public EquipmentService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public EquipmentDTO Get(Guid id)
        {
            Equipment equipment = _unitOfWork.Equipments.Get(id);

            return Mapper.Map<Equipment, EquipmentDTO>(equipment);
        }

        public IEnumerable<EquipmentDTO> GetAll()
        {
            List<Equipment> allEquipments = _unitOfWork.Equipments.GetAll().ToList();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Equipment, EquipmentDTO>());
            
            return config.CreateMapper().Map<List<Equipment>, List<EquipmentDTO>>(allEquipments);
        }
    }
}
