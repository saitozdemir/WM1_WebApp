using System;
using System.Collections.Generic;

#nullable disable

namespace Ilk_Mvc_Projesi.Models
{
    public partial class TblLanguage
    {
        public TblLanguage()
        {
            TblFilms = new HashSet<TblFilm>();
        }

        public int LanguageId { get; set; }
        public string Language { get; set; }

        public virtual ICollection<TblFilm> TblFilms { get; set; }
    }
}
