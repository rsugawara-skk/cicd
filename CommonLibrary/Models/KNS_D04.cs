namespace CommonLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KNS_D04
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(7)]
        public string SHAIN_CD { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string NINSE_CD { get; set; }

        [Required]
        [StringLength(2)]
        public string STR_HR
        {
            get { return __str_hr; }
            set { __str_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __str_hr;

        [Required]
        [StringLength(2)]
        public string STR_MIN
        {
            get { return __str_min; }
            set { __str_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __str_min;

        [Required]
        [StringLength(2)]
        public string END_HR
        {
            get { return __end_hr; }
            set { __end_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __end_hr;

        [Required]
        [StringLength(2)]
        public string END_MIN
        {
            get { return __end_min; }
            set { __end_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __end_min;

        [Required]
        [StringLength(1)]
        public string END_PAR { get; set; }

        [StringLength(2)]
        public string RESTS1_HR
        {
            get { return __rests1_hr; }
            set { __rests1_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __rests1_hr;

        [StringLength(2)]
        public string RESTS1_MIN
        {
            get { return __rests1_min; }
            set { __rests1_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __rests1_min;

        [StringLength(2)]
        public string RESTE1_HR
        {
            get { return __reste1_hr; }
            set { __reste1_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __reste1_hr;

        [StringLength(2)]
        public string RESTE1_MIN
        {
            get { return __reste1_min; }
            set { __reste1_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __reste1_min;

        [StringLength(2)]
        public string RESTS2_HR
        {
            get { return __rests2_hr; }
            set { __rests2_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __rests2_hr;

        [StringLength(2)]
        public string RESTS2_MIN
        {
            get { return __rests2_min; }
            set { __rests2_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __rests2_min;

        [StringLength(2)]
        public string RESTE2_HR
        {
            get { return __reste2_hr; }
            set { __reste2_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __reste2_hr;

        [StringLength(2)]
        public string RESTE2_MIN
        {
            get { return __reste2_min; }
            set { __reste2_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __reste2_min;

        [StringLength(2)]
        public string RESTS3_HR
        {
            get { return __rests3_hr; }
            set { __rests3_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __rests3_hr;

        [StringLength(2)]
        public string RESTS3_MIN
        {
            get { return __rests3_min; }
            set { __rests3_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __rests3_min;

        [StringLength(2)]
        public string RESTE3_HR
        {
            get { return __reste3_hr; }
            set { __reste3_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __reste3_hr;

        [StringLength(2)]
        public string RESTE3_MIN
        {
            get { return __reste3_min; }
            set { __reste3_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __reste3_min;

        public int DKIJYUN_MIN { get; set; }

        public int DKINM { get; set; }

        [Required]
        [StringLength(1)]
        public string FLX_FLG { get; set; }

        public DateTime UPD_DATE { get; set; }

        [StringLength(10)]
        public string PROJ_CD { get; set; }

        [StringLength(2)]
        public string SAGYO_CD { get; set; }

        public TimeRange GetWorkTimeRange()
        {
            try
            {
                DateTime _s = DateTime.Parse($"{STR_HR}:{STR_MIN}");
                DateTime _e = DateTime.Parse($"{END_HR}:{END_MIN}");
                if (END_PAR == "1") _e += +TimeSpan.FromDays(1);

                return new TimeRange(_s, _e);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public TimeRange GetRest1TimeRange()
        {
            TimeRange worktr = GetWorkTimeRange();

            // 全て空白またはnullならnullで返す
            if (string.IsNullOrWhiteSpace(RESTS1_HR) && string.IsNullOrWhiteSpace(RESTS1_MIN) && string.IsNullOrWhiteSpace(RESTE1_HR) && string.IsNullOrWhiteSpace(RESTE1_MIN))
            {
                return null;
            }

            // 休憩時間をDateTime型に変換
            DateTime _s, _e;
            try
            {
                _s = DateTime.Parse($"{RESTS1_HR}:{RESTS1_MIN}");
                _e = DateTime.Parse($"{RESTE1_HR}:{RESTE1_MIN}");
            }
            catch (FormatException e)
            {
                throw new KinmuException("有効な休憩時間1が設定されていませんでした。0:00～23:59を指定してください。", e);
            }

            // 翌日フラグ時の対応
            if (worktr != null && END_PAR == "1" && _s < worktr.Begin) _s += TimeSpan.FromDays(1);
            if (worktr != null && END_PAR == "1" && _e < _s) _e += TimeSpan.FromDays(1);

            TimeRange _tr = new TimeRange(_s, _e);
            if (24 * 60 < _tr.GetRangeMinutes()) throw new KinmuException("休憩時間1が24時間を超過しました。");

            return _tr;
        }

        public TimeRange GetRest2TimeRange()
        {
            TimeRange worktr = GetWorkTimeRange();

            // 全て空白またはnullならnullで返す
            if (string.IsNullOrWhiteSpace(RESTS2_HR) && string.IsNullOrWhiteSpace(RESTS2_MIN) && string.IsNullOrWhiteSpace(RESTE2_HR) && string.IsNullOrWhiteSpace(RESTE2_MIN))
            {
                return null;
            }

            // 休憩時間をDateTime型に変換
            DateTime _s, _e;
            try
            {
                _s = DateTime.Parse($"{RESTS2_HR}:{RESTS2_MIN}");
                _e = DateTime.Parse($"{RESTE2_HR}:{RESTE2_MIN}");
            }
            catch (FormatException e)
            {
                throw new KinmuException("有効な休憩時間2が設定されていませんでした。0:00～23:59を指定してください。", e);
            }

            // 翌日フラグ時の対応
            if (worktr != null && END_PAR == "1" && _s < GetWorkTimeRange().Begin) _s += TimeSpan.FromDays(1);
            if (worktr != null && END_PAR == "1" && _e < _s) _e += TimeSpan.FromDays(1);

            TimeRange _tr = new TimeRange(_s, _e);
            if (24 * 60 < _tr.GetRangeMinutes()) throw new KinmuException("休憩時間2が24時間を超過しました。");

            return _tr;
        }

        public TimeRange GetRest3TimeRange()
        {
            TimeRange worktr = GetWorkTimeRange();

            // 全て空白またはnullならnullで返す
            if (string.IsNullOrWhiteSpace(RESTS3_HR) && string.IsNullOrWhiteSpace(RESTS3_MIN) && string.IsNullOrWhiteSpace(RESTE3_HR) && string.IsNullOrWhiteSpace(RESTE3_MIN))
            {
                return null;
            }

            // 休憩時間をDateTime型に変換
            DateTime _s, _e;
            try
            {
                _s = DateTime.Parse($"{RESTS3_HR}:{RESTS3_MIN}");
                _e = DateTime.Parse($"{RESTE3_HR}:{RESTE3_MIN}");
            }
            catch (FormatException e)
            {
                throw new KinmuException("有効な休憩時間3が設定されていませんでした。0:00～23:59を指定してください。", e);
            }

            // 翌日フラグ時の対応
            if (worktr != null && END_PAR == "1" && _s < GetWorkTimeRange().Begin) _s += TimeSpan.FromDays(1);
            if (worktr != null && END_PAR == "1" && _e < _s) _e += TimeSpan.FromDays(1);

            TimeRange _tr = new TimeRange(_s, _e);
            if (24 * 60 < _tr.GetRangeMinutes()) throw new KinmuException("休憩時間3が24時間を超過しました。");

            return _tr;
        }

        /// <summary>
        /// ３つの休憩時間の合計（分）を返却
        /// </summary>
        /// <returns>休憩時間の合計（分）</returns>
        public int GetTotalRestTime()
        {
            int time = 0;
            time += GetRest1TimeRange()?.GetRangeMinutes() ?? 0;
            time += GetRest2TimeRange()?.GetRangeMinutes() ?? 0;
            time += GetRest3TimeRange()?.GetRangeMinutes() ?? 0;

            return time;
        }

        /// <summary>
        /// 法定で定められた必要最低限の休憩時間を返します。
        /// </summary>
        /// <param name="_workTime"></param>
        /// <returns></returns>
        public static int GetMinimumRequiredRestTime(int _workTime)
        {
            if (8 * 60 < _workTime)
            {
                return 60;
            }
            else if (6 * 60 < _workTime)
            {
                return 45;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// このインスタンスの勤務時間を分換算でで取得します。
        /// </summary>
        /// <returns></returns>
        public int GetKinmuTime()
        {
            return GetWorkTimeRange()?.GetRangeMinutes() ?? 0;;
        }

        /// <summary>
        /// このインスタンスの実労働時間を分換算でで取得します。
        /// </summary>
        /// <returns></returns>
        public int GetWorkingTime()
        {
            int time = GetKinmuTime();
            time -= GetTotalRestTime();
            return 0 < time ? time : 0;
        }

        /// <summary>
        /// ３つの休憩時間の範囲を取得
        /// </summary>
        /// <returns></returns>
        public TimeRange[] GetRestTimeRanges()
        {
            return new TimeRange[] { GetRest1TimeRange(), GetRest2TimeRange(), GetRest3TimeRange() };
        }

        /// <summary>
        /// 入力値のバリデーションチェック
        /// </summary>
        public void CheckValidationForForm()
        {
            // Null-空白check
            if (string.IsNullOrWhiteSpace(SHAIN_CD)) { throw new KinmuException("社員コードが空白です。"); }
            if (string.IsNullOrWhiteSpace(STR_HR)) { throw new KinmuException("始業時刻（時）が空白です。"); }
            if (string.IsNullOrWhiteSpace(STR_MIN)) { throw new KinmuException("始業時刻（分）が空白です。"); }
            if (string.IsNullOrWhiteSpace(END_HR)) { throw new KinmuException("終業時刻（時）が空白です。"); }
            if (string.IsNullOrWhiteSpace(END_MIN)) { throw new KinmuException("終業時刻（分）が空白です。"); }
            if (string.IsNullOrWhiteSpace(END_PAR)) { throw new KinmuException("翌日フラグが空白です。"); }

            // 翌日フラグ妥当性
            if (END_PAR != "0" && END_PAR != "1") { throw new KinmuException("翌日フラグに「" + END_PAR + "」が指定されました。翌日フラグは0か1を指定します。"); }

            // 勤務時間
            TimeRange _workTime = null;
            if (!string.IsNullOrWhiteSpace(STR_HR) || !string.IsNullOrWhiteSpace(STR_MIN) || !string.IsNullOrWhiteSpace(END_HR) || !string.IsNullOrWhiteSpace(END_MIN))
            {
                _workTime = GetWorkTimeRange() ?? throw new KinmuException("有効な勤務時間が設定されていませんでした。0:00～23:59を指定してください。");
                if (24 * 60 < _workTime.GetRangeMinutes()) throw new KinmuException("勤務時間が24時間を超過しました。24時間を超過する場合は翌日の勤務として入力してください。");
            }

            // 休憩時間の妥当性
            TimeRange[] _rests = GetRestTimeRanges();

            // 休憩時間同士の重複チェックと勤務時間内に設定されているかのチェック
            for (int i = 0; i < _rests.Length; i++)
            {
                if (_rests[i] == null) { continue; }
                if (_rests[i].IsOverlap(_rests[(i + 1) % 3])) { throw new KinmuException("休憩時間" + (i + 1) + "と休憩時間" + (i + 2) % 3 + "が重複しています。"); }
                if (_rests[i].GetOverlapMinutes(_workTime) != _rests[i].GetRangeMinutes()) { throw new KinmuException("休憩時間" + (i + 1) + "が勤務時間外に設定されています。"); }
                if (_rests[i].Begin == _workTime.Begin || _rests[i].Last == _workTime.Last) { throw new KinmuException("休憩時間" + (i + 1) + "が勤務時間の開始、もしくは終了時刻に取られています。"); }
            }

            // 勤務時間と休憩時間の関連性チェック
            int workTimeMinute = GetKinmuTime();
            int minRestTime = GetMinimumRequiredRestTime(workTimeMinute);
            if (GetTotalRestTime() < minRestTime) { throw new KinmuException("現在の勤務時間の場合、最低でも" + minRestTime + "分の休憩が必要です。"); }
        }
    }
}
