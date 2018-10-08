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
        MapperConfiguration config;
        public EmployeeService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            config = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDTO>());
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public EmployeeDTO Get(int id)
        {
            Employee employee = _unitOfWork.Employees.Get(id);

            return config.CreateMapper().Map<EmployeeDTO>(employee);
        }

        public IEnumerable<EmployeeDTO> GetAll()
        {
            List<Employee> employees = _unitOfWork.Employees.GetAll().ToList();

            List<EmployeeDTO> emps = config.CreateMapper().Map< List<EmployeeDTO>>(employees);

            return emps;
        }
    }
}
