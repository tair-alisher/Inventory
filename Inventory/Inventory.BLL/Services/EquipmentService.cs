using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using Inventory.BLL.Infrastructure;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZXing;

namespace Inventory.BLL.Services
{
    public class EquipmentService : IEquipmentService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public EquipmentService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public void Add(EquipmentDTO item)
        {
            AddAndGetId(item);
        }

        public Guid AddAndGetId(EquipmentDTO equipmentDTO)
        {
            Equipment equipment = BLLEquipmentMapper.DtoToEntity(equipmentDTO);
            equipment.Id = Guid.NewGuid();

            string url = $"http://localhost:58644/Equipment/Details/{equipment.Id}";
            equipment.QRCode = GenerateQRCode(url, equipment.Id);

            _unitOfWork.Equipments.Create(equipment);
            _unitOfWork.Save();

            return equipment.Id;
        }
        private string GenerateQRCode(string qrcodeText, Guid id)
        {
            string folderPath = "~/Content/Images/";
            string imagePath = $"/Content/Images/{id}.jpg";
            // If the directory doesn't exist then create it.
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(folderPath)))
            {
                Directory.CreateDirectory(folderPath);
            }

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            string barcodePath = HttpContext.Current.Server.MapPath(imagePath);
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            return imagePath;
        }


        public EquipmentDTO Get(Guid id)
        {
            Equipment equipment = _unitOfWork.Equipments.Get(id);

            return BLLEquipmentMapper.EntityToDto(equipment);
        }

        public IEnumerable<EquipmentDTO> GetAll()
        {
            List<Equipment> equipments = _unitOfWork.Equipments.GetAll().ToList();

            return BLLEquipmentMapper.EntityToDto(equipments);
        }

        public void Update(EquipmentDTO item)
        {
            Equipment equipment = _unitOfWork.Equipments.Get(item.Id);
            equipment.EquipmentTypeId = item.EquipmentTypeId;
            equipment.EquipmentType = _unitOfWork.EquipmentTypes.Get(item.EquipmentTypeId);
            equipment.InventNumber = item.InventNumber;
            equipment.QRCode = item.QRCode;
            equipment.Price = item.Price;
            equipment.Supplier = item.Supplier;

            _unitOfWork.Equipments.Update(equipment);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            if (HasRelations(id))
                throw new HasRelationsException();

            Equipment equipment = _unitOfWork.Equipments.Get(id);
            if (equipment == null)
                throw new NotFoundException();

            _unitOfWork.Equipments.Delete(id);
            _unitOfWork.Save();
        }

        public bool HasRelations(Guid id)
        {
            if (HasRelationsWithEmployees(id))
                return true;
            if (HasRelationsWithComponents(id))
                return true;

            return false;
        }

        private bool HasRelationsWithEmployees(Guid id)
        {
            var relations = _unitOfWork.EquipmentEmployeeRelations.Find(r => r.EquipmentId == id);

            return relations.Count() > 0;
        }

        private bool HasRelationsWithComponents(Guid id)
        {
            var relations = _unitOfWork.EquipmentComponentRelations.Find(r => r.EquipmentId == id);

            return relations.Count() > 0;
        }

        //private bool HasRelationsWithComponents(Guid id)
        //{
        //    var relations = _unitOfWork.EquipmentComponentRelations.Find(r => r.EquipmentId == id);

        //    return relations.Count() > 0;
        //}

        public IEnumerable<OwnerInfoDTO> GetOwnerHistory(Guid id)
        {
            IEnumerable<int> equipmentEmployeeIds = _unitOfWork
                .EquipmentEmployeeRelations
                .Find(e => e.EquipmentId == id)
                .Select(emp => emp.EmployeeId)
                .ToList();

            if (equipmentEmployeeIds.Count() <= 0)
                return Enumerable.Empty<OwnerInfoDTO>();

            IEnumerable<OwnerInfoDTO> ownerHistory = (
                from
                    relation in _unitOfWork.EquipmentEmployeeRelations.GetAll()
                join
                    emp in _unitOfWork.Employees.GetAll()
                on
                    relation.EmployeeId equals emp.EmployeeId
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
                    relation.EquipmentId == id
                select new OwnerInfoDTO
                {
                    EmployeeId = emp.EmployeeId,
                    FullName = emp.EmployeeFullName,
                    Room = emp.EmployeeRoom,
                    Position = pos.PositionName,
                    Department = dep.DepartmentName,
                    Administration = adm.AdministrationName,
                    StartDate = relation.CreatedAt,
                    EndDate = relation.UpdatedAt,
                    IsActual = relation.IsOwner
                }).OrderBy(o => o.StartDate);

            return ownerHistory;
        }

        public OwnerInfoDTO GetOwnerInfo(Guid equipmentId, int EmployeeId)
        {
            OwnerInfoDTO ownerInfoDTO = (
                from
                    relation in _unitOfWork.EquipmentEmployeeRelations.GetAll()
                join
                    emp in _unitOfWork.Employees.GetAll()
                on
                    relation.EmployeeId equals emp.EmployeeId
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
                join
                    div in _unitOfWork.Divisions.GetAll()
                on
                    adm.DivisionId equals div.DivisionId
                where relation.EquipmentId == equipmentId &&
                    relation.EmployeeId == EmployeeId
                select new OwnerInfoDTO
                {
                    EmployeeId = emp.EmployeeId,
                    FullName = emp.EmployeeFullName,
                    Room = emp.EmployeeRoom,
                    Position = pos.PositionName,
                    Department = dep.DepartmentName,
                    Administration = adm.AdministrationName,
                    Division = div.DivisionName,
                    StartDate = relation.CreatedAt,
                    EndDate = relation.UpdatedAt,
                    IsActual = relation.IsOwner
                }).First();

            return ownerInfoDTO;
        }

        public IEnumerable<ComponentDTO> GetComponents(Guid id)
        {

            IEnumerable<Guid> equipmentComponentIds = _unitOfWork
                .EquipmentComponentRelations
                .Find(e => e.EquipmentId == id)
                .Select(com => com.ComponentId);

            if (equipmentComponentIds.Count() <= 0)
                return Enumerable.Empty<ComponentDTO>();

            IEnumerable<ComponentDTO> components = (
                from
                    relation in _unitOfWork.EquipmentComponentRelations.GetAll()
                join
                    component in _unitOfWork.Components.GetAll()
                on
                    relation.ComponentId equals component.Id
                join
                    comtype in _unitOfWork.ComponentTypes.GetAll()
                on
                    component.ComponentTypeId equals comtype.Id
                where
                    relation.EquipmentId == id
                select new ComponentDTO
                {
                    Id = component.Id,
                    ComponentTypeId = component.ComponentTypeId,
                    ModelName = component.ModelName,
                    Name = component.Name,
                    Description = component.Description,
                    Price = component.Price,
                    InventNumber = component.InventNumber,
                    Supplier = component.Supplier,
                    ComponentType = new ComponentTypeDTO
                    {
                        Id = comtype.Id,
                        Name = comtype.Name
                    }
                });

            return components;
        }

        public IEnumerable<DivisionEquipmentDTO> GetEquipmentByStructure()
        {
            IEnumerable<DivisionEquipmentDTO> equipments = (
                from
                    div in _unitOfWork.Divisions.GetAll()
                select new DivisionEquipmentDTO
                {
                    Divisionid = div.DivisionId,
                    DivisionName = div.DivisionName,
                    Administrations = GetDivisionAdministrations(div.DivisionId)
                });

            return equipments;
        }

        private List<AdministrationEquipmentDTO> GetDivisionAdministrations(int divisionId)
        {
            return (
                from
                    adm in _unitOfWork.Administrations.GetAll()
                where
                    adm.DivisionId == divisionId
                select new AdministrationEquipmentDTO
                {
                    AdministrationId = adm.AdministrationId,
                    AdministrationName = adm.AdministrationName,
                    Departments = GetAdministrationDepartments(adm.AdministrationId)
                }).ToList();
        }

        private List<DepartmentEquipmentDTO> GetAdministrationDepartments(int administrationId)
        {
            return (
                from
                    dep in _unitOfWork.Departments.GetAll()
                where
                    dep.AdministrationId == administrationId
                select new DepartmentEquipmentDTO
                {
                    DepartmentId = dep.DepartmentId,
                    DepartmentName = dep.DepartmentName,
                    Equipments = GetDepartmentEquipment(dep.DepartmentId)
                }).ToList();
        }

        private List<StructuredEquipmentDTO> GetDepartmentEquipment(int departmentId)
        {
            return (
                from
                    relation in _unitOfWork.EquipmentEmployeeRelations.GetAll()
                join
                    equip in _unitOfWork.Equipments.GetAll()
                on
                    relation.EquipmentId equals equip.Id
                join
                    eq_type in _unitOfWork.EquipmentTypes.GetAll()
                on
                    equip.EquipmentTypeId equals eq_type.Id
                join
                    emp in _unitOfWork.Employees.GetAll()
                on
                    relation.EmployeeId equals emp.EmployeeId
                where
                    emp.DepartmentId == departmentId && relation.IsOwner == true
                select new StructuredEquipmentDTO
                {
                    Id = equip.Id,
                    EquipmentType = eq_type.Name,
                    InventNumber = equip.InventNumber
                }).ToList();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
