namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_M04
    {
        [Key]
        [StringLength(2)]
        public string NINSHO_CD { get; set; }

        [Required]
        [StringLength(10)]
        public string NINSHO_NM { get; set; }

        [Required]
        [StringLength(1)]
        public string REDIRECT_PATTERN { get; set; }

        [Required]
        [StringLength(2)]
        public string CALC_PATTERN { get; set; }
        
        public DateTime? UPD_DATE { get; set; }
    }
}
