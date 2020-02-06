namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_M02
    {
        [Key]
        [StringLength(10)]
        public string PROJ_CD { get; set; }

        [Required]
        [StringLength(30)]
        public string PROJ_NM { get; set; }

        public DateTime? UPD_DATE { get; set; }
    }
}
