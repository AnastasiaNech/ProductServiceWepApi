using Microsoft.EntityFrameworkCore;
using ProductServiceWepApi.Models;

namespace ProductServiceWepApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<Product> Product { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
}
