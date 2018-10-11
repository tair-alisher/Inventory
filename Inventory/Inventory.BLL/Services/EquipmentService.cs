using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using CatalogEntities;
using Inventory.BLL.Infrastructure;

namespace Inventory.BLL.Services
{
    public class EquipmentService : IEquipmentService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public EquipmentService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public void Add(EquipmentDTO item)
        {
            Equipment equipment = BLLEquipmentMapper.DtoToEntity(item);
            equipment.Id = Guid.NewGuid();

            _unitOfWork.Equipments.Create(equipment);
            _unitOfWork.Save();
        }

        public Guid AddAndGetId(EquipmentDTO item)
        {
            Equipment equipment = BLLEquipmentMapper.DtoToEntity(item);
            equipment.Id = Guid.NewGuid();

            _unitOfWork.Equipments.Create(equipment);
            _unitOfWork.Save();

            return equipment.Id;
        }

        public EquipmentDTO Get(Guid id)
        {
            Equipment equipment = _unitOfWork.Equipments.Get(id);

            return BLLEquipmentMapper.EntityToDto(equipment);
        }

        public IEnumerable<EquipmentDTO> GetAll()
        {
            List<Equipment> equipments = _unitOfWork.Equipments.GetAll().ToList();

            return BLLEquipmentMapper.EntityToDto(equipments);
        }

        public void Delete(Guid id)
        {
            Equipment equipment = _unitOfWork.Equipments.Get(id);
            if (equipment == null)
                throw new NotFoundException();

            _unitOfWork.Equipments.Delete(id);
            _unitOfWork.Save();
        }

        public EmployeeDTO GetEquipmentOwner(Guid id)
        {
            IEnumerable<EquipmentEmployeeRelation> equipmentEmployee = _unitOfWork
                .EquipmentEmployeeRelations
                .Find(e => e.EquipmentId == id);

            if (equipmentEmployee.Count() <= 0)
                return null;

            Employee employee = _unitOfWork
                .Employees
                .Get(equipmentEmployee.First().EmployeeId);

            return BLLEmployeeMapper.EntityToDto(employee);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
