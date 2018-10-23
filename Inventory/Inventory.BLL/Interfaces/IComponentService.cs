using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IComponentService : IService<ComponentDTO>
    {
        Guid AddAndGetId(ComponentDTO component);
        IEnumerable<ComponentDTO> GetComponentsBy(string type, string value);
    }
}
