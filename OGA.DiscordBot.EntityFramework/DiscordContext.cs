using Microsoft.EntityFrameworkCore;

namespace OGA.DiscordBot.EntityFramework;

public class DiscordContext(DbContextOptions<DiscordContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("DiscordBot");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
            throw new Exception($"{typeof(DiscordContext)} not properly configured.");
    }
}
