using AutoMapper;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Models.FolderModels;
using FileStorage.BLL.Models.UserModels;
using FileStorage.DAL.Entities;
using File = FileStorage.DAL.Entities.File;

namespace FileStorage.BLL.Mapping;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<User, UserViewModel>()
            .ForMember(um => um.FoldersIds, mo => mo.MapFrom(u => u.Folders.Select(f => f.Id)))
            .ForMember(um => um.FilesIds, mo => mo.MapFrom(u => u.Files.Select(si => si.Id)))
            .ReverseMap();
        
        CreateMap<User, UserEditModel>()
            .ReverseMap();
        
        CreateMap<User, UserLoginModel>()
            .ReverseMap();
        
        CreateMap<User, UserRegisterModel>()
            .ReverseMap();
        
        CreateMap<User, UserChangePasswordModel>()
            .ReverseMap();
        
        CreateMap<Folder, FolderViewModel>()
            .ForMember(fm => fm.FilesIds, mo => mo.MapFrom(f => f.Files.Select(si => si.Id)))
            .ForMember(fm => fm.UserId, mo => mo.MapFrom(f => f.UserId))
            .ReverseMap();
        
        CreateMap<Folder, FolderCreateModel>()
            .ReverseMap();
        
        CreateMap<Folder, FolderEditModel>()
            .ReverseMap();
        
        CreateMap<File, FileViewModel>()
            .ForMember(sim => sim.ParentFolderId, mo => mo.MapFrom(si => si.ParentFolderId))
            .ForMember(sim => sim.UserId, mo => mo.MapFrom(si => si.UserId))
            .ReverseMap();
        
        CreateMap<File, FileCreateModel>()
            .ReverseMap();
        
        CreateMap<File, FileEditModel>()
            .ReverseMap();
    }
}