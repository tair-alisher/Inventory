using Inventory.DAL.Entities;
using System.Data.Entity;

namespace Inventory.DAL.EF
{
    public class InventoryContext : DbContext
    {
        public DbSet<Component> Components { get; set; }
        public DbSet<ComponentType> ComponentTypes { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentComponentRelation> EquipmentComponentRelations { get; set; }
        public DbSet<EquipmentEmployeeRelation> EquipmentEmployeeRelations { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<RepairPlace> RepairPlaces { get; set; }
        public DbSet<StatusType> StatusTypes { get; set; }
        
        public DbSet<CatalogEntities.Administration> Administrations { get; set; }
        public DbSet<CatalogEntities.Department> Departments { get; set; }
        public DbSet<CatalogEntities.Division> Divisions { get; set; }
        public DbSet<CatalogEntities.Employee> Employees { get; set; }
        public DbSet<CatalogEntities.Position> Positions { get; set; }

        public InventoryContext(string connectionString) : base(connectionString)
		{
			Database.SetInitializer<InventoryContext>(null);
		}
    }
}
