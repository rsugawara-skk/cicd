using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using CommonLibrary;
using System.IO;
using CommonLibrary.Models;
using KinmuLibrary;
using static PDFLibrary.Properties.Settings;
using static CommonLibrary.CommonDefine;
using static CommonLibrary.Util;

namespace PDFLibrary
{
#pragma warning disable S101 // Types should be named in PascalCase
    /// <summary>
    /// PDFの作成に関する処理を行うクラスです。
    /// </summary>
    public class PDFManager : IPDFManager
#pragma warning restore S101 // Types should be named in PascalCase
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        #region 変数
        private readonly KinmuManager kinmuManager;
        private readonly DBManager dbManager = new DBManager();

        //勤務認証コードと勤務認証名、を結びつけるテーブルを丸ごと持ってくる
        private readonly List<KNS_M04> kinmuNinsyoMaster;

        private string EmployeeCD { get; set; }
        private int Year { get; set; }
        private int Month { get; set; }

        #endregion

        #region　共通
        /// <summary>
        /// PDFの作成、ダウンロードに関して管理するマネージャを作成します。
        /// </summary>
        /// <param name="_employeeCD"></param>
        /// <param name="_year"></param>
        /// <param name="_month"></param>
        public PDFManager(string _employeeCD, int _year, int _month)
        {
            EmployeeCD = _employeeCD;
            Year = _year;
            Month = _month;

            kinmuManager = new KinmuManager(EmployeeCD, Year, Month);
            kinmuNinsyoMaster = kinmuManager.NinsyoCDMaster;
        }

        /// <summary>
        /// PDFの作成、ダウンロードに関して管理するマネージャを作成します。
        /// </summary>
        /// <param name="_km"></param>
        public PDFManager(KinmuManager _km)
        {
            EmployeeCD = _km.EmployeeCD;
            Year = _km.Year;
            Month = _km.Month;

            kinmuManager = _km;
            kinmuNinsyoMaster = kinmuManager.NinsyoCDMaster;
        }

        /// <summary>
        /// プロジェクトコードを元にプロジェクト名を取得します。
        /// </summary>
        /// <param name="projectCode">プロジェクトコード</param>
        /// <returns>プロジェクト名</returns>
        private string GetProjectName(string projectCode)
        {

            string pjName = "";
            try
            {
                pjName = dbManager.PJCDToString(projectCode);
            }
            catch (Exception)
            {
                pjName = "(PJマスタに存在しません)";
            }

            return pjName;
        }

        /// <summary>
        /// 作業コードを元に作業名を取得します。
        /// </summary>
        /// <param name="sagyoCode">作業コード</param>
        /// <returns>作業名</returns>
        private string GetSagyoName(string sagyoCode)
        {

            string sagyoName = "";
            try
            {
                sagyoName = dbManager.SagyoCDToString(sagyoCode);
            }
            catch (Exception)
            {
                sagyoName = "(作業コードマスタに存在しません)";
            }

            return sagyoName;
        }
        #endregion

        #region 作業日誌
        /// <summary>
        /// 社員コード、年、月をもとにDBから作業日誌を取得し、帳票にバインドできる形に変換して返します。
        /// </summary>
        /// <returns>バインドできる形に成形した作業日誌に記入するデータ</returns>
        public List<object> GetWorkDiaryList()
        {
            var RawWorkDiaryList = dbManager.GetSagyoNisshi(EmployeeCD, Year, Month);

            //バインドするためにオブジェクトを整形する
            var CanBindWorkDiaryList = new List<object>();

            for (int j = 0; j < RawWorkDiaryList.Count; j++)
            {
                string worktime = MinutesToStringFormat(RawWorkDiaryList[j].SAGYO_MIN, "{0}時間{1:00}分");

                CanBindWorkDiaryList.Add(
                    new
                    {
                        Day = RawWorkDiaryList[j].DATA_D + "日",
                        ProjectName = RawWorkDiaryList[j].PROJ_CD + " " + GetProjectName(RawWorkDiaryList[j].PROJ_CD),
                        WorkName = RawWorkDiaryList[j].SAGYO_CD + " " + GetSagyoName(RawWorkDiaryList[j].SAGYO_CD),
                        WorkTime = worktime
                    }
                );
            }
            return CanBindWorkDiaryList;
        }
        #endregion

