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

            return BLLEquipmentTypeMapper.EntityToDto(equipmentType);
        }

        public IEnumerable<EquipmentTypeDTO> GetAll()
        {
            List<EquipmentType> equipmentTypes = _unitOfWork
                .EquipmentTypes
                .GetAll()
                .ToList();

            return BLLEquipmentTypeMapper.EntityToDto(equipmentTypes);
        }

		public void Add(EquipmentTypeDTO item)
		{
			EquipmentType equipmentType = BLLEquipmentTypeMapper.DtoToEntity(item);
			equipmentType.Id = Guid.NewGuid();

			_unitOfWork.EquipmentTypes.Create(equipmentType);
			_unitOfWork.Save();
		}

		public void Delete(Guid id)
		{
			EquipmentType equipmentType = _unitOfWork.EquipmentTypes.Get(id);
			if (equipmentType == null)
				throw new NotFoundException();

			_unitOfWork.EquipmentTypes.Delete(id);
			_unitOfWork.Save();
		}

		public void Dispose()
		{
			_unitOfWork.Dispose();
		}
	}
}
