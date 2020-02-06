using CommonLibrary;
using CommonLibrary.Models;
using KinmuLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CommonLibrary.CommonDefine;
using static CommonLibrary.Util;


namespace KinmuSystem.View
{
    public partial class Flex : Page
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private DBManager dbManager = new DBManager();
        private List<KNS_M05> calender;
        private Dictionary<string, string> ninsyoCD;

        private KNS_M01 viewShainInfo;
        private KNS_M01 loginShainInfo;
        private DateTime viewDateTime;
        private string timeFormat = "{0:HH:mm}～{1:HH:mm}";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);
                ((KinmuSystem)Master).SetSessionData(ref loginShainInfo, ref viewShainInfo, ref viewDateTime);
                ninsyoCD = dbManager.GetKinmuNinsyoCDMasterALL().ToDictionary(_ => _.NINSHO_CD, _ => _.NINSHO_NM);
                if (!IsPostBack)
                {
                    InitPage();
                }
                InitInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText);
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
        }

        #region 初期化処理
        /// <summary>
        /// ページの初期化を行います
        /// </summary>
        private void InitPage()
        {
            try
            {
                logger.Debug(LOG_START);
                DateLabel.Text = viewDateTime.ToString("yyyy年MM月");
                calender = dbManager.GetCalender(viewDateTime.Year, viewDateTime.Month);
                CreateCalenderTable();
                CreateFlexTable();
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KinmuException(ex.Message, ex);
            }
        }
        #endregion

        #region 月遷移処理
        /// <summary>
        /// 表示年月を変更し、画面を更新します。
        /// </summary>
        /// <param name="months">遷移する月数（相対値）</param>
        private void ChangeViewDateTimeAndUpdateView(int months)
        {
            try
            {
                logger.Debug(LOG_START);
                viewDateTime = ((DateTime)Session[SESSION_STRING_VIEW_DATETIME]).AddMonths(months);
                Session[SESSION_STRING_VIEW_DATETIME] = viewDateTime;

                try
                {
                    InitPage();
                }
                catch (KinmuException)
                {
                    Session[SESSION_STRING_VIEW_DATETIME] = viewDateTime.AddMonths(-months);
                    viewDateTime = viewDateTime.AddMonths(-months);
                    DateLabel.Text = viewDateTime.ToString("yyyy年MM月");
                    calender = dbManager.GetCalender(viewDateTime.Year, viewDateTime.Month);
                    CreateCalenderTable();
                    CreateFlexTable();
                    throw;
                }
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KinmuException(ex.Message, ex);
            }
        }
        #endregion

        #region フレックステーブル作成処理
        /// <summary>
        /// 日付部分のテーブルを作成
        /// </summary>
        private void CreateCalenderTable()
        {
            try
            {
                logger.Debug(LOG_START);
                // 社員名項目
                TableRow tableRow = new TableRow();
                TableCell tableCell = new TableCell
                {
                    Text = "社員名",
                    CssClass = "shainNameTitle"
                };
                tableRow.Cells.Add(tableCell);
                kinmuConfirmTableCalender.Rows.Add(tableRow);

                // 超勤予定時間項目
                tableRow = new TableRow();
                tableCell = new TableCell
                {
                    Text = "月間超勤予定時間",
                    CssClass = "overKinmuTimeHeaderTitle"
                };
                tableRow.Cells.Add(tableCell);
                kinmuConfirmTableCalender.Rows.Add(tableRow);

                // 日付分ループする
                foreach (var item in calender)
                {
                    tableRow = new TableRow();
                    tableCell = new TableCell();

                    // 曜日によってCSSを変更する
                    DayOfWeek week = DateTime.Parse($"{item.DATA_Y}/{item.DATA_M}/{item.DATA_D}").DayOfWeek;
                    switch (week)
                    {
                        case DayOfWeek.Sunday:
                            tableCell.CssClass = "sunday";
                            break;
                        case DayOfWeek.Saturday:
                            tableCell.CssClass = "saturday";
                            break;
                        default:
                            tableCell.CssClass = "usualday";
                            break;
                    }

                    // 祝日フラグがある場合はCSSを祝日に上書き
                    if (item.SHUKU_FLG == "1")
                    {
                        tableCell.CssClass = "holiday";
                    }

                    // 日本語曜日を作成
                    string[] jpnweek = { "日", "月", "火", "水", "木", "金", "土" };
                    tableCell.Text = item.DATA_D + "日(" + jpnweek[(int)week] + ")";

                    // テーブルに追加
                    tableRow.Cells.Add(tableCell);
                    kinmuConfirmTableCalender.Rows.Add(tableRow);
                }
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KinmuException(ex.Message, ex);
            }
        }

        /// <summary>
        /// フレックス確認テーブルの作成
        /// </summary>
        private void CreateFlexTable()
        {
            try
            {
                logger.Debug(LOG_START);
                List<KNS_M01> shainMaster = dbManager.GetSyainMasterAll();
                List<KinmuRecordRow> kinmuAll = dbManager.GetKinmuRecordRow(null, viewDateTime.Year, viewDateTime.Month);
                List<TableRow> tr = new List<TableRow>();
                BtnCodeUpdate.Enabled = false;

                // 行を日付＋ヘッダ2行分作成
                for (int i = 0; i < calender.Count + 2; i++)
                {
                    tr.Add(new TableRow());
                }

                // 全社員分のデータを日付行に追加
                foreach (var item in shainMaster)
                {
                    // 社員名セル
                    TableCell tableCell = new TableCell
                    {
                        ID = "tc" + item.SHAIN_CD,
                        CssClass = "shainName"
                    };

                    if (item.SHONIN_SHAIN_CD == loginShainInfo.SHAIN_CD)
                    {
                        // 承認社員＝自分の場合、予定確認チェックボックスを追加
                        CheckBox cb = new CheckBox
                        {
                            ID = "cd" + item.SHAIN_CD,
                            Text = item.SHAIN_NM,
                        };
                        tableCell.Controls.Add(cb);
                        BtnCodeUpdate.Enabled = true;
                    }
                    else
                    {
                        // それ以外は社員名を表示
                        tableCell.Text = item.SHAIN_NM;
                    }
                    tr[0].Cells.Add(tableCell);

                    // 超勤時間セル
                    List<KNS_D01> tmp_d01s = KinmuManager.CallB(kinmuAll.Where(_ => _.EmployeeCD == item.SHAIN_CD).ToList());
                    tr[1].Cells.Add(new TableCell() { Text = MinutesToStringFormat(KinmuManager.CalcFastGekkanTankaBJikan(tmp_d01s)), CssClass = "overKinmuTimeHeaderRow" });

                    // 勤務情報セル（日付分）
                    for (int day = 1; day <= calender.Count; day++)
                    {
                        KinmuRecordRow kinmu = kinmuAll.Single(_ => _.EmployeeCD == item.SHAIN_CD && _.CalendarMaster.DATA_Y == viewDateTime.ToString("yyyy") && _.CalendarMaster.DATA_M == viewDateTime.ToString("MM") && _.CalendarMaster.DATA_D == day.ToString("00"));
                        tr[day + 1].Cells.Add(CreateFlexDataTableCellFromKinmuRecordRow(kinmu));
                    }
                }

                // テーブルにバインド
                kinmuConfirmTableData.Rows.AddRange(tr.ToArray());
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KinmuException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 勤務情報からフレックステーブルに表示するセルを作成します。
        /// </summary>
        /// <param name="kinmu"></param>
        /// <returns></returns>
        private TableCell CreateFlexDataTableCellFromKinmuRecordRow(KinmuRecordRow kinmu)
        {
            try
            {
                logger.Debug(LOG_START);
                TableCell cell = new TableCell
                {
                    Text = "　",
                    CssClass = ""
                };

                if (!string.IsNullOrWhiteSpace(kinmu.KinmuJisseki.NINKA_CD))
                {
                    // 実績データを使う場合
                    SetTableCellData(kinmu.KinmuJisseki, out cell);
                }
                else if (!string.IsNullOrWhiteSpace(kinmu.KinmuYotei.YOTEI_CD))
                {
                    // 予定データを使う場合
                    SetTableCellData(kinmu.KinmuYotei, out cell);
                }
                else if (!string.IsNullOrWhiteSpace(kinmu.CalendarMaster.NINYO_CD))
                {
                    // カレンダーマスターを使う場合
                    SetTableCellData(kinmu.CalendarMaster, out cell);
                }

                return cell;
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KinmuException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 実績情報に応じた<see cref="TableCell"/>を作成します
        /// </summary>
        /// <param name="jisseki">勤務実績データ</param>
        /// <param name="cell"><see cref="KNS_D01"/> <paramref name="jisseki"/>に応じ作成された<see cref="TableCell"/>が格納されます。</param>
        private void SetTableCellData(KNS_D01 jisseki, out TableCell cell)
        {
            try
            {
                logger.Debug(LOG_START);
                cell = new TableCell()
                {
                    CssClass = "jisseki",
                    Text = ninsyoCD[jisseki.NINKA_CD]
                };

                // 勤務時間があればテキストを書き換える
                TimeRange tr = jisseki.GetWorkTimeRange();
                if (tr != null)
                {
                    cell.Text = tr.ToString(timeFormat);
                }
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KinmuException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 予定情報に応じた<see cref="TableCell"/>を作成します
        /// </summary>
        /// <param name="yotei">勤務予定データ</param>
        /// <param name="cell"><see cref="KNS_D13"/> <paramref name="yotei"/>に応じ作成された<see cref="TableCell"/>が格納されます。</param>
        private void SetTableCellData(KNS_D13 yotei, out TableCell cell)
        {
            try
            {
                logger.Debug(LOG_START);
                cell = new TableCell()
                {
                    CssClass = "notConfirmYotei",
                    Text = ninsyoCD[yotei.YOTEI_CD]
                };

                // 勤務時間があればテキストを書き換える
                TimeRange tr = yotei.GetWorkTimeRange();
                if (tr != null)
                {
                    cell.Text = tr.ToString(timeFormat);
                }

                // 承認フラグがあればCSSを書き換える
                if (yotei.SHONIN_FLG == "1")
                {
                    cell.CssClass = "confirmYotei";
                }
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KinmuException(ex.Message, ex);
            }
        }

        /// <summary>
        /// カレンダーマスターに応じた<see cref="TableCell"/>を作成します
        /// </summary>
        /// <param name="calendar">カレンダーマスター</param>
        /// <param name="cell"><see cref="KNS_M05"/> <paramref name="calendar"/>に応じ作成された<see cref="TableCell"/>が格納されます。</param>
        private void SetTableCellData(KNS_M05 calendar, out TableCell cell)
        {
            try
            {
                logger.Debug(LOG_START);
                cell = new TableCell()
                {
                    CssClass = "",
                    Text = ninsyoCD[calendar.NINYO_CD]
                };
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new KinmuException(ex.Message, ex);
            }
        }
        #endregion

        #region イベント処理
        /// <summary>
        /// 予定確認ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnCodeUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);
                Regex rg = new Regex("[0-9]{7}$");
                foreach (var key in Request.Form.AllKeys)
                {
                    if (rg.IsMatch(key))
                    {
                        string tmpcd = rg.Match(key).Value;
                        dbManager.UpdateShoninFlag(loginShainInfo.SHAIN_CD, tmpcd, viewDateTime.Year, viewDateTime.Month);
                    }
                }

                InitPage();
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
        }

        /// <summary>
        /// 先月ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Goto1MonthBefore_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);
                ChangeViewDateTimeAndUpdateView(-1);
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
        }

        /// <summary>
        /// 翌月ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Goto1MonthAfter_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                ChangeViewDateTimeAndUpdateView(1);
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
        }
        #endregion
    }
}