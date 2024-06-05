using talentX.WebScrapper.Allabolog.Entities;

namespace talentX.WebScrapper.Allabolog.Repositories.Contracts
{
    public interface IScrapDataRepo
    {
        Task AddInitialScrapDataAsync(InitialScrapOutputData outputData);

        Task AddRangeInitialScrapDataAsync(List<InitialScrapOutputData> outputDatas);

        Task AddDetailedScrapDataAsync(DetailedScrapOutputData outputData);
        Task DeleteDetailedScrapDataAsync();
        Task DeleteInitialScrapDataAsync();
        Task<List<DetailedScrapOutputData>> FindAllDetailedScrapDataAsync(string? filterInput);

        Task<List<InitialScrapOutputData>> ListOfurlsNotExistingInDb(List<InitialScrapOutputData> outputDatas);
    }
}
