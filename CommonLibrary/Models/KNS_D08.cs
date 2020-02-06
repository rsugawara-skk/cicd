namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_D08
    {
        [Key]
        [StringLength(10)]
        public string HYOJI_FLG { get; set; }

        [StringLength(10)]
        public string F1 { get; set; }

        [StringLength(10)]
        public string F2 { get; set; }

        [StringLength(10)]
        public string F3 { get; set; }

        [StringLength(50)]
        public string DB_MSG1 { get; set; }

        [StringLength(50)]
        public string DB_MSG2 { get; set; }

        [StringLength(50)]
        public string DB_MSG3 { get; set; }

        [StringLength(50)]
        public string DB_MSG4 { get; set; }

        [StringLength(50)]
        public string DB_MSG5 { get; set; }

        [StringLength(50)]
        public string DB_MSG6 { get; set; }

        [StringLength(50)]
        public string DB_MSG7 { get; set; }

        [StringLength(50)]
        public string DB_MSG8 { get; set; }

        [StringLength(50)]
        public string DB_MSG9 { get; set; }

        [StringLength(50)]
        public string DB_MSG10 { get; set; }

        public DateTime? UPD_DATE { get; set; }
    }
}
