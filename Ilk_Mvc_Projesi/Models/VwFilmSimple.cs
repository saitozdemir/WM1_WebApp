using System;
using System.Collections.Generic;

#nullable disable

namespace Ilk_Mvc_Projesi.Models
{
    public partial class VwFilmSimple
    {
        public int FilmId { get; set; }
        public string FilmName { get; set; }
        public int? FilmBoxOfficeDollars { get; set; }
    }
}
