using System;
using System.Collections.Generic;

namespace Inventory.BLL.DTO
{
    public class DivisionEquipmentDTO
    {
        public int Divisionid { get; set; }
        public string DivisionName { get; set; }
        public IEnumerable<AdministrationEquipmentDTO> Administrations { get; set; }
    }

    public class AdministrationEquipmentDTO
    {
        public int AdministrationId { get; set; }
        public string AdministrationName { get; set; }
        public IEnumerable<DepartmentEquipmentDTO> Departments { get; set; }
    }

    public class DepartmentEquipmentDTO
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public IEnumerable<StructuredEquipmentDTO> Equipments { get; set; }
    }

    public class StructuredEquipmentDTO
    {
        public Guid Id { get; set; }
        public string EquipmentType { get; set; }
        public string InventNumber { get; set; }
        public string Supplier { get; set; }
        public IEnumerable<ComponentDTO> Components { get; set; }
        public IEnumerable<OwnerInfoDTO> Owners { get; set; }
    }
}
