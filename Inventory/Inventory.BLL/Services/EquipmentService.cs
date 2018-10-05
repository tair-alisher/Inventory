using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using CatalogEntities;

namespace Inventory.BLL.Services
{
    public class EquipmentService : IEquipmentService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public EquipmentService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public EquipmentDTO Get(Guid id)
        {
            Equipment equipment = _unitOfWork.Equipments.Get(id);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<EmployeeDTO, EmployeeDTO>());

            return config.CreateMapper().Map<EquipmentDTO>(equipment);
        }

        public IEnumerable<EquipmentDTO> GetAll()
        {
            List<Equipment> allEquipments = _unitOfWork.Equipments.GetAll().ToList();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Equipment, EquipmentDTO>());
            
            return config.CreateMapper().Map<List<EquipmentDTO>>(allEquipments);
        }

        public EmployeeDTO GetEquipmentOwner(Guid id)
        {
            int employeeId = _unitOfWork.EquipmentEmployee.Find(e => e.EquipmentId == id).First().EmployeeId;

            Employee employee = _unitOfWork.Employees.Get(employeeId);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDTO>());

            return config.CreateMapper().Map<EmployeeDTO>(employee);
        }
    }
}
