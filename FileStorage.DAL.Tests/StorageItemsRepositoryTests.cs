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

public class StorageItemsRepositoryTests
{
    private AppDbContext _context;
    private IStorageItemsRepository _storageItemsRepository;
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
        _storageItemsRepository = new StorageItemsRepository(_context);
    }
    
    [Test]
    public async Task StorageItemsGetAllAsyncTest()
    {
        var actual = await _storageItemsRepository.GetAllAsync();

        Assert.That(_unitTestHelper.StorageItems,
            Is.EquivalentTo(actual).Using(new StorageItemComparer()));
    }
    
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task StorageItemGetByIdAsyncTest(int storageItemId)
    {

        var storageItem = await _storageItemsRepository.GetByIdAsync(storageItemId);

        var actual = await _context.StorageItems.FindAsync(storageItemId);

        Assert.That(storageItem, Is.EqualTo(actual).Using(new StorageItemComparer()));
    }

    [Test]
    [TestCase(14)]
    [TestCase(22)]
    [TestCase(30)]
    public void StorageItemGetByIdAsync_StorageItemDoesNotExistTest(int storageItemId)
    {
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _storageItemsRepository.GetByIdAsync(storageItemId));
    }

    [Test]
    public async Task StorageItemAddAsyncTest()
    {
        var storageItem = new StorageItem
        {
            Id = 9, 
            ParentFolderId = 5, 
            Name = "File9",
            RelativePath = "MyStorage/Folder3/",
            Extension = "mp3"
        };

        await _storageItemsRepository.AddAsync(storageItem);

        var actual = await _context.StorageItems.FindAsync(9);
        Assert.That(storageItem, Is.EqualTo(actual).Using(new StorageItemComparer()));
    }

    [Test]
    public void StorageItemDeleteTest()
    {
        var storageItem = _unitTestHelper.StorageItems[0];

        _storageItemsRepository.Delete(storageItem);

        var actual = _context.StorageItems.Find(storageItem.Id);
        Assert.Null(actual);
    }


    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task StorageItemDeleteByIdAsyncTest(int storageItemId)
    {
        await _storageItemsRepository.DeleteByIdAsync(storageItemId);

        var actual = await _context.StorageItems.FindAsync(storageItemId);
        Assert.Null(actual);
    }

    [Test]
    [TestCase(14)]
    [TestCase(-15)]
    [TestCase(30)]
    public void StorageItemDeleteByIdAsync_StorageItemDoesNotExistTest(int storageItemId)
    {
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _storageItemsRepository.DeleteByIdAsync(storageItemId));
    }

    [Test]
    public void StorageItemUpdateTest()
    {
        var storageItem = _unitTestHelper.StorageItems[0];

        storageItem.ParentFolderId = 4;
        storageItem.Name = "NewFile";
        storageItem.RelativePath = "MyStorage/Folder2/";
        storageItem.Extension = "mp3";

        _storageItemsRepository.Update(storageItem);

        var actual = _context.StorageItems.Find(storageItem.Id);
        Assert.That(storageItem, Is.EqualTo(actual).Using(new StorageItemComparer()));
    }
}