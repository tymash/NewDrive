using System;
using System.Collections.Generic;
using FileStorage.DAL.Entities;

namespace FileStorage.DAL.Tests.Comparers;

public class FolderComparer : IEqualityComparer<Folder>
{
    public bool Equals(Folder folder1, Folder folder2)
    {
        if (ReferenceEquals(folder1, folder2)) return true;
        if (ReferenceEquals(folder1, null)) return false;
        if (ReferenceEquals(folder2, null)) return false;
        if (folder1.GetType() != folder2.GetType()) return false;
        return folder1.UserId == folder2.UserId &&
               folder1.IsPrimaryFolder == folder2.IsPrimaryFolder && folder1.Name == folder2.Name &&
               folder1.CreatedOn.Equals(folder2.CreatedOn) && folder1.StorageItems.Equals(folder2.StorageItems);
    }

    public int GetHashCode(Folder obj)
    {
        return HashCode.Combine(obj.UserId, obj.IsPrimaryFolder, obj.Name, obj.CreatedOn, obj.StorageItems);
    }
}