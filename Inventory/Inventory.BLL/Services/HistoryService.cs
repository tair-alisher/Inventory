using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BLL.Services
{
    public class HistoryService : IHistoryService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public HistoryService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public HistoryDTO Get(Guid id)
        {
            History history = _unitOfWork.History.Get(id);
            return BLLHistoryMapper.EntityToDto(history);
        }

        public IEnumerable<HistoryDTO> GetAll()
        {
            List<History> histories = _unitOfWork.History.GetAll().ToList();
            return BLLHistoryMapper.EntityToDto(histories);
        }

        public void Add(HistoryDTO item)
        {
            History history = BLLHistoryMapper.DtoToEntity(item);
            history.Id = Guid.NewGuid();
            history.ChangeDate = DateTime.Now;
            _unitOfWork.History.Create(history);
            _unitOfWork.Save();
        }

        public void Update(HistoryDTO item)
        {
            History history = BLLHistoryMapper.DtoToEntity(item);
            history.ChangeDate = DateTime.Now;
            _unitOfWork.History.Update(history);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            History history = _unitOfWork.History.Get(id);

            if (history == null)
                throw new NotFoundException();

            _unitOfWork.History.Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
