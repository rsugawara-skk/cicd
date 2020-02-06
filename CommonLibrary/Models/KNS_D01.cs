using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CommonLibrary.Models
{

#pragma warning disable S101 // Types should be named in PascalCase
    /// <summary>
    /// 日別勤務実績データ
    /// </summary>
    public partial class KNS_D01
#pragma warning restore S101 // Types should be named in PascalCase
    {
        /// <summary>
        /// 社員コード
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [StringLength(7)]
        public string SHAIN_CD { get; set; }

        /// <summary>
        /// 実績年
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string DATA_Y
        {
            get { return __data_y; }
            set { __data_y = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value, 4); }
        }
        private string __data_y;

        /// <summary>
        /// 実績月
        /// </summary>
        [Key]
        [Column(Order = 2)]
        [StringLength(2)]
        public string DATA_M
        {
            get { return __data_m; }
            set { __data_m = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __data_m;

        /// <summary>
        /// 実績日
        /// </summary>
        [Key]
        [Column(Order = 3)]
        [StringLength(2)]
        public string DATA_D
        {
            get { return __data_d; }
            set { __data_d = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __data_d;

        /// <summary>
        /// 勤務認証コード（予定）
        /// 出勤、特休、公休などカレンダーで定められている既知の予定です。
        /// </summary>
        [StringLength(2)]
        public string NINYO_CD { get; set; }

        /// <summary>
        /// 勤務認証コード（確定）
        /// 実際の出勤、年休等の実績です。
        /// </summary>
        [StringLength(2)]
        public string NINKA_CD { get; set; }

        /// <summary>
        /// 日別基準時間
        /// NINYO_CDで定められた勤務基準時間（分単位）です。
        /// </summary>
        [StringLength(4)]
        public string DKIJYUN_MIN { get; set; }

        /// <summary>
        /// 始業時刻（時）
        /// </summary>
        [StringLength(2)]
        public string STR_HR
        {
            get { return __str_hr; }
            set { __str_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __str_hr;

        /// <summary>
        /// 始業時刻（分）
        /// </summary>
        [StringLength(2)]
        public string STR_MIN
        {
            get { return __str_min; }
            set { __str_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __str_min;

        /// <summary>
        /// 終業時刻（時）
        /// </summary>
        [StringLength(2)]
        public string END_HR
        {
            get { return __end_hr; }
            set { __end_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __end_hr;

        /// <summary>
        /// 終業時刻（分）
        /// </summary>
        [StringLength(2)]
        public string END_MIN
        {
            get { return __end_min; }
            set { __end_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __end_min;

        /// <summary>
        /// 翌日フラグ。"1"なら日付またがりの勤務になる。
        /// </summary>
        [StringLength(1)]
        public string END_PAR
        {
            get { return __end_par; }
            set { __end_par = (value == "1") ? "1" : "0"; }
        }
        private string __end_par;

        /// <summary>
        /// 休憩時間1（開始時）
        /// </summary>
        [StringLength(2)]
        public string RESTS1_HR
        {
            get { return __rests1_hr; }
            set { __rests1_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __rests1_hr;

        /// <summary>
        /// 休憩時間1（開始分）
        /// </summary>
        [StringLength(2)]
        public string RESTS1_MIN
        {
            get { return __rests1_min; }
            set { __rests1_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __rests1_min;


        /// <summary>
        /// 休憩時間1（終了時）
        /// </summary>
        [StringLength(2)]
        public string RESTE1_HR
        {
            get { return __reste1_hr; }
            set { __reste1_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __reste1_hr;

        /// <summary>
        /// 休憩時間1（終了分）
        /// </summary>
        [StringLength(2)]
        public string RESTE1_MIN
        {
            get { return __reste1_min; }
            set { __reste1_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __reste1_min;

        /// <summary>
        /// 休憩時間2（開始時）
        /// </summary>
        [StringLength(2)]
        public string RESTS2_HR
        {
            get { return __rests2_hr; }
            set { __rests2_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __rests2_hr;

        /// <summary>
        /// 休憩時間2（開始分）
        /// </summary>
        [StringLength(2)]
        public string RESTS2_MIN
        {
            get { return __rests2_min; }
            set { __rests2_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __rests2_min;

        /// <summary>
        /// 休憩時間2（終了時）
        /// </summary>
        [StringLength(2)]
        public string RESTE2_HR
        {
            get { return __reste2_hr; }
            set { __reste2_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __reste2_hr;

        /// <summary>
        /// 休憩時間2（終了分）
        /// </summary>
        [StringLength(2)]
        public string RESTE2_MIN
        {
            get { return __reste2_min; }
            set { __reste2_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __reste2_min;

        /// <summary>
        /// 休憩時間3（開始時）
        /// </summary>
        [StringLength(2)]
        public string RESTS3_HR
        {
            get { return __rests3_hr; }
            set { __rests3_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __rests3_hr;

        /// <summary>
        /// 休憩時間3（開始分）
        /// </summary>
        [StringLength(2)]
        public string RESTS3_MIN
        {
            get { return __rests3_min; }
            set { __rests3_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __rests3_min;

        /// <summary>
        /// 休憩時間3（終了時）
        /// </summary>
        [StringLength(2)]
        public string RESTE3_HR
        {
            get { return __reste3_hr; }
            set { __reste3_hr = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __reste3_hr;

        /// <summary>
        /// 休憩時間3（終了分）
        /// </summary>
        [StringLength(2)]
        public string RESTE3_MIN
        {
            get { return __reste3_min; }
            set { __reste3_min = KinmuSystemDB.SanitizeAndZeroPaddingToIntString(value); }
        }
        private string __reste3_min;

        /// <summary>
        /// 勤務実績
        /// </summary>
        public int? DKINM { get; set; }

        /// <summary>
        /// 超勤B
        /// </summary>
        public int? DCHOB { get; set; }

        /// <summary>
        /// 超勤D
        /// </summary>
        public int? DCHOD { get; set; }

        /// <summary>
        /// 夜勤C
        /// </summary>
        public int? DYAKN { get; set; }

        /// <summary>
        /// 祝日C（未使用）
        /// </summary>
        public int? DSHUK { get; set; }

        /// <summary>
        /// みなし1の時間
        /// </summary>
        public int? DMINA1 { get; set; }

        /// <summary>
        /// みなし2の時間
        /// </summary>
        public int? DMINA2 { get; set; }

        /// <summary>
        /// 確認フラグ（ユーザが更新したら1になります）
        /// </summary>
        [StringLength(1)]
        public string KAKN_FLG { get; set; }

        /// <summary>
        /// 記事
        /// </summary>
        [StringLength(50)]
        public string DKIJI { get; set; }

        /// <summary>
        /// レコード更新時刻
        /// </summary>
        public DateTime? UPD_DATE { get; set; }

        /// <summary>
        /// レコード更新者
        /// </summary>
        [StringLength(7)]
        public string UPD_SHAIN_CD { get; set; }

        /// <summary>
        /// 勤務時間を<see cref="TimeRange"/>型で返します。勤務時間が無い、もしくは不正な場合はnullを返します。
        /// </summary>
        /// <returns></returns>
        public TimeRange GetWorkTimeRange()
        {
            // 開始時刻
            if (!DateTime.TryParse($"{DATA_Y}/{DATA_M}/{DATA_D} {STR_HR}:{STR_MIN}", out DateTime _s)) return null;

            //終了時刻
            if (!DateTime.TryParse($"{DATA_Y}/{DATA_M}/{DATA_D} {END_HR}:{END_MIN}", out DateTime _e)) return null;

            // 日付跨ぎ
            if (END_PAR == "1") _e = _e.AddDays(1);

            return new TimeRange(_s, _e);
        }

        /// <summary>
        /// 休憩時間1を<see cref="TimeRange"/>型で返します。休憩時間が無い、もしくは不正な場合はnullを返します。
        /// </summary>
        /// <returns></returns>
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
                _s = DateTime.Parse($"{DATA_Y}/{DATA_M}/{DATA_D} {RESTS1_HR}:{RESTS1_MIN}");
                _e = DateTime.Parse($"{DATA_Y}/{DATA_M}/{DATA_D} {RESTE1_HR}:{RESTE1_MIN}");
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

        /// <summary>
        /// 休憩時間2を<see cref="TimeRange"/>型で返します。休憩時間が無い、もしくは不正な場合はnullを返します。
        /// </summary>
        /// <returns></returns>
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
                _s = DateTime.Parse($"{DATA_Y}/{DATA_M}/{DATA_D} {RESTS2_HR}:{RESTS2_MIN}");
                _e = DateTime.Parse($"{DATA_Y}/{DATA_M}/{DATA_D} {RESTE2_HR}:{RESTE2_MIN}");
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


        /// <summary>
        /// 休憩時間3を<see cref="TimeRange"/>型で返します。休憩時間が無い、もしくは不正な場合はnullを返します。
        /// </summary>
        /// <returns></returns>
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
                _s = DateTime.Parse($"{DATA_Y}/{DATA_M}/{DATA_D} {RESTS3_HR}:{RESTS3_MIN}");
                _e = DateTime.Parse($"{DATA_Y}/{DATA_M}/{DATA_D} {RESTE3_HR}:{RESTE3_MIN}");
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
        /// 休憩時間の合計を分換算で返します。
        /// </summary>
        /// <returns></returns>
        public int GetTotalRestTime()
        {
            int time = 0;
            time += GetRest1TimeRange()?.GetRangeMinutes() ?? 0;
            time += GetRest2TimeRange()?.GetRangeMinutes() ?? 0;
            time += GetRest3TimeRange()?.GetRangeMinutes() ?? 0;

            return time;
        }

        /// <summary>
        /// このインスタンスの実労働時間を分換算でで取得します。
        /// </summary>
        /// <returns></returns>
        public int GetWorkingTime()
        {
            int time = GetWorkTimeRange()?.GetRangeMinutes() ?? 0;
            time -= GetTotalRestTime();
            return 0 < time ? time : 0;
        }

        /// <summary>
        /// 休憩時間全ての<see cref="TimeRange"/>を配列で返します。
        /// </summary>
        /// <returns></returns>
        public TimeRange[] GetRestTimeRanges()
        {
            return new TimeRange[] { GetRest1TimeRange(), GetRest2TimeRange(), GetRest3TimeRange() };
        }

        /// <summary>
        /// このインスタンスの日付を返します。
        /// </summary>
        /// <returns></returns>
        public DateTime GetDate()
        {
            try
            {
                return DateTime.Parse($"{DATA_Y}/{DATA_M}/{DATA_D}");
            }
            catch (Exception e)
            {
                throw new KinmuException("勤務実績の日付が不正な値でした。", e);
            }
        }

        /// <summary>
        /// フォームから入力した値をセットしたあとに実施するバリデーションです。
        /// </summary>
        public void CheckValidationForForm()
        {
            // Null-空白check
            if (string.IsNullOrWhiteSpace(SHAIN_CD)) { throw new KinmuException("社員コードが空白です。"); }
            if (string.IsNullOrWhiteSpace(DATA_Y)) { throw new KinmuException("年が空白です。"); }
            if (string.IsNullOrWhiteSpace(DATA_M)) { throw new KinmuException("月が空白です。"); }
            if (string.IsNullOrWhiteSpace(DATA_D)) { throw new KinmuException("日が空白です。"); }
            //つかわない
            //if (string.IsNullOrWhiteSpace(NINYO_CD)) { throw new KinmuException("勤務認証コード（予定）が空白です。"); }
            if (string.IsNullOrWhiteSpace(NINKA_CD)) { throw new KinmuException("勤務認証コード（確定）が空白です。"); }

            //if (string.IsNullOrWhiteSpace(STR_HR)) { throw new KinmuException("始業時刻（時）が空白です。"); }
            //if (string.IsNullOrWhiteSpace(STR_MIN)) { throw new KinmuException("始業時刻（分）が空白です。"); }
            //if (string.IsNullOrWhiteSpace(END_HR)) { throw new KinmuException("終業時刻（時）が空白です。"); }
            //if (string.IsNullOrWhiteSpace(END_MIN)) { throw new KinmuException("終業時刻（分）が空白です。"); }
            if (string.IsNullOrWhiteSpace(END_PAR)) { throw new KinmuException("翌日フラグが空白です。"); }

            // 日付妥当性
            try
            {
                KinmuSystemDB.ValidationDate(DATA_Y, DATA_M, DATA_D);
            }
            catch (InvalidCastException e)
            {
                throw new KinmuException(e.Message, e);
            }

            // 翌日フラグ妥当性
            if (END_PAR != "0" && END_PAR != "1") { throw new KinmuException("翌日フラグに「" + END_PAR + "」が指定されました。翌日フラグは0か1を指定します。"); }
            if (END_PAR == "1" && GetWorkTimeRange() ==null) { throw new KinmuException("翌日フラグが設定されましたが、有効な勤務時間が設定されていませんでした。"); }

            // 勤務時間
            TimeRange _workTime = null;
            if (!string.IsNullOrWhiteSpace(STR_HR) || !string.IsNullOrWhiteSpace(STR_MIN) || !string.IsNullOrWhiteSpace(END_HR) || !string.IsNullOrWhiteSpace(END_MIN))
            {
                // 勤務時間帯を作れなかったらエラー
                _workTime = GetWorkTimeRange() ?? throw new KinmuException("有効な勤務時間が設定されていませんでした。0:00～23:59を指定してください。");
                // 勤務時間が0以下ならエラー
                if (_workTime.GetRangeMinutes() <= 0) throw new KinmuException("有効な勤務時間が設定されていませんでした。0:00～23:59を指定してください。");
                // 勤務時間が24時間を超過した場合はエラー
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

            // 認証コードは今後変更になる場合があるため、ここで存在するかのバリデーションせず、入力時にバリデーションする。
        }

        /// <summary>
        /// このインスタンスを複製します。
        /// </summary>
        /// <returns></returns>
        public KNS_D01 Clone()
        {
            return (KNS_D01)MemberwiseClone();
        }

        /// <summary>
        /// このインスタンスの翌日フラグを取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetNextDayFlagString(string value)
        {
            if (END_PAR != "1")
            {
                return "";
            }
            else if (string.IsNullOrWhiteSpace(value))
            {
                return "*";
            }
            else
            {
                return value;
            }
        }
    }
}
