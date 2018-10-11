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
		MapperConfiguration config;
		public ComponentTypeService(IUnitOfWork uow)
		{
			_unitOfWork = uow;
		}

		public ComponentTypeDTO Get(Guid id)
		{
			ComponentType componentType = _unitOfWork.ComponentTypes.Get(id);
			config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentType, ComponentTypeDTO>());

			return config.CreateMapper().Map<ComponentTypeDTO>(componentType);
		}

		public IEnumerable<ComponentTypeDTO> GetAll()
		{
			List<ComponentType> componentTypes = _unitOfWork.ComponentTypes.GetAll().ToList();
			config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentType, ComponentTypeDTO>());

			return config.CreateMapper().Map<List<ComponentTypeDTO>>(componentTypes);
		}

		public void Add(ComponentTypeDTO item)
		{
			config = new MapperConfiguration(cfg => cfg.CreateMap<ComponentTypeDTO, ComponentType>());

			ComponentType componentType = config.CreateMapper().Map<ComponentType>(item);

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
