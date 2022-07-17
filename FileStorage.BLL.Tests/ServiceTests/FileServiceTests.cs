using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FileStorage.BLL.Mapping;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Services;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Validation;
using FileStorage.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using File = FileStorage.DAL.Entities.File;

namespace FileStorage.BLL.Tests.ServiceTests;

public class FileServiceTests
{
    private IFileService _fileService;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IFormFile> _mockFormFile;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var mapperProfile = new AutomapperProfile();
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
        _mapper = new Mapper(mapperConfiguration);

        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockFormFile = new Mock<IFormFile>();
        _fileService = new FileService(_mockUnitOfWork.Object, _mapper);
    }

    [Test]
    public async Task GetByIdAsyncTest()
    {
        _mockUnitOfWork.Setup(uow => uow.FilesRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(TestFiles.ToList()[0]);

        await _fileService.GetByIdAsync(1);

        _mockUnitOfWork.Verify(uow => uow.FilesRepository.GetByIdAsync(It.Is<int>(id => id == 1)));
    }

    [Test]
    public async Task GetAllAsyncTest()
    {
        _mockUnitOfWork.Setup(uow => uow.FilesRepository.GetAllAsync())
            .ReturnsAsync(TestFiles.ToList());

        await _fileService.GetAllAsync();

        _mockUnitOfWork.Verify(uow => uow.FilesRepository.GetAllAsync());
    }

    [Test]
    public async Task AddAsyncTest()
    {
        var fileModel = new FileCreateModel
        {
            Name = "File1",
            Path = "/",
            UserId = "1"
        };
        
        _mockUnitOfWork.Setup(uow => uow.FilesRepository.AddAsync(It.IsAny<File>()));
        _mockUnitOfWork.Setup(uow =>
            uow.FileStorageRepository.ProcessFormFileAsync(It.IsAny<FormFile>(), It.IsAny<long>()));
        

        await _fileService.UploadAsync(fileModel.UserId, _mockFormFile.Object);

        _mockUnitOfWork.Verify(uow => uow.FilesRepository.AddAsync(It.Is<File>(si =>
            si.Name == fileModel.Name
            && si.Path == fileModel.Path && si.UserId == fileModel.UserId
            && si.IsPublic == fileModel.IsPublic && si.IsRecycled == fileModel.IsRecycled
            && si.Extension == fileModel.Extension)));
    }

    [Test]
    public void AddAsyncErrorTest()
    {
        Assert.ThrowsAsync<FileStorageException>(async () =>
            await _fileService.UploadAsync("", _mockFormFile.Object));
    }

    [Test]
    public async Task UpdateAsyncTest()
    {
        var file = TestFiles.ToList()[0];
        _mockUnitOfWork.Setup(uow => uow.FilesRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(file);

        var editFileModel = new FileEditModel
        {
            Id = 1,
            Extension = "txt",
            Name = "File1",
            Path = "/"
        };

        await _fileService.UpdateAsync(editFileModel);

        Assert.IsTrue(file.Id == editFileModel.Id
                      && file.Extension == editFileModel.Extension
                      && file.Name == editFileModel.Name);
    }

    [Test]
    public void UpdateAsyncErrorTest()
    {
        Assert.ThrowsAsync<FileStorageException>(async () =>
            await _fileService.UpdateAsync(new FileEditModel()));
    }


    [Test]
    public async Task DeleteAsyncTest()
    {
        _mockUnitOfWork.Setup(uow => uow.FilesRepository.DeleteByIdAsync(It.IsAny<int>()));

        await _fileService.DeleteAsync(1);

        _mockUnitOfWork.Verify(uow => uow.FilesRepository.DeleteByIdAsync(It.Is<int>(id => id == 1)));
    }

    [Test]
    [TestCaseSource(nameof(GetFilesByFilterTest))]
    public async Task GetByFilterAsyncTest
        ((FilterModel, IEnumerable<File>) testData)
    {
        var filterModel = testData.Item1;
        var expectedFiles = testData.Item2;
        _mockUnitOfWork.Setup(uow => uow.FilesRepository.GetAllAsync())
            .ReturnsAsync(TestFiles);

        var fileViewModels = await _fileService.GetByFilterAsync(filterModel);

        var files = _mapper.Map<IEnumerable<File>>(fileViewModels);
        Assert.AreEqual(files.Select(si => si.Id), expectedFiles.Select(si => si.Id));
    }

    private static IEnumerable<File> TestFiles =>
        new List<File>
        {
            new() {Id = 1, UserId = "1", Name = "File1", Path = "/", Extension = "txt"},
            new() {Id = 2, UserId = "1", Name = "File2", Path = "/Folder1/", Extension = "pdf"},
            new() {Id = 3,  UserId = "1", Name = "File3", Path = "/Folder1/", Extension = "docx"},
            new() {Id = 4,  UserId = "2", Name = "File4", Path = "/", Extension = "txt"},
            new() {Id = 5,  UserId = "2", Name = "File5", Path = "/", Extension = "txt"},
            new()
            {
                Id = 6,  UserId = "2", Name = "File6", Path = "MyStorage/Folder2/", Extension = "pdf"
            },
            new()
            {
                Id = 7, UserId = "2", Name = "File7", Path = "MyStorage/Folder2/", Extension = "pdf"
            },
            new()
            {
                Id = 8, UserId = "3", Name = "File8", Path = "MyStorage/Folder3/", Extension = "pdf"
            }
        };


    private static IEnumerable<(FilterModel, IEnumerable<File>)> GetFilesByFilterTest()
    {
        yield return (new FilterModel {Name = "File1"},
            new List<File> {TestFiles.ToList()[0]});
        yield return (new FilterModel {DateSort = Sort.Ascending},
            TestFiles.OrderBy(si => si.CreatedOn));
        yield return (new FilterModel {DateSort = Sort.Descending},
            TestFiles.OrderByDescending(si => si.CreatedOn));
        yield return (new FilterModel {NameSort = Sort.Ascending},
            TestFiles.OrderBy(si => si.Name));
        yield return (new FilterModel {NameSort = Sort.Descending},
            TestFiles.OrderByDescending(si => si.Name));
        yield return (new FilterModel {SizeSort = Sort.Ascending},
            TestFiles.OrderBy(si => si.Size));
        yield return (new FilterModel {SizeSort = Sort.Descending},
            TestFiles.OrderByDescending(si => si.Size));
        yield return (new FilterModel(),
            TestFiles);
    }
}