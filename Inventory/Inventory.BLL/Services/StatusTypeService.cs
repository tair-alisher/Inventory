﻿using AutoMapper;
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
    public class StatusTypeService : IStatusTypeService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public StatusTypeService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public StatusTypeDTO Get(Guid id)
        {
            StatusType statusType = _unitOfWork.StatusTypes.Get(id);

            return Mapper.Map<StatusTypeDTO>(statusType);
        }

        public IEnumerable<StatusTypeDTO> GetAll()
        {
            List<StatusType> statusTypes = _unitOfWork.StatusTypes.GetAll().ToList();

            return Mapper.Map<IEnumerable<StatusTypeDTO>>(statusTypes);
        }

        public void Add(StatusTypeDTO statusTypeDTO)
        {
            StatusType statusType = Mapper.Map<StatusType>(statusTypeDTO);
            statusType.Id = Guid.NewGuid();

            _unitOfWork.StatusTypes.Create(statusType);
            _unitOfWork.Save();
        }

        public void Update(StatusTypeDTO statusTypeDTO)
        {
            StatusType statusType = Mapper.Map<StatusType>(statusTypeDTO);

            _unitOfWork.StatusTypes.Update(statusType);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            if (HasRelations(id))
                throw new HasRelationsException();

            StatusType statusType = _unitOfWork.StatusTypes.Get(id);
            if (statusType == null)
                throw new NotFoundException();

            _unitOfWork.StatusTypes.Delete(id);
            _unitOfWork.Save();
        }

        public bool HasRelations(Guid id)
        {
            var relations = _unitOfWork.History.Find(h => h.StatusTypeId == id);

            return relations.Count() > 0;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
