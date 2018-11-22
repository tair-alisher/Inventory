using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IEquipmentTypeService : IService<EquipmentTypeDTO>
    {
        IEnumerable<EquipmentTypeDTO> GetListOrderedByName();
        EquipmentTypeDTO Get(Guid? id);
    }
}
