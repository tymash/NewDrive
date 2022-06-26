using System;
using FileStorage.DAL.Context;
using FileStorage.DAL.Entities;
using NUnit.Framework;

namespace FileStorage.DAL.Tests;

public class UnitTestHelper
{
    public UnitTestHelper()
    {
        InitializeData();
    }

    public StorageItem[] StorageItems { get; private set; }
    public Folder[] Folders { get; private set; }
    public User[] Users { get; private set; }

    public void UploadTestDataIntoDB(AppDbContext appDbContext)
    {
        appDbContext.StorageItems.AddRange(StorageItems);
        appDbContext.Folders.AddRange(Folders);
        appDbContext.Users.AddRange(Users);
        appDbContext.SaveChanges();
    }

    private void InitializeData()
    {
        Users = new User[]
        {
            new() {UserName = "JohnDoe222", Name = "John", Surname = "Doe", Id = "1"},
            new() {UserName = "Tymash33", Name = "Tymofii", Surname = "Karakash", Id = "2"},
            new() {UserName = "Julia102", Name = "Julia", Surname = "Holovko", Id = "3"}
        };

        Folders = new Folder[]
        {
            new() {Id = 1, IsPrimaryFolder = true, UserId = "1", Name = "MyStorage", RelativePath = "/"},
            new() {Id = 2, IsPrimaryFolder = false, UserId = "1", Name = "Folder1", RelativePath = "/Folder1/"},
            new() {Id = 3, IsPrimaryFolder = true, UserId = "2", Name = "MyStorage", RelativePath = "/" },
            new() {Id = 4, IsPrimaryFolder = false, UserId = "2", Name = "Folder2", RelativePath = "/Folder2/"},
            new() {Id = 5, IsPrimaryFolder = false, UserId = "3", Name = "Folder3", RelativePath = "/Folder3/"}
        };

        StorageItems = new StorageItem[]
        {
            new() {Id = 1, ParentFolderId = 1, UserId = "1", Name = "File1", RelativePath = "/", Extension = "txt"},
            new() {Id = 2, ParentFolderId = 2, UserId = "1", Name = "File2", RelativePath = "/Folder1/", Extension = "pdf"},
            new() {Id = 3, ParentFolderId = 2, UserId = "1", Name = "File3", RelativePath = "/Folder1/", Extension = "docx"},
            new() {Id = 4, ParentFolderId = 3, UserId = "2", Name = "File4", RelativePath = "/", Extension = "txt"},
            new() {Id = 5, ParentFolderId = 3, UserId = "2", Name = "File5", RelativePath = "/", Extension = "txt"},
            new() {Id = 6, ParentFolderId = 4, UserId = "2", Name = "File6", RelativePath = "MyStorage/Folder2/", Extension = "pdf"},
            new() {Id = 7, ParentFolderId = 4, UserId = "2", Name = "File7", RelativePath = "MyStorage/Folder2/", Extension = "pdf"},
            new() {Id = 8, ParentFolderId = 5, UserId = "3", Name = "File8", RelativePath = "MyStorage/Folder3/", Extension = "pdf"}
        };

       
    }
}