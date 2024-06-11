using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace talentX.WebScrapper.Allabolog.Entities
{
    public class InitialScrapOutputData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
        public string Verksamhet { get; set; }
        public string SearchFieldText { get; set; }

        public string? Remarks { get; set; }
    }
}
