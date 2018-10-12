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
            Component component = BLLComponentMapper.DtoToEntity(item);
            component.Id = Guid.NewGuid();

            _unitOfWork.Components.Create(component);
            _unitOfWork.Save();
        }

        public void Update(ComponentDTO item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            Component component = _unitOfWork.Components.Get(id);
            if (component == null)
                throw new NotFoundException();

            _unitOfWork.Components.Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
