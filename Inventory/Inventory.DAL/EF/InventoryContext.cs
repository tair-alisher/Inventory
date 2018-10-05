using Inventory.DAL.Entities;
using System.Data.Entity;

namespace Inventory.DAL.EF
{
    public class InventoryContext : DbContext
    {
        public DbSet<Component> Components { get; set; }
        public DbSet<ComponentType> ComponentTypes { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentComponent> EquipmentComponent { get; set; }
        public DbSet<EquipmentEmployee> EquipmentEmployee { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<RepairPlace> RepairPlaces { get; set; }
        public DbSet<StatusType> StatusTypes { get; set; }

        public InventoryContext(string connectionString) : base(connectionString) { }
    }
}
