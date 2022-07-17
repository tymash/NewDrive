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

public class FilesRepositoryTests
{
    private AppDbContext _context;
    private IFilesRepository _filesRepository;
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
        _filesRepository = new FilesRepository(_context);
    }
    
    [Test]
    public async Task FilesGetAllAsyncTest()
    {
        var actual = await _filesRepository.GetAllAsync();

        Assert.That(_unitTestHelper.Files,
            Is.EquivalentTo(actual).Using(new FileComparer()));
    }
    
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task FileGetByIdAsyncTest(int fileId)
    {

        var file = await _filesRepository.GetByIdAsync(fileId);

        var actual = await _context.Files.FindAsync(fileId);

        Assert.That(file, Is.EqualTo(actual).Using(new FileComparer()));
    }

    [Test]
    [TestCase(14)]
    [TestCase(22)]
    [TestCase(30)]
    public void FileGetByIdAsync_FileDoesNotExistTest(int fileId)
    {
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _filesRepository.GetByIdAsync(fileId));
    }

    [Test]
    public async Task FileAddAsyncTest()
    {
        var file = new File
        {
            Id = 9, 
            UserId = "3",
            Name = "File9",
            Path = "MyStorage/Folder3/",
            Extension = "mp3"
        };

        await _filesRepository.AddAsync(file);

        var actual = await _context.Files.FindAsync(9);
        Assert.That(file, Is.EqualTo(actual).Using(new FileComparer()));
    }

    [Test]
    public void FileDeleteTest()
    {
        var file = _unitTestHelper.Files[0];

        _filesRepository.Delete(file);

        var actual = _context.Files.Find(file.Id);
        Assert.Null(actual);
    }


    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public async Task FileDeleteByIdAsyncTest(int fileId)
    {
        await _filesRepository.DeleteByIdAsync(fileId);

        var actual = await _context.Files.FindAsync(fileId);
        Assert.Null(actual);
    }

    [Test]
    [TestCase(14)]
    [TestCase(-15)]
    [TestCase(30)]
    public void FileDeleteByIdAsync_FileDoesNotExistTest(int fileId)
    {
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _filesRepository.DeleteByIdAsync(fileId));
    }

    [Test]
    public void FileUpdateTest()
    {
        var file = _unitTestHelper.Files[0];

        file.Name = "NewFile";
        file.Path = "MyStorage/Folder2/";
        file.Extension = "mp3";

        _filesRepository.Update(file);

        var actual = _context.Files.Find(file.Id);
        Assert.That(file, Is.EqualTo(actual).Using(new FileComparer()));
    }
}