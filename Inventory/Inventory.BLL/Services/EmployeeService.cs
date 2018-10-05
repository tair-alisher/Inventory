using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CatalogEntities;
using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Interfaces;

namespace Inventory.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public EmployeeService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public EmployeeDTO Get(int id)
        {
            Employee employee = _unitOfWork.Employees.Get(id);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDTO>());

            return config.CreateMapper().Map<EmployeeDTO>(employee);
        }

        public IEnumerable<EmployeeDTO> GetAll()
        {
            List<Employee> employees = _unitOfWork.Employees.GetAll().ToList();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<List<Employee>, List<EmployeeDTO>>());

            return config.CreateMapper().Map<List<EmployeeDTO>>(employees);
        }
    }
}
