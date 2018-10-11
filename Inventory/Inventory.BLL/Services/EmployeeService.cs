using System.Collections.Generic;
using System.Linq;
using CatalogEntities;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
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

            return BLLEmployeeMapper.EntityToDto(employee);
        }

        public IEnumerable<EmployeeDTO> GetAll()
        {
            List<Employee> employees = _unitOfWork.Employees.GetAll().ToList();

            return BLLEmployeeMapper.EntityToDto(employees);
        }
    }
}
