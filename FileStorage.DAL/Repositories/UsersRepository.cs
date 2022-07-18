using FileStorage.DAL.Entities;
using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.DAL.Repositories;

/// <summary>

/// The users repository class

/// </summary>

/// <seealso cref="BaseRepository{string, User}"/>

/// <seealso cref="IUsersRepository"/>

public class UsersRepository : BaseRepository<string, User>, IUsersRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UsersRepository"/> class
    /// </summary>
    /// <param name="context">The context</param>
    public UsersRepository(DbContext context) : base(context)
    {
    }
}