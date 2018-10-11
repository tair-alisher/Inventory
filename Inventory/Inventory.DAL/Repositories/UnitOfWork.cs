using Inventory.DAL.EF;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using CatalogEntities;
using System;

namespace Inventory.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private BaseRepository<Component> componentRepository;
        private BaseRepository<ComponentType> componentTypeRepository;
        private BaseRepository<Equipment> equipmentRepository;
        private BaseRepository<EquipmentComponentRelation> equipmentComponentRelationsRepository;
        private BaseRepository<EquipmentEmployeeRelation> equipmentEmployeeRelationsRepository;
        private BaseRepository<EquipmentType> equipmentTypeRepository;
        private BaseRepository<History> historyRepository;
        private BaseRepository<RepairPlace> repairPlaceRepository;
        private BaseRepository<StatusType> statusTypeRepository;

        private PartialRepository<Employee> employeeRepository;

        private InventoryContext context;

        public UnitOfWork(string connectionString)
        {
            context = new InventoryContext(connectionString);
        }

        public IRepository<Component> Components
        {
            get
            {
                if (componentRepository == null)
                    componentRepository = new BaseRepository<Component>(context);
                return componentRepository;
            }
        }

        public IRepository<ComponentType> ComponentTypes
        {
            get
            {
                if (componentTypeRepository == null)
                    componentTypeRepository = new BaseRepository<ComponentType>(context);
                return componentTypeRepository;
            }
        }

        public IRepository<Equipment> Equipments
        {
            get
            {
                if (equipmentRepository == null)
                    equipmentRepository = new BaseRepository<Equipment>(context);
                return equipmentRepository;
            }
        }

        public IRepository<EquipmentComponentRelation> EquipmentComponentRelations
        {
            get
            {
                if (equipmentComponentRelationsRepository == null)
                    equipmentComponentRelationsRepository = new BaseRepository<EquipmentComponentRelation>(context);
                return equipmentComponentRelationsRepository;
            }
        }

        public IRepository<EquipmentEmployeeRelation> EquipmentEmployeeRelations
        {
            get
            {
                if (equipmentEmployeeRelationsRepository == null)
                    equipmentEmployeeRelationsRepository = new BaseRepository<EquipmentEmployeeRelation>(context);
                return equipmentEmployeeRelationsRepository;
            }
        }

        public IRepository<EquipmentType> EquipmentTypes
        {
            get
            {
                if (equipmentTypeRepository == null)
                    equipmentTypeRepository = new BaseRepository<EquipmentType>(context);
                return equipmentTypeRepository;
            }
        }

        public IRepository<History> History
        {
            get
            {
                if (historyRepository == null)
                    historyRepository = new BaseRepository<History>(context);
                return historyRepository;
            }
        }

        public IRepository<RepairPlace> RepairPlaces
        {
            get
            {
                if (repairPlaceRepository == null)
                    repairPlaceRepository = new BaseRepository<RepairPlace>(context);
                return repairPlaceRepository;
            }
        }

        public IRepository<StatusType> StatusTypes
        {
            get
            {
                if (statusTypeRepository == null)
                    statusTypeRepository = new BaseRepository<StatusType>(context);
                return statusTypeRepository;
            }
        }

        public IPartialRepository<Employee> Employees
        {
            get
            {
                if (employeeRepository == null)
                    employeeRepository = new PartialRepository<Employee>(context);
                return employeeRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    context.Dispose();
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
