using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using talentX.WebScrapper.Allabolog.Entities;
using talentX.WebScrapper.Allabolog.Repositories.Contracts;
using talentX.WebScrapper.Allabolog.Utils;
using talentX.WebScrapper.Allabolog.Extensions;
using CsvHelper;
using OpenQA.Selenium.Chrome;

namespace talentX.WebScrapper.Allabolog.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebScrapController : ControllerBase
    {
        private readonly IScrapDataRepo _scrapDataRepo;

        public WebScrapController(IScrapDataRepo scrapDataRepo)
        {
            _scrapDataRepo = scrapDataRepo;
        }


        [HttpPost("ScrapInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ScrapInfo(string filterInput)
        {
            int noOfDataToBeScrapped = 0;
            int dataScrapped = 0;

            try
            {
                var driver = ChromeDriverUtils.CreateChromeDriverHeadless("https://www.allabolag.se/");

                // Deal with compliance overlay
                Thread.Sleep(5000);
                ChromeDriverUtils.CloseComplainaceOverlay(driver);


                // enter industry
                Thread.Sleep(2000);
                var input = driver.FindElementById("search-bar");
                input.Clear();
                input.SendKeys(filterInput);
                var parentElement = driver.FindElementByClass("tw-max-w-lg");
                parentElement.ClickButtonByTag("button");

                //scrapping the titles
                Thread.Sleep(5000);
                ChromeDriverUtils.ScrollToBottmOfPage(driver);

                Thread.Sleep(5000);
                var parentElementForTittle = driver.FindElementByClass("page__main");
                var allTitles = parentElementForTittle.FindAllByTag("a");
                var allArticles = parentElementForTittle.FindAllByTag("article");
      

                var scrappedData = new List<InitialScrapOutputData>();

                foreach (var article in allArticles)
                {
                    var parentElementForDetails = article.FindAllByClass("search-results__item__details").FirstOrDefault();
                    var title = article.FindElementByTag("a");
                    var categoryEl = parentElementForDetails.FindAllByTag("dd") ;

                    string category;
                    if (categoryEl.Count> 0)
                    {
                        category = categoryEl[1].Text;
                    }
                    else
                    {
                        category = null;
                    }
                    Console.WriteLine(category);

                    string remarksText;
                    var remarks = article.FindAllByClass("remarks");
                    if (remarks.Count > 0)
                    {
                        remarksText = remarks[0].Text;
                    }
                    else
                    {
                        remarksText = null;
                    }
                    var initialOutputData = new InitialScrapOutputData
                    {
                        Title = title.Text,
                        Url = title.GetAttribute("href"),
                        Verksamhet = category,
                        Remarks = remarksText,
                        SearchFieldText = filterInput

                    };
                    scrappedData.Add(initialOutputData);
                }
                driver.Quit();

                var initialScrappedData = scrappedData.Where((x) => x.Url != null && x.Title != null).ToList();
                await _scrapDataRepo.AddRangeInitialScrapDataAsync(initialScrappedData);

                var listOfDataToGetDetailedScrapData = await _scrapDataRepo.ListOfurlsNotExistingInDb(initialScrappedData);
                dataScrapped = await ScrapingDetailedDataFromEachLink(listOfDataToGetDetailedScrapData);
                noOfDataToBeScrapped = listOfDataToGetDetailedScrapData.Count();
                var apiResponse = ResponseUtils.GetSuccesfulResponse("Data Scrapped scuccesfully and is ready for download!");
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var responseMessage = dataScrapped + "/" + noOfDataToBeScrapped + "data scrapped successfully.Issues with Scrapping Data. Please try again for the rest!";
                var apiResponse = ResponseUtils.GetBadRequestResponse(responseMessage);
                return BadRequest(apiResponse);
            }
        }

        private async Task<int> ScrapingDetailedDataFromEachLink(List<InitialScrapOutputData> initialScrappedData)
        {
            int noOfDataSuccessfullyScrapped = 0;
            ChromeDriver newDriver = null;
            foreach (var data in initialScrappedData)
            {
                newDriver = ChromeDriverUtils.CreateChromeDriverHeadless(data.Url);
                ChromeDriverUtils.CloseComplainaceOverlay(newDriver);
                
                try
                {

                    Thread.Sleep(5000);
                    var orgnr = newDriver.FindElementTextByClass("orgnr");
                    var ceo = newDriver.FindElementTextByXPath("//*[@id=\"company-card_overview\"]/div[3]/div[1]/dl/dd[1]/a");
                    var address = newDriver.FindElementTextByXPath("//*[@id=\"company-card_overview\"]/div[3]/div[2]/dl/dd[2]/a[1]");
                    var location = newDriver.FindElementTextByXPath("//*[@id=\"company-card_overview\"]/div[3]/div[2]/dl/dd[3]");
                    var yearFounded = newDriver.FindElementTextByXPath("//*[@id=\"company-card_overview\"]/div[3]/div[1]/dl/dd[5]");
                    var revenue = newDriver.FindElementTextByXPath("//*[@id=\"company-card_overview\"]/div[2]/div[2]/div[1]/table/tbody/tr[1]/td[1]");
                    string urlForEmployeeDetails = MiscUtils.GetUrl(orgnr);

                    newDriver.Navigate().GoToUrl(urlForEmployeeDetails);

                    var employees = newDriver.FindAllByClass("tw-text-allabolag-600");
                    var employeeNames = string.Join('|', employees.Select((x) => x.Text).ToArray());


                    var companyDetail = new DetailedScrapOutputData
                    {
                        CompanyName = data.Title,
                        SearchFieldText = data.SearchFieldText,
                        Verksamhet = data.Verksamhet,
                        Remarks = data.Remarks,
                        DateOfSearch = DateTime.Now,
                        AllabolagUrl = data.Url,
                        OrgNo = orgnr,
                        CEO = ceo,
                        Address = address,
                        Location = location,
                        YearOfEstablishment = yearFounded,
                        Revenue = revenue,
                        EmployeeNames = employeeNames

                    };
                    await _scrapDataRepo.AddDetailedScrapDataAsync(companyDetail);
                    noOfDataSuccessfullyScrapped = noOfDataSuccessfullyScrapped + 1;


                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    newDriver.Quit();
                   
                }
            }
            return noOfDataSuccessfullyScrapped;
        }


        [HttpGet("GetScrapInfoAsCSV")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/csv")]
        public async Task<IActionResult> GetScrapInfoAsCSV()
        {
            try
            {
                List<DetailedScrapOutputData> data = await _scrapDataRepo.FindAllDetailedScrapDataAsync();
                return GenerateCsv(null, data);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var apiResponse = ResponseUtils.GetBadRequestResponse(ex.Message);
                return BadRequest(apiResponse);
            }
            
        }

        [HttpGet("GetScrapInfoAsCSVBySearchInput")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetScrapInfoAsCSVBySearchInput(string? filterInput = null)
        {
            try
            {
                List<DetailedScrapOutputData> data = await _scrapDataRepo.FilterDetailedScrapDataBySearchInputAsync(filterInput);
                if (data.Count == 0 )
                {
                    var apiResponse = ResponseUtils.GetSuccesfulResponse("No data available in that Category");
                    return Ok(apiResponse);
                }
                return GenerateCsv(filterInput, data);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var apiResponse = ResponseUtils.GetBadRequestResponse(ex.Message);
                return BadRequest(apiResponse);

            }

        }
        [HttpGet("GetScrapInfoAsCSVByCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetScrapInfoAsCSVByCategory(string? filterInput = null)
        {
            try
            {
                List<DetailedScrapOutputData> data = await _scrapDataRepo.FilterDetailedScrapDataByCategoryAsync(filterInput);
                if (data.Count == 0)
                {
                    var apiResponse = ResponseUtils.GetSuccesfulResponse("No data available in that Category");
                    return Ok(apiResponse);
                }
                return GenerateCsv(filterInput, data);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var apiResponse = ResponseUtils.GetBadRequestResponse(ex.Message);
                return BadRequest(apiResponse);

            }

            
        }


        [HttpDelete("DeleteAllScrapOutputData")]
        public async Task<IActionResult> DeleteAllScrapOutputData()
        {
            try
            {
                await _scrapDataRepo.DeleteInitialScrapDataAsync();
                await _scrapDataRepo.DeleteDetailedScrapDataAsync();
                var apiResponse = ResponseUtils.GetSuccesfulResponse("Data Deleted Successfully!");
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var apiResponse = ResponseUtils.GetBadRequestResponse(ex.Message);
                return BadRequest(apiResponse);

            }
        }

        [HttpDelete("DeleteScrapOutputDataByCategory")]
        public async Task<IActionResult> DeleteScrapOutputDataByCategory(string filterInput)
        {
            try
            {
                var list = await _scrapDataRepo.FilterDetailedScrapDataByCategoryAsync(filterInput);
                var initialScrapDataList = await _scrapDataRepo.FilterInitialScrapDataByCategoryAsync(filterInput);

                ApiResponseDto<string> apiResponse = new();
                if (list.Count == 0)
                {
                    apiResponse = ResponseUtils.GetSuccesfulResponse("No data available in that Category");
                    return Ok(apiResponse);
                }
                else
                {
                    await _scrapDataRepo.DeleteDetailedScrapOutputDataByCategory(filterInput);
                    await _scrapDataRepo.DeleteInitialScrapOutputDataByCategory(filterInput);
                    apiResponse = ResponseUtils.GetSuccesfulResponse("Data Deleted Successfully!");

                    return Ok(apiResponse);

                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var apiResponse = ResponseUtils.GetBadRequestResponse(ex.Message);
                return BadRequest(apiResponse);

            }
        }

        [HttpDelete("DeleteScrapOutputDataBySearchFilterInput")]
        public async Task<IActionResult> DeleteScrapOutputDataBySearchFilterInput(string filterInput)
        {
            try
            {
                var list = await _scrapDataRepo.FilterDetailedScrapDataBySearchInputAsync(filterInput);
                var initialScrapDataList = await _scrapDataRepo.FilterInitialScrapDataBySearchInputAsync(filterInput);

                ApiResponseDto<string> apiResponse = new();
                if (list.Count == 0)
                {
                    apiResponse = ResponseUtils.GetSuccesfulResponse("No data available in that Category");
                    return Ok(apiResponse);
                }
                else
                {
                    await _scrapDataRepo.DeleteDetailedScrapOutputDataBySearchinput(filterInput);
                    await _scrapDataRepo.DeleteInitialScrapOutputDataBySearchinput(filterInput);
                    apiResponse = ResponseUtils.GetSuccesfulResponse("Data Deleted Successfully!");
                    return Ok(apiResponse);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var apiResponse = ResponseUtils.GetBadRequestResponse(ex.Message);
                return BadRequest(apiResponse);

            }
        }

        [HttpGet("filtersBySearchInput")]
        public async Task<IActionResult> GetFilters()
        {
            try
            {
                var searchInputText = await _scrapDataRepo.GetFiltersBySearchInputFieldAsync();
                var apiResponse = ResponseUtils.GetSuccesfulResponse(searchInputText);
                return Ok(apiResponse);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var apiResponse = ResponseUtils.GetBadRequestResponse(ex.Message);
                return BadRequest(apiResponse);

            }

        }

        [HttpGet("filtersByCategory")]
        public async Task<IActionResult> GetFiltersByCategory()
        {
            try
            {
                var categories = await _scrapDataRepo.GetFiltersByCategory();
                var apiResponse = ResponseUtils.GetSuccesfulResponse(categories);
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var apiResponse = ResponseUtils.GetBadRequestResponse(ex.Message);
                return BadRequest(apiResponse);

            }

        }

        

        private IActionResult GenerateCsv(string? filterInput, List<DetailedScrapOutputData> data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new(memoryStream))
                using (CsvWriter csvWriter = new(streamWriter, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteRecords(data);
                }
                return File(memoryStream.ToArray(), "text/csv", $"AllabollagScrapper-{filterInput}{DateTime.Now.ToString("s")}.csv");
            }
        }


    }
}
