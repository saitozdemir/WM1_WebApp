using System;
using System.Collections.Generic;

#nullable disable

namespace Ilk_Mvc_Projesi.Models
{
    public partial class TblDirector
    {
        public TblDirector()
        {
            TblFilms = new HashSet<TblFilm>();
        }

        public int DirectorId { get; set; }
        public string DirectorName { get; set; }
        public DateTime? DirectorDob { get; set; }
        public string DirectorGender { get; set; }

        public virtual ICollection<TblFilm> TblFilms { get; set; }
    }
}
