using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;

namespace Inventory.BLL.Services
{
    public class ComponentTypeService : IComponentTypeService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public ComponentTypeService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public ComponentTypeDTO Get(Guid id)
        {
            ComponentType componentType = _unitOfWork.ComponentTypes.Get(id);

            return Mapper.Map<ComponentTypeDTO>(componentType);
        }

        public IEnumerable<ComponentTypeDTO> GetAll()
        {
            List<ComponentType> componentTypes = _unitOfWork.ComponentTypes.GetAll().ToList();

            return Mapper.Map<IEnumerable<ComponentTypeDTO>>(componentTypes);
        }

        public void Add(ComponentTypeDTO componentTypeDTO)
        {
            ComponentType componentType = Mapper.Map<ComponentType>(componentTypeDTO);
            componentType.Id = Guid.NewGuid();

            _unitOfWork.ComponentTypes.Create(componentType);
            _unitOfWork.Save();
        }

        public void Update(ComponentTypeDTO componentTypeDTO)
        {
            ComponentType componentType = Mapper.Map<ComponentType>(componentTypeDTO);

            _unitOfWork.ComponentTypes.Update(componentType);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            if (HasRelations(id))
                throw new HasRelationsException();

            ComponentType componentType = _unitOfWork.ComponentTypes.Get(id);
            if (componentType == null)
                throw new NotFoundException();

            _unitOfWork.ComponentTypes.Delete(id);
            _unitOfWork.Save();
        }

        public bool HasRelations(Guid id)
        {
            var relations = _unitOfWork.Components.Find(c => c.ComponentTypeId == id);

            return relations.Count() > 0;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
