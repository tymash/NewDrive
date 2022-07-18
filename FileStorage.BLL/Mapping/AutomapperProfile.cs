using AutoMapper;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Models.UserModels;
using FileStorage.DAL.Entities;
using File = FileStorage.DAL.Entities.File;

namespace FileStorage.BLL.Mapping;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<User, UserViewModel>()
            .ForMember(um => um.FilesIds, mo => mo.MapFrom(u => u.Files.Select(si => si.Id)))
            .ReverseMap()
            .ForPath(u => u.UserName, cfg => cfg.MapFrom(um => um.Email));
        
        CreateMap<User, UserEditModel>()
            .ReverseMap()
            .ForPath(u => u.UserName, cfg => cfg.MapFrom(um => um.Email));
        
        CreateMap<User, UserLoginModel>()
            .ReverseMap();
        
        CreateMap<User, UserRegisterModel>()
            .ReverseMap()
            .ForPath(u => u.UserName, cfg => cfg.MapFrom(um => um.Email));
        
        CreateMap<User, UserChangePasswordModel>()
            .ReverseMap();

        CreateMap<File, FileViewModel>()
            .ForMember(sim => sim.UserId, mo => mo.MapFrom(si => si.UserId))
            .ForMember(sim => sim.User, mo => mo.MapFrom(si => si.User))
            .ReverseMap();
        
        CreateMap<File, FileCreateModel>()
            .ReverseMap();
        
        CreateMap<File, FileEditModel>()
            .ReverseMap();
    }
}