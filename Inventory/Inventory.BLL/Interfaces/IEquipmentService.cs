using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IEquipmentService : IService<EquipmentDTO>
    {
        EquipmentDTO Get(Guid? id);
        Guid AddAndGetId(EquipmentDTO equipment);
        IEnumerable<OwnerInfoDTO> GetOwnerHistory(Guid id);
        OwnerInfoDTO GetOwnerInfo(Guid? equipmentId, int? employeeId);
        IEnumerable<ComponentDTO> GetComponents(Guid? id);
        IEnumerable<DivisionEquipmentDTO> GetEquipmentByStructure();
    }
}
