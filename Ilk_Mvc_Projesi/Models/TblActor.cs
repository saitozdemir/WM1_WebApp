using System;
using System.Collections.Generic;

#nullable disable

namespace Ilk_Mvc_Projesi.Models
{
    public partial class TblActor
    {
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public DateTime? ActorDob { get; set; }
        public string ActorGender { get; set; }
    }
}
