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


    }
}
