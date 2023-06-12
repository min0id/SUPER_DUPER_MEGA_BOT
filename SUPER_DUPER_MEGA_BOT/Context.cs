using Microsoft.EntityFrameworkCore;
using SUPER_DUPER_MEGA_BOT.Entities;

namespace SUPER_DUPER_MEGA_BOT;

internal class Context : DbContext
{
    public Context()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(@"host=host.docker.internal;port=5432;database=db;username=postgres;password=password");
    }

    public DbSet<Person> Persons { get; set; }
}
