using System;
using System.Collections.Generic;

namespace CommonLibrary.Models
{
    /// <summary>
    /// 1日単位の勤務実績、勤務予定、カレンダーマスターを含んだクラスです。
    /// </summary>
    public class KinmuRecordRow
    {
        /// <summary>
        /// この勤務情報の所有者の社員コードです。
        /// 勤務実績や予定がない場合、社員コードを参照できないため別で所持するようにしています。
        /// </summary>
        public string EmployeeCD { get; set; }

        /// <summary>
        /// 勤務実績です。Entity Flameworkで自動生成されたEntityです。
        /// </summary>
        public KNS_D01 KinmuJisseki { get; set; }

        /// <summary>
        /// 勤務予定です。Entity Flameworkで自動生成されたEntityです。
        /// </summary>
        public KNS_D13 KinmuYotei { get; set; }

        /// <summary>
        /// 作業日誌です。Entity Flameworkで自動生成されたEntityです。
        /// </summary>
        public List<KNS_D02> SagyoNisshi { get; set; }

        /// <summary>
        /// カレンダーマスタです。Entity Flameworkで自動生成されたEntityです。
        /// マスタなので、読み取り専用としています。
        /// </summary>
        public KNS_M05 CalendarMaster { get; }

        /// <summary>
        /// 1日単位の勤務実績を作成します。
        /// </summary>
        /// <param name="_EmployeeCD">勤務情報の所有者社員コード</param>
        /// <param name="_KinmuJisseki">勤務実績</param>
        /// <param name="_SagyoNisshi">作業日誌</param>
        /// <param name="_KinmuYotei">勤務予定</param>
        /// <param name="_CalendarMaster">カレンダーマスタ</param>
        public KinmuRecordRow(string _EmployeeCD, KNS_D01 _KinmuJisseki, KNS_D13 _KinmuYotei, List<KNS_D02> _SagyoNisshi, KNS_M05 _CalendarMaster)
        {
            EmployeeCD = _EmployeeCD ?? throw new ArgumentNullException("_EmployeeCD", "社員コードは必須のため、Nullでオブジェクトを作成することはできません。");
            KinmuJisseki = _KinmuJisseki ?? new KNS_D01();
            KinmuYotei = _KinmuYotei ?? new KNS_D13();
            SagyoNisshi = _SagyoNisshi ?? new List<KNS_D02>();
            CalendarMaster = _CalendarMaster ?? throw new ArgumentNullException("_CalendarMaster", "カレンダーマスタをNullでオブジェクトを作成することはできません。KNS_M05テーブルを参照し、対象日付のカレンダーマスタが作成されているか確認してください。");
        }

        /// <summary>
        /// 引数なしのコンストラクタは使用しません。
        /// </summary>
        private KinmuRecordRow() { }
    }
}
