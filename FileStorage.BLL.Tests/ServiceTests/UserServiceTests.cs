using System;
using System.Threading.Tasks;
using AutoMapper;
using FileStorage.BLL.Mapping;
using FileStorage.BLL.Models.UserModels;
using FileStorage.BLL.Services;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Tokens;
using FileStorage.BLL.Validation;
using FileStorage.DAL.Entities;
using FileStorage.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace FileStorage.BLL.Tests.ServiceTests;

public class UserServiceTests
{
    private IUserService _userService;
    private Mock<MockUserManager> _mockUserManager;
    private Mock<MockSignInManager> _mockSignInManager;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<ITokenGenerator> _mockTokenGenerator;

    [SetUp]
    public void SetUp()
    {
        var automapperProfile = new AutomapperProfile();
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(automapperProfile));
        var mapper = new Mapper(mapperConfiguration);

        _mockUserManager = new Mock<MockUserManager>();
        _mockSignInManager = new Mock<MockSignInManager>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTokenGenerator = new Mock<ITokenGenerator>();

        _userService = new UserService(_mockUnitOfWork.Object, mapper, _mockUserManager.Object,
            _mockSignInManager.Object, _mockTokenGenerator.Object);
    }

    [Test]
    public async Task RegisterUserAsyncTest()
    {
        var userModel = new UserRegisterModel
        {
            Email = "email@email.com",
            Name = "Name",
            Surname = "Surname",
            Password = "password",
            UserName = "username"
        };


        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _mockSignInManager.Setup(sim => sim.SignInAsync(It.IsAny<User>(), false, String.Empty))
            .Returns(Task.CompletedTask);

        await _userService.RegisterAsync(userModel);

        _mockUserManager.Verify(um => um.CreateAsync(It.Is<User>(u => u.Email == userModel.Email),
            It.Is<string>(s => s == userModel.Password)));
        _mockUserManager.Verify(um =>
            um.AddToRoleAsync(It.Is<User>(u => u.Email == userModel.Email), It.Is<string>(s => s == "User")));
    }

    [Test]
    public void RegisterUserAsyncErrorTest()
    {
        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        Assert.ThrowsAsync<FileStorageException>(async () => await _userService.RegisterAsync(new UserRegisterModel()));
    }

    [Test]
    public async Task LoginUserAsyncTest()
    {
        var userModel = new UserLoginModel
        {
            UserName = "username",
            Password = "password",
        };

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = userModel.UserName,
            Email = "email@email.com",
            Name = "Name",
            Surname = "Surname"
        };

        _mockSignInManager.Setup(sim => sim.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), true, false))
            .ReturnsAsync(SignInResult.Success);
        _mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _mockTokenGenerator.Setup(tg => tg.BuildNewToken(It.IsAny<User>()))
            .Returns(It.IsAny<string>());
        

        await _userService.LoginAsync(userModel);

        _mockSignInManager.Verify(sim => sim.PasswordSignInAsync(
            It.Is<string>(username => username == userModel.UserName),
            It.Is<string>(password => password == userModel.Password), true, false));
    }

    [Test]
    public void LoginUserAsyncErrorTest()
    {
        _mockSignInManager.Setup(sim => sim.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), true, false))
            .ReturnsAsync(SignInResult.Failed);

        Assert.ThrowsAsync<FileStorageException>(async () => await _userService.LoginAsync(new UserLoginModel()));
    }

    [Test]
    public async Task LogoutUserAsyncTest()
    {
        _mockSignInManager.Setup(sim => sim.SignOutAsync());

        await _userService.LogoutAsync();

        _mockSignInManager.Verify(sim => sim.SignOutAsync());
    }

    [Test]
    public async Task GetByIdAsyncTest()
    {
        var user = new User {Id = Guid.NewGuid().ToString()};
        _mockUnitOfWork.Setup(uow => uow.UsersRepository.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var userModel = await _userService.GetByIdAsync(user.Id);

        Assert.AreEqual(user.Id, userModel.Id);
    }

    [Test]
    public async Task EditUserDataAsyncTest()
    {
        var userModel = new UserEditModel
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "username",
            Name = "Name",
            Surname = "Surname",
            Email = "email@email.com"
        };

        _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new User
            {
                Id = userModel.Id,
                UserName = userModel.UserName,
                Email = userModel.Email,
                Name = userModel.Name,
                Surname = userModel.Surname
            });
        _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        await _userService.EditUserDataAsync(userModel);

        _mockUserManager.Verify(um => um.UpdateAsync(It.Is<User>(u => u.Id == userModel.Id
                                                                      && u.Email == userModel.Email &&
                                                                      u.Name == userModel.Name
                                                                      && u.Surname == userModel.Surname)));
    }

    [Test]
    public void EditUserDataAsyncErrorTest()
    {
        _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new User {Id = Guid.NewGuid().ToString()});
        _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Failed());

        Assert.ThrowsAsync<FileStorageException>(async () => await _userService.EditUserDataAsync(new UserEditModel()));
    }

    [Test]
    public async Task ChangeUserPasswordAsyncTest()
    {
        var userModel = new UserChangePasswordModel
        {
            Id = Guid.NewGuid().ToString(),
            Password = "password"
        };

        _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new User {Id = userModel.Id});
        _mockUserManager.Setup(um => um.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        await _userService.ChangeUserPasswordAsync(userModel);

        _mockUserManager.Verify(um => um.ResetPasswordAsync(It.Is<User>(u => u.Id == userModel.Id),
            It.IsAny<string>(), It.Is<string>(s => s == userModel.Password)));
    }

    [Test]
    public void ChangeUserPasswordAsyncErrorTest()
    {
        _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new User {Id = Guid.NewGuid().ToString()});
        _mockUserManager.Setup(um => um.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        Assert.ThrowsAsync<FileStorageException>(async () =>
            await _userService.ChangeUserPasswordAsync(new UserChangePasswordModel()));
    }
}