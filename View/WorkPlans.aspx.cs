using CommonLibrary;
using CommonLibrary.Models;
using KinmuLibrary;
using PDFLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CommonLibrary.CommonDefine;
using static CommonLibrary.Util;

namespace KinmuSystem.View
{
    public partial class WorkPlans : System.Web.UI.Page
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private KNS_M01 viewShainInfo;
        private DateTime viewDateTime;
        private KNS_M01 loginShainInfo;
        private KinmuManager kinmuManager;

        private int workTimeTotal = 0;
        private int minashiTimeTotal = 0;
        private int restTimeTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            // セッションデータの取得
            KinmuSystem.SetSessionData(ref loginShainInfo, ref viewShainInfo, ref viewDateTime, Response, Request, Session);           

            try
            {
                kinmuManager = new KinmuManager(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month);
                var pdfManager = new PDFManager(kinmuManager);

                YearLabel.Text = viewDateTime.ToString("yyyy");
                MonthLabel.Text = viewDateTime.ToString("MM");
                CompanyNameLabel.Text = "（株）エスケイケイ";
                EmployeeCodeLabel.Text = viewShainInfo.SHAIN_CD;
                NameLabel.Text = viewShainInfo.SHAIN_NM;
                Title = "勤務予定表 " + viewShainInfo.SHAIN_NM + " " + viewDateTime.ToString("yyyy年MM月");
                RenderWorkPlansTableBody();

                OverTimeGridView.DataSource = pdfManager.GetOverTimeList();
                OverTimeGridView.DataBind();

                Check36GridView.DataSource = pdfManager.GetCheck36List();
                Check36GridView.DataBind();

                //Tableのデータは直接入れる
                TotalTable.Rows[0].Cells[1].Text = MinutesToStringFormat(restTimeTotal);
                //みなし1が含まれている？
                TotalTable.Rows[0].Cells[2].Text = MinutesToStringFormat(workTimeTotal);
                //みなし2のみ？
                TotalTable.Rows[0].Cells[3].Text = MinutesToStringFormat(minashiTimeTotal);

                KoKyuWorkTable.Rows[0].Cells[1].Text = kinmuManager.CalcGekkanKokyuRoudouNissu() + "日";

                NenkyuTable.Rows[1].Cells[1].Text = kinmuManager.CalcGekkanNenkyu() + "日";
                NenkyuTable.Rows[2].Cells[1].Text = kinmuManager.CaclGekkanAMHankyu() + "日";
                NenkyuTable.Rows[3].Cells[1].Text = kinmuManager.CaclGekkanPMHankyu() + "日";

                TokukyuAndKokyuTable.Rows[1].Cells[1].Text = kinmuManager.CalcGekkanTokkyuYoteiNissu() + "日";
                TokukyuAndKokyuTable.Rows[1].Cells[2].Text = kinmuManager.CalcGekkanTokkyuKakuteiNissu() + "日";
                TokukyuAndKokyuTable.Rows[2].Cells[1].Text = kinmuManager.CalcGekkanKoukyuYoteiNissu() + "日";
                TokukyuAndKokyuTable.Rows[2].Cells[2].Text = kinmuManager.CalcGekkanKoukyuKakuteiNissu() + "日";

                WorkDaysAndTimeTable.Rows[0].Cells[1].Text = kinmuManager.CalcGekkanSyoteiRoudoNissu() + "日";
                WorkDaysAndTimeTable.Rows[0].Cells[2].Text = MinutesToStringFormat(kinmuManager.CalcGekkanSyoteiRoudoJikan());
                WorkDaysAndTimeTable.Rows[1].Cells[1].Text = kinmuManager.CalcGekkanHouteiNissu() + "日";
                WorkDaysAndTimeTable.Rows[1].Cells[2].Text = MinutesToStringFormat(kinmuManager.CalcGekkanHouTeiRoudoJikan());

                //作業時間が法定労働時間を超過している場合注意喚起を表示する
                HoteiRoudoCheckMessageLabel.Text = pdfManager.GetHoteiRoudoCheckMessage();

            }
            catch (KinmuException ex)
            {
                WorkPlansPanel.Visible = false;
                ErrorPanel.Visible = true;
                ErrorMessageLabel.Text = "データ読み込み時にエラーが発生しました。管理者に連絡してください。" + "\nErrorMessage：" + ex.Message + "\nStackTrace：" + ex.StackTrace;

            }

            
        }

        /// <summary>
        /// 36協定チェック欄のヘッダーを調整します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Check36GridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            logger.Debug(LOG_START);
            if (e.Row.RowType == DataControlRowType.Header)
            {
                List<TableCell> cells = new List<TableCell>();

                foreach (TableCell cell in e.Row.Cells)
                {
                    cells.Add(cell);
                }

                GridViewRow row1 = new GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);


                TableHeaderCell HeaderCell = new TableHeaderCell
                {
                    ColumnSpan = 2,
                    CssClass = "Check36Header2"
                };
                HeaderCell.Controls.Add(new LiteralControl("36協定チェック欄(③補正)"));

                row1.Cells.Add(HeaderCell);

                Check36GridView.Controls[0].Controls.Clear();
                Check36GridView.Controls[0].Controls.Add(row1);

            }
        }


        //下のOverTimeGridView_RowDataBoundで使用
        private string PreCellText = "";
        private TableCell KeyCell = null;

        /// <summary>
        /// 列にデータがバインドされたときに呼ばれる。一番左のセルに対して、項目名が上と同じ場合セルを統合する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OverTimeGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            logger.Debug(LOG_START);

            //データが入っているセルでなければリターン
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }


            string CellText = e.Row.Cells[0].Text;

            //ひとつ前のセルと内容が違ったらそのセルを基準として登録する
            if (CellText != PreCellText)
            {
                KeyCell = e.Row.Cells[0];
                PreCellText = CellText;
                return;
            }

            //ひとつ前のセルと内容が同じなら、そのセルの列数を増やす
            e.Row.Cells[0].Visible = false;
            if (KeyCell.RowSpan == 0)
            {
                KeyCell.RowSpan = 2;
            }
            else
            {
                KeyCell.RowSpan++;
            }


        }

        /// <summary>
        /// 社員コード、年、月をもとにDBから勤務予定を取得し、帳票にバインドできる形に変換して返します。
        /// </summary>
        /// <returns>バインドできる形に成形した勤務予定に記入するデータ</returns>
        private void RenderWorkPlansTableBody()
        {
            logger.Debug(LOG_START);
            for (int i = 1; i <= DateTime.DaysInMonth(kinmuManager.Year, kinmuManager.Month); i++)
            {
                // 本当はKinmuRecordRowを取得しておきたかったが、処理の関係上こうなってる
                // なんか良い方法あったら教えてください
                KNS_D01 KinmuJisseki = kinmuManager.KinmuJisseki.SingleOrDefault(_ => _.DATA_D == i.ToString(PADDING_ZERO_2)) ?? new KNS_D01();
                KNS_D13 KinmuYotei = kinmuManager.KinmuYotei.SingleOrDefault(_ => _.DATA_D == i.ToString(PADDING_ZERO_2)) ?? new KNS_D13();
                KNS_M05 CalendarMaster = kinmuManager.CalendarMaster.SingleOrDefault(_ => _.DATA_D == i.ToString(PADDING_ZERO_2)) ?? new KNS_M05();

                workTimeTotal += KinmuJisseki.DKINM ?? 0;
                minashiTimeTotal += KinmuJisseki.DMINA1 ?? 0;
                minashiTimeTotal += KinmuJisseki.DMINA2 ?? 0;
                restTimeTotal += KinmuJisseki.GetTotalRestTime();

                KinmuGridViewWorkPlansRow a = new KinmuGridViewWorkPlansRow(new KinmuRecordRow(kinmuManager.EmployeeCD, KinmuJisseki, KinmuYotei, new List<KNS_D02>(), CalendarMaster), workTimeTotal);

                TableRow row = new TableRow();
                row.Cells.AddRange(new TableCell[] {
                    new TableCell() { Text = a.Date, CssClass = "Date" },
                    new TableCell() { Text = a.Day, CssClass = "Day" },
                    new TableCell() { Text = a.NinsyoSchedule, CssClass = "Plan" },
                    new TableCell() { Text = a.NinsyoResult, CssClass = "Result" },
                    new TableCell() { Text = a.OpeningTimeToClosingTimeSchedule, CssClass = "OpeningTimeToClosingTimeSchedule" },
                    new TableCell() { Text = a.OpeningTimeToClosingTimeResult, CssClass = "OpeningTimeToClosingTimeResult" },
                    new TableCell() { Text = a.NextDayFlag, CssClass = "NextDay" },
                    new TableCell() { Text = a.RestTime, CssClass = "BreakTime" },
                    new TableCell() { Text = a.WorkTime, CssClass = "WorkTime" },
                    new TableCell() { Text = a.Minashi, CssClass = "Minashi" },
                    new TableCell() { Text = a.WorkTimeTotal, CssClass = "WorkTimeTotal" },
                    new TableCell() { Text = a.Article, CssClass = "Article" }
                });

                WorkPlansTable.Rows.Add(row);
            }
        }
    }
}
