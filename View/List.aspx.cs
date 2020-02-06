using CommonLibrary;
using CommonLibrary.Models;
using KinmuLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CommonLibrary.CommonDefine;
using static CommonLibrary.Util;

namespace KinmuSystem.View
{
    public partial class List : Page
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private DBManager dbManager = new DBManager();
        private KNS_M01 viewShainInfo;
        private DateTime viewDateTime;
        private KNS_M01 loginShainInfo;
        private List<KNS_M01> empDropDownListData = new List<KNS_M01>();
        private List<KinmuRecordRow> kinmuRecordRowList = new List<KinmuRecordRow>();
        private int[] syuyaku;
        private double[] tokkyuKokyu;
        private double nenkyu = -1;
        private DateTime syuyakubi = DateTime.Now.AddMonths(-1);
        private List<string> noticeLabelData = new List<string>();
        private String ableButtonCSS = "ui-button ui-widget ui-state-default  ui-button-text-only ui-corner-right ui-corner-left";
        private String disableButtonCSS = "ui-button ui-widget ui-state-default  ui-button-text-only ui-corner-right ui-corner-left ui-button-disabled ui-state-disabled";
        private String selectedButtonCSS = "ui-button ui-widget ui-state-default ui-button-text-only ui-corner-right ui-corner-left ui-state-active";
        private String highlightButtonCSS = "ui-button ui-widget ui-state-default ui-button-text-only ui-corner-right ui-corner-left ui-state-highlight";

        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                // セッションデータの取得
                ((KinmuSystem)Master).SetSessionData(ref loginShainInfo, ref viewShainInfo, ref viewDateTime);

                // ページの初期化
                InitPage();

                // ポストバックならここで終了
                if (IsPostBack) return;

