using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;

namespace Inventory.BLL.Services
{
    public class EquipmentEmployeeRelationService : IEquipmentEmployeeRelationService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public EquipmentEmployeeRelationService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public EquipmentEmployeeRelationDTO Get(Guid id)
        {
            EquipmentEmployeeRelation relation = _unitOfWork.EquipmentEmployeeRelations.Get(id);

            return BLLEquipmentEmployeeMapper.EntityToDto(relation);
        }

        public EquipmentEmployeeRelationDTO GetByEquipmentAndEmployee(Guid equipmentId, int employeeId)
        {
            EquipmentEmployeeRelation relation = _unitOfWork
                .EquipmentEmployeeRelations
                .Find(r => r.EquipmentId == equipmentId &&
                    r.EmployeeId == employeeId)
                .FirstOrDefault();

            if (relation == null)
                throw new NotFoundException();

            return BLLEquipmentEmployeeMapper.EntityToDto(relation);
        }

        public IEnumerable<EquipmentEmployeeRelationDTO> GetAll()
        {
            List<EquipmentEmployeeRelation> relations = _unitOfWork
                .EquipmentEmployeeRelations
                .GetAll()
                .ToList();

            return BLLEquipmentEmployeeMapper.EntityToDto(relations);
        }

        public void Create(Guid equipmentId, int employeeId)
        {
            EquipmentEmployeeRelationDTO relation = new EquipmentEmployeeRelationDTO
            {
                EquipmentId = equipmentId,
                EmployeeId = employeeId
            };
            this.Add(relation);
        }

        public void Create(Guid equipmentId, string[] employeeIds)
        {
            foreach (string employeeId in employeeIds)
                this.Add(new EquipmentEmployeeRelationDTO
                {
                    EquipmentId = equipmentId,
                    EmployeeId = int.Parse(employeeId)
                });
        }

        public void Add(EquipmentEmployeeRelationDTO item)
        {
            EquipmentEmployeeRelation relation = BLLEquipmentEmployeeMapper.DtoToEntity(item);

            relation.Id = Guid.NewGuid();
            relation.CreatedAt = DateTime.Now;
            relation.UpdatedAt = DateTime.Now;
            relation.IsOwner = false;

            _unitOfWork.EquipmentEmployeeRelations.Create(relation);
            _unitOfWork.Save();
        }

        public void Update(EquipmentEmployeeRelationDTO item)
        {
            EquipmentEmployeeRelation relation = BLLEquipmentEmployeeMapper.DtoToEntity(item);

            _unitOfWork.EquipmentEmployeeRelations.Update(relation);
            _unitOfWork.Save();
        }

        public void UpdateDates(EquipmentEmployeeRelationDTO relationDTO)
        {
            EquipmentEmployeeRelation relation = _unitOfWork.EquipmentEmployeeRelations.Get(relationDTO.Id);

            relation.CreatedAt = relationDTO.CreatedAt;
            relation.UpdatedAt = relationDTO.UpdatedAt;

            _unitOfWork.EquipmentEmployeeRelations.Update(relation);
            _unitOfWork.Save();
        }

        public void UpdateEquipmentRelations(Guid equipmentId, string[] employeeIds)
        {
            List<int> equipmentEmployeeIds = _unitOfWork
                .EquipmentEmployeeRelations
                .Find(r => r.EquipmentId == equipmentId)
                .Select(r => r.EmployeeId)
                .ToList();

            List<int> intEmployeeIds = employeeIds
                .Select(id => int.Parse(id))
                .ToList();

            foreach (int employeeId in intEmployeeIds)
                if (!equipmentEmployeeIds.Contains(employeeId))
                    this.Create(equipmentId, employeeId);

            foreach (int equipmentEmployeeId in equipmentEmployeeIds)
                if (!intEmployeeIds.Contains(equipmentEmployeeId))
                    this.DeleteEquipmentRelation(equipmentId, equipmentEmployeeId);
        }

        public void DeleteEquipmentRelation(Guid equipmentId, int employeeId)
        {
            EquipmentEmployeeRelation relation = _unitOfWork
                .EquipmentEmployeeRelations
                .Find(r => r.EquipmentId == equipmentId && r.EmployeeId == employeeId)
                .FirstOrDefault();

            if (relation != null)
                this.Delete(relation.Id);
        }

        public void DeleteEquipmentRelations(Guid id)
        {
            IEnumerable<Guid> relationIds = _unitOfWork
                .EquipmentEmployeeRelations
                .Find(r => r.EquipmentId == id)
                .Select(r => r.Id);

            foreach (Guid relationId in relationIds)
                Delete(relationId);
        }

        public void Delete(Guid id)
        {
            EquipmentEmployeeRelation relation = _unitOfWork.EquipmentEmployeeRelations.Get(id);
            if (relation == null)
                throw new NotFoundException();

            _unitOfWork.EquipmentEmployeeRelations.Delete(id);
            _unitOfWork.Save();
        }

        public void UnsetOwner(Guid equipmentId)
        {
            EquipmentEmployeeRelation relation = _unitOfWork
                .EquipmentEmployeeRelations
                .Find(r => r.EquipmentId == equipmentId && r.IsOwner == true)
                .FirstOrDefault();

            if (relation != null)
                UpdateIsOwnerField(relation.Id, false);
        }

        public void ResetOwner(Guid equipmentId, int employeeId)
        {
            EquipmentEmployeeRelation relation = _unitOfWork
                .EquipmentEmployeeRelations
                .Find(r => r.EquipmentId == equipmentId && r.IsOwner == true)
                .FirstOrDefault();

            if (relation != null)
                UpdateIsOwnerField(relation.Id, false);

            SetOwner(equipmentId, employeeId);
        }

        public void UpdateIsOwnerField(Guid relationId, bool value)
        {
            EquipmentEmployeeRelation relation = _unitOfWork
                .EquipmentEmployeeRelations
                .Get(relationId);

            relation.IsOwner = value;
            _unitOfWork.EquipmentEmployeeRelations.Update(relation);
            _unitOfWork.Save();
        }

        public void SetOwner(Guid equipmentId, int employeeId)
        {
            EquipmentEmployeeRelation relation = _unitOfWork
                .EquipmentEmployeeRelations
                .Find(r => r.EquipmentId == equipmentId && r.EmployeeId == employeeId)
                .First();

            UpdateIsOwnerField(relation.Id, true);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
