using Microsoft.EntityFrameworkCore;
using talentX.WebScrapper.Allabolog.Entities;

namespace talentX.WebScrapper.Allabolog.Repositories.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<InitialScrapOutputData> InitialScrapOutputData { get; set; }
        public DbSet<DetailedScrapOutputData> DetailedScrapOutputData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("allabolog");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer(x => x.MigrationsHistoryTable("__EFMigrationsHistory", "allabolog"));
    }
}
