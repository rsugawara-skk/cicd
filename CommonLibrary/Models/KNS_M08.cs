namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_M08
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(1)]
        public string MIBUN { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SHUGYOKIKAN { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(2)]
        public string HATSUREI_M { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FUYO_D { get; set; }

        public DateTime? UPD_DATE { get; set; }
    }
}
