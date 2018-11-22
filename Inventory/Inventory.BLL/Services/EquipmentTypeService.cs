using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;

namespace Inventory.BLL.Services
{
    public class EquipmentTypeService : IEquipmentTypeService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public EquipmentTypeService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public EquipmentTypeDTO Get(Guid id)
        {
            EquipmentType equipmentType = _unitOfWork.EquipmentTypes.Get(id);

            return Mapper.Map<EquipmentTypeDTO>(equipmentType);
        }

        public EquipmentTypeDTO Get(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException();

            EquipmentType equipmentType = _unitOfWork.EquipmentTypes.Get(id);
            if (equipmentType == null)
                throw new NotFoundException();

            return Mapper.Map<EquipmentTypeDTO>(equipmentType);
        }

        public IEnumerable<EquipmentTypeDTO> GetAll()
        {
            List<EquipmentType> equipmentTypes = _unitOfWork
                .EquipmentTypes
                .GetAll()
                .ToList();

            return Mapper.Map<IEnumerable<EquipmentTypeDTO>>(equipmentTypes);
        }

        public IEnumerable<EquipmentTypeDTO> GetListOrderedByName()
        {
            List<EquipmentType> equipmentTypes = _unitOfWork.EquipmentTypes.GetAll().OrderBy(t => t.Name).ToList();

            return Mapper.Map<IEnumerable<EquipmentTypeDTO>>(equipmentTypes);
        }

        public void Add(EquipmentTypeDTO equipmentTypeDTO)
        {
            EquipmentType equipmentType = Mapper.Map<EquipmentType>(equipmentTypeDTO);
            equipmentType.Id = Guid.NewGuid();

            _unitOfWork.EquipmentTypes.Create(equipmentType);
            _unitOfWork.Save();
        }

        public void Update(EquipmentTypeDTO equipmentTypeDTO)
        {
            EquipmentType equipmentType = Mapper.Map<EquipmentType>(equipmentTypeDTO);

            _unitOfWork.EquipmentTypes.Update(equipmentType);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            if (HasRelations(id))
                throw new HasRelationsException();

            EquipmentType equipmentType = _unitOfWork.EquipmentTypes.Get(id);
            if (equipmentType == null)
                throw new NotFoundException();

            _unitOfWork.EquipmentTypes.Delete(id);
            _unitOfWork.Save();
        }

        public bool HasRelations(Guid id)
        {
            var relations = _unitOfWork.Equipments.Find(e => e.EquipmentTypeId == id);

            return relations.Count() > 0;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
