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

        Task DeleteDetailedScrapOutputDataByCategory(string input);
        Task<List<DetailedScrapOutputData>> FindAllDetailedScrapDataAsync();

        Task<List<DetailedScrapOutputData>> FilterDetailedScrapDataBySearchInputAsync(string filterInput);
        Task<List<DetailedScrapOutputData>> FilterDetailedScrapDataByCategoryAsync(string filterInput);

        Task<List<InitialScrapOutputData>> ListOfurlsNotExistingInDb(List<InitialScrapOutputData> outputDatas);

        Task<List<string>> GetFiltersBySearchInputFieldAsync();
        Task<List<string>> GetFiltersByCategory();
    }
}
