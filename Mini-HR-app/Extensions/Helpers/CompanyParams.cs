using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.Helpers
{
    public class CompanyParams : QueryStringParameters
    {
        public CompanyParams()
        {
            OrderBy = "companyName";
        }

        public int? MinYearOfEstablishment { get; set; }
        public int? MaxYearOfEstablishment { get; set; } = DateTime.Now.Year;
        public bool ValidYearRange => MaxYearOfEstablishment > MinYearOfEstablishment;

        public string CompanyName { get; set; }
    }
}
