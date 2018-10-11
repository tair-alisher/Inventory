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

			return BLLComponentTypeMapper.EntityToDto(componentType);
		}

		public IEnumerable<ComponentTypeDTO> GetAll()
		{
			List<ComponentType> componentTypes = _unitOfWork.ComponentTypes.GetAll().ToList();

			return BLLComponentTypeMapper.EntityToDto(componentTypes);
		}

		public void Add(ComponentTypeDTO item)
		{
			ComponentType componentType = BLLComponentTypeMapper.DtoToEntity(item);

			_unitOfWork.ComponentTypes.Create(componentType);
			_unitOfWork.Save();
		}

		public void Delete(Guid id)
		{
			ComponentType componentType = _unitOfWork.ComponentTypes.Get(id);
			if (componentType == null)
				throw new NotFoundException();

			_unitOfWork.ComponentTypes.Delete(id);
			_unitOfWork.Save();
		}

		public void Dispose()
		{
			_unitOfWork.Dispose();
		}
	}
}
