using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.Web.Models;
using Inventory.Web.Models.Account;

namespace Inventory.Web.MappingProfiles
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<ComponentDTO, ComponentVM>(MemberList.None).ReverseMap();
            CreateMap<ComponentTypeDTO, ComponentTypeVM>(MemberList.None).ReverseMap();
            CreateMap<EquipmentEmployeeRelationDTO, EquipmentEmployeeRelationVM>(MemberList.None).ReverseMap();
            CreateMap<EquipmentDTO, EquipmentVM>(MemberList.None).ReverseMap();
            CreateMap<EquipmentTypeDTO, EquipmentTypeVM>(MemberList.None).ReverseMap();
            CreateMap<HistoryDTO, HistoryVM>(MemberList.None).ReverseMap();
            CreateMap<OwnerInfoDTO, OwnerInfoVM>(MemberList.None).ReverseMap();
            CreateMap<RepairPlaceDTO, RepairPlaceVM>(MemberList.None).ReverseMap();
            CreateMap<StatusTypeDTO, StatusTypeVM>(MemberList.None).ReverseMap();
            CreateMap<UserDTO, UserVM>(MemberList.None).ReverseMap();
        }
    }
}