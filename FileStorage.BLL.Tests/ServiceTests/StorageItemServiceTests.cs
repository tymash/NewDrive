using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FileStorage.BLL.Mapping;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.StorageItemModels;
using FileStorage.BLL.Services;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Validation;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Tests;
using FileStorage.DAL.UnitOfWork;
using Moq;
using NUnit.Framework;

namespace FileStorage.BLL.Tests.ServiceTests;

public class StorageItemServiceTests
{
    private IStorageItemService _storageItemService;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var mapperProfile = new AutomapperProfile();
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
        _mapper = new Mapper(mapperConfiguration);

        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _storageItemService = new StorageItemService(_mockUnitOfWork.Object, _mapper);
    }

    [Test]
    public async Task GetByIdAsyncTest()
    {
        _mockUnitOfWork.Setup(uow => uow.StorageItemsRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(TestStorageItems.ToList()[0]);
    
        await _storageItemService.GetByIdAsync(1);
    
        _mockUnitOfWork.Verify(uow => uow.StorageItemsRepository.GetByIdAsync(It.Is<int>(id => id == 1)));
    }

    [Test] 
    public async Task GetAllAsyncTest()
    {
        _mockUnitOfWork.Setup(uow => uow.StorageItemsRepository.GetAllAsync())
            .ReturnsAsync(TestStorageItems.ToList());
    
        await _storageItemService.GetAllAsync();
    
        _mockUnitOfWork.Verify(uow => uow.StorageItemsRepository.GetAllAsync());
    }
    
    [Test]
    public async Task AddAsyncTest()
    {
        _mockUnitOfWork.Setup(uow => uow.StorageItemsRepository.AddAsync(It.IsAny<StorageItem>()));

        var storageItemModel = new StorageItemCreateModel()
        {
            Name = "File1",
            RelativePath = "folder1/",
            UserId = "1"
        };

        await _storageItemService.AddAsync(storageItemModel);

        _mockUnitOfWork.Verify(uow => uow.StorageItemsRepository.AddAsync(It.Is<StorageItem>(si =>
            si.Name == storageItemModel.Name
            && si.RelativePath == storageItemModel.RelativePath && si.UserId == storageItemModel.UserId
            && si.IsPublic == storageItemModel.IsPublic && si.IsRecycled == storageItemModel.IsRecycled
            && si.Extension == storageItemModel.Extension)));
    }
    
    [Test]
    public void AddAsyncErrorTest()
    {
        Assert.ThrowsAsync<FileStorageException>(async () =>
            await _storageItemService.AddAsync(new StorageItemCreateModel()));
    }

    [Test]
    public async Task UpdateAsyncTest()
    {
        _mockUnitOfWork.Setup(uow => uow.FoldersRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Folder {Id = 1, Name = "Folder1"});
    
        var storageItem = TestStorageItems.ToList()[0];
        _mockUnitOfWork.Setup(uow => uow.StorageItemsRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(storageItem);
    
        var editStorageItemModel = new StorageItemEditModel
        {
            Id = 1,
            CreatedOn = storageItem.CreatedOn,
            Extension = "txt",
            Name = "File1",
            UserId = "1",
            ParentFolderId = 1,
            RelativePath = "/"
        };
    
        await _storageItemService.UpdateAsync(editStorageItemModel);
    
        Assert.IsTrue(storageItem.Id == editStorageItemModel.Id 
                      && storageItem.CreatedOn == editStorageItemModel.CreatedOn 
                      && storageItem.Extension == editStorageItemModel.Extension
                      && storageItem.Name == editStorageItemModel.Name
                      && storageItem.UserId == editStorageItemModel.UserId
                      && storageItem.ParentFolderId == editStorageItemModel.ParentFolderId);
    }
    
    [Test]
    public void UpdateAsyncErrorTest()
    {
        Assert.ThrowsAsync<FileStorageException>(async () =>
            await _storageItemService.UpdateAsync(new StorageItemEditModel()));
    }

    
    [Test] 
    public async Task DeleteAsyncTest()
    {
        _mockUnitOfWork.Setup(uow => uow.StorageItemsRepository.DeleteByIdAsync(It.IsAny<int>()));
    
        await _storageItemService.DeleteAsync(1);
    
        _mockUnitOfWork.Verify(uow => uow.StorageItemsRepository.DeleteByIdAsync(It.Is<int>(id => id == 1)));
    }
    
    [Test]
    [TestCaseSource(nameof(GetStorageItemsByFilterTest))]
    public async Task GetByFilterAsyncTest
        ((FilterModel, IEnumerable<StorageItem>) testData)
    {
        var filterModel = testData.Item1;
        var expectedStorageItems = testData.Item2;
        _mockUnitOfWork.Setup(uow => uow.StorageItemsRepository.GetAllAsync())
            .ReturnsAsync(TestStorageItems);
    
        var storageItemViewModels = await _storageItemService.GetByFilterAsync(filterModel);
    
        var storageItems = _mapper.Map<IEnumerable<StorageItem>>(storageItemViewModels);
        Assert.AreEqual(storageItems.Select(si => si.Id), expectedStorageItems.Select(si => si.Id));
    }
    
    private static IEnumerable<StorageItem> TestStorageItems =>
        new List<StorageItem>
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

    private static IEnumerable<(FilterModel, IEnumerable<StorageItem>)> GetStorageItemsByFilterTest()
    {
        yield return (new FilterModel {Name = "File1"},
            new List<StorageItem> {TestStorageItems.ToList()[0]});
        yield return (new FilterModel {DateSort = Sort.Ascending},
            TestStorageItems.OrderBy(si => si.CreatedOn));
        yield return (new FilterModel {DateSort = Sort.Descending},
            TestStorageItems.OrderByDescending(si => si.CreatedOn));
        yield return (new FilterModel {NameSort = Sort.Ascending},
            TestStorageItems.OrderBy(si => si.Name));
        yield return (new FilterModel {NameSort = Sort.Descending},
            TestStorageItems.OrderByDescending(si => si.Name));
        yield return (new FilterModel {SizeSort = Sort.Ascending},
            TestStorageItems.OrderBy(si => si.Size));
        yield return (new FilterModel {SizeSort = Sort.Descending},
            TestStorageItems.OrderByDescending(si => si.Size));
        yield return (new FilterModel(),
            TestStorageItems);
    }
    
}