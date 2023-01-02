using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Webinar.API.Infrastructure.EntityConfigurations;

namespace Webinar.API.Infrastructure;

public class WebinarDbContext : DbContext
{
    public WebinarDbContext(DbContextOptions<WebinarDbContext> options) : base(options)
    {
    }
    public DbSet<Entities.Webinar> Webinars { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new WebinarEntityTypeConfiguration());
    }
}

public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<WebinarDbContext>
{
    public WebinarDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WebinarDbContext>()
            .UseSqlServer("Server=.;Initial Catalog=ModularMonolith.EventDb;Integrated Security=true");

        return new WebinarDbContext(optionsBuilder.Options);
    }
}