using System;
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
        const int MaxNumberOfWordsInFullName = 3;
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

            return Mapper.Map<EmployeeDTO>(employee);
        }

        public IEnumerable<EmployeeDTO> GetAll()
        {
            List<Employee> employees = _unitOfWork.Employees.GetAll().ToList();

            return Mapper.Map<IEnumerable<EmployeeDTO>>(employees);
        }

        public IEnumerable<OwnerInfoDTO> ValidateNameAndGetEmployeesByName(string inputName)
        {
            IEnumerable<OwnerInfoDTO> foundEmployees = Enumerable.Empty<OwnerInfoDTO>();

            string name = inputName.Trim();
            if (string.IsNullOrEmpty(name))
                return foundEmployees;

            string[] nameParts = name.Split(' ');
            int wordsAmount = MaxNumberOfWordsInFullName;
            if (nameParts.Length < MaxNumberOfWordsInFullName)
                wordsAmount = nameParts.Length;

            if (wordsAmount == 1)
                foundEmployees = GetEmployeesByName(nameParts[0]);
            else if (wordsAmount == 2)
                foundEmployees = GetEmployeesByName(nameParts[0], nameParts[1]);
            else if (wordsAmount == 3)
                foundEmployees = GetEmployeesByName(nameParts[0], nameParts[1], nameParts[2]);

            return foundEmployees;
        }

        private IEnumerable<OwnerInfoDTO> GetEmployeesByName(string fname)
        {
            IEnumerable<OwnerInfoDTO> employees = (
                from
                    emp in _unitOfWork.Employees.GetAll()
                join
                    pos in _unitOfWork.Positions.GetAll()
                on
                    emp.PositionId equals pos.PositionId
                join
                    dep in _unitOfWork.Departments.GetAll()
                on
                    emp.DepartmentId equals dep.DepartmentId
                join
                    adm in _unitOfWork.Administrations.GetAll()
                on
                    dep.AdministrationId equals adm.AdministrationId
                where
                    emp.EmployeeFullName.IndexOf(fname, StringComparison.CurrentCultureIgnoreCase) >= 0
                select new OwnerInfoDTO
                {
                    EmployeeId = emp.EmployeeId,
                    FullName = emp.EmployeeFullName,
                    Room = emp.EmployeeRoom,
                    Position = pos.PositionName,
                    Department = dep.DepartmentName,
                    Administration = adm.AdministrationName
                }).Take(10);

            return employees;
        }

        private IEnumerable<OwnerInfoDTO> GetEmployeesByName(string fname, string lname)
        {
            IEnumerable<OwnerInfoDTO> employees = (
                from
                    emp in _unitOfWork.Employees.GetAll()
                join
                    pos in _unitOfWork.Positions.GetAll()
                on
                    emp.PositionId equals pos.PositionId
                join
                    dep in _unitOfWork.Departments.GetAll()
                on
                    emp.DepartmentId equals dep.DepartmentId
                join
                    adm in _unitOfWork.Administrations.GetAll()
                on
                    dep.AdministrationId equals adm.AdministrationId
                where
                    emp.EmployeeFullName.IndexOf(fname, StringComparison.CurrentCultureIgnoreCase) >= 0 &&
                    emp.EmployeeFullName.IndexOf(lname, StringComparison.CurrentCultureIgnoreCase) >= 0
                select new OwnerInfoDTO
                {
                    EmployeeId = emp.EmployeeId,
                    FullName = emp.EmployeeFullName,
                    Room = emp.EmployeeRoom,
                    Position = pos.PositionName,
                    Department = dep.DepartmentName,
                    Administration = adm.AdministrationName
                }).Take(10);

            return employees;
        }

        private IEnumerable<OwnerInfoDTO> GetEmployeesByName(string fname, string lname, string mname)
        {
            IEnumerable<OwnerInfoDTO> employees = (
                from
                    emp in _unitOfWork.Employees.GetAll()
                join
                    pos in _unitOfWork.Positions.GetAll()
                on
                    emp.PositionId equals pos.PositionId
                join
                    dep in _unitOfWork.Departments.GetAll()
                on
                    emp.DepartmentId equals dep.DepartmentId
                join
                    adm in _unitOfWork.Administrations.GetAll()
                on
                    dep.AdministrationId equals adm.AdministrationId
                where
                    emp.EmployeeFullName.IndexOf(fname, StringComparison.CurrentCultureIgnoreCase) >= 0 &&
                    emp.EmployeeFullName.IndexOf(lname, StringComparison.CurrentCultureIgnoreCase) >= 0 &&
                    emp.EmployeeFullName.IndexOf(mname, StringComparison.CurrentCultureIgnoreCase) >= 0
                select new OwnerInfoDTO
                {
                    EmployeeId = emp.EmployeeId,
                    FullName = emp.EmployeeFullName,
                    Room = emp.EmployeeRoom,
                    Position = pos.PositionName,
                    Department = dep.DepartmentName,
                    Administration = adm.AdministrationName
                }).Take(10);

            return employees;
        }
    }
}
