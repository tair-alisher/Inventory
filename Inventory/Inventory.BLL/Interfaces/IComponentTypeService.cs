using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IComponentTypeService : IService<ComponentTypeDTO>
    {
        IEnumerable<ComponentTypeDTO> GetListOrderedByName();
        ComponentTypeDTO Get(Guid? id);
    }
}
