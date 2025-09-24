using Application.Commands.ChatRooms.AddChatRoom;
using Application.Commands.ChatRooms.AddRoomMember;
using Application.Commands.Tenants.CreateTenant;
using Application.Commands.Users.AddUser;
using Application.Commands.Workspaces.AddWorkspace;
using Application.Queries.ChatRooms;
using Application.Queries.Messages.GetMessages;
using Application.Queries.Users;
using Application.Queries.Workspaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Common
{
    public class ApplicationAutoMapperProfile : Profile
    {
        public ApplicationAutoMapperProfile()
        {
            CreateMap<AddChatRoomRequest, ChatRoom>();

            CreateMap<AddRoomMembersRequest, RoomMember>();

            CreateMap<AddWorkspaceRequest, Workspace>();

            CreateMap<AddUserRequest, User>();

            // Map RoomMember entity to GetChatRoomDto
            CreateMap<RoomMember, GetChatRoomDto>()
     .ForMember(d => d.ChatRoomId, o => o.MapFrom(s => s.ChatRoom.Id));

            CreateMap<RoomMember, GetUserDto>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.ProfilePicUrl, opt => opt.MapFrom(src => src.User.ProfileImageUrl))
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.IsAdmin));

            CreateMap<ChatMessage, MessageDto>();

            CreateMap<User, GetUserDto>();

            CreateMap<Workspace, GetWorkspaceDto>();

            CreateMap<CreateTenantRequest, Tenant>();
        }
    }
}
