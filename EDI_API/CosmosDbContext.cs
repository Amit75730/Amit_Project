using Microsoft.EntityFrameworkCore;
using EdiWebAPI.Models;

public class CosmosDbContext : DbContext
{
    public DbSet<RequiredFields> RequiredFields { get; set; }

    public CosmosDbContext(DbContextOptions<CosmosDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RequiredFields>()
            .ToContainer("EdiData")
            .HasPartitionKey(rf => rf.ContainerNumber)
            .HasKey(rf => rf.Id);//primary key

        modelBuilder.Entity<RequiredFields>().HasNoDiscriminator();
    }
}