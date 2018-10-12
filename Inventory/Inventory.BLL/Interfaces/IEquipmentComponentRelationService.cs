using Inventory.BLL.DTO;
using System;

namespace Inventory.BLL.Interfaces
{
    public interface IEquipmentComponentRelationService : IService<EquipmentComponentRelationDTO>
    {
        void Create(Guid componentId, Guid equipmentId);
    }
}
