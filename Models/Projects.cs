using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace portfolioApi.Models
{
    public class Projects
    {
        [Key]
        public int Id { get; set; }
        public string TitleProject { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public string Status { get; set; }
        public string Technologies{get; set;}
    }
}