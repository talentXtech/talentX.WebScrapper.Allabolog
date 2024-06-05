using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using talentX.WebScrapper.Allabolog.Entities;
using talentX.WebScrapper.Allabolog.Repositories.Contracts;
using talentX.WebScrapper.Allabolog.Utils;
using talentX.WebScrapper.Allabolog.Extensions;
using CsvHelper;
using OpenQA.Selenium.Chrome;
using System;

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


        [HttpGet("ScrapInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ScrapInfo(string filterInput = "biotech")
        {
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
                // driver.ScrollToBottmOfPage();

                Thread.Sleep(5000);
                var parentElementForTittle = driver.FindElementByClass("page__main");
                var allTitles = parentElementForTittle.FindAllByTag("a");

                var scrappedData = new List<InitialScrapOutputData>();

                foreach (var title in allTitles)
                {
                    var initialOutputData = new InitialScrapOutputData
                    {
                        Title = title.Text,
                        Url = title.GetAttribute("href")
                    };
                    scrappedData.Add(initialOutputData);
                }
                driver.Quit();

                var initialScrappedData = scrappedData.Where((x) => x.Url != null && x.Title != null).ToList();
                await _scrapDataRepo.AddRangeInitialScrapDataAsync(initialScrappedData);

                var listOfDataToGetDetailedScrapData = await _scrapDataRepo.ListOfurlsNotExistingInDb(initialScrappedData);

                await ScrapingDetailedDataFromEachLink(listOfDataToGetDetailedScrapData, filterInput);
                return Ok(initialScrappedData);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);

            }

        }


        [HttpGet("GetScrapInfoAsCSV")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/csv")]
        public async Task<IActionResult> GetScrapInfoAsCSV(string? filterInput = null)
        {
            var data = await _scrapDataRepo.FindAllDetailedScrapDataAsync(filterInput);

            using (var memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new(memoryStream))
                using (CsvWriter csvWriter = new(streamWriter, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteRecords(data);
                }

                return File(memoryStream.ToArray(), "text/csv", $"AllabollagScrapper-{DateTime.Now.ToString("s")}.csv");
            }
        }

        [HttpDelete("DeleteInitialScrapOutputData")]
        public async Task<IActionResult> DeleteInitialScrapOutputData()
        {
            try
            {
                await _scrapDataRepo.DeleteInitialScrapDataAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);

            }
        }

        [HttpDelete("DeleteDetailedScrapOutputData")]
        public async Task<IActionResult> DeleteDetailedScrapOutputData()
        {
            try
            {
                await _scrapDataRepo.DeleteDetailedScrapDataAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);

            }
        }

        private async Task ScrapingDetailedDataFromEachLink(List<InitialScrapOutputData> finalScrappedDatas, string filterInput)
        {
            ChromeDriver newDriver = null;
            foreach (var data in finalScrappedDatas)
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
                        SearchFieldText = filterInput,
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


                }
                catch (Exception ex)
                {
                    var companyDetail = new DetailedScrapOutputData
                    {
                        CompanyName = data.Title,
                        SearchFieldText = filterInput,
                        DateOfSearch = new DateTime(),
                        AllabolagUrl = data.Url,
                        OrgNo = "Issue with getting data",
                        CEO = "Issue with getting data",
                        Address = "Issue with getting data",
                        Location = "Issue with getting data",
                        YearOfEstablishment = "Issue with getting data",
                        Revenue = "Issue with getting data",
                        EmployeeNames = "Issue with getting data"
                    };
                    await _scrapDataRepo.AddDetailedScrapDataAsync(companyDetail);
                    Console.WriteLine(ex.Message);
                    throw;
                }
                newDriver.Quit();

            }
        }


    }
}
