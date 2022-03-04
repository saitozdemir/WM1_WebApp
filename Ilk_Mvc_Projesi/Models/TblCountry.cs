using System;
using System.Collections.Generic;

#nullable disable

namespace Ilk_Mvc_Projesi.Models
{
    public partial class TblCountry
    {
        public TblCountry()
        {
            TblFilms = new HashSet<TblFilm>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public virtual ICollection<TblFilm> TblFilms { get; set; }
    }
}
