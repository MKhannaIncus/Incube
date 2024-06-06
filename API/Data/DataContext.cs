using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options) { }

    #region Entities added to DbContext
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Deal> Deals { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Fund> Funds { get; set; }
    

    #endregion
}
