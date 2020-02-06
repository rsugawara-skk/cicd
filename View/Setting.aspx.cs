using CommonLibrary;
using CommonLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserLibrary;
using static CommonLibrary.CommonDefine;

namespace KinmuSystem.View
{
    public partial class Setting : Page
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private KNS_M01 viewShainInfo;
        private KNS_M01 loginShainInfo;
        private DateTime viewDateTime;

        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                ((KinmuSystem)Master).SetSessionData(ref loginShainInfo, ref viewShainInfo, ref viewDateTime);

                // 画面更新用のDBアクセス
                // ユーザマネージャのインスタンスを生成
                UserManager userManager = new UserManager();

                // 社員コードの変数を定義
                string requestShainCD = viewShainInfo.SHAIN_CD;

                // ログイン中の社員コードから、個人設定データを取得
                UserSetting currentUserSetting = userManager.GetUserSettingByShainCD(requestShainCD);

                /*
                 * タブ：入力初期値
                 * 個人データのバインド  
                 */

                if (!IsPostBack)
                {
                    // 「勤務認証:」ドロップダウンリストへのバインド
                    ninshoCDDropDownList.DataSource = userManager.GetKinmuShubetsuByShainCD(requestShainCD);
                    ninshoCDDropDownList.DataTextField = "Value";
                    ninshoCDDropDownList.DataValueField = "Key";
                    ninshoCDDropDownList.DataBind();

                    // 「プロジェクトコード:」ドロップダウンリストへのバインド
                    projCDDropDownList.DataSource = userManager.GetProjCDMstDictionary();
                    projCDDropDownList.DataTextField = "Value";
                    projCDDropDownList.DataValueField = "Key";
                    projCDDropDownList.DataBind();

                    // 「作業コード:」ドロップダウンリストへのバインド
                    sagyoCDDropDownList.DataSource = userManager.GetSagyoCDMstDictionary();
                    sagyoCDDropDownList.DataTextField = "Value";
                    sagyoCDDropDownList.DataValueField = "Key";
                    sagyoCDDropDownList.DataBind();

                    // タブ：入力初期値のデータが取得できた場合
                    if (currentUserSetting.KinmuJissekiMasterList.Count > 0)
                    {
                        // 「勤務認証:」へのバインド
                        ninshoCDDropDownList.SelectedValue = currentUserSetting.KinmuJissekiMasterList[0].NINSE_CD;

                        // 「勤務時間:」へのバインド
                        workStartHour.Text = currentUserSetting.KinmuJissekiMasterList[0].STR_HR;
                        workStartMinute.Text = currentUserSetting.KinmuJissekiMasterList[0].STR_MIN;
                        workEndHour.Text = currentUserSetting.KinmuJissekiMasterList[0].END_HR;
                        workEndMinute.Text = currentUserSetting.KinmuJissekiMasterList[0].END_MIN;
                        workEndNextDayFlg.Checked = (currentUserSetting.KinmuJissekiMasterList[0].END_PAR == "1");

                        // 「休憩時間:」へのバインド
                        // [1]
                        restStartHour_1.Text = currentUserSetting.KinmuJissekiMasterList[0].RESTS1_HR;
                        restStartMinute_1.Text = currentUserSetting.KinmuJissekiMasterList[0].RESTS1_MIN;
                        restEndHour_1.Text = currentUserSetting.KinmuJissekiMasterList[0].RESTE1_HR;
                        restEndMinute_1.Text = currentUserSetting.KinmuJissekiMasterList[0].RESTE1_MIN;
                        // [2]
                        restStartHour_2.Text = currentUserSetting.KinmuJissekiMasterList[0].RESTS2_HR;
                        restStartMinute_2.Text = currentUserSetting.KinmuJissekiMasterList[0].RESTS2_MIN;
                        restEndHour_2.Text = currentUserSetting.KinmuJissekiMasterList[0].RESTE2_HR;
                        restEndMinute_2.Text = currentUserSetting.KinmuJissekiMasterList[0].RESTE2_MIN;
                        // [3]
                        restStartHour_3.Text = currentUserSetting.KinmuJissekiMasterList[0].RESTS3_HR;
                        restStartMinute_3.Text = currentUserSetting.KinmuJissekiMasterList[0].RESTS3_MIN;
                        restEndHour_3.Text = currentUserSetting.KinmuJissekiMasterList[0].RESTE3_HR;
                        restEndMinute_3.Text = currentUserSetting.KinmuJissekiMasterList[0].RESTE3_MIN;

                        // 個人設定データを選択状態にする
                        projCDDropDownList.SelectedValue = currentUserSetting.KinmuJissekiMasterList[0].PROJ_CD;

                        // 個人設定データを選択状態にする
                        sagyoCDDropDownList.SelectedValue = currentUserSetting.KinmuJissekiMasterList[0].SAGYO_CD;
                    }

                    /*
                     * タブ：利用コード
                     * 個人データのバインド  
                     */

                    // 個人設定のプロジェクトコードリストボックスに
                    // 取得した個人設定データからプロジェクトコードリストをバインド
                    projCDListListBox.DataSource = userManager.GetProjCDListDictionary(currentUserSetting.PJMasterList);
                    projCDListListBox.DataTextField = "Value";
                    projCDListListBox.DataValueField = "Key";
                    projCDListListBox.DataBind();

                    // 作業コードタブの作業コードリストボックスに
                    // 取得した個人設定データから作業コードリストをバインド
                    sagyoCDListBox.DataSource = userManager.GetSagyoCDListDictionary(currentUserSetting.SagyoCDMasterList);
                    sagyoCDListBox.DataTextField = "Value";
                    sagyoCDListBox.DataValueField = "Key";
                    sagyoCDListBox.DataBind();

                    /*   追加ボタン用   */
                    // 利用コードタブのプロジェクトコードドロップダウンリストに
                    // 取得した個人設定データからプロジェクトコードリストをバインド
                    additionalProjCDDropDownList.DataSource = userManager.GetAddProjCDDictionary(currentUserSetting.PJMasterList);
                    additionalProjCDDropDownList.DataTextField = "Value";
                    additionalProjCDDropDownList.DataValueField = "Key";
                    additionalProjCDDropDownList.DataBind();

                    // 作業コードタブの作業コードドロップダウンリストに
                    // 取得した個人設定データから作業コードリストをバインド
                    additionalSagyoCDDropDownList.DataSource = userManager.GetAddSagyoCDDictionary(currentUserSetting.SagyoCDMasterList);
                    additionalSagyoCDDropDownList.DataTextField = "Value";
                    additionalSagyoCDDropDownList.DataValueField = "Key";
                    additionalSagyoCDDropDownList.DataBind();
                }
            }
            catch (KinmuException ex)
            {
                // 業務エラーメッセージ表示
                ErrorMessageLabel.Text = ex.Message;
                ErrorMessagePanel.Visible = true;
                MessagePanel.Visible = false;
            }
            catch (Exception)
            {
                // 業務エラー以外の例外発生時
                ErrorMessageLabel.Text = "CM0009:データ取得に失敗しました。(個人入力設定)";
                ErrorMessagePanel.Visible = true;
                MessagePanel.Visible = false;
            }
        }

        /// <summary>
        /// 画面上のPJコード、作業コードをDBに反映させる処理
        /// </summary>
        protected void UpdateCodeUserSetting()
        {
            logger.Debug(LOG_START);
            // ユーザマネージャのインスタンスを生成
            UserManager userManager = new UserManager();

            // 社員コードの変数を定義
            string requestShainCD = viewShainInfo.SHAIN_CD;

            // ログイン中の社員コードから、個人設定データを取得
            UserSetting currentUserSetting = userManager.GetUserSettingByShainCD(requestShainCD);

            // DB更新用の利用コード格納用インスタンスを定義
            List<KNS_D05> updateProjCDList = new List<KNS_D05>();
            List<KNS_D06> updateSagyoCDList = new List<KNS_D06>();

            // 更新後のリストを取得
            int projCDViewOrder = 0;
            foreach (ListItem currentProj in projCDListListBox.Items)
            {
                KNS_D05 targetProj = new KNS_D05
                {
                    PROJ_CD = currentProj.Value,
                    VIEW_ORDER = projCDViewOrder.ToString(),
                    SHAIN_CD = requestShainCD,
                    UPD_DATE = DateTime.UtcNow.AddHours(9)
                };

                // マスタデータを表示用に整形して格納
                updateProjCDList.Add(targetProj);

                projCDViewOrder++;
            }

            int sagyoCDViewOrder = 0;
            foreach (ListItem currentSagyo in sagyoCDListBox.Items)
            {
                KNS_D06 targetSagyo = new KNS_D06
                {
                    SAGYO_CD = currentSagyo.Value,
                    VIEW_ORDER = sagyoCDViewOrder.ToString(),
                    SHAIN_CD = requestShainCD,
                    UPD_DATE = DateTime.UtcNow.AddHours(9)
                };

                // マスタデータを表示用に整形して格納
                updateSagyoCDList.Add(targetSagyo);

                sagyoCDViewOrder++;
            }

            // 格納用個人設定インスタンスを定義
            UserSetting updateUserSetting = new UserSetting(requestShainCD, currentUserSetting.KinmuJissekiMasterList, updateProjCDList, updateSagyoCDList);

            // 個人設定の更新
            UpdateUserSettingAndDisplayPanel(updateUserSetting);
        }

        /// <summary>
        /// 画面上の入力初期値をDBに反映させる処理
        /// </summary>
        protected void UpdateInitValueUserSetting()
        {
            logger.Debug(LOG_START);
            // ユーザマネージャのインスタンスを生成
            UserManager userManager = new UserManager();

            // 社員コードの変数を定義
            string requestShainCD = viewShainInfo.SHAIN_CD;

            // ログイン中の社員コードから、個人設定データを取得
            UserSetting currentUserSetting = userManager.GetUserSettingByShainCD(requestShainCD);

            // DB更新用の個人設定インスタンスを定義
            KNS_D04 updateInitialValue = new KNS_D04();
            List<KNS_D04> updateInitialValueList = new List<KNS_D04>();

            // 勤務認証:
            updateInitialValue.NINSE_CD = ninshoCDDropDownList.SelectedValue;

            // 勤務時間:
            updateInitialValue.STR_HR = workStartHour.Text;
            updateInitialValue.STR_MIN = workStartMinute.Text;
            updateInitialValue.END_HR = workEndHour.Text;
            updateInitialValue.END_MIN = workEndMinute.Text;
            if (workEndNextDayFlg.Checked)
            {
                updateInitialValue.END_PAR = "1";
            }
            else
            {
                updateInitialValue.END_PAR = "0";
            }

            // 休憩時間:
            // [1]
            updateInitialValue.RESTS1_HR = restStartHour_1.Text;
            updateInitialValue.RESTS1_MIN = restStartMinute_1.Text;
            updateInitialValue.RESTE1_HR = restEndHour_1.Text;
            updateInitialValue.RESTE1_MIN = restEndMinute_1.Text;
            // [2]
            updateInitialValue.RESTS2_HR = restStartHour_2.Text;
            updateInitialValue.RESTS2_MIN = restStartMinute_2.Text;
            updateInitialValue.RESTE2_HR = restEndHour_2.Text;
            updateInitialValue.RESTE2_MIN = restEndMinute_2.Text;
            // [3]
            updateInitialValue.RESTS3_HR = restStartHour_3.Text;
            updateInitialValue.RESTS3_MIN = restStartMinute_3.Text;
            updateInitialValue.RESTE3_HR = restEndHour_3.Text;
            updateInitialValue.RESTE3_MIN = restEndMinute_3.Text;

            // 選択プロジェクトコード
            updateInitialValue.PROJ_CD = projCDDropDownList.SelectedValue;

            // 選択作業コード
            updateInitialValue.SAGYO_CD = sagyoCDDropDownList.SelectedValue;

            // 社員コード
            updateInitialValue.SHAIN_CD = requestShainCD;

            // フレックスフラグの格納
            updateInitialValue.FLX_FLG = "1";

            // 最終更新時間
            updateInitialValue.UPD_DATE = DateTime.UtcNow.AddHours(9);

            // リストに格納
            updateInitialValueList.Add(updateInitialValue);


            // 格納用個人設定インスタンスを定義
            UserSetting updateUserSetting = new UserSetting(requestShainCD, updateInitialValueList
                , currentUserSetting.PJMasterList, currentUserSetting.SagyoCDMasterList);

            // 個人設定の更新
            UpdateUserSettingAndDisplayPanel(updateUserSetting);
        }

        /// <summary>
        /// DB反映の共通処理。更新後のメッセージをパネルに表示
        /// </summary>
        /// <param name="updateUserSetting"></param>
        protected void UpdateUserSettingAndDisplayPanel(UserSetting updateUserSetting)
        {
            logger.Debug(LOG_START);
            // 個人設定更新処理
            try
            {
                // ユーザマネージャのインスタンスを生成
                UserManager userManager = new UserManager();

                //　更新用データのバリデーションチェック
                updateUserSetting.CheckValidation();

                // 個人設定の更新
                string resultMessage = userManager.UpdateUserSetting(updateUserSetting);

                // メッセージパネルを両方非表示
                MessagePanel.Visible = false;
                ErrorMessagePanel.Visible = false;

                // DB更新完了メッセージ表示
                MessageLabel.Text = resultMessage;
                MessagePanel.Visible = true;
                ErrorMessagePanel.Visible = false;
            }
            catch (KinmuException ex)
            {
                // 業務エラーメッセージ表示
                ErrorMessageLabel.Text = ex.Message;
                ErrorMessagePanel.Visible = true;
            }
            catch (Exception)
            {
                // 業務エラー以外の例外発生時
                ErrorMessageLabel.Text = "CM0007:DB更新において異常が発生しました。個人入力設定";
                ErrorMessagePanel.Visible = true;
            }
        }

        /* ボタン押下時の処理 */
        /// <summary>
        /// 入力初期値タブの更新ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void InitValueUpdateButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                // 初期表示タブ変更
                defaultTab.Value = string.Empty;
                // メッセージパネルを両方非表示
                MessagePanel.Visible = false;
                ErrorMessagePanel.Visible = false;
                // 画面上の値を更新
                UpdateInitValueUserSetting();
            }
            catch (KinmuException ex)
            {
                // 業務エラーメッセージ表示
                ErrorMessageLabel.Text = ex.Message;
                ErrorMessagePanel.Visible = true;
            }
            catch (Exception)
            {
                // 業務エラー以外の例外発生時
                ErrorMessageLabel.Text = "CM0007:DB更新において異常が発生しました。個人入力設定";
                ErrorMessagePanel.Visible = true;
            }
        }

        /* 画面の項目の並び替え・移動用メソッド */
        /// <summary>
        /// プロジェクトコード並び替えボタン[↑]押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ProjCDUpButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                // PJコードリストの選択中インデックスを取得
                int selectedIndex = projCDListListBox.SelectedIndex;

                // 最上部の項目が選択されている場合はなにもしない
                if (selectedIndex <= 0)
                {
                    return;
                }
                // PJコードリストから選択アイテムを取得
                ListItem selectedItem = projCDListListBox.SelectedItem;
                // PJコードリストから選択アイテムを削除
                projCDListListBox.Items.Remove(selectedItem);
                // 元の位置の１つ上に挿入
                projCDListListBox.Items.Insert(selectedIndex - 1, selectedItem);
                // 選択中の項目を戻す
                projCDListListBox.SelectedIndex = selectedIndex - 1;
            }
            catch (NullReferenceException)
            {
                //未選択系の例外処理取得時は何もしない
            }
        }

        /// <summary>
        /// プロジェクトコード並び替えボタン[↓]押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ProjCDDowmButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                // PJコードリストの選択中インデックスを取得
                int selectedIndex = projCDListListBox.SelectedIndex;

                // 最下部の項目が選択されている場合はなにもしない
                if (selectedIndex >= projCDListListBox.Items.Count - 1)
                {
                    return;
                }
                // PJコードリストから選択アイテムを取得
                ListItem selectedItem = projCDListListBox.SelectedItem;
                // PJコードリストから選択アイテムを削除
                projCDListListBox.Items.Remove(selectedItem);
                // 元の位置の１つ上に挿入
                projCDListListBox.Items.Insert(selectedIndex + 1, selectedItem);
                // 選択中の項目を戻す
                projCDListListBox.SelectedIndex = selectedIndex + 1;
            }
            catch (NullReferenceException)
            {
                //未選択系の例外処理取得時は何もしない
            }
        }

        /// <summary>
        /// プロジェクトコード削除ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ProjCDDeleteButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                // PJコードリストから選択アイテムを取得
                ListItem selectedItem = projCDListListBox.SelectedItem;
                // PJコードリストから選択アイテムを削除
                projCDListListBox.Items.Remove(selectedItem);
                // 追加用のPJコードドロップダウンリストに削除した項目を追加
                selectedItem.Selected = false;
                additionalProjCDDropDownList.Items.Add(selectedItem);

                // 無効化状態の解除
                if (additionalProjCDDropDownList.Items.Count > 0)
                {
                    additionalProjCDDropDownList.Enabled = true;
                }
            }
            catch (NullReferenceException)
            {
                //未選択系の例外処理取得時は何もしない
            }
        }


        /// <summary>
        /// プロジェクトコード追加ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ProjCDAddButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                // 追加用のPJコードドロップダウンリストが空の場合はなにもしない
                if (additionalProjCDDropDownList.Items.Count <= 0)
                {
                    additionalProjCDDropDownList.Enabled = false;
                    return;
                }
                // 追加用のPJコードドロップダウンリストから選択アイテムを取得
                ListItem selectedItem = additionalProjCDDropDownList.SelectedItem;
                // 追加用のPJコードドロップダウンリストから選択アイテムを削除
                additionalProjCDDropDownList.Items.Remove(additionalProjCDDropDownList.SelectedItem);
                // PJコードリストに選択した項目を追加
                selectedItem.Selected = false;
                projCDListListBox.Items.Add(selectedItem);
            }
            catch (NullReferenceException)
            {
                //未選択系の例外処理取得時は何もしない
            }
        }

        /// <summary>
        /// 作業コード並び替えボタン[↑]押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SagyoCDUpButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                // 作業コードリストの選択中インデックスを取得
                int selectedIndex = sagyoCDListBox.SelectedIndex;

                // 最上部の項目が選択されている場合はなにもしない
                if (selectedIndex <= 0)
                {
                    return;
                }

                // PJコードリストから選択アイテムを取得
                ListItem selectedItem = sagyoCDListBox.SelectedItem;
                // PJコードリストから選択アイテムを削除
                sagyoCDListBox.Items.Remove(selectedItem);
                // 元の位置の１つ上に挿入
                sagyoCDListBox.Items.Insert(selectedIndex - 1, selectedItem);
                // 選択中の項目を戻す
                sagyoCDListBox.SelectedIndex = selectedIndex - 1;
            }
            catch (NullReferenceException)
            {
                //未選択系の例外処理取得時は何もしない
            }

        }

        /// <summary>
        /// 作業コード並び替えボタン[↓]押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SagyoCDDownButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                // 作業コードリストの選択中インデックスを取得
                int selectedIndex = sagyoCDListBox.SelectedIndex;

                // 最下部の項目が選択されている場合はなにもしない
                if (selectedIndex >= sagyoCDListBox.Items.Count - 1)
                {
                    return;
                }
                // PJコードリストから選択アイテムを取得
                ListItem selectedItem = sagyoCDListBox.SelectedItem;
                // PJコードリストから選択アイテムを削除
                sagyoCDListBox.Items.Remove(selectedItem);
                // 元の位置の１つ上に挿入
                sagyoCDListBox.Items.Insert(selectedIndex + 1, selectedItem);
                // 選択中の項目を戻す
                sagyoCDListBox.SelectedIndex = selectedIndex + 1;
            }
            catch (NullReferenceException)
            {
                //未選択系の例外処理取得時は何もしない
            }
        }

        /// <summary>
        /// 作業コード削除ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SagyoCDDeleteButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            try
            {
                // PJコードリストから選択アイテムを取得
                ListItem selectedItem = sagyoCDListBox.SelectedItem;
                // PJコードリストから選択アイテムを削除
                sagyoCDListBox.Items.Remove(selectedItem);
                // 追加用のPJコードドロップダウンリストに削除した項目を追加
                selectedItem.Selected = false;
                additionalSagyoCDDropDownList.Items.Add(selectedItem);

                // 無効化状態の解除
                if (additionalSagyoCDDropDownList.Items.Count > 0)
                {
                    additionalSagyoCDDropDownList.Enabled = true;
                    sagyoCDAddButton.Enabled = true;
                }
            }
            catch (NullReferenceException)
            {
                //未選択系の例外処理取得時は何もしない
            }
        }

        /// <summary>
        /// 作業コード追加ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SagyoCDAddButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            // 追加用のPJコードドロップダウンリストが空の場合はなにもしない
            if (additionalSagyoCDDropDownList.Items.Count <= 0)
            {
                additionalSagyoCDDropDownList.Enabled = false;
                return;
            }
            // 追加用のPJコードドロップダウンリストから選択アイテムを取得
            ListItem selectedItem = additionalSagyoCDDropDownList.SelectedItem;
            // 追加用のPJコードドロップダウンリストから選択アイテムを削除
            additionalSagyoCDDropDownList.Items.Remove(additionalSagyoCDDropDownList.SelectedItem);
            // PJコードリストに選択した項目を追加
            selectedItem.Selected = false;
            sagyoCDListBox.Items.Add(selectedItem);
        }

        /// <summary>
        /// 利用コードタブの更新ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CodeUpdateButton_Click(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            // 初期表示タブ変更
            defaultTab.Value = "tabCode";
            // メッセージパネルを両方非表示
            MessagePanel.Visible = false;
            ErrorMessagePanel.Visible = false;
            // 画面上の値を更新
            UpdateCodeUserSetting();
        }
    }
}