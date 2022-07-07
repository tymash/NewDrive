using System;
using System.Collections.Generic;
using FileStorage.DAL.Entities;

namespace FileStorage.DAL.Tests.Comparers;

public class FolderComparer : IEqualityComparer<Folder>
{
    public bool Equals(Folder x, Folder y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.UserId == y.UserId 
               && x.User.Equals(y.User) 
               && x.IsPrimaryFolder == y.IsPrimaryFolder 
               && x.Name == y.Name 
               && x.CreatedOn.Equals(y.CreatedOn) 
               && x.Path == y.Path 
               && x.Files.Equals(y.Files);
    }

    public int GetHashCode(Folder obj)
    {
        return HashCode.Combine(obj.UserId, obj.User, obj.IsPrimaryFolder, obj.Name, obj.CreatedOn, obj.Path, obj.Files);
    }
}