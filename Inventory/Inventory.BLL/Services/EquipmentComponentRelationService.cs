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
    public class EquipmentComponentRelationService : IEquipmentComponentRelationService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public EquipmentComponentRelationService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public EquipmentComponentRelationDTO Get(Guid id)
        {
            EquipmentComponentRelation relation = _unitOfWork.EquipmentComponentRelations.Get(id);

            return BLLEquipmentComponentMapper.EntityToDto(relation);
        }

        public IEnumerable<EquipmentComponentRelationDTO> GetAll()
        {
            List<EquipmentComponentRelation> relations = _unitOfWork
                .EquipmentComponentRelations
                .GetAll()
                .ToList();

            return BLLEquipmentComponentMapper.EntityToDto(relations);
        }

        public void Create(Guid equipmentId, Guid componentId)
        {
            EquipmentComponentRelationDTO relation = new EquipmentComponentRelationDTO
            {
                ComponentId = componentId,
                EquipmentId = equipmentId
            };
            this.Add(relation);
        }

        public void Add(EquipmentComponentRelationDTO item)
        {
            EquipmentComponentRelation relation = BLLEquipmentComponentMapper.DtoToEntity(item);

            relation.Id = Guid.NewGuid();
            relation.CreatedAt = DateTime.Now;
            relation.UpdatedAt = DateTime.Now;
            relation.IsActual = false;

            _unitOfWork.EquipmentComponentRelations.Create(relation);
            _unitOfWork.Save();
        }

        public void Update(EquipmentComponentRelationDTO item)
        {
            throw new NotImplementedException();
        }

        public void UpdateEquipmentRelations(Guid equipmentId, string[] componentIds)
        {
            List<Guid> equipmentComponentIds = _unitOfWork
                .EquipmentComponentRelations
                .Find(r => r.EquipmentId == equipmentId)
                .Select(r => r.ComponentId)
                .ToList();

            List<Guid> guidComponentIds = componentIds
                .Select(id => Guid.Parse(id))
                .ToList();

            foreach (Guid componentId in guidComponentIds)
                if (!equipmentComponentIds.Contains(componentId))
                    Create(equipmentId, componentId);

            foreach (Guid equipmentComponentId in equipmentComponentIds)
                if (!guidComponentIds.Contains(equipmentComponentId))
                    DeleteEquipmentRelation(equipmentId, equipmentComponentId);
        }

        private void DeleteEquipmentRelation(Guid equipmentId, Guid componentId)
        {
            EquipmentComponentRelation relation = _unitOfWork
                .EquipmentComponentRelations
                .Find(r => r.EquipmentId == equipmentId && r.ComponentId == componentId)
                .FirstOrDefault();

            if (relation != null)
                Delete(relation.Id);
        }

        public void DeleteRelationsByEquipmentId(Guid id)
        {
            IEnumerable<Guid> relationIds = _unitOfWork
                .EquipmentComponentRelations
                .Find(r => r.EquipmentId == id)
                .Select(r => r.Id)
                .ToList();

            foreach (Guid relationId in relationIds)
                Delete(relationId);
        }

        public void Delete(Guid id)
        {
            EquipmentComponentRelation relation = _unitOfWork.EquipmentComponentRelations.Get(id);
            if (relation == null)
                throw new NotFoundException();

            _unitOfWork.EquipmentComponentRelations.Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