        #region 勤務予定
        /// <summary>
        /// 超勤に関する表に記入するデータを取得して返します。
        /// </summary>
        /// <returns>バインドできる形に成形した超勤に関する表に記入するデータ</returns>
        public List<object> GetOverTimeList()
        {
            logger.Debug(LOG_START);
            var tmp = new List<object>();

            try
            {
                tmp.Add(new { RuleHeader = "36協定(7:40換算)", OvertimeHeader = "①B単価計", ResultTime = MinutesToStringFormat(kinmuManager.CalcGekkanTankaBJikan()), PredictionTime = MinutesToStringFormat(kinmuManager.CalcGekkanTankaBJikanYosoku()) });
                tmp.Add(new { RuleHeader = "36協定(7:40換算)", OvertimeHeader = "②D単価計（特休）", ResultTime = MinutesToStringFormat(kinmuManager.CalcGekkanTankaTokkyuDJikan()), PredictionTime = MinutesToStringFormat(kinmuManager.CalcGekkanTankaTokkyuDJikanYosoku()) });
                tmp.Add(new { RuleHeader = "36協定(7:40換算)", OvertimeHeader = "③D単価計（公休）", ResultTime = MinutesToStringFormat(kinmuManager.CalcGekkanTankaKoukyuDJikan()), PredictionTime = MinutesToStringFormat(kinmuManager.CalcGekkanTankaKoukyuDJikanYosoku()) });
                tmp.Add(new { RuleHeader = "36協定(7:40換算)", OvertimeHeader = "④E単価計", ResultTime = MinutesToStringFormat(kinmuManager.CalcGekkanTankaEJikan()), PredictionTime = MinutesToStringFormat(kinmuManager.CalcGekkanTankaEJikanYosoku()) });
                tmp.Add(new { RuleHeader = "36協定(7:40換算)", OvertimeHeader = "当月累計（①+②+③）", ResultTime = MinutesToStringFormat(kinmuManager.CalcGekkanTankaBJikan() + kinmuManager.CalcGekkanTankaTokkyuDJikan() + kinmuManager.CalcGekkanTankaKoukyuDJikan()), PredictionTime = MinutesToStringFormat(kinmuManager.CalcToutsukiRuikeiTyoukinJissekiJikanYosoku()) });
                tmp.Add(new { RuleHeader = "36協定(7:40換算)", OvertimeHeader = "当年累計", ResultTime = MinutesToStringFormat(kinmuManager.CalcTounenRuikeiTyoukinJissekiJikan()), PredictionTime = MinutesToStringFormat(kinmuManager.CalcTounenRuikeiTyoukinJissekiJikanYosoku()) });
                tmp.Add(new { RuleHeader = "法定外（8:00換算）", OvertimeHeader = "当月累計", ResultTime = MinutesToStringFormat(kinmuManager.CalcGekkanJikangaiRoudoJikan()), PredictionTime = MinutesToStringFormat(kinmuManager.CalcToutsukiRuikeiHouteigaiJissekiJikanYosoku()) });
                tmp.Add(new { RuleHeader = "法定外（8:00換算）", OvertimeHeader = "当年累計", ResultTime = MinutesToStringFormat(kinmuManager.CalcTounenRuikeiHouteigaiJissekiJikan()), PredictionTime = MinutesToStringFormat(kinmuManager.CalcTounenRuikeiHouteigaiJissekiJikanYosoku()) });
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new KinmuException(e.Message, e);
            }

            return tmp;
        }

        /// <summary>
        /// 36協定チェック欄に記入するデータを取得して返します。
        /// </summary>
        /// <returns>バインドできる形に成形した36協定チェック欄に記入するデータ</returns>
        public List<object> GetCheck36List()
        {
            logger.Debug(LOG_START);
            var tmp = new List<object>();

            // 正直36協定チェック欄は機能していないので不要だと思う。
            // 総務と相談して予定表自体を削除する方向にしたい ＠2019/03/27 越川
            try
            {
                tmp.Add(new { ResultTime = MinutesToStringFormat(kinmuManager.CalcToutsukiRuikeiTyoukinJissekiJikan36Jisseki()), PredictionTime = MinutesToStringFormat(kinmuManager.CalcToutsukiRuikeiTyoukinJissekiJikan36Yosoku()) });
                tmp.Add(new { ResultTime = MinutesToStringFormat(kinmuManager.CalcTounenRuikeiTyoukinJissekiJikan36Jisseki()), PredictionTime = MinutesToStringFormat(kinmuManager.CalcTounenRuikeiTyoukinJissekiJikan36Yosoku()) });
                tmp.Add(new { ResultTime = MinutesToStringFormat(kinmuManager.CalcToutsukiRuikeiHouteigaiJissekiJikan36Jisseki()), PredictionTime = MinutesToStringFormat(kinmuManager.CalcToutsukiRuikeiHouteigaiJissekiJikan36Yosoku()) });
                tmp.Add(new { ResultTime = MinutesToStringFormat(kinmuManager.CalcTounenRuikeiHouteigaiJissekiJikan36Jisseki()), PredictionTime = MinutesToStringFormat(kinmuManager.CalcTounenRuikeiHouteigaiJissekiJikan36Yosoku()) });
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new KinmuException(e.Message, e);
            }

            return tmp;
        }

        /// <summary>
        /// 作業時間が法定労働時間を超過している場合、注意喚起のメッセージを返します。
        /// </summary>
        /// <returns>注意喚起のメッセージ</returns>
        public string GetHoteiRoudoCheckMessage()
        {
            logger.Debug(LOG_START);
            if (kinmuManager.CalcGekkanTotalJitsuRoudoJikan() > kinmuManager.CalcGekkanHouTeiRoudoJikan())
            {
                return "※作業時間が法定労働時間を超過しました。";
            }
            return "";
        }
        #endregion
    }
}