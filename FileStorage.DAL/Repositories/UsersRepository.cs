using FileStorage.DAL.Entities;
using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.DAL.Repositories;

public class UsersRepository : BaseRepository<string, User>, IUsersRepository
{
    public UsersRepository(DbContext context) : base(context)
    {
    }
}