using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vaja3.Models
{
    public class Racun
    {
        [Key]
        public string upime { get; set; }
        public string geslo { get; set; }
        public string vrsta { get; set; }
    }
}
