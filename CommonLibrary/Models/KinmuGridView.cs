using static CommonLibrary.CommonDefine;
using static CommonLibrary.Util;

namespace CommonLibrary.Models
{
    /// <summary>
    /// 勤務表示用のグリッドビューモデルです。
    /// </summary>
    public class KinmuGridView
    {
        /// <summary>
        /// 日付
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 曜日
        /// </summary>
        public string Day { get; set; }
        /// <summary>
        /// 認証コード（予定）
        /// </summary>
        public string NinsyoSchedule { get; set; }
        /// <summary>
        /// 勤務時間（予定）
        /// </summary>
        public string OpeningTimeToClosingTimeSchedule { get; set; }
        /// <summary>
        /// 認証コード（実績）
        /// </summary>
        public string NinsyoResult { get; set; }
        /// <summary>
        /// 勤務時間（実績）
        /// </summary>
        public string OpeningTimeToClosingTimeResult { get; set; }
        /// <summary>
        /// 休暇時間
        /// </summary>
        public string RestTime { get; set; }
        /// <summary>
        /// 勤務時間
        /// </summary>
        public string WorkTime { get; set; }
        /// <summary>
        /// 記事
        /// </summary>
        public string Article { get; set; }
        /// <summary>
        /// 承認フラグ（DB上ではcharだが、boolに変換）
        /// </summary>
        public bool IsAccepted { get; set; }

        /// <summary>
        /// 基底クラスのインスタンスを作成します。
        /// </summary>
        /// <param name="item"></param>
        public KinmuGridView(KinmuRecordRow item)
        {
            string yoteiCD = ((NinsyoCD)int.Parse((item.KinmuYotei.YOTEI_CD ?? item.CalendarMaster.NINYO_CD) ?? "1")).ToString();
            string kakuteiCD = ((NinsyoCD)int.Parse((item.KinmuJisseki.NINKA_CD ?? item.CalendarMaster.NINYO_CD) ?? "1")).ToString();
            TimeRange jissekitr = item.KinmuJisseki.GetWorkTimeRange();
            TimeRange yoteitr = item.KinmuYotei.GetWorkTimeRange();
            int restmin = item.KinmuJisseki.GetTotalRestTime();
            int workmin = item.KinmuJisseki.GetWorkingTime() - (item.KinmuJisseki.DMINA1 ?? 0);

            Date = item.CalendarMaster.DATA_D;
            Day = item.CalendarMaster.GetWeekDayString();

            if (item.CalendarMaster.SHUKU_FLG == "1")
            {
                Day += "　祝";
            }

            NinsyoSchedule = yoteiCD;
            WorkTime = 0 < workmin ? MinutesToStringFormat(workmin) : "";
            OpeningTimeToClosingTimeSchedule = yoteitr != null ? yoteitr.ToString() : "";
            NinsyoResult = item.KinmuJisseki.KAKN_FLG == "1" ? kakuteiCD : yoteiCD;
            OpeningTimeToClosingTimeResult = jissekitr != null ? jissekitr.ToString() : "";
            RestTime = 0 < restmin ? MinutesToStringFormat(restmin) : "";
            Article = item.KinmuJisseki.KAKN_FLG == "1" ? item.KinmuJisseki.DKIJI : "";
            IsAccepted = item.KinmuYotei.SHONIN_FLG == "1";
        }
    }

    /// <summary>
    /// 編集画面のビューモデル
    /// </summary>
    public class KinmuGridViewEditRow : KinmuGridView
    {
        /// <summary>
        /// みなし１の時間
        /// </summary>
        public string Minashi1 { get; set; }
        /// <summary>
        /// みなし２の時間
        /// </summary>
        public string Minashi2 { get; set; }
        /// <summary>
        /// 累積勤務時間
        /// </summary>
        public string WorkTimeTotal { get; set; }

