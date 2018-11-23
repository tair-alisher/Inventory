using Inventory.BLL.DTO;
using System.Collections.Generic;
using PagedList;

namespace Inventory.BLL.Interfaces
{
    public interface IHistoryService : IService<HistoryDTO>
    {
        IEnumerable<HistoryDTO> Filter(int pageNumber, int pageSize, IEnumerable<HistoryDTO> histories, string equipmentId, string employeeId, string repairPlaceId, string statusTypeId);
    }
}
