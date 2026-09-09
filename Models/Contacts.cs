using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace portfolioApi.Models
{
    public class Contacts
    {
        [Key]
        public int Id { get; set; }
        public string Whatsapp { get; set; }
        public string Email { get; set; }
        public string Linkedin { get; set; }
    }
}