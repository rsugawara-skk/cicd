namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_D03
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(7)]
        public string SHAIN_CD { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string DATA_Y
        {
            get { return __data_y; }
            set { __data_y = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value,4); }
        }
        private string __data_y;

        [Key]
        [Column(Order = 2)]
        [StringLength(2)]
        public string DATA_M
        {
            get { return __data_m; }
            set { __data_m = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __data_m;

        [Key]
        [Column(Order = 3)]
        [StringLength(2)]
        public string DATA_D
        {
            get { return __data_d; }
            set { __data_d = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __data_d;

        [Key]
        [Column(Order = 4)]
        [StringLength(4)]
        public string TEATE_CD { get; set; }

        [StringLength(2)]
        public string DTEATE_CNT { get; set; }

        public DateTime? UPD_DATE { get; set; }
    }
}
