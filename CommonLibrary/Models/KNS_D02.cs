using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibrary.Models
{
    public partial class KNS_D02
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
        [StringLength(10)]
        public string PROJ_CD { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(2)]
        public string SAGYO_CD { get; set; }

        public int SAGYO_MIN { get; set; }

        public DateTime? UPD_DATE { get; set; }

        public void CheckValidationForForm()
        {
            // Null-空白check
            if (string.IsNullOrWhiteSpace(SHAIN_CD)) { throw new KinmuException("社員コードが空白です。"); }
            if (string.IsNullOrWhiteSpace(DATA_Y)) { throw new KinmuException("年が空白です。"); }
            if (string.IsNullOrWhiteSpace(DATA_M)) { throw new KinmuException("月が空白です。"); }
            if (string.IsNullOrWhiteSpace(DATA_D)) { throw new KinmuException("日が空白です。"); }
            if (string.IsNullOrWhiteSpace(PROJ_CD)) { throw new KinmuException("プロジェクトコードが空白です。"); }
            if (string.IsNullOrWhiteSpace(SAGYO_CD)) { throw new KinmuException("作業コードが空白です。"); }

            // 日付妥当性
            try
            {
                KinmuSystemDB.ValidationDate(DATA_Y, DATA_M, DATA_D);
            }
            catch (InvalidCastException e)
            {
                throw new KinmuException(e.Message, e);
            }

            // 作業時間妥当性
            if (SAGYO_MIN <= 0) { throw new KinmuException("作業時間が0以下です。"); }
            if (1440 <= SAGYO_MIN) { throw new KinmuException("作業時間が24時間を超過しています。"); }
        }

        public KNS_D02 Clone()
        {
            return (KNS_D02)MemberwiseClone();
        }
    }
}
