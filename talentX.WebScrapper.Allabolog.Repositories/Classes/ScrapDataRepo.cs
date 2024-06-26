﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using talentX.WebScrapper.Allabolog.Entities;
using talentX.WebScrapper.Allabolog.Repositories.Contracts;
using talentX.WebScrapper.Allabolog.Repositories.Data;

namespace talentX.WebScrapper.Allabolog.Repositories.Classes
{
    public class ScrapDataRepo : IScrapDataRepo
    {

        private readonly DataContext _context;
        private readonly ILogger<ScrapDataRepo> _logger;

        public ScrapDataRepo(DataContext context, ILogger<ScrapDataRepo> logger)
        {
            _context = context;
            _logger = logger;

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
                _logger.LogError(ex, ex.Message);
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
                _logger.LogError(ex, ex.Message);
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
                _logger.LogError(ex, ex.Message);
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
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteDetailedScrapOutputDataByCategory(string input)
        {
            try
            {
                var list = _context.DetailedScrapOutputData.Where(x => x.Verksamhet.Contains(input));
                _context.DetailedScrapOutputData.RemoveRange(list);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        public async Task DeleteInitialScrapOutputDataByCategory(string input)
        {
            try
            {
                var list = _context.InitialScrapOutputData.Where(x => x.Verksamhet.Contains(input));
                _context.InitialScrapOutputData.RemoveRange(list);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }



        public async Task DeleteDetailedScrapOutputDataBySearchinput(string input)
        {
            try
            {
                var list = _context.DetailedScrapOutputData.Where(x => x.SearchFieldText == input);
                _context.DetailedScrapOutputData.RemoveRange(list);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        public async Task DeleteInitialScrapOutputDataBySearchinput(string input)
        {
            try
            {
                var list = _context.InitialScrapOutputData.Where(x => x.SearchFieldText == input);
                _context.InitialScrapOutputData.RemoveRange(list);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        public async Task<List<DetailedScrapOutputData>> FindAllDetailedScrapDataAsync()
        {
            try
            {
                    var list = _context.DetailedScrapOutputData.ToList();
                    return list;
        
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<DetailedScrapOutputData>> FilterDetailedScrapDataBySearchInputAsync(string filterInput )
        {
            try
            {
                   
                    var list = _context.DetailedScrapOutputData.Where(x => x.SearchFieldText == filterInput).ToList();
                    return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<DetailedScrapOutputData>> FilterDetailedScrapDataByCategoryAsync(string filterInput)
        {
            try
            {
                var list = _context.DetailedScrapOutputData.Where(x => x.Verksamhet.Contains(filterInput)).ToList();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

       public async Task<List<InitialScrapOutputData>> FilterInitialScrapDataBySearchInputAsync(string filterInput)
        {
            try
            {

                var list = _context.InitialScrapOutputData.Where(x => x.SearchFieldText == filterInput).ToList();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }
        public async Task<List<InitialScrapOutputData>> FilterInitialScrapDataByCategoryAsync(string filterInput)
        {
            try
            {
                var list = _context.InitialScrapOutputData.Where(x => x.Verksamhet.Contains(filterInput)).ToList();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
                _logger.LogError(ex, ex.Message);
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
