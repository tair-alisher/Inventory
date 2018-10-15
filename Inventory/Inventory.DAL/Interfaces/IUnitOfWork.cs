using Inventory.DAL.Entities;
using CatalogEntities;
using System;

namespace Inventory.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Component> Components { get; }
        IRepository<ComponentType> ComponentTypes { get; }
        IRepository<Equipment> Equipments { get; }
        IRepository<EquipmentComponentRelation> EquipmentComponentRelations { get; }
        IRepository<EquipmentEmployeeRelation> EquipmentEmployeeRelations { get; }
        IRepository<EquipmentType> EquipmentTypes { get; }
        IRepository<History> History { get; }
        IRepository<RepairPlace> RepairPlaces { get; }
        IRepository<StatusType> StatusTypes { get; }

        IPartialRepository<Employee> Employees { get; }
        IPartialRepository<Position> Positions { get; }
        IPartialRepository<Department> Departments { get; }
        IPartialRepository<Administration> Administrations { get; }
        IPartialRepository<Division> Divisions { get; }


        void Save();
    }
}
