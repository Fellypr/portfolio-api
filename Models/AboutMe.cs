using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace portfolioApi.Models
{
    public class AboutMe
    {
        [Key]
        public int Id { get; set; }
        public string NameInPortfolio { get; set; }
        public string SubDescriptions { get; set; }
        public string Profession { get; set; }
        public string DescriptionAboutMe { get; set; }
    }
}