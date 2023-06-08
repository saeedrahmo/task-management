using AutoMapper;
using TaskManagement.Core.DTOs;

namespace TaskManagement.Core.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Task, PagedTaskListDto>()
              .ForMember(dest => dest.Assignee, opt =>
              {
                  opt.MapFrom(src => src.User != null ? src.User.UserName : null);
              });

            CreateMap<Models.Task, ReadTaskDto>()
              .ForMember(dest => dest.Assignee, opt =>
              {
                  opt.MapFrom(src => src.User != null ? src.User.UserName : null);
              });

            CreateMap<Models.Task, DeleteTaskDto>()
              .ForMember(dest => dest.Assignee, opt =>
              {
                  opt.MapFrom(src => src.User != null ? src.User.UserName : null);
              });

            CreateMap<Models.Task, UpdateTaskDto>();

            CreateMap<UpdateTaskDto, Models.Task>();


            CreateMap<Models.ApplicationUser, UserListDto>();

            CreateMap<CreateTaskDto, Models.Task>();
        }
    }
}
