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
	public class EquipmentEmployeeRelationService : IEquipmentEmployeeRelationService
	{
		private IUnitOfWork _unitOfWork { get; set; }
		MapperConfiguration config;
		public EquipmentEmployeeRelationService(IUnitOfWork uow)
		{
			_unitOfWork = uow;
		}

		public void Add(EquipmentEmployeeRelationDTO item)
		{
			config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentEmployeeRelationDTO, EquipmentEmployeeRelation>());

			EquipmentEmployeeRelation equipmentEmployeeRelation = config.CreateMapper().Map<EquipmentEmployeeRelation>(item);

			equipmentEmployeeRelation.Id = Guid.NewGuid();
			equipmentEmployeeRelation.IsOwner = false;
			equipmentEmployeeRelation.CreatedAt = DateTime.Now;
			equipmentEmployeeRelation.UpdatedAt = DateTime.Now;

			_unitOfWork.EquipmentEmployeeRelations.Create(equipmentEmployeeRelation);
			_unitOfWork.Save();
		}

		public EquipmentEmployeeRelationDTO Get(Guid id)
		{
			EquipmentEmployeeRelation equipmentEmployeeRelation = _unitOfWork.EquipmentEmployeeRelations.Get(id);
			config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentEmployeeRelation, EquipmentEmployeeRelationDTO>());

			return config.CreateMapper().Map<EquipmentEmployeeRelationDTO>(equipmentEmployeeRelation);
		}

		public IEnumerable<EquipmentEmployeeRelationDTO> GetAll()
		{
			List<EquipmentEmployeeRelation> equipmentEmployeeRelations = _unitOfWork.EquipmentEmployeeRelations.GetAll().ToList();
			config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentEmployeeRelation, EquipmentEmployeeRelationDTO>());

			return config.CreateMapper().Map<List<EquipmentEmployeeRelationDTO>>(equipmentEmployeeRelations);
		}

		public void Delete(Guid id)
		{
			EquipmentEmployeeRelation equipmentEmployeeRelatiion = _unitOfWork.EquipmentEmployeeRelations.Get(id);
			if (equipmentEmployeeRelatiion == null)
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
