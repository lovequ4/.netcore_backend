namespace backend.Context;

using Microsoft.EntityFrameworkCore;
using backend.Models;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
        
    }
    public DbSet<User> users { get; set; }
    public DbSet<Product> products { get; set; }
}
