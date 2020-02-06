using CommonLibrary;
using CommonLibrary.Models;
using KinmuLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using static CommonLibrary.CommonDefine;
using static CommonLibrary.Util;

namespace KinmuSystem.View
{
    public partial class WorkResults : System.Web.UI.Page
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private KNS_M01 viewShainInfo;
        private KNS_M01 loginShainInfo;
        private DateTime viewDateTime;
        string companyName = "(株) エスケイケイ";
        string dataErrorMessage = "データ読み込み時にエラーが発生しました。管理者に連絡してください。";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);
                KinmuSystem.SetSessionData(ref loginShainInfo, ref viewShainInfo, ref viewDateTime, Response, Request, Session);

                int workTimeTotal = 0;
                List<KinmuGridViewWorkResultsRow> data = new List<KinmuGridViewWorkResultsRow>();
                KinmuManager kinmuManager = new KinmuManager(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month);

                DateTimeStringLabel.Text = viewDateTime.ToString("yyyy年MM月");
                CompanyNameLabel.Text = companyName;
                EmployeeCodeLabel.Text = viewShainInfo.SHAIN_CD;
                NameLabel.Text = viewShainInfo.SHAIN_NM;
                Title = "勤務実績整理簿 " + viewShainInfo.SHAIN_NM + " " + viewDateTime.ToString("yyyy年MM月");

                //メインの表をバインド
                for (int i = 1; i <= DateTime.DaysInMonth(kinmuManager.Year, kinmuManager.Month); i++)
                {
                    // 本当はKinmuRecordRowを取得しておきたかったが、処理の関係上こうなってる
                    // なんか良い方法あったら教えてください
                    KNS_D01 KinmuJisseki = kinmuManager.KinmuJisseki.SingleOrDefault(_ => _.DATA_D == i.ToString(PADDING_ZERO_2)) ?? new KNS_D01();
                    KNS_D13 KinmuYotei = kinmuManager.KinmuYotei.SingleOrDefault(_ => _.DATA_D == i.ToString(PADDING_ZERO_2)) ?? new KNS_D13();
                    KNS_M05 CalendarMaster = kinmuManager.CalendarMaster.SingleOrDefault(_ => _.DATA_D == i.ToString(PADDING_ZERO_2)) ?? new KNS_M05();
                    workTimeTotal += KinmuJisseki.DKINM ?? 0;
                    data.Add(
                        new KinmuGridViewWorkResultsRow(
                            new KinmuRecordRow(kinmuManager.EmployeeCD, KinmuJisseki, KinmuYotei, new List<KNS_D02>(), CalendarMaster),
                            workTimeTotal
                        )
                    );
                }
                WorkResultsGridView.DataSource = data;
                WorkResultsGridView.DataBind();

                // 所定日数
                A1Label.Text = kinmuManager.CalcGekkanSyoteiNissu().ToString();
                // 出勤日数
                A2Label.Text = kinmuManager.CalcGekkanSyukkinNissu().ToString();
                // 休労日数
                A3Label.Text = kinmuManager.CalcGekkanKyujitsuRoudouNissu().ToString();
                // -日数
                A4Label.Text = kinmuManager.CalcBarNissu().ToString();
                // 代休日数
                A5Label.Text = kinmuManager.CalcGekkanDaikyuNissu().ToString();
                // 非番日数
                A6Label.Text = kinmuManager.CalcGekkanHibanNissu().ToString();
                // 有給日数
                A7Label.Text = kinmuManager.CalcGekkanYukyuNissu().ToString();
                // 無休日数
                A8Label.Text = kinmuManager.CalcGekkanMukyuNissu().ToString();

                // 組休日数
                B1Label.Text = kinmuManager.CalcGekkanKumikyuNissu().ToString();
                // 公休日労働回数
                B2Label.Text = kinmuManager.CalcGekkanKokyuRoudouNissu().ToString();
                // 所定労働日数
                B3Label.Text = kinmuManager.CalcGekkanSyoteiRoudoNissu().ToString();
                // 労働日数
                B4Label.Text = kinmuManager.CalcGekkanRoudouNissu().ToString();
                // 特休日数 予定
                B5Label.Text = kinmuManager.CalcGekkanTokkyuYoteiNissu().ToString();
                // 特休日数 確定
                B6Label.Text = kinmuManager.CalcGekkanTokkyuKakuteiNissu().ToString();
                // 公休日数 予定
                B7Label.Text = kinmuManager.CalcGekkanKoukyuYoteiNissu().ToString();
                // 公休日数 確定
                B8Label.Text = kinmuManager.CalcGekkanKoukyuKakuteiNissu().ToString();

                // 実総労働時間
                int tmp = kinmuManager.CalcGekkanTotalJitsuRoudoJikan();
                C1Label1.Text = MinutesToStringFormat(tmp);
                C1Label2.Text = tmp.ToString();
                // みなし２
                tmp = kinmuManager.CalcGekkanYukyuJikan();
                C2Label1.Text = MinutesToStringFormat(tmp);
                C2Label2.Text = tmp.ToString();
                // 超勤A
                tmp = kinmuManager.CalcGekkanTankaAJikan();
                C3Label1.Text = MinutesToStringFormat(tmp);
                C3Label2.Text = tmp.ToString();
                // 超勤B
                tmp = kinmuManager.CalcGekkanTankaBJikan();
                C4Label1.Text = MinutesToStringFormat(tmp);
                C4Label2.Text = tmp.ToString();
                // 超勤D（特休）
                tmp = kinmuManager.CalcGekkanTankaTokkyuDJikan();
                C5Label1.Text = MinutesToStringFormat(tmp);
                C5Label2.Text = tmp.ToString();
                // 超勤D（公休）
                tmp = kinmuManager.CalcGekkanTankaKoukyuDJikan();
                C6Label1.Text = MinutesToStringFormat(tmp);
                C6Label2.Text = tmp.ToString();
                // 夜勤C
                tmp = kinmuManager.CalcGekkanTankaCJikan();
                C7Label1.Text = MinutesToStringFormat(tmp);
                C7Label2.Text = tmp.ToString();
                // 控除A
                tmp = kinmuManager.CalcGekkanKoujyoAJikan();
                C8Label1.Text = MinutesToStringFormat(tmp);
                C8Label2.Text = tmp.ToString();

                // 減額A
                tmp = kinmuManager.CalcGekkanGengakuAjikan();
                D1Label1.Text = MinutesToStringFormat(tmp);
                D1Label2.Text = tmp.ToString();
                // 時間外労働時間
                tmp = kinmuManager.CalcGekkanJikangaiRoudoJikan();
                D2Label1.Text = MinutesToStringFormat(tmp);
                D2Label2.Text = tmp.ToString();
                // 経営公休日労働時間
                tmp = kinmuManager.CalcGekkanKeieiKokyuRoudoJikan();
                D3Label1.Text = MinutesToStringFormat(tmp);
                D3Label2.Text = tmp.ToString();
                // 法定労働時間
                tmp = kinmuManager.CalcGekkanHouTeiRoudoJikan();
                D4Label1.Text = MinutesToStringFormat(tmp);
                D4Label2.Text = tmp.ToString();
                // 所定労働時間
                tmp = kinmuManager.CalcGekkanSyoteiRoudoJikan();
                D5Label1.Text = MinutesToStringFormat(tmp);
                D5Label2.Text = tmp.ToString();
                // みなし１
                tmp = kinmuManager.CalcGekkanMinashi1Jikan();
                D6Label1.Text = MinutesToStringFormat(tmp);
                D6Label2.Text = tmp.ToString();
                // 超勤E
                tmp = kinmuManager.CalcGekkanTankaEJikan();
                D7Label1.Text = MinutesToStringFormat(tmp);
                D7Label2.Text = tmp.ToString();
                // 祝日C
                tmp = kinmuManager.CalcGekkanSyukujituRoudoJikan();
                D8Label1.Text = MinutesToStringFormat(tmp);
                D8Label2.Text = tmp.ToString();
            }
            catch (KinmuException ex)
            {
                WorkResultsPanel.Visible = false;
                ErrorPanel.Visible = true;
                ErrorMessageLabel.Text = dataErrorMessage + "ErrorMessage：" + ex.Message + "StackTrace：" + ex.StackTrace;
            }
        }
    }
}