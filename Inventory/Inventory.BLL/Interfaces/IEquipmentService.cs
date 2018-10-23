using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IEquipmentService : IService<EquipmentDTO>
    {
        Guid AddAndGetId(EquipmentDTO equipment);
        IEnumerable<OwnerInfoDTO> GetOwnerHistory(Guid id);
        IEnumerable<ComponentDTO> GetComponents(Guid id);
    }
}