        /// <summary>
        /// 編集画面のビューモデルのインスタンスを作成します。
        /// </summary>
        /// <param name="item"></param>
        public KinmuGridViewEditRow(KinmuRecordRow item) : base(item)
        {
            Minashi1 = (item.KinmuJisseki.DMINA1 == null || item.KinmuJisseki.DMINA1 <= 0) ? "" : MinutesToStringFormat((int)item.KinmuJisseki.DMINA1);
            Minashi2 = "";
            if (item.KinmuJisseki.KAKN_FLG == "1")
            {
                if (item.KinmuJisseki.DMINA2 == null)
                {
                    Minashi2 = "";
                }
                else if (0 == item.KinmuJisseki.DMINA2)
                {
                    Minashi2 = "***";
                }
                else
                {
                    Minashi2 = MinutesToStringFormat((int)item.KinmuJisseki.DMINA2);
                }
            }
            WorkTimeTotal = "0";
        }
    }

    /// <summary>
    /// コピー画面のビューモデル
    /// </summary>
    public class KinmuGridViewCopyRow : KinmuGridView
    {
        /// <summary>
        /// コピー元用ラジオボタンセル
        /// </summary>
        public string CopyRadioButton { get; set; }
        /// <summary>
        /// コピー先用チェックボックス
        /// </summary>
        public string CopyCheckBox { get; set; }

        /// <summary>
        /// コピー用のビューモデルインスタンスを作成します。
        /// </summary>
        /// <param name="item"></param>
        public KinmuGridViewCopyRow(KinmuRecordRow item) : base(item)
        {
            CopyRadioButton = "<input type=\"radio\" name=\"Original\" value=\"" + item.CalendarMaster.DATA_D + "\"/>";
            CopyCheckBox = "<input type=\"checkbox\" name=\"CopyTo\" value=\"" + item.CalendarMaster.DATA_D + "\"/>";
        }
    }

    /// <summary>
    /// 実績クリア用のビューモデルです
    /// </summary>
    public class KinmuGridViewClearRow : KinmuGridView
    {
        /// <summary>
        /// 実績クリア用のチェックボックスセル
        /// </summary>
        public string ClearCheckBox { get; set; }

        /// <summary>
        /// 実績クリア用のビューモデルインスタンスを作成します。
        /// </summary>
        /// <param name="item"></param>
        public KinmuGridViewClearRow(KinmuRecordRow item) : base(item)
        {
            ClearCheckBox = "<input type=\"checkbox\" name=\"ClearFlag\" value=\"" + item.CalendarMaster.DATA_D + "\"/>";
        }
    }

    /// <summary>
    /// 勤務予定表用のビューモデルです。
    /// </summary>
    public class KinmuGridViewWorkPlansRow : KinmuGridViewWorkResultsRow
    {
        /// <summary>
        /// みなしの時間
        /// </summary>
        public string Minashi { get; set; }

        /// <summary>
        /// 勤務予定表用のビューモデルインスタンスを作成します。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="time"></param>
        public KinmuGridViewWorkPlansRow(KinmuRecordRow item, int time) : base(item, time)
        {
            // みなしを合算して表示するので足す
            int m = item.KinmuJisseki.DMINA1 ?? 0;
            m += item.KinmuJisseki.DMINA2 ?? 0;

            Minashi = 0 < m ? MinutesToStringFormat(m) : "";
        }
    }

    /// <summary>
    /// 勤務実績表用のビューモデルです。
    /// </summary>
    public class KinmuGridViewWorkResultsRow : KinmuGridView
    {
        /// <summary>
        /// 翌日フラグ
        /// </summary>
        public string NextDayFlag { get; set; }

        /// <summary>
        /// 累積勤務時間
        /// </summary>
        public string WorkTimeTotal { get; set; }

        /// <summary>
        /// 勤務実績表用のビューモデルインスタンスを作成します。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="time"></param>
        public KinmuGridViewWorkResultsRow(KinmuRecordRow item, int time) : base(item)
        {
            if (item.KinmuJisseki.KAKN_FLG != "1" || (item.KinmuYotei.YOTEI_CD ?? item.CalendarMaster.NINYO_CD) == item.KinmuJisseki.NINKA_CD)
            {
                NinsyoResult = "";
            }
            WorkTimeTotal = 0 < item.KinmuJisseki.GetWorkingTime() ? MinutesToStringFormat(time) : "";
            NextDayFlag = item.KinmuJisseki.END_PAR == "1" ? "*" : "";
            if (item.KinmuJisseki.KAKN_FLG != "1")
            {
                Article = "実績が入力されていません。";
            }
        }
    }
}
