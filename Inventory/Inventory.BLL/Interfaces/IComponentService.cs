using Inventory.BLL.DTO;
using System;

namespace Inventory.BLL.Interfaces
{
    public interface IComponentService : IService<ComponentDTO>
    {
        Guid AddAndGetId(ComponentDTO component);
    }
}
