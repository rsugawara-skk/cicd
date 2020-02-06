namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_M01
    {
        [Key]
        [StringLength(7)]
        public string SHAIN_CD { get; set; }

        [Required]
        [StringLength(4)]
        public string HATSUREI_Y { get; set; }

        [Required]
        [StringLength(2)]
        public string HATSUREI_M { get; set; }

        [Required]
        [StringLength(2)]
        public string HATSUREI_D { get; set; }

        [Required]
        [StringLength(50)]
        public string SHAIN_NM { get; set; }

        [Required]
        [StringLength(1)]
        public string IK_KUBUN { get; set; }

        [Required]
        [StringLength(1)]
        public string KINSHUBETSU { get; set; }

        [Required]
        [StringLength(1)]
        public string MIBUN { get; set; }

        [Required]
        [StringLength(1)]
        public string FLX_KUBUN { get; set; }

        [Required]
        [StringLength(1)]
        public string GYOMU_KUBUN { get; set; }

        public DateTime? UPD_DATE { get; set; }

        [Required]
        [StringLength(50)]
        public string ALIAS { get; set; }

        [StringLength(1)]
        public string MANAGER { get; set; }

        [StringLength(1)]
        public string SHAIN_KUBUN { get; set; }

        [StringLength(7)]
        public string SHONIN_SHAIN_CD { get; set; }

        [StringLength(40)]
        public string PC_NAME { get; set; }
    }
}
