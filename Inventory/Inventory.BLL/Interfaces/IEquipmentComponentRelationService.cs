using Inventory.BLL.DTO;
using System;

namespace Inventory.BLL.Interfaces
{
    public interface IEquipmentComponentRelationService : IService<EquipmentComponentRelationDTO>
    {
        void Create(Guid equipmentId, Guid componentId);
        void UpdateEquipmentRelations(Guid equipmentId, string[] componentIds);
        void DeleteRelationsByEquipmentId(Guid id);
    }
}
