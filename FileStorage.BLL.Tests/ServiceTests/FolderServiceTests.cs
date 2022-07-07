using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FileStorage.BLL.Mapping;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FolderModels;
using FileStorage.BLL.Services;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Validation;
using FileStorage.DAL.Entities;
using FileStorage.DAL.UnitOfWork;
using Moq;
using NUnit.Framework;

namespace FileStorage.BLL.Tests.ServiceTests;

public class FolderServiceTests
{
    private IFolderService _folderService;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var mapperProfile = new AutomapperProfile();
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
        _mapper = new Mapper(mapperConfiguration);

        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _folderService = new FolderService(_mockUnitOfWork.Object, _mapper);
    }

    [Test]
    public async Task GetByIdAsyncTest()
    {
        _mockUnitOfWork.Setup(uow => uow.FoldersRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(TestFolders.ToList()[0]);
    
        await _folderService.GetByIdAsync(1);
    
        _mockUnitOfWork.Verify(uow => uow.FoldersRepository.GetByIdAsync(It.Is<int>(id => id == 1)));
    }

    [Test] 
    public async Task GetAllAsyncTest()
    {
        _mockUnitOfWork.Setup(uow => uow.FoldersRepository.GetAllAsync())
            .ReturnsAsync(TestFolders.ToList());
    
        await _folderService.GetAllAsync();
    
        _mockUnitOfWork.Verify(uow => uow.FoldersRepository.GetAllAsync());
    }
    
    [Test]
    public async Task AddAsyncTest()
    {
        var folder = new Folder
        {
            Id = 1,
            UserId = "1",
            Name = "Folder1",
            Path = "/",
            IsPrimaryFolder = true,
            Files = new List<File>()
        };
        
        _mockUnitOfWork.Setup(uow => uow.FoldersRepository.AddAsync(It.IsAny<Folder>()));

        var folderModel = new FolderCreateModel
        {
            Name = "Folder1",
            Path = "/",
            IsPrimaryFolder = true,
            UserId = "1"
        };

        await _folderService.AddAsync(folderModel);
        
        Assert.IsTrue(folder.IsPrimaryFolder == folderModel.IsPrimaryFolder 
                      && folder.Path == folderModel.Path
                      && folder.Name == folderModel.Name
                      && folder.UserId == folderModel.UserId);
    }
    
    [Test]
    public void AddAsyncErrorTest()
    {
        Assert.ThrowsAsync<FileStorageException>(async () =>
            await _folderService.AddAsync(new FolderCreateModel()));
    }

    [Test]
    public async Task UpdateAsyncTest()
    {
        _mockUnitOfWork.Setup(uow => uow.FoldersRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Folder {Id = 1, Name = "Folder1"});
    
        var folder = TestFolders.ToList()[0];
        _mockUnitOfWork.Setup(uow => uow.FoldersRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(folder);
    
        var editFolderModel = new FolderEditModel
        {
            Id = 1,
            Name = "Folder1", 
            Path = "/", 
            IsPrimaryFolder = true,
        };
    
        await _folderService.UpdateAsync(editFolderModel);
    
        Assert.IsTrue(folder.Id == editFolderModel.Id 
                      && folder.IsPrimaryFolder == editFolderModel.IsPrimaryFolder 
                      && folder.Path == editFolderModel.Path
                      && folder.Name == editFolderModel.Name);
    }
    
    [Test]
    public void UpdateAsyncErrorTest()
    {
        Assert.ThrowsAsync<FileStorageException>(async () =>
            await _folderService.UpdateAsync(new FolderEditModel()));
    }

    
    [Test] 
    public async Task DeleteAsyncTest()
    {
        _mockUnitOfWork.Setup(uow => uow.FoldersRepository.DeleteByIdAsync(It.IsAny<int>()));
    
        await _folderService.DeleteAsync(1);
    
        _mockUnitOfWork.Verify(uow => uow.FoldersRepository.DeleteByIdAsync(It.Is<int>(id => id == 1)));
    }
    
    [Test]
    [TestCaseSource(nameof(GetFoldersByFilterTest))]
    public async Task GetByFilterAsyncTest
        ((FilterModel, IEnumerable<Folder>) testData)
    {
        var filterModel = testData.Item1;
        var expectedFolders = testData.Item2;
        _mockUnitOfWork.Setup(uow => uow.FoldersRepository.GetAllAsync())
            .ReturnsAsync(TestFolders);
    
        var folderViewModels = await _folderService.GetByFilterAsync(filterModel);
    
        var folders = _mapper.Map<IEnumerable<Folder>>(folderViewModels);
        Assert.AreEqual(folders.Select(f => f.Id), expectedFolders.Select(f => f.Id));
    }
    
    private static IEnumerable<Folder> TestFolders =>
        new List<Folder>
        {
            new() {Id = 1, UserId = "1", Name = "Folder1", Path = "/", IsPrimaryFolder = true , Files = new List<File>()},
            new() {Id = 2, UserId = "1", Name = "Folder2", Path = "/Folder1/", IsPrimaryFolder = false , Files = new List<File>()},
            new() {Id = 3, UserId = "1", Name = "Folder3", Path = "/Folder1/", IsPrimaryFolder = false , Files = new List<File>()},
            new() {Id = 4, UserId = "2", Name = "Folder4", Path = "/", IsPrimaryFolder = true , Files = new List<File>()},
            new() {Id = 5, UserId = "2", Name = "Folder5", Path = "/", IsPrimaryFolder = true , Files = new List<File>()},
            new() {Id = 6, UserId = "2", Name = "Folder6", Path = "MyStorage/Folder2/", IsPrimaryFolder = false , Files = new List<File>()},
            new() {Id = 7, UserId = "2", Name = "Folder7", Path = "MyStorage/Folder2/", IsPrimaryFolder = false , Files = new List<File>()},
            new() {Id = 8, UserId = "3", Name = "Folder8", Path = "MyStorage/Folder3/", IsPrimaryFolder = false , Files = new List<File>()},
        };

    private static IEnumerable<(FilterModel, IEnumerable<Folder>)> GetFoldersByFilterTest()
    {
        yield return (new FilterModel {Name = "Folder1"},
            new List<Folder> {TestFolders.ToList()[0]});
        yield return (new FilterModel {DateSort = Sort.Ascending},
            TestFolders.OrderBy(f => f.CreatedOn));
        yield return (new FilterModel {DateSort = Sort.Descending},
            TestFolders.OrderByDescending(f => f.CreatedOn));
        yield return (new FilterModel {NameSort = Sort.Ascending},
            TestFolders.OrderBy(f => f.Name));
        yield return (new FilterModel {NameSort = Sort.Descending},
            TestFolders.OrderByDescending(f => f.Name));
        yield return (new FilterModel(),
            TestFolders);
    }
    
}