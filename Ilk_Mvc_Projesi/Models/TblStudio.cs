using System;
using System.Collections.Generic;

#nullable disable

namespace Ilk_Mvc_Projesi.Models
{
    public partial class TblStudio
    {
        public TblStudio()
        {
            TblFilms = new HashSet<TblFilm>();
        }

        public int StudioId { get; set; }
        public string StudioName { get; set; }

        public virtual ICollection<TblFilm> TblFilms { get; set; }
    }
}
