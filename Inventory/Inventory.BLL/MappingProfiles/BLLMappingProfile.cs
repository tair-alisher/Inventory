using AutoMapper;
using CatalogEntities;
using Inventory.BLL.DTO;
using Inventory.DAL.Entities;

namespace Inventory.BLL.MappingProfiles
{
    public class BLLMappingProfile : Profile
    {
        public BLLMappingProfile()
        {
            CreateMap<Component, ComponentDTO>(MemberList.None).ReverseMap();
            CreateMap<ComponentType, ComponentTypeDTO>(MemberList.None).ReverseMap();
            CreateMap<Employee, EmployeeDTO>(MemberList.None).ReverseMap();
            CreateMap<EquipmentComponentRelation, EquipmentComponentRelationDTO>(MemberList.None).ReverseMap();
            CreateMap<EquipmentEmployeeRelation, EquipmentEmployeeRelationDTO>(MemberList.None).ReverseMap();
            CreateMap<Equipment, EquipmentDTO>(MemberList.None).ReverseMap();
            CreateMap<EquipmentType, EquipmentTypeDTO>(MemberList.None).ReverseMap();
            CreateMap<History, HistoryDTO>(MemberList.None).ReverseMap();
            CreateMap<RepairPlace, RepairPlaceDTO>(MemberList.None).ReverseMap();
            CreateMap<ApplicationRole, RoleDTO>(MemberList.None).ReverseMap();
            CreateMap<ApplicationUser, UserDTO>(MemberList.None).ReverseMap();
            CreateMap<StatusType, StatusTypeDTO>(MemberList.None).ReverseMap();
        }
    }
}
