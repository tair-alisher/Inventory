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
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            EquipmentEmployeeRelation relation = _unitOfWork.EquipmentEmployeeRelations.Get(id);
            if (relation == null)
                throw new NotFoundException();

            _unitOfWork.EquipmentEmployeeRelations.Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
