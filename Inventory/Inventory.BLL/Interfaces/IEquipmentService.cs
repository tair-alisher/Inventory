using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IEquipmentService : IService<EquipmentDTO>
    {
        IEnumerable<OwnerInfoDTO> GetOwnerHistory(Guid id);
        Guid AddAndGetId(EquipmentDTO equipment);
    }
}
