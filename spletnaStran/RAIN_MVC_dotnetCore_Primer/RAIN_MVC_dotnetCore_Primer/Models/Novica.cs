using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vaja3.Models
{
    public class Novica
    {
        [Key]
        public int id { get; set; }
        public string avtor { get; set; }
        public string naziv { get; set; }
        public string besedilo { get; set; }
        public string datum { get; set; }
    }
}
