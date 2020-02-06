namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_D14
    {
        [Key]
        [StringLength(7)]
        public string SHAIN_CD { get; set; }

        [StringLength(20)]
        public string LAST_LOGIN_DATE { get; set; }
    }
}
