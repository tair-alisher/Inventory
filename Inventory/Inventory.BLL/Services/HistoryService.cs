using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using AutoMapper;

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

            return Mapper.Map<HistoryDTO>(history);
        }

        public IEnumerable<HistoryDTO> GetAll()
        {
            List<History> histories = _unitOfWork.History.GetAll().ToList();

            return Mapper.Map<IEnumerable<HistoryDTO>>(histories);
        }

        public void Add(HistoryDTO historyDTO)
        {
            History history = Mapper.Map<History>(historyDTO);
            history.Id = Guid.NewGuid();
            history.ChangeDate = DateTime.Now;
            _unitOfWork.History.Create(history);
            _unitOfWork.Save();
        }

        public void Update(HistoryDTO historyDTO)
        {
            History history = Mapper.Map<History>(historyDTO);
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

        public IEnumerable<HistoryDTO> Filter(int pageNumber, int pageSize, IEnumerable<HistoryDTO> histories, string equipmentId, string employeeId, string repairPlaceId, string statusTypeId)
        {
            var rawData = (from e in GetAll()
                           select e).ToList();
            var employee = from e in rawData
                           select e;

            if (!String.IsNullOrEmpty(equipmentId))
                histories = histories.Where(e => e.Equipment.Id.ToString() == equipmentId).ToPagedList(pageNumber, pageSize);

            if (!String.IsNullOrEmpty(employeeId))
                histories = histories.Where(e => e.Employee.EmployeeId.ToString() == employeeId).ToPagedList(pageNumber, pageSize);

            if (!String.IsNullOrEmpty(repairPlaceId))
                histories = histories.Where(e => e.RepairPlace.Id.ToString() == repairPlaceId).ToPagedList(pageNumber, pageSize);
            if (!String.IsNullOrEmpty(statusTypeId))
                histories = histories.Where(e => e.StatusType.Id.ToString() == statusTypeId).ToPagedList(pageNumber, pageSize);

            return histories;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
