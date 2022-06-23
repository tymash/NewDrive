using AutoMapper;
using FileStorage.BLL.Models;
using FileStorage.DAL.Entities;

namespace FileStorage.BLL.Mapping;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<User, UserModel>()
            .ForMember(um => um.FoldersIds, mo => mo.MapFrom(u => u.Folders.Select(f => f.Id)))
            .ForMember(um => um.StorageItemsIds, mo => mo.MapFrom(u => u.StorageItems.Select(si => si.Id)))
            .ReverseMap();

        CreateMap<StorageItem, StorageItemModel>()
            .ForMember(sim => sim.UserId, mo => mo.MapFrom(si => si.UserId))
            .ForMember(sim => sim.ParentFolderId, mo => mo.MapFrom(si => si.UserId))
            .ReverseMap();

        CreateMap<Folder, FolderModel>()
            .ForMember(fm => fm.StorageItemsIds, mo => mo.MapFrom(f => f.StorageItems.Select(si => si.Id)))
            .ForMember(fm => fm.UserId, mo => mo.MapFrom(f => f.UserId))
            .ReverseMap();
    }
}