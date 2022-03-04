using System;
using System.Collections.Generic;

#nullable disable

namespace Ilk_Mvc_Projesi.Models
{
    public partial class TblCast
    {
        public int CastId { get; set; }
        public int? CastFilmId { get; set; }
        public int? CastActorId { get; set; }
        public string CastCharacterName { get; set; }
    }
}
