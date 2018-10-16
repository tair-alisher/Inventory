using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
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
            AddAndGetId(item);
        }

        public Guid AddAndGetId(EquipmentDTO equipmentDTO)
        {
            Equipment equipment = BLLEquipmentMapper.DtoToEntity(equipmentDTO);
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

        public void Update(EquipmentDTO item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            if (HasRelations(id))
                throw new HasRelationsException();

            Equipment equipment = _unitOfWork.Equipments.Get(id);
            if (equipment == null)
                throw new NotFoundException();

            _unitOfWork.Equipments.Delete(id);
            _unitOfWork.Save();
        }

        public bool HasRelations(Guid id)
        {
            if (HasRelationsWithEmployees(id))
                return true;
            if (HasRelationsWithComponents(id))
                return true;

            return false;
        }

        private bool HasRelationsWithEmployees(Guid id)
        {
            var relations = _unitOfWork.EquipmentEmployeeRelations.Find(r => r.EquipmentId == id);

            return relations.Count() > 0;
        }

        private bool HasRelationsWithComponents(Guid id)
        {
            var relations = _unitOfWork.EquipmentComponentRelations.Find(r => r.EquipmentId == id);

            return relations.Count() > 0;
        }

        public IEnumerable<OwnerInfoDTO> GetOwnerHistory(Guid id)
        {
            IEnumerable<int> equipmentEmployeeIds = _unitOfWork
                .EquipmentEmployeeRelations
                .Find(e => e.EquipmentId == id)
                .Select(emp => emp.EmployeeId);

            if (equipmentEmployeeIds.Count() <= 0)
                return null;

            IEnumerable<OwnerInfoDTO> ownerHistory = (
                from
                    relation in _unitOfWork.EquipmentEmployeeRelations.GetAll()
                join
                    emp in _unitOfWork.Employees.GetAll()
                on
                    relation.EmployeeId equals emp.EmployeeId
                join
                    pos in _unitOfWork.Positions.GetAll()
                on
                    emp.PositionId equals pos.PositionId
                join
                    dep in _unitOfWork.Departments.GetAll()
                on
                    emp.DepartmentId equals dep.DepartmentId
                join
                    adm in _unitOfWork.Administrations.GetAll()
                on
                    dep.AdministrationId equals adm.AdministrationId
                where
                    relation.EquipmentId == id
                select new OwnerInfoDTO
                {
                    EmployeeId = emp.EmployeeId,
                    FullName = emp.EmployeeFullName,
                    Room = emp.EmployeeRoom,
                    Position = pos.PositionName,
                    Department = dep.DepartmentName,
                    Administration = adm.AdministrationName,
                    StartDate = relation.CreatedAt,
                    EndDate = relation.UpdatedAt,
                    IsActual = relation.IsOwner
                }).OrderBy(o => o.StartDate);

            return ownerHistory;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
