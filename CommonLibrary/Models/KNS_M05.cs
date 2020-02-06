namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// カレンダーマスター
    /// </summary>
    public partial class KNS_M05
    {
        /// <summary>
        /// 年
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [StringLength(4)]
        public string DATA_Y { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string DATA_M { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        [Key]
        [Column(Order = 2)]
        [StringLength(2)]
        public string DATA_D { get; set; }

        /// <summary>
        /// 会社としての勤務予定コード
        /// </summary>
        [Required]
        [StringLength(2)]
        public string NINYO_CD { get; set; }

        /// <summary>
        /// 会社としての勤務形態コード（使用してるか不明）
        /// </summary>
        [Required]
        [StringLength(1)]
        public string KEITAI { get; set; }

        /// <summary>
        /// 祝日フラグ（"1"なら祝日）
        /// </summary>
        [Required]
        [StringLength(1)]
        public string SHUKU_FLG { get; set; }

        /// <summary>
        /// レコード更新時刻
        /// </summary>
        public DateTime? UPD_DATE { get; set; }

        /// <summary>
        /// このインスタンスの年月日を<see cref="DateTime"/>型で取得します。
        /// </summary>
        /// <exception cref="KinmuException"><see cref="DateTime.Parse(string)"/>で正常に変換できなかった場合に例外が発生します。</exception>
        /// <returns></returns>
        public DateTime GetDateTime()
        {
            try
            {
                return DateTime.Parse($"{DATA_Y}/{DATA_M}/{DATA_D}");
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// このインスタンスの曜日を文字列で取得します。年月日が不正な場合は空文字を返します。
        /// </summary>
        /// <returns></returns>
        public string GetWeekDayString()
        {
            try
            {
                return GetDateTime().ToString("ddd");
            }
            catch (KinmuException)
            {
                return "";
            }
        }
    }
}
