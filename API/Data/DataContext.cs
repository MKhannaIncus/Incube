using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data;

public class DataContext : DbContext
{
    //called each time that the datacontext is created
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppUser> Users {get; set;}

}
