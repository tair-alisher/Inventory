using AutoMapper;
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
    public class RepairPlaceService : IRepairPlaceService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public RepairPlaceService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public RepairPlaceDTO Get(Guid id)
        {
            RepairPlace repairPlace = _unitOfWork.RepairPlaces.Get(id);

            return Mapper.Map<RepairPlaceDTO>(repairPlace);
        }

        public IEnumerable<RepairPlaceDTO> GetAll()
        {
            List<RepairPlace> repairPlaces = _unitOfWork.RepairPlaces.GetAll().ToList();

            return Mapper.Map<IEnumerable<RepairPlaceDTO>>(repairPlaces);
        }

        public void Add(RepairPlaceDTO repairPlaceDTO)
        {
            RepairPlace repairPlace = Mapper.Map<RepairPlace>(repairPlaceDTO);
            repairPlace.Id = Guid.NewGuid();

            _unitOfWork.RepairPlaces.Create(repairPlace);
            _unitOfWork.Save();
        }

        public void Update(RepairPlaceDTO repairPlaceDTO)
        {
            RepairPlace repairPlace = Mapper.Map<RepairPlace>(repairPlaceDTO);

            _unitOfWork.RepairPlaces.Update(repairPlace);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            if (HasRelations(id))
                throw new HasRelationsException();

            RepairPlace repairPlace = _unitOfWork.RepairPlaces.Get(id);
            if (repairPlace == null)
                throw new NotFoundException();

            _unitOfWork.RepairPlaces.Delete(id);
            _unitOfWork.Save();
        }

        public bool HasRelations(Guid id)
        {
            var relations = _unitOfWork.History.Find(h => h.RepairPlaceId == id);

            return relations.Count() > 0;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
