namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_D12
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(7)]
        public string SHAIN_CD { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string NENDO
        {
            get { return __nendo; }
            set { __nendo = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value,4); }
        }
        private string __nendo;

        public DateTime YUKOD_FROM { get; set; }

        public DateTime YUKOD_TO { get; set; }

        public double NENZAN { get; set; }

        [Required]
        [StringLength(4)]
        public string SHUYAKUY
        {
            get { return __shuyaku_y; }
            set { __shuyaku_y = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value,4); }
        }
        private string __shuyaku_y;

        [Required]
        [StringLength(2)]
        public string SHUYAKUM
        {
            get { return __shuyaku_m; }
            set { __shuyaku_m = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __shuyaku_m;

        public DateTime UPD_DATE { get; set; }
    }
}
