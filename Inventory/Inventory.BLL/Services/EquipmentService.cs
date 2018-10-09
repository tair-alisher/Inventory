using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using CatalogEntities;
using System.Net;
using Inventory.BLL.Infrastructure;

namespace Inventory.BLL.Services
{
    public class EquipmentService : IEquipmentService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        MapperConfiguration config;
        public EquipmentService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

		public void Add(EquipmentDTO item)
		{
			config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentDTO, Equipment>());

			Equipment equipment = config.CreateMapper().Map<Equipment>(item);
			equipment.Id = Guid.NewGuid();

			_unitOfWork.Equipments.Create(equipment);
			_unitOfWork.Save();
		}

		public Guid AddAndGetId(EquipmentDTO item)
        {
            config = new MapperConfiguration(cfg => cfg.CreateMap<EquipmentDTO, Equipment>());

            Equipment equipment = config.CreateMapper().Map<Equipment>(item);
            equipment.Id = Guid.NewGuid();

            _unitOfWork.Equipments.Create(equipment);
            _unitOfWork.Save();

            return equipment.Id;
        }

		public EquipmentDTO Get(Guid id)
		{
			Equipment equipment = _unitOfWork.Equipments.Get(id);
			config = new MapperConfiguration(cfg => cfg.CreateMap<Equipment, EquipmentDTO>());

			return config.CreateMapper().Map<EquipmentDTO>(equipment);
		}

		public IEnumerable<EquipmentDTO> GetAll()
		{
			List<Equipment> allEquipments = _unitOfWork.Equipments.GetAll().ToList();
			config = new MapperConfiguration(cfg => cfg.CreateMap<Equipment, EquipmentDTO>());

			return config.CreateMapper().Map<List<EquipmentDTO>>(allEquipments);
		}

		public void Delete(Guid id)
        {
            Equipment equipment = _unitOfWork.Equipments.Get(id);
			if (equipment == null)
				throw new NotFoundException("Item with given id was not found.");

			_unitOfWork.Equipments.Delete(id);
			_unitOfWork.Save();
		}

        public EmployeeDTO GetEquipmentOwner(Guid id)
        {
            IEnumerable<EquipmentEmployee> equipmentEmployee = _unitOfWork.EquipmentEmployee.Find(e => e.EquipmentId == id);

            if (equipmentEmployee.Count() <= 0)
                return null;

            Employee employee = _unitOfWork.Employees.Get(equipmentEmployee.First().EmployeeId);

            config = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDTO>());

            return config.CreateMapper().Map<EmployeeDTO>(employee);
        }

		public void Dispose()
		{
			_unitOfWork.Dispose();
		}
	}
}
