namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_D05
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(7)]
        public string SHAIN_CD { get; set; }
        
        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string PROJ_CD { get; set; }

        public DateTime UPD_DATE { get; set; }

        [Required]
        [StringLength(2)]
        public string VIEW_ORDER { get; set; }
    }
}
