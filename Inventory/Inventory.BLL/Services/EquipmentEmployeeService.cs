using System;
using System.Collections.Generic;
using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;

namespace Inventory.BLL.Services
{
	public class EquipmentEmployeeService : IEquipmentEmployeeService
	{
		private IUnitOfWork _unitOfWork { get; set; }
		MapperConfiguration config;
		public EquipmentEmployeeService(IUnitOfWork uow)
		{
			_unitOfWork = uow;
		}

		public void Add(EquipmentEmployeeDTO item)
		{
			config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentEmployeeDTO, EquipmentEmployee>());

			EquipmentEmployee equipmentEmployeeRelation = config.CreateMapper().Map<EquipmentEmployee>(item);

			equipmentEmployeeRelation.Id = Guid.NewGuid();

			_unitOfWork.EquipmentEmployee.Create(equipmentEmployeeRelation);
			_unitOfWork.Save();
		}

		public EquipmentEmployeeDTO Get(Guid id)
		{
			EquipmentEmployee equipmentEmployeeRelation = _unitOfWork.EquipmentEmployee.Get(id);
			config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentEmployee, EquipmentEmployeeDTO>());

			return config.CreateMapper().Map<EquipmentEmployeeDTO>(equipmentEmployeeRelation);
		}

		public IEnumerable<EquipmentEmployeeDTO> GetAll()
		{
			throw new NotImplementedException();
		}

		public void Delete(Guid id)
		{
			EquipmentEmployee equipmentEmployeeRelatiion = _unitOfWork.EquipmentEmployee.Get(id);
			if (equipmentEmployeeRelatiion == null)
				throw new NotFoundException("Item with given id was not found.");

			_unitOfWork.EquipmentEmployee.Delete(id);
			_unitOfWork.Save();
		}

		public void Dispose()
		{
			_unitOfWork.Dispose();
		}
	}
}
