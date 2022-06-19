using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.DAL.Context;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Repositories;
using FileStorage.DAL.Repositories.Interfaces;
using FileStorage.DAL.Tests.Comparers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FileStorage.DAL.Tests;

public class FoldersRepositoryTests
{
    private AppDbContext _context;
    private IFoldersRepository _foldersRepository;
    private UnitTestHelper _unitTestHelper;

    [SetUp]
    public void Setup()
    {
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(dbOptions);
        _unitTestHelper = new UnitTestHelper();
        _unitTestHelper.UploadTestDataIntoDB(_context);
        _foldersRepository = new FoldersRepository(_context);
    }
    
    [Test]
    public async Task FoldersGetAllAsyncTest()
    {
        var actual = await _foldersRepository.GetAllAsync();

        Assert.That(_unitTestHelper.Folders,
            Is.EquivalentTo(actual).Using(new FolderComparer()));
    }
    
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task FolderGetByIdAsyncTest(int folderId)
    {

        var folder = await _foldersRepository.GetByIdAsync(folderId);

        var actual = await _context.Folders.FindAsync(folderId);

        Assert.That(folder, Is.EqualTo(actual).Using(new FolderComparer()));
    }

    [Test]
    [TestCase(14)]
    [TestCase(22)]
    [TestCase(30)]
    public void FolderGetByIdAsync_FolderDoesNotExistTest(int folderId)
    {
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _foldersRepository.GetByIdAsync(folderId));
    }

    [Test]
    public async Task FolderAddAsyncTest()
    {
        var folder = new Folder
        {
            Id = 6,
            IsPrimaryFolder = true, 
            UserId = "3", 
            Name = "MyStorage"
        };

        await _foldersRepository.AddAsync(folder);

        var actual = await _context.Folders.FindAsync(6);
        Assert.That(folder, Is.EqualTo(actual).Using(new FolderComparer()));
    }

    [Test]
    public void FolderDeleteTest()
    {
        var folder = _unitTestHelper.Folders[0];

        _foldersRepository.Delete(folder);

        var actual = _context.Folders.Find(folder.Id);
        Assert.Null(actual);
    }


    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task FolderDeleteByIdAsyncTest(int folderId)
    {
        await _foldersRepository.DeleteByIdAsync(folderId);

        var actual = await _context.Folders.FindAsync(folderId);
        Assert.Null(actual);
    }

    [Test]
    [TestCase(14)]
    [TestCase(-15)]
    [TestCase(30)]
    public void FolderDeleteByIdAsync_FolderDoesNotExistTest(int folderId)
    {
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _foldersRepository.DeleteByIdAsync(folderId));
    }

    [Test]
    public void FolderUpdateTest()
    {
        var folder = _unitTestHelper.Folders[0];

        folder.IsPrimaryFolder = false;
        folder.UserId = "2";
        folder.Name = "Folder5";

        _foldersRepository.Update(folder);

        var actual = _context.Folders.Find(folder.Id);
        Assert.That(folder, Is.EqualTo(actual).Using(new FolderComparer()));
    }
}