using Inventory.BLL.DTO;
using System;

namespace Inventory.BLL.Interfaces
{
    public interface IEquipmentEmployeeRelationService : IService<EquipmentEmployeeRelationDTO>
    {
        EquipmentEmployeeRelationDTO GetByEquipmentAndEmployee(Guid equipmentId, int employeeId);
        void Create(Guid equipmentId, int employeeId);
        void Create(Guid equipmentId, string[] employeeIds);
        void UpdateDates(EquipmentEmployeeRelationDTO relation);
        void UpdateEquipmentRelations(Guid equipmentId, string[] employeeIds);
        void DeleteRelationsByEquipmentId(Guid id);
        void SetOwner(Guid equipmentId, int employeeId);
        void UnsetOwner(Guid equipmentId);
        void ResetOwner(Guid equipmentId, int employeeId);
    }
}
