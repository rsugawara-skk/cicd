using CommonLibrary;
using CommonLibrary.Models;
using KinmuLibrary;
using PDFLibrary;
using System;
using static CommonLibrary.CommonDefine;
using static CommonLibrary.Util;

namespace KinmuSystem.View
{
    public partial class WorkDiary : System.Web.UI.Page
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private KNS_M01 viewShainInfo;
        private KNS_M01 loginShainInfo;
        private DateTime viewDateTime;

        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            // セッションデータの取得
            KinmuSystem.SetSessionData(ref loginShainInfo, ref viewShainInfo, ref viewDateTime, Response, Request, Session);

            string companyName = "（株）エスケイケイ";
            string dataErrorMessage = "データ読み込み時にエラーが発生しました。管理者に連絡してください。";

            

            try
            {

                var kinmuManager = new KinmuManager(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month);
                var pdfManager = new PDFManager(kinmuManager);

                //データをバインドする
                YearLabel.Text = viewDateTime.ToString("yyyy");
                MonthLabel.Text = viewDateTime.ToString("MM");
                CompanyNameLabel.Text = companyName;
                NameLabel.Text = viewShainInfo.SHAIN_NM;
                Title = "作業日誌 " + viewShainInfo.SHAIN_NM + " " + viewDateTime.ToString("yyyy年MM月");

                WorkDiaryGridView.DataSource = pdfManager.GetWorkDiaryList();
                WorkDiaryGridView.DataBind();

                //minuteをhourにする
                int totalWorkTime = kinmuManager.CalcGekkanTotalJitsuRoudoJikan();
                TotalWorkTimeLabel.Text = MinutesToStringFormat(totalWorkTime,"{0}時間{1:00}分");
            }
            catch (KinmuException ex)
            {
                WorkDiaryPanel.Visible = false;
                ErrorPanel.Visible = true;
                ErrorMessageLabel.Text = dataErrorMessage + "\nErrorMessage：" + ex.Message + "\nStackTrace：" + ex.StackTrace;
            }
        }
    }
}