                // 初期アクセス時
                IsNotPostBack();
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
            catch (Exception ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, new KinmuException(ex.Message, ex));
            }
        }

        #region ボタンイベント
        /// <summary>
        /// 部下一覧ドロップダウンリストが変更された際のイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EmployeeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                viewShainInfo = dbManager.GetSyainMasterByShainCD(employeeDropDownList.SelectedValue);
                Session[SESSION_STRING_VIEW_SHAIN_INFO] = viewShainInfo;
                InitPage();
                IsNotPostBack();
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
        }

        /// <summary>
        /// 実行ボタン押下イベントの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RunButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                switch (Session["mode"])
                {
                    case "CopyPlans":
                        CopyPlans();
                        break;
                    case "CopyResults":
                        CopyResults();
                        break;
                    case "ClearResults":
                        ClearResults();
                        break;
                }
                // DBから更新後のデータを再取得
                kinmuRecordRowList = dbManager.GetKinmuRecordRow(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month);
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
            finally
            {
                RenderDataTableHeadder();
                UpdateGridViewBySessionModeValue();
            }
        }

        /// <summary>
        /// 前月ボタン押下イベントの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PrevMonthButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);

            try
            {
                Session["mode"] = "Edit";
                ChangeViewDateTimeAndUpdateView(-1);
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
        }

        protected void NextMonthButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);

            try
            {
                Session["mode"] = "Edit";
                ChangeViewDateTimeAndUpdateView(1);
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                Session["mode"] = "Edit";
                UpdateEditGridViewBindData();
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
        }

        protected void CopyPlansButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                Session["mode"] = "CopyPlans";
                UpdateCopyPlansGridViewBindData();
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
        }

        protected void CopyResultsButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                Session["mode"] = "CopyResults";
                UpdateCopyResultsGridViewBindData();
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
        }

        protected void ClearResultsButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                Session["mode"] = "ClearResults";
                UpdateClearResultsGridViewBindData();
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
        }

        protected void EditGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                Session[SESSION_STRING_VIEW_DATETIME] = new DateTime(viewDateTime.Year, viewDateTime.Month, ((GridView)sender).SelectedIndex + 1);
                Response.Redirect("Update.aspx");
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, ex);
            }
        }
        #endregion

        /// <summary>
        /// 表示年月を変更し、画面を更新します。
        /// </summary>
        /// <param name="months">遷移する月数（相対値）</param>
        private void ChangeViewDateTimeAndUpdateView(int months)
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
                throw;
            }
            finally
            {
                RenderDataTableHeadder();
                UpdateGridViewBySessionModeValue();
            }
        }

        /// <summary>
        /// モードに対応したGridViewを更新します
        /// </summary>
        private void UpdateGridViewBySessionModeValue()
        {
            logger.Debug(LOG_START);
            switch ((string)Session["mode"])
            {
                case "Edit":
                    UpdateEditGridViewBindData();
                    break;
                case "CopyPlans":
                    UpdateCopyPlansGridViewBindData();
                    break;
                case "CopyResults":
                    UpdateCopyResultsGridViewBindData();
                    break;
                case "ClearResults":
                    UpdateClearResultsGridViewBindData();
                    break;
            }
        }

        /// <summary>
        /// コピー元日付を取得します。
        /// </summary>
        /// <returns></returns>
        private int GetOriginDayFromRequest()
        {
            if (string.IsNullOrEmpty(Request.Form["Original"]))
            {
                throw new KinmuException("コピー元日付を選択してください。");
            }

            if (!int.TryParse(Request.Form["Original"], out int originDay))
            {
                throw new KinmuException("コピー元日付に不正な値が入力されました。値：" + Request.Form["Original"]);
            }

            return originDay;
        }

        /// <summary>
        /// コピー先日付を取得します。
        /// </summary>
        /// <returns></returns>
        private int[] GetCopytoDaysFromRequest()
        {
            if (string.IsNullOrEmpty(Request.Form["CopyTo"]))
            {
                throw new KinmuException("コピー先日付を選択してください。");
            }

            try
            {
                // ","でセパレート、int.Parseで変換、そして配列に変換
                return Request.Form["CopyTo"].Split(',').Select(int.Parse).ToArray();
            }
            catch (Exception ex)
            {
                throw new KinmuException("コピー先日付に不正な値が入力されました。", ex);
            }
        }

        /// <summary>
        /// クリアフラグを取得します。
        /// </summary>
        /// <returns></returns>
        private int[] GetClearFlagFromRequest()
        {
            if (string.IsNullOrEmpty(Request.Form["ClearFlag"]))
            {
                throw new KinmuException("削除する日付を選択してください。");
            }

            try
            {
                // ","でセパレート、int.Parseで変換、そして配列に変換
                return Request.Form["ClearFlag"].Split(',').Select(int.Parse).ToArray();
            }
            catch (Exception ex)
            {
                throw new KinmuException("削除する日付に不正な値が入力されました。", ex);
            }
        }

        private void CopyPlans()
        {
            logger.Debug(LOG_START);

            // コピー元の勤務データを取得
            KNS_D13 origin = GetLocalKinmuYotei(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month, GetOriginDayFromRequest());

            //コピー先の勤務データを取得
            int[] destDays = GetCopytoDaysFromRequest();
            List<KNS_D13> copytoList = new List<KNS_D13>();
            foreach (int dest in destDays)
            {
                if (origin == null)
                {
                    // コピー元にデータがない場合は削除用リストの作成を行う。
                    KNS_D13 copyto = GetLocalKinmuYotei(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month, dest);
                    if (copyto != null) copytoList.Add(copyto);
                }
                else
                {
                    KNS_D13 copytod13 = origin.Clone();
                    copytod13.DATA_D = dest.ToString();
                    copytod13.SHONIN_FLG = "0";

                    // 都度DBに更新処理をかけるが、業務ルールのcheckをかけたいのでExecuteUpdateを使っている。
                    KinmuManager.ExecuteUpdate(ref copytod13);
                }
            }

            // 予定の削除はここで一括してやる
            if (origin == null)
            {
                dbManager.DeleteKinmuYotei(copytoList);
            }

            SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, "勤務予定のコピーが完了しました。", InformationLevel.Highlight);
        }

        /// <summary>
        /// 実績コピー処理を行います。
        /// </summary>
        private void CopyResults()
        {
            KNS_D01 origin_01 = GetLocalKinmuJisseki(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month, GetOriginDayFromRequest());
            List<KNS_D02> origin_02 = GetLocalSagyoNisshi(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month, GetOriginDayFromRequest());
            int[] copyToValues = GetCopytoDaysFromRequest();
            List<KNS_D01> copytoList_01 = new List<KNS_D01>();
            List<KNS_D02> copytoList_02 = new List<KNS_D02>();

            foreach (int copyToValue in copyToValues)
            {
                if (origin_01 == null)
                {
                    KNS_D01 tmp = GetLocalKinmuJisseki(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month, copyToValue);
                    if (tmp != null) copytoList_01.Add(tmp);
                }
                else
                {
                    KNS_D01 copytod01 = origin_01.Clone();
                    copytod01.DATA_D = copyToValue.ToString();
                    KinmuManager.ExecuteUpdate(ref copytod01);
                }

                if (origin_01 == null || origin_02 == null)
                {
                    List<KNS_D02> tmp = GetLocalSagyoNisshi(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month, copyToValue);
                    if (tmp.Any()) copytoList_02.AddRange(tmp);
                }
                else
                {
                    List<KNS_D02> copytod02 = new List<KNS_D02>();
                    origin_02.ForEach(item =>
                    {
                        KNS_D02 tmp = item.Clone();
                        tmp.DATA_D = copyToValue.ToString();
                        copytod02.Add(tmp);
                    });
                    KinmuManager.ExecuteUpdate(ref copytod02);
                }
            }

            try
            {
                // 削除対象がなくても実施。Listの有無の判断をマネージャ側でいい感じにやってくれる
                dbManager.DeleteKinmuJisseki(copytoList_01);
                dbManager.DeleteSagyoNisshi(copytoList_02);
            }
            catch (Exception ex)
            {
                throw new KinmuException(ex.Message, ex);
            }

            SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, "勤務実績のコピーが完了しました。", InformationLevel.Highlight);
        }

        /// <summary>
        /// 実績クリアの処理を行います
        /// </summary>
        private void ClearResults()
        {
            logger.Debug(LOG_START);
            int[] clearValue = GetClearFlagFromRequest();
            List<KNS_D01> cleartoList_01 = new List<KNS_D01>();
            List<KNS_D02> cleartoList_02 = new List<KNS_D02>();

            // 削除対象の作成
            foreach (int day in clearValue)
            {
                KNS_D01 tmp = GetLocalKinmuJisseki(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month, day);
                if (tmp != null) cleartoList_01.Add(tmp);
                var tmp2 = GetLocalSagyoNisshi(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month, day);
                if (tmp2.Any()) cleartoList_02.AddRange(tmp2);
            }

            // 削除の実行
            int cnt = 0;
            cnt = dbManager.DeleteKinmuJisseki(cleartoList_01);
            cnt += dbManager.DeleteSagyoNisshi(cleartoList_02);

            if (0 < cnt) SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, "勤務実績のクリアが完了しました。", InformationLevel.Highlight);
            else SetInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText, "クリア対象の勤務実績がありませんでした。", InformationLevel.Error);
        }

        /// <summary>
        /// DBからデータを取得しなおしてデータをバインドします。
        /// </summary>
        private void UpdateEditGridViewBindData()
        {
            logger.Debug(LOG_START);
            EditPanel.Visible = true;

            //ボタンの活性とCSSの設定
            RunButton.Enabled = false;
            RunButton.CssClass = disableButtonCSS;
            EditButton.CssClass = selectedButtonCSS;


            // データバインド
            EditGridView.DataSource = GetEditList();
            EditGridView.DataBind();
            for (int rowIndex = 0; rowIndex < EditGridView.Rows.Count; rowIndex++)
            {
                GridViewRow row = EditGridView.Rows[rowIndex];

                //勤務予定未認証の場合のCSS適用
                if (!((List<KinmuGridViewEditRow>)EditGridView.DataSource)[rowIndex].IsAccepted)
                {
                    row.CssClass = "kinmuDataRow mishoninRow odd";
                }

                //日付のCSS適用
                SetGridViewRowCommonCss(ref row);
            }
        }


        private void UpdateCopyPlansGridViewBindData()
        {
            logger.Debug(LOG_START);
            CopyPlansGridView.Visible = true;

            //ボタンの活性とCSSの設定
            RunButton.Enabled = (syuyakubi <= viewDateTime);
            RunButton.CssClass = (syuyakubi <= viewDateTime) ? highlightButtonCSS : disableButtonCSS;
            CopyPlansButton.CssClass = (syuyakubi <= viewDateTime) ? selectedButtonCSS : disableButtonCSS;

            // データバインド
            CopyPlansGridView.DataSource = GetCopyList();
            CopyPlansGridView.DataBind();

            // CSSの適用
            CopyPlansGridView.AlternatingRowStyle.CssClass = "kinmuDataRow even";
            CopyPlansGridView.CssClass = "kinmuTableStyle dataTable";

            //（元→先）・休憩・記事
            CopyPlansGridView.HeaderRow.Cells[5].ControlStyle.CssClass = "colGroupWorkTime colMinashi1 headerSmall sorting_disabled";
            CopyPlansGridView.HeaderRow.Cells[8].ControlStyle.CssClass = "colGroupWorkTime colMinashi1 headerSmall sorting_disabled";
            CopyPlansGridView.HeaderRow.Cells[9].ControlStyle.CssClass = "colGroupWorkTime colMinashi1 headerSmall sorting_disabled";

            for (int rowIndex = 0; rowIndex < CopyPlansGridView.Rows.Count; rowIndex++)
            {
                GridViewRow row = CopyPlansGridView.Rows[rowIndex];

                //勤務予定未認証の場合のCSS適用
                if (!((List<KinmuGridViewCopyRow>)CopyPlansGridView.DataSource)[rowIndex].IsAccepted)
                {
                    row.CssClass = "kinmuDataRow mishoninRow odd";
                }

                //日付のCSS適用
                SetGridViewRowCommonCss(ref row);
            }
        }

        private void UpdateCopyResultsGridViewBindData()
        {
            logger.Debug(LOG_START);
            CopyResultsPanel.Visible = true;

            //ボタンの活性とCSSの設定
            RunButton.Enabled = (syuyakubi <= viewDateTime);
            RunButton.CssClass = (syuyakubi <= viewDateTime) ? highlightButtonCSS : disableButtonCSS;
            CopyResultsButton.CssClass = (syuyakubi <= viewDateTime) ? selectedButtonCSS : disableButtonCSS;



            // データバインド
            CopyResultsGridView.DataSource = GetCopyList();
            CopyResultsGridView.DataBind();

            //cssの適用
            CopyResultsGridView.AlternatingRowStyle.CssClass = "kinmuDataRow even";
            CopyResultsGridView.CssClass = "kinmuTableStyle dataTable";

            //休憩・記事・（元→先）
            CopyResultsGridView.HeaderRow.Cells[6].ControlStyle.CssClass = "colGroupWorkTime colMinashi1 headerSmall sorting_disabled";
            CopyResultsGridView.HeaderRow.Cells[7].ControlStyle.CssClass = "colGroupWorkTime colMinashi1 headerSmall sorting_disabled";
            CopyResultsGridView.HeaderRow.Cells[9].ControlStyle.CssClass = "colGroupWorkTime colMinashi1 headerSmall sorting_disabled";

            for (int rowIndex = 0; rowIndex < CopyResultsGridView.Rows.Count; rowIndex++)
            {
                GridViewRow row = CopyResultsGridView.Rows[rowIndex];

                //勤務予定未認証の場合のCSS適用
                if (!((List<KinmuGridViewCopyRow>)CopyResultsGridView.DataSource)[rowIndex].IsAccepted)
                {
                    row.CssClass = "kinmuDataRow mishoninRow odd";
                }

                //日付のCSS適用
                SetGridViewRowCommonCss(ref row);

            }
        }

        private void UpdateClearResultsGridViewBindData()
        {
            logger.Debug(LOG_START);
            ClearResultsPanel.Visible = true;

            //ボタンの活性とCSSの設定
            RunButton.Enabled = (syuyakubi <= viewDateTime);
            RunButton.CssClass = (syuyakubi <= viewDateTime) ? highlightButtonCSS : disableButtonCSS;
            ClearResultsButton.CssClass = (syuyakubi <= viewDateTime) ? selectedButtonCSS : disableButtonCSS;

            // データバインド
            ClearResultsGridView.DataSource = GetClearList();
            ClearResultsGridView.DataBind();

            //cssの適用
            ClearResultsGridView.AlternatingRowStyle.CssClass = "kinmuDataRow even";
            ClearResultsGridView.CssClass = "kinmuTableStyle dataTable";
            //休憩・記事
            ClearResultsGridView.HeaderRow.Cells[6].ControlStyle.CssClass = "colGroupWorkTime colMinashi1 headerSmall sorting_disabled";
            ClearResultsGridView.HeaderRow.Cells[7].ControlStyle.CssClass = "colGroupWorkTime colMinashi1 headerSmall sorting_disabled";

            for (int rowIndex = 0; rowIndex < ClearResultsGridView.Rows.Count; rowIndex++)
            {
                GridViewRow row = ClearResultsGridView.Rows[rowIndex];

                //勤務予定未認証の場合のCSS適用
                if (!((List<KinmuGridViewClearRow>)ClearResultsGridView.DataSource)[rowIndex].IsAccepted)
                {
                    row.CssClass = "kinmuDataRow mishoninRow odd";
                }

                //日付のCSS適用
                SetGridViewRowCommonCss(ref row);

            }
        }

        private void SetGridViewRowCommonCss(ref GridViewRow row)
        {
            logger.Debug(LOG_START);
            row.Cells[0].CssClass = "colDay textRight";

            if (row.Cells[1].Text.Contains("祝"))
            {
                row.Cells[1].CssClass = "holiday";
            }
            else
            {
                switch (row.Cells[1].Text)
                {
                    case "日":
                        row.Cells[1].CssClass = "sunday";
                        break;
                    case "土":
                        row.Cells[1].CssClass = "saturday";
                        break;
                    default:
                        row.Cells[1].CssClass = "usualday";
                        break;
                }
            }

            row.Cells[2].CssClass = "colYoteiNinsho";

            //勤務実績の表示されるインデックスを指定
            int JissekiNinshoIndex = ((string)Session["mode"] == "CopyPlans" ? 6 : 4);

            if (row.Cells[2].Text == row.Cells[JissekiNinshoIndex].Text)
            {
                row.Cells[JissekiNinshoIndex].CssClass = "colJissekiNinsho fontGray";
            }
            if (row.Cells[2].Text == "特休" || row.Cells[2].Text == "公休" || row.Cells[JissekiNinshoIndex].Text == "特休" || row.Cells[JissekiNinshoIndex].Text == "公休")
            {
                row.CssClass = "tokkyuKoukyuRow";         
            }

            if (row.Cells[2].Text == "特休" && row.Cells[JissekiNinshoIndex].Text != "特休")
            {
                row.CssClass = "kinmuDataRow workOnTokkyuRow odd";
            }

            if (row.Cells[2].Text == "公休" && row.Cells[JissekiNinshoIndex].Text != "公休")
            {
                row.CssClass = "kinmuDataRow workOnKoukyuRow even";
            }

        }

        private KNS_D13 GetLocalKinmuYotei(string employeeCD, int year, int month, int day)
        {
            logger.Debug(LOG_START);

            if (!DateTime.TryParse($"{year}/{month}/{day}", out DateTime a)) throw new KinmuException($"日付文字列が不正です。入力された値：{year}/{month}/{day}");
            string y = year.ToString(PADDING_ZERO_4);
            string m = month.ToString(PADDING_ZERO_2);
            string d = day.ToString(PADDING_ZERO_2);

            try
            {
                KinmuRecordRow row = kinmuRecordRowList.Single(_ => _.EmployeeCD == employeeCD && _.KinmuYotei.DATA_Y == y && _.KinmuYotei.DATA_M == m && _.KinmuYotei.DATA_D == d);
                return row.KinmuYotei;
            }
            catch (Exception)
            {
                return null;
            }
        }


        private KNS_D01 GetLocalKinmuJisseki(string employeeCD, int year, int month, int day)
        {
            logger.Debug(LOG_START);

            if (!DateTime.TryParse($"{year}/{month}/{day}", out DateTime a)) throw new KinmuException($"日付文字列が不正です。入力された値：{year}/{month}/{day}");
            string y = year.ToString(PADDING_ZERO_4);
            string m = month.ToString(PADDING_ZERO_2);
            string d = day.ToString(PADDING_ZERO_2);

            try
            {
                KinmuRecordRow row = kinmuRecordRowList.Single(_ => _.EmployeeCD == employeeCD && _.KinmuJisseki.DATA_Y == y && _.KinmuJisseki.DATA_M == m && _.KinmuJisseki.DATA_D == d);
                return row.KinmuJisseki;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private List<KNS_D02> GetLocalSagyoNisshi(string employeeCD, int year, int month, int day)
        {
            logger.Debug(LOG_START);

            if (!DateTime.TryParse($"{year}/{month}/{day}", out DateTime a)) throw new KinmuException($"日付文字列が不正です。入力された値：{year}/{month}/{day}");
            string y = year.ToString(PADDING_ZERO_4);
            string m = month.ToString(PADDING_ZERO_2);
            string d = day.ToString(PADDING_ZERO_2);

            try
            {
                KinmuRecordRow row = kinmuRecordRowList.Single(_ => _.EmployeeCD == employeeCD && _.CalendarMaster.DATA_Y == y && _.CalendarMaster.DATA_M == m && _.CalendarMaster.DATA_D == d);
                return row.SagyoNisshi;
            }
            catch (Exception)
            {
                return new List<KNS_D02>();
            }
        }

        /// <summary>
        /// 勤務システムの編集画面にバインドするListを返します。
        /// </summary>
        /// <returns></returns>
        public List<KinmuGridViewEditRow> GetEditList()
        {
            logger.Debug(LOG_START);
            List<KinmuGridViewEditRow> view = new List<KinmuGridViewEditRow>();
            int workTimeTotal = 0;

            foreach (KinmuRecordRow item in kinmuRecordRowList)
            {
                workTimeTotal += item.KinmuJisseki.DKINM ?? 0;
                KinmuGridViewEditRow row = new KinmuGridViewEditRow(item)
                {
                    WorkTimeTotal = MinutesToStringFormat(workTimeTotal)
                };
                view.Add(row);
            }

            return view;
        }

        /// <summary>
        /// 勤務システムの予定コピー・実績コピー画面にバインドするListを返します。
        /// </summary>
        /// <returns></returns>
        public List<KinmuGridViewCopyRow> GetCopyList()
        {
            logger.Debug(LOG_START);
            return new List<KinmuGridViewCopyRow>(kinmuRecordRowList.Select(_ => new KinmuGridViewCopyRow(_)));
        }

        /// <summary>
        /// 勤務システムの実績クリア画面にバインドするListを返します。
        /// </summary>
        /// <returns></returns>
        public List<KinmuGridViewClearRow> GetClearList()
        {
            logger.Debug(LOG_START);
            return new List<KinmuGridViewClearRow>(kinmuRecordRowList.Select(_ => new KinmuGridViewClearRow(_)));
        }

        private void IsNotPostBack()
        {
            logger.Debug(LOG_START);
            Session["mode"] = "Edit";
            CreateEmpoloyeeDropDownList();
            RenderDataTableHeadder();
            UpdateEditGridViewBindData();
        }

        /// <summary>
        /// ページの初期化処理を行います。
        /// </summary>
        private void InitPage()
        {
            logger.Debug(LOG_START);
            // 特休・公休の初期化
            dbManager.InitTokkyuKoukyuKinmuRecordRow(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month);

            // 勤務情報の取得
            if (
                !kinmuRecordRowList.Any() ||
                kinmuRecordRowList.First().EmployeeCD != viewShainInfo.SHAIN_CD ||
                kinmuRecordRowList.First().CalendarMaster.DATA_Y != viewDateTime.Year.ToString(PADDING_ZERO_4) ||
                kinmuRecordRowList.First().CalendarMaster.DATA_M != viewDateTime.Month.ToString(PADDING_ZERO_2)
            )
            {
                // 勤務情報が未取得 or 社員情報が異なる or 年が異なる or 月が異なる 場合はデータベースから取得する
                kinmuRecordRowList = dbManager.GetKinmuRecordRow(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month);
            }

            // メッセージパネルの初期化
            InitInformationPanel(ref InformationPanel, ref InfoIcon, ref InfoText);

            // 社員名と部下一覧の表示の初期化
            if (loginShainInfo.MANAGER == "1")
            {
                employeeNameLabel.Visible = false;
                employeeDropDownList.Visible = true;
            }
            else
            {
                employeeNameLabel.Visible = true;
                employeeDropDownList.Visible = false;
            }

            // 集約日データ取得
            SetSyuyakuDate();

            //集約日以前のデータを編集できないようにボタンを無効化
            ChangeListButtonEnabled(syuyakubi <= viewDateTime);

            //EditButtonのCSSを初期化
            EditButton.CssClass = ableButtonCSS;



            // メッセージデータ取得
            SetMessageData();

            // 休暇情報取得（集約日データ取得後に実行する）
            SetKyukaData();

            // 全てのパネルを非表示にする。
            EditPanel.Visible = false;
            CopyPlansGridView.Visible = false;
            CopyResultsPanel.Visible = false;
            ClearResultsPanel.Visible = false;
            RunButton.Enabled = false;
        }

        /// <summary>
        /// 部下一覧用のドロップダウンリストを作成します。一般社員の場合は自分の社員名を表示して終了します。
        /// </summary>
        private void CreateEmpoloyeeDropDownList()
        {
            logger.Debug(LOG_START);
            // マネージャー権限がなければ社員名を表示して終了
            if (loginShainInfo.MANAGER != "1")
            {
                employeeNameLabel.Visible = true;
                return;
            }

            // ドロップダウンリスト用データが未取得ならDBから取得
            if (!empDropDownListData.Any())
            {
                empDropDownListData = dbManager.GetSyainMasterByShoninShainCD(loginShainInfo.SHAIN_CD);
            }

            // ドロップダウンリストに自分のデータがなければ追加
            if (!empDropDownListData.Any(_ => _.SHAIN_CD == loginShainInfo.SHAIN_CD))
            {
                empDropDownListData.Add(loginShainInfo);
            }

            // データバインディング
            employeeDropDownList.DataSource = empDropDownListData;
            employeeDropDownList.DataTextField = "SHAIN_NM";
            employeeDropDownList.DataValueField = "SHAIN_CD";
            employeeDropDownList.SelectedValue = viewShainInfo.SHAIN_CD;
            employeeDropDownList.DataBind();
            employeeDropDownList.Visible = true;
        }

        /// <summary>
        /// 集約日をデータベースから取得します。
        /// </summary>
        private void SetSyuyakuDate()
        {
            logger.Debug(LOG_START);
            if (syuyaku == null)
            {
                syuyaku = dbManager.GetShuyakuYM();
            }

            if (!DateTime.TryParse($"{syuyaku[0]}/{syuyaku[1]}/1", out syuyakubi))
            {
                throw new KinmuException(string.Format("集約日に不正な文字列が設定されています。{0}/{1}", syuyaku[0], syuyaku[1]));
            }
            syuyakubi = syuyakubi.AddMonths(1);
        }

        /// <summary>
        /// お知らせをデータベースから取得します。
        /// </summary>
        private void SetMessageData()
        {
            logger.Debug(LOG_START);
            if (!noticeLabelData.Any())
            {
                noticeLabelData = dbManager.GetLatestMessage();
            }
        }

        /// <summary>
        /// 休暇情報をデータベースから取得します
        /// </summary>
        private void SetKyukaData()
        {
            logger.Debug(LOG_START);
            if (nenkyu < 0)
            {
                nenkyu = dbManager.GetNenkyuZan(viewShainInfo.SHAIN_CD);
            }

            // 特休・公休の取得
            if (tokkyuKokyu == null)
            {
                tokkyuKokyu = dbManager.GetTokkyuKoukyuZan(viewShainInfo.SHAIN_CD, syuyakubi.AddMonths(-3).Year);
            }
        }

        /// <summary>
        /// データテーブルのヘッダ情報を作成します
        /// </summary>
        private void RenderDataTableHeadder()
        {
            logger.Debug(LOG_START);

            if (tokkyuKokyu?[0] != null)
            {
                tokukyuLabel.Text = "特休 " + tokkyuKokyu[0].ToString("0.0");
            }
            if (tokkyuKokyu?[1] != null)
            {
                koukyuLabel.Text = "公休 " + tokkyuKokyu[1].ToString("0.0");
            }


            // おしらせ
            noticeLabel.Text = string.Join("<br/>", noticeLabelData.Where(_ => _ != null));

            // 社員コード
            employeeCodeLabel.Text = viewShainInfo.SHAIN_CD;

            // 社員名
            employeeNameLabel.Text = viewShainInfo.SHAIN_NM;

            // 年休残り日数基準日
            closingDateLabel.Text = string.Format("年休等残日数（{0:yyyy/MM/dd}現在）", syuyakubi);

            // 超勤予定時間
            overtimePlansLabel.Text = "月間超勤予定時間 " + MinutesToStringFormat(KinmuManager.CalcFastGekkanTankaBJikan(KinmuManager.CallB(kinmuRecordRowList)));

            // 年休残り
            nenkyuLabel.Text = "年休 " + nenkyu.ToString("0.0");

            // 勤務情報表示年月
            selectedMonthLabel.Text = viewDateTime.ToString("yyyy年MM月");
        }

        /// <summary>
        /// 予定コピー・実績コピー・実績クリア・実行ボタンの活性・非活性を変更します。CSSも応じて変更します。
        /// </summary>
        /// <param name="isEnable"></param>
        private void ChangeListButtonEnabled(bool isEnable)
        {
            CopyPlansButton.Enabled = isEnable;
            CopyResultsButton.Enabled = isEnable;
            ClearResultsButton.Enabled = isEnable;
            RunButton.Enabled = isEnable;

            if (isEnable)
            {            
                CopyPlansButton.CssClass = ableButtonCSS;
                CopyResultsButton.CssClass = ableButtonCSS;
                ClearResultsButton.CssClass = ableButtonCSS;
                RunButton.CssClass = ableButtonCSS;
            }
            else
            {
                CopyPlansButton.CssClass = disableButtonCSS;
                CopyResultsButton.CssClass = disableButtonCSS;
                ClearResultsButton.CssClass = disableButtonCSS;
                RunButton.CssClass = disableButtonCSS;
            }

        }

    }
}