using System;
using System.Collections.Generic;

#nullable disable

namespace Ilk_Mvc_Projesi.Models
{
    public partial class TblFilm
    {
        public int FilmId { get; set; }
        public string FilmName { get; set; }
        public DateTime? FilmReleaseDate { get; set; }
        public int? FilmDirectorId { get; set; }
        public int? FilmLanguageId { get; set; }
        public int? FilmCountryId { get; set; }
        public int? FilmStudioId { get; set; }
        public string FilmSynopsis { get; set; }
        public int? FilmRunTimeMinutes { get; set; }
        public long? FilmCertificateId { get; set; }
        public int? FilmBudgetDollars { get; set; }
        public int? FilmBoxOfficeDollars { get; set; }
        public int? FilmOscarNominations { get; set; }
        public int? FilmOscarWins { get; set; }

        public virtual TblCertificate FilmCertificate { get; set; }
        public virtual TblCountry FilmCountry { get; set; }
        public virtual TblDirector FilmDirector { get; set; }
        public virtual TblLanguage FilmLanguage { get; set; }
        public virtual TblStudio FilmStudio { get; set; }
    }
}
