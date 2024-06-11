namespace talentX.WebScrapper.Allabolog.Repositories.Data
{
    public class DbInitializer
    {
        public static void Initialize(DataContext context)
        {

            context.SaveChanges();
        }
    }
}
