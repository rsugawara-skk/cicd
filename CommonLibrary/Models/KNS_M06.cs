namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_M06
    {
        [Key]
        [StringLength(4)]
        public string TEATE_CD { get; set; }

        [StringLength(10)]
        public string TEATE_NM { get; set; }

        public DateTime? UPD_DATE { get; set; }
    }
}
