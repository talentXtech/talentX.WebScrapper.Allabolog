using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talentX.WebScrapper.Allabolog.Entities
{
    public class ApiResponseDto<T>
    {
        public T Data { get; set; }
        public bool isSuccess { get; set; }
    }
}
