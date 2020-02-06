namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_D10
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(7)]
        public string SHAIN_CD { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string SHUYAKU_Y
        {
            get { return __shuyaku_y; }
            set { __shuyaku_y = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value,4); }
        }
        private string __shuyaku_y;

        [Key]
        [Column(Order = 2)]
        [StringLength(2)]
        public string SHUYAKU_M
        {
            get { return __shuyaku_m; }
            set { __shuyaku_m = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __shuyaku_m;

        public int M_SHORODO_D { get; set; }

        public int M_RODO_D { get; set; }

        public int M_RODO_T { get; set; }

        public double M_NENKYU { get; set; }

        public int M_CHIGEN { get; set; }

        public int MCHOB { get; set; }

        public int MCHOD { get; set; }

        public int MCHOD_TOKU { get; set; }

        public int MCHOD_KOU { get; set; }

        public int MCHOE { get; set; }

        public int MYAKAN { get; set; }

        public int MSHUK { get; set; }

        public int? MKEKKIN { get; set; }

        public int? MBYOKETU { get; set; }

        public DateTime UPD_DATE { get; set; }
    }
}
