using Microsoft.EntityFrameworkCore;
using talentX.WebScrapper.Allabolog.Entities;
using talentX.WebScrapper.Allabolog.Repositories.Contracts;
using talentX.WebScrapper.Allabolog.Repositories.Data;

namespace talentX.WebScrapper.Allabolog.Repositories.Classes
{
    public class ScrapDataRepo : IScrapDataRepo
    {

        private readonly DataContext _context;

        public ScrapDataRepo(DataContext context)
        {
            _context = context;

        }
        public async Task AddInitialScrapDataAsync(InitialScrapOutputData outputData)
        {
            try
            {
                await _context.InitialScrapOutputData.AddAsync(outputData);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task AddRangeInitialScrapDataAsync(List<InitialScrapOutputData> outputDatas)
        {
            try
            {
                var list = new List<InitialScrapOutputData>();
                foreach (var data in outputDatas)
                {
                    if (!_context.InitialScrapOutputData.Any(x => x.Url == data.Url))
                    {
                        list.Add(data);

                    }
                }

                await _context.InitialScrapOutputData.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task AddDetailedScrapDataAsync(DetailedScrapOutputData outputData)
        {
            try
            {
                await _context.DetailedScrapOutputData.AddAsync(outputData);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task DeleteDetailedScrapDataAsync()
        {
            try
            {
                _context.Database.ExecuteSqlRaw("TRUNCATE TABLE DetailedScrapOutputData");
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task DeleteInitialScrapDataAsync()
        {
            try
            {
                _context.Database.ExecuteSqlRaw("TRUNCATE TABLE InitialScrapOutputData");
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public async Task<List<DetailedScrapOutputData>> FindAllDetailedScrapDataAsync(string? filterInput = null)
        {
            try
            {
                if(filterInput == null)
                {
                    var list = _context.DetailedScrapOutputData.ToList();
                    return list;
                }
                else
                {
                    var list = _context.DetailedScrapOutputData.Where(x => x.SearchFieldText == filterInput).ToList();
                    return list;
                }
               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<List<InitialScrapOutputData>> ListOfurlsNotExistingInDb(List<InitialScrapOutputData> outputDatas)
        {
            try
            {
                var listOfUrlNotExistingInDb = new List<InitialScrapOutputData>();
                foreach(var data in outputDatas)
                {
                    if (!_context.DetailedScrapOutputData.Any(x => x.AllabolagUrl == data.Url)) 
                    {
                        listOfUrlNotExistingInDb.Add(data);
                    }
                }
                return listOfUrlNotExistingInDb;
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<string>> GetFiltersBySearchInputFieldAsync()
        {
          var searchInputFilter = await  _context.DetailedScrapOutputData.Select(p => p.SearchFieldText).Distinct().ToListAsync();
            return searchInputFilter;
        }

        public async Task<List<string>> GetFiltersByCategory()
        {
            var categories = await _context.DetailedScrapOutputData.Select(p => p.Verksamhet).Distinct().ToListAsync();
            return categories;
        }
    }
}
