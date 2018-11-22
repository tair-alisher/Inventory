using Inventory.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Inventory.BLL.Interfaces
{
    public interface IComponentService : IService<ComponentDTO>
    {
        Guid AddAndGetId(ComponentDTO component);
        ComponentDTO Get(Guid? id);
        IEnumerable<ComponentDTO> GetComponentsBy(string type, string value);
        IEnumerable<ComponentDTO> Filter(int pageNumber, int pageSize, IEnumerable<ComponentDTO> components, string componentTypeId, string modelName, string name);
        IEnumerable<ComponentDTO> GetFilteredList(FilterParamsDTO parameters);
    }
}
