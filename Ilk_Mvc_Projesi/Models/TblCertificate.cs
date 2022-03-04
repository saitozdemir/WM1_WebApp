using System;
using System.Collections.Generic;

#nullable disable

namespace Ilk_Mvc_Projesi.Models
{
    public partial class TblCertificate
    {
        public TblCertificate()
        {
            TblFilms = new HashSet<TblFilm>();
        }

        public long CertificateId { get; set; }
        public string Certificate { get; set; }

        public virtual ICollection<TblFilm> TblFilms { get; set; }
    }
}
