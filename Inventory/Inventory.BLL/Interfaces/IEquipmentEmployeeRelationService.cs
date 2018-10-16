using Inventory.BLL.DTO;
using System;

namespace Inventory.BLL.Interfaces
{
    public interface IEquipmentEmployeeRelationService : IService<EquipmentEmployeeRelationDTO>
    {
        void Create(Guid equipmentId, int employeeId);
        void Create(Guid equipmentId, string[] employeeIds);
        void DeleteEquipmentRelations(Guid id);
        void SetOwner(Guid equipmentId, int employeeId);
    }
}
