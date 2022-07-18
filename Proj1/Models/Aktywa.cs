using System;
using System.Collections.Generic;

namespace Proj1.Models
{
    public partial class Aktywa
    {
        public Aktywa()
        {
            Notowania = new HashSet<Notowanium>();
        }

        public int Idaktywa { get; set; }
        public string? NazwaAktywa { get; set; }
        public string? KodAktywa { get; set; }

        public virtual ICollection<Notowanium> Notowania { get; set; }
    }
}
