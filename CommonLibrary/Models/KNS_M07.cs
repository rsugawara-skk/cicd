namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_M07
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(4)]
        public string SHUYAKUY { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string SHUYAKUM { get; set; }

        public DateTime? SHUYAKUD { get; set; }

        public DateTime? UPD_DATE { get; set; }
    }
}
