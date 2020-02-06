namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_D09
    {
        [Key]
        [StringLength(7)]
        public string SHAIN_CD { get; set; }

        [Required]
        [StringLength(10)]
        public string PASSWORD { get; set; }

        public DateTime? UPD_DATE { get; set; }
    }
}
