﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.ViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string FiscalCode { get; set; }
        public string RegistryNo { get; set; }
        public string Euid { get; set; }
        public DateTime DateOfEstablishment { get; set; }
        public string NaceCode { get; set; }
        public string Activity { get; set; }
        public string ActivityDescription { get; set; }
    }
}