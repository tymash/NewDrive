using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FileStorage.DAL.Context;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Repositories;
using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FileStorage.DAL.Tests;

public class UsersRepositoryTests
{
    private AppDbContext _context;
    private IUsersRepository _usersRepository;
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
        _usersRepository = new UsersRepository(_context);
    }
    
    [Test]
    public async Task UsersGetAllAsyncTest()
    {
        var actual = await _usersRepository.GetAllAsync();

        Assert.That(_unitTestHelper.Users,
            Is.EquivalentTo(actual));
    }
    
    [Test]
    [TestCase("1")]
    [TestCase("2")]
    [TestCase("3")]
    public async Task UserGetByIdAsyncTest(string userId)
    {

        var user = await _usersRepository.GetByIdAsync(userId);

        var actual = await _context.Users.FindAsync(userId);

        Assert.That(user, Is.EqualTo(actual));
    }

    [Test]
    [TestCase("14")]
    [TestCase("22")]
    [TestCase("30")]
    public void UserGetByIdAsync_UserDoesNotExistTest(string userId)
    {
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _usersRepository.GetByIdAsync(userId));
    }

    [Test]
    public async Task UserAddAsyncTest()
    {
        var user = new User
        {
            Id = "4",
            UserName = "Mark405"
        };

        await _usersRepository.AddAsync(user);

        var actual = await _context.Users.FindAsync("4");
        Assert.That(user, Is.EqualTo(actual));
    }

    [Test]
    public void UserDeleteTest()
    {
        var user = _unitTestHelper.Users[0];

        _usersRepository.Delete(user);

        var actual = _context.Users.Find(user.Id);
        Assert.Null(actual);
    }


    [Test]
    [TestCase("1")]
    [TestCase("2")]
    [TestCase("3")]
    public async Task UserDeleteByIdAsyncTest(string userId)
    {
        await _usersRepository.DeleteByIdAsync(userId);

        var actual = await _context.Users.FindAsync(userId);
        Assert.Null(actual);
    }

    [Test]
    [TestCase("14")]
    [TestCase("-15")]
    [TestCase("30.5")]
    public void UserDeleteByIdAsync_UserDoesNotExistTest(string userId)
    {
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _usersRepository.DeleteByIdAsync(userId));
    }

    [Test]
    public void UserUpdateTest()
    {
        var user = _unitTestHelper.Users[0];

        user.UserName = "JackWhite777";

        _usersRepository.Update(user);

        var actual = _context.Users.Find(user.Id);
        Assert.That(user, Is.EqualTo(actual));
    }
}