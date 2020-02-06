namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_D13
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

        [StringLength(2)]
        public string STR_Y_HR
        {
            get { return __str_hr; }
            set { __str_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __str_hr;

        [StringLength(2)]
        public string STR_Y_MIN
        {
            get { return __str_min; }
            set { __str_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __str_min;

        [StringLength(2)]
        public string END_Y_HR
        {
            get { return __end_hr; }
            set { __end_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __end_hr;

        [StringLength(2)]
        public string END_Y_MIN
        {
            get { return __end_min; }
            set { __end_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __end_min;

        [StringLength(1)]
        public string END_Y_PAR { get; set; }

        [StringLength(1)]
        public string KAKN_FLG { get; set; }

        public DateTime? UPD_DATE { get; set; }

        [StringLength(7)]
        public string UPD_SHAIN_CD { get; set; }

        [StringLength(2)]
        public string YOTEI_CD { get; set; }

        [Required]
        [StringLength(1)]
        public string SHONIN_FLG { get; set; }

        public TimeRange GetWorkTimeRange()
        {
            // 開始時刻
            DateTime _s;
            if (!DateTime.TryParse($"{DATA_Y}/{DATA_M}/{DATA_D} {STR_Y_HR}:{STR_Y_MIN}", out _s)) return null;

            //終了時刻
            DateTime _e;
            if (!DateTime.TryParse($"{DATA_Y}/{DATA_M}/{DATA_D} {END_Y_HR}:{END_Y_MIN}", out _e)) return null;

            // 日付跨ぎ
            if (END_Y_PAR == "1") _e += +TimeSpan.FromDays(1);

            return new TimeRange(_s, _e);
        }

        /// <summary>
        /// このインスタンスの実労働時間を分換算でで取得します。
        /// </summary>
        /// <returns></returns>
        public int GetWorkingTime()
        {
            int time = GetWorkTimeRange()?.GetRangeMinutes() ?? 0;
            return 0 < time ? time : 0;
        }

        public DateTime GetDate()
        {
            return DateTime.Parse($"{DATA_Y}/{DATA_M}/{DATA_D}");
        }

        public void CheckValidationForForm()
        {
            // Null-空白check
            if (string.IsNullOrWhiteSpace(SHAIN_CD)) { throw new KinmuException("社員コードが空白です。"); }
            if (string.IsNullOrWhiteSpace(DATA_Y)) { throw new KinmuException("年が空白です。"); }
            if (string.IsNullOrWhiteSpace(DATA_M)) { throw new KinmuException("月が空白です。"); }
            if (string.IsNullOrWhiteSpace(DATA_D)) { throw new KinmuException("日が空白です。"); }
            if (string.IsNullOrWhiteSpace(YOTEI_CD)) { throw new KinmuException("勤務認証コード（予定）が空白です。"); }
            if (string.IsNullOrWhiteSpace(END_Y_PAR)) { throw new KinmuException("翌日フラグが空白です。"); }
            if (string.IsNullOrWhiteSpace(SHONIN_FLG)) { throw new KinmuException("承認フラグが空白です。"); }

            // 日付妥当性
            try
            {
                KinmuSystemDB.ValidationDate(DATA_Y, DATA_M, DATA_D);
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }

            // 翌日フラグ妥当性
            if (END_Y_PAR != "0" && END_Y_PAR != "1") { throw new KinmuException("翌日フラグに「" + END_Y_PAR + "」が指定されました。翌日フラグは0か1を指定します。"); }
            if (END_Y_PAR == "1" && GetWorkTimeRange() == null) { throw new KinmuException("翌日フラグが設定されましたが、有効な勤務時間が設定されていませんでした。"); }

            // 勤務時間
            TimeRange _workTime = null;
            if (YOTEI_CD == ((int)CommonDefine.NinsyoCD.出勤).ToString("00") || !string.IsNullOrWhiteSpace(STR_Y_HR) || !string.IsNullOrWhiteSpace(STR_Y_MIN) || !string.IsNullOrWhiteSpace(END_Y_HR) || !string.IsNullOrWhiteSpace(END_Y_MIN))
            {
                // 勤務時間帯を作れなかったらエラー
                _workTime = GetWorkTimeRange() ?? throw new KinmuException("有効な勤務時間が設定されていませんでした。0:00～23:59を指定してください。");
                // 勤務時間が0以下ならエラー
                if (_workTime.GetRangeMinutes() <= 0) throw new KinmuException("有効な勤務時間が設定されていませんでした。0:00～23:59を指定してください。");
                // 勤務時間が24時間を超過した場合はエラー
                if (24 * 60 < _workTime.GetRangeMinutes()) throw new KinmuException("勤務時間が24時間を超過しました。24時間を超過する場合は翌日の勤務として入力してください。");
            }
            // 認証コードは今後変更になる場合があるため、ここで存在するかのバリデーションせず、入力時にバリデーションする。
        }

        public KNS_D13 Clone()
        {
            return (KNS_D13)MemberwiseClone();
        }
    }
}
