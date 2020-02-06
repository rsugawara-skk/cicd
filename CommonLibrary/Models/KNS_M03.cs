namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_M03
    {
        [Key]
        [StringLength(2)]
        public string SAGYO_CD { get; set; }

        [StringLength(40)]
        public string SAGYO_NM { get; set; }

        public DateTime? UPD_DATE { get; set; }
    }
}
