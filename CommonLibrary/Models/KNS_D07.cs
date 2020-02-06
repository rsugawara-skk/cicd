　namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_D07
    {
        [Key]
        [StringLength(7)]
        public string SHAIN_CD { get; set; }

        [Required]
        [StringLength(1)]
        public string F1 { get; set; }

        [Required]
        [StringLength(1)]
        public string F2 { get; set; }

        [Required]
        [StringLength(1)]
        public string F3 { get; set; }

        [Required]
        [StringLength(1)]
        public string F4 { get; set; }

        [Required]
        [StringLength(1)]
        public string F5 { get; set; }

        [Required]
        [StringLength(1)]
        public string F6 { get; set; }

        [Required]
        [StringLength(1)]
        public string F7 { get; set; }

        [Required]
        [StringLength(1)]
        public string F8 { get; set; }

        [Required]
        [StringLength(1)]
        public string F9 { get; set; }

        [Required]
        [StringLength(1)]
        public string F10 { get; set; }

        public DateTime? UPD_DATE { get; set; }
    }
}
