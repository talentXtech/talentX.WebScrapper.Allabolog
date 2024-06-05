using talentX.WebScrapper.Allabolog.Entities;

namespace talentX.WebScrapper.Allabolog.Repositories.Data
{
    public class DbInitializer
    {
        public static void Initialize(DataContext context)
        {

            if (context.InitialScrapOutputData.Any()) return;

            var testData1 = new InitialScrapOutputData
            {
                Title = "test",
                Url = "testUrl"

            };

            context.InitialScrapOutputData.Add(testData1);
            context.SaveChanges();
        }
    }
}
