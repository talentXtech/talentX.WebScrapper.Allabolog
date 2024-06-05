using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talentX.WebScrapper.Allabolog.Utils
{
    public class MiscUtils
    {
        public static string GetUrl(string orgnr)
        {
            string[] characters = orgnr.Split(":");
            string[] characters2 = characters[1].Trim().Split("-");

            var urlForEmployeeDetails = $"https://www.allabolag.se/{characters2[0]}{characters2[1]}/befattningar";
            return urlForEmployeeDetails;
        }
    }
}
