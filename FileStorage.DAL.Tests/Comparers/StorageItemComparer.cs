using System;
using System.Collections.Generic;
using FileStorage.DAL.Entities;

namespace FileStorage.DAL.Tests.Comparers;

public class StorageItemComparer : IEqualityComparer<StorageItem>
{
    public bool Equals(StorageItem storageItem1, StorageItem storageItem2)
    {
        if (ReferenceEquals(storageItem1, storageItem2)) return true;
        if (ReferenceEquals(storageItem1, null)) return false;
        if (ReferenceEquals(storageItem2, null)) return false;
        if (storageItem1.GetType() != storageItem2.GetType()) return false;
        return storageItem1.CreatedOn.Equals(storageItem2.CreatedOn) && 
               storageItem1.Extension == storageItem2.Extension && 
               storageItem1.Name == storageItem2.Name && 
               storageItem1.Size == storageItem2.Size && 
               storageItem1.IsRecycled == storageItem2.IsRecycled && 
               storageItem1.IsPublic == storageItem2.IsPublic && 
               storageItem1.RelativePath == storageItem2.RelativePath && 
               storageItem1.ParentFolderId == storageItem2.ParentFolderId;
    }

    public int GetHashCode(StorageItem obj)
    {
        var hashCode = new HashCode();
        hashCode.Add(obj.CreatedOn);
        hashCode.Add(obj.Extension);
        hashCode.Add(obj.Name);
        hashCode.Add(obj.Size);
        hashCode.Add(obj.IsRecycled);
        hashCode.Add(obj.IsPublic);
        hashCode.Add(obj.RelativePath);
        hashCode.Add(obj.ParentFolderId);
        return hashCode.ToHashCode();
    }
}