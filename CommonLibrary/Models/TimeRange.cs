using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Models
{
    /// <summary>
    /// 開始時刻と終了時刻を持つ時間帯を表すオブジェクトです。
    /// </summary>
    public class TimeRange
    {
        /// <summary>
        /// このインスタンスの開始時刻を取得します。
        /// </summary>
        public DateTime Begin { get; set; }

        /// <summary>
        /// このインスタンスの終了時刻を取得します。
        /// </summary>
        public DateTime Last { get; set; }

        /// <summary>
        /// 開始時刻と終了時刻を持つ時間帯を表すオブジェクトを作成します。
        /// </summary>
        /// <param name="_begin">このインスタンスの開始時刻</param>
        /// <param name="_last">このインスタンスの終了時刻</param>
        /// <exception cref="KinmuException"></exception>
        public TimeRange(DateTime _begin, DateTime _last)
        {
            Begin = _begin;
            Last = _last;
            if (Last < Begin) throw new KinmuException("開始時刻と終了時刻が逆転しているため、オブジェクトを作成できませんでした。");
        }

        /// <summary>
        /// 開始時刻と終了時刻を持つ時間帯を表すオブジェクトを作成します。
        /// </summary>
        /// <param name="beginTimeValue">このインスタンスの開始時刻</param>
        /// <param name="lastTimeValue">このインスタンスの終了時刻</param>
        public TimeRange(int beginTimeValue, int lastTimeValue)
            : this(DateTime.Now - DateTime.Now.TimeOfDay + TimeSpan.FromMinutes(beginTimeValue), DateTime.Now - DateTime.Now.TimeOfDay + TimeSpan.FromMinutes(lastTimeValue))
        {
        }

        /// <summary>
        /// このインスタンスと引数の<see cref="TimeRange"/>が重複しているか確認します。
        /// </summary>
        /// <param name="timeRange">確認したい時間帯</param>
        /// <returns>重複していたらTrue、していなければFalse</returns>
        public bool IsOverlap(TimeRange timeRange)
        {
            if (timeRange == null) return false;
            if (Begin <= timeRange.Begin && timeRange.Begin <= Last) return true;
            if (Begin <= timeRange.Last && timeRange.Last <= Last) return true;

            if (timeRange.Begin <= Begin && Begin <= timeRange.Last) return true;
            if (timeRange.Begin <= Last && Last <= timeRange.Last) return true;

            return false;
        }

        /// <summary>
        /// このインスタンスの時間を分換算で表します。
        /// </summary>
        /// <returns>インスタンスの時間</returns>
        public int GetRangeMinutes()
        {
            return (int)(Last - Begin).TotalMinutes;
        }

        /// <summary>
        /// このインスタンスと引数の<see cref="TimeRange"/>が重複している時間を分換算で取得します。重複していない場合は戻り値は0になります。
        /// </summary>
        /// <param name="timeRange">確認したい時間帯</param>
        /// <returns>重複している時間</returns>
        public int GetOverlapMinutes(TimeRange timeRange)
        {
            if (!IsOverlap(timeRange)) return 0;

            // 開始時刻範囲内？
            DateTime _begin = timeRange.Begin < Begin ? Begin : timeRange.Begin;
            // 終了時刻範囲内？
            DateTime _last = Last < timeRange.Last ? Last : timeRange.Last;
            int _m = (int)(_last - _begin).TotalMinutes;

            return 0 < _m ? _m : 0;
        }

        /// <summary>
        /// このインスタンスと引数の<see cref="KNS_D01"/>が重複している実労働時間を分換算で取得します。重複していない場合は戻り値は0になります。
        /// </summary>
        /// <param name="KinmuJisseki">確認したい勤務実績</param>
        /// <returns>重複している時間</returns>
        public int GetOverlapMinutes(KNS_D01 KinmuJisseki)
        {
            int time =0 ;
            time += GetOverlapMinutes(KinmuJisseki.GetWorkTimeRange());
            time -= GetOverlapMinutes(KinmuJisseki.GetRest1TimeRange());
            time -= GetOverlapMinutes(KinmuJisseki.GetRest2TimeRange());
            time -= GetOverlapMinutes(KinmuJisseki.GetRest3TimeRange());

            return time;
        }

        /// <summary>
        /// このインスタンスに設定された開始時刻終了時刻を文字列で返します。
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            return string.Format(format, Begin, Last);
        }

        /// <summary>
        /// このインスタンスに設定された開始時刻終了時刻を文字列で返します。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString("{0:HH：mm}　～　{1:HH：mm}");
        }
    }
}
