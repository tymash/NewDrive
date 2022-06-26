using AutoMapper;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FolderModels;
using FileStorage.BLL.Models.StorageItemModels;
using FileStorage.BLL.Models.UserModels;
using FileStorage.DAL.Entities;

namespace FileStorage.BLL.Mapping;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<User, UserViewModel>()
            .ForMember(um => um.FoldersIds, mo => mo.MapFrom(u => u.Folders.Select(f => f.Id)))
            .ForMember(um => um.StorageItemsIds, mo => mo.MapFrom(u => u.StorageItems.Select(si => si.Id)))
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
            .ForMember(fm => fm.StorageItemsIds, mo => mo.MapFrom(f => f.StorageItems.Select(si => si.Id)))
            .ForMember(fm => fm.UserId, mo => mo.MapFrom(f => f.UserId))
            .ReverseMap();
        
        CreateMap<Folder, FolderCreateModel>()
            .ReverseMap();
        
        CreateMap<Folder, FolderEditModel>()
            .ReverseMap();
        
        CreateMap<StorageItem, StorageItemViewModel>()
            .ForMember(sim => sim.ParentFolderId, mo => mo.MapFrom(si => si.ParentFolderId))
            .ForMember(sim => sim.UserId, mo => mo.MapFrom(si => si.UserId))
            .ReverseMap();
        
        CreateMap<StorageItem, StorageItemCreateModel>()
            .ReverseMap();
        
        CreateMap<StorageItem, StorageItemEditModel>()
            .ReverseMap();
    }
}