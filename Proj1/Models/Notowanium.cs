using System;
using System.Collections.Generic;

namespace Proj1.Models
{
    public partial class Notowanium
    {
        public int Idnotowania { get; set; }
        public decimal? CenaOtwarcia { get; set; }
        public decimal? CenaZamkniecia { get; set; }
        public DateTime? DataIgodzina { get; set; }
        public decimal? CenaMin { get; set; }
        public decimal? CenaMax { get; set; }
        public int? Idaktywa { get; set; }

        public virtual Aktywa? IdaktywaNavigation { get; set; }
    }
}
