using System;

namespace Mini_HR_app.Helpers
{
    public class CompanyParams : QueryStringParameters
    {
        public int MinYear { get; set; } = 1900;
        public int MaxYear { get; set; } = DateTime.UtcNow.Year;
        public bool ValidYearRange => MaxYear > MinYear;

        public string CompanyName { get; set; }
        public string Property { get; set; }
    }
}
