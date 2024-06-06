namespace Dot.Net.WebApi.Controllers.Domain
{
    public class Rating
    {
        // TODO: Map columns in data table RATING with corresponding fields
        public int Id { get; set; }
        public string MoodysRating { get; set; }
        public string SandPRating { get; set; }
        public string FitchRating { get; set; }
        public byte? OrderNumber { get; set; }
    }
}