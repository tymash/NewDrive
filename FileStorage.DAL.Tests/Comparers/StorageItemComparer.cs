using System;
using System.Collections.Generic;
using FileStorage.DAL.Entities;

namespace FileStorage.DAL.Tests.Comparers;

public class FileComparer : IEqualityComparer<File>
{
    public bool Equals(File x, File y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.CreatedOn.Equals(y.CreatedOn) 
               && x.Extension == y.Extension 
               && x.Name == y.Name 
               && x.Size == y.Size 
               && x.IsRecycled == y.IsRecycled 
               && x.IsPublic == y.IsPublic 
               && x.Path == y.Path 
               && x.UserId == y.UserId 
               && x.User.Equals(y.User) 
               && x.ParentFolderId == y.ParentFolderId 
               && x.ParentFolder.Equals(y.ParentFolder);
    }

    public int GetHashCode(File obj)
    {
        var hashCode = new HashCode();
        hashCode.Add(obj.CreatedOn);
        hashCode.Add(obj.Extension);
        hashCode.Add(obj.Name);
        hashCode.Add(obj.Size);
        hashCode.Add(obj.IsRecycled);
        hashCode.Add(obj.IsPublic);
        hashCode.Add(obj.Path);
        hashCode.Add(obj.UserId);
        hashCode.Add(obj.User);
        hashCode.Add(obj.ParentFolderId);
        hashCode.Add(obj.ParentFolder);
        return hashCode.ToHashCode();
    }
}