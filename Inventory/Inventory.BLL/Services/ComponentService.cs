using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;

namespace Inventory.BLL.Services
{
    public class ComponentService : IComponentService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public ComponentService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public ComponentDTO Get(Guid id)
        {
            Component component = _unitOfWork.Components.Get(id);

            return BLLComponentMapper.EntityToDto(component);
        }

        public IEnumerable<ComponentDTO> GetAll()
        {
            List<Component> components = _unitOfWork.Components.GetAll().ToList();

            return BLLComponentMapper.EntityToDto(components);
        }

        public void Add(ComponentDTO item)
        {
            AddAndGetId(item);
        }

        public Guid AddAndGetId(ComponentDTO componentDTO)
        {
            Component component = BLLComponentMapper.DtoToEntity(componentDTO);
            component.Id = Guid.NewGuid();

            _unitOfWork.Components.Create(component);
            _unitOfWork.Save();

            return component.Id;
        }

        public void Update(ComponentDTO item)
        {
            Component component = BLLComponentMapper.DtoToEntity(item);

            _unitOfWork.Components.Update(component);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            if (HasRelations(id))
                throw new HasRelationsException();

            Component component = _unitOfWork.Components.Get(id);
            if (component == null)
                throw new NotFoundException();

            _unitOfWork.Components.Delete(id);
            _unitOfWork.Save();
        }

        public bool HasRelations(Guid id)
        {
            var relations = _unitOfWork.EquipmentComponentRelations.Find(r => r.ComponentId == id);

            return relations.Count() > 0;
        }

        public IEnumerable<ComponentDTO> GetComponentsBy(string type, string value)
        {
            IEnumerable<ComponentDTO> components = Enumerable.Empty<ComponentDTO>();
            switch (type)
            {
                case "type":
                    components = GetComponentsByType(value);
                    break;
                case "model":
                    components = GetComponentsByModel(value);
                    break;
                case "number":
                    components = GetComponentsByNumber(value);
                    break;
            }

            return components;
        }

        private IEnumerable<ComponentDTO> GetComponentsByType(string value)
        {
            IEnumerable<Component> components = (
                from
                    com in _unitOfWork.Components.GetAll()
                join
                    type in _unitOfWork.ComponentTypes.GetAll()
                on
                    com.ComponentTypeId equals type.Id
                where
                    type.Name == value
                select com);

            if (components.Count() <= 0)
                return Enumerable.Empty<ComponentDTO>();

            return BLLComponentMapper.EntityToDto(components);
        }

        private IEnumerable<ComponentDTO> GetComponentsByModel(string value)
        {
            IEnumerable<Component> components = _unitOfWork
                .Components
                .Find(c => c.ModelName == value);

            if (components.Count() <= 0)
                return Enumerable.Empty<ComponentDTO>();

            return BLLComponentMapper.EntityToDto(components);
        }

        private IEnumerable<ComponentDTO> GetComponentsByNumber(string value)
        {
            IEnumerable<Component> components = _unitOfWork
                .Components
                .Find(c => c.InventNumber == value);

            if (components.Count() <= 0)
                return Enumerable.Empty<ComponentDTO>();

            return BLLComponentMapper.EntityToDto(components);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
