using AutoMapper;
using CatalogEntities;
using Inventory.BLL.DTO;
using System.Collections.Generic;

namespace Inventory.BLL.Infrastructure
{
    public class BLLEmployeeMapper
    {
        public static EmployeeDTO EntityToDto(Employee employee)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDTO>());

            return config.CreateMapper().Map<EmployeeDTO>(employee);
        }

        public static IEnumerable<EmployeeDTO> EntityToDto(IEnumerable<Employee> employeeDTOs)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDTO>());

            return config.CreateMapper().Map<IEnumerable<EmployeeDTO>>(employeeDTOs);
        }
    }
}
