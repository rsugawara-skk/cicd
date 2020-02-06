using CommonLibrary;
using CommonLibrary.Models;
using KinmuLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserLibrary;
using static CommonLibrary.CommonDefine;
using static CommonLibrary.Util;

namespace KinmuSystem.View
{
    /// <summary>
    /// 勤務実績入力画面
    /// </summary>
    public partial class Update : Page
    {
        // ログ出力用
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // マネージャー
        private DBManager db = new DBManager();
        private UserManager um = new UserManager();

        // ラベル用プロパティ
        private int sagyoTimeMin;
        private int nisshiSagyoTimeRemainMin;
        private int kousokuTimeMin;
        private int totalKyukeiTimeMin;
        private int yoteiTimeMin;
        private NinsyoCD calenderNinsyoCD;

        // 変更の許可
        private bool dataChangeIsEnabled = false;

        // 表示するデータ
        private Dictionary<string, string> kojinSettingPJCD = new Dictionary<string, string>();
        private Dictionary<string, string> kojinSettingSagyoCD = new Dictionary<string, string>();
        private KNS_D04 kojinSettingKinmuJisseki;
        private KinmuRecordRow record;

        // セッションデータ
        private KNS_M01 viewShainInfo;
        private KNS_M01 loginShainInfo;
        private DateTime viewDateTime;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);

                // セッションから値を取得
                ((KinmuSystem)Master).SetSessionData(ref loginShainInfo, ref viewShainInfo, ref viewDateTime);

                // 勤務情報を取得
                SetKinmuRecordRow(ref record, viewShainInfo.SHAIN_CD, viewDateTime);

                // 変更可能かチェックして値をセット
                SetDataChangeIsEnabled(ref dataChangeIsEnabled);

                // 個人設定の読み込み
                SetKojinSettingKinmuJisseki(ref kojinSettingKinmuJisseki, viewShainInfo.SHAIN_CD);
                SetKojinSettingPJCD(ref kojinSettingPJCD, viewShainInfo.SHAIN_CD);
                SetKojinSettingSagyoCD(ref kojinSettingSagyoCD, viewShainInfo.SHAIN_CD);

                // メッセージパネルの初期化
                InitInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel);

                // ポストバック時はここで終了
                if (IsPostBack) return;

                // フォームの初期化
                FormInit();

                // データのバインド
                BindKinmuRecordRow(record);
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
            }
        }

        #region 値を取得し変数に代入する処理
        /// <summary>
        /// 勤務情報をデータベースから取得し、引数に指定した変数に代入します。
        /// </summary>
        /// <param name="record">代入する勤務情報</param>
        /// <param name="cd">取得対象の社員コード</param>
        /// <param name="time">取得対象の日付</param>
        private void SetKinmuRecordRow(ref KinmuRecordRow record, string cd, DateTime time)
        {
            try
            {
                logger.Debug(LOG_START);
                List<KinmuRecordRow> data = db.GetKinmuRecordRow(cd, time.Year, time.Month, time.Day);
                if (!data.Any()) throw new KinmuException("勤務実績がありませんでした。");
                record = data.First();
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 勤務情報の変更が可能か確認し、結果を引数の変数に代入します。集約日よりも表示する勤務情報の日付が前の場合は編集が不可能です。
        /// </summary>
        /// <param name="isEnabled">代入する変更可否情報</param>
        private void SetDataChangeIsEnabled(ref bool isEnabled)
        {
            try
            {
                logger.Debug(LOG_START);
                int[] syuyaku = db.GetShuyakuYM();
                if (!DateTime.TryParse($"{syuyaku[0]}/{syuyaku[1]}/1", out DateTime syuyakubi))
                {
                    throw new KinmuException(string.Format("集約日に不正な文字列が設定されています。{0}/{1}", syuyaku[0], syuyaku[1]));
                }
                isEnabled = syuyakubi.AddMonths(1) <= viewDateTime;
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 勤務実績の個人設定を取得し、結果を引数の変数に代入します。
        /// </summary>
        /// <param name="jisseki">個人設定を代入する変数</param>
        /// <param name="cd">個人設定を取得する社員番号</param>
        private void SetKojinSettingKinmuJisseki(ref KNS_D04 jisseki, string cd)
        {
            try
            {
                logger.Debug(LOG_START);
                List<KNS_D04> d04s = db.GetKojinSetteiKinmuJisseki(cd);
                if (d04s.Any())
                {
                    jisseki = d04s.First();
                }
                else
                {
                    // throwしないのは、このまま処理続行しても問題ないため。しかし、エラー表示はする。
                    var ex = new KinmuException("社員番号：" + cd + "の個人設定がありませんでした。");
                    SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
                }
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// プロジェクトコードの個人設定を取得し、結果を引数の変数に代入します
        /// </summary>
        /// <param name="pjcds">プロジェクトコードを代入する変数</param>
        /// <param name="cd">個人設定を取得する社員番号</param>
        private void SetKojinSettingPJCD(ref Dictionary<string, string> pjcds, string cd)
        {
            try
            {
                logger.Debug(LOG_START);
                List<KNS_D05> kojinPJCD = db.GetKojinSetteiPJCD(cd);
                pjcds = um.GetProjCDListDictionary(kojinPJCD);
                if (pjcds.Any())
                {
                    pjcds.Add("", "");
                }
                else
                {
                    // throwしないのは、このまま処理続行しても問題ないため。しかし、エラー表示はする。
                    var ex = new KinmuException("個人設定でPJコードが何も設定されていません。初めに個人設定画面からPJコードを設定してください。");
                    SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
                }
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 作業コードの個人設定を取得し、結果を引数の変数に代入します
        /// </summary>
        /// <param name="pjcds">作業コードを代入する変数</param>
        /// <param name="cd">個人設定を取得する社員番号</param>
        private void SetKojinSettingSagyoCD(ref Dictionary<string, string> sagyocds, string cd)
        {
            try
            {
                logger.Debug(LOG_START);
                List<KNS_D06> kojinPJCD = db.GetKojinSetteiSagyoCD(cd);
                sagyocds = um.GetSagyoCDListDictionary(kojinPJCD);
                if (sagyocds.Any())
                {
                    sagyocds.Add("", "");
                }
                else
                {
                    // throwしないのは、このまま処理続行しても問題ないため。しかし、エラー表示はする。
                    var ex = new KinmuException("個人設定で作業コードが何も設定されていません。初めに個人設定画面から作業コードを設定してください。");
                    SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
                }
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }
        #endregion

        #region 画面の初期化を行う処理
        /// <summary>
        /// フォームの初期化します
        /// </summary>
        private void FormInit()
        {
            try
            {
                logger.Debug(LOG_START);
                InitPageHeader();
                BindNinshoCodeMaster();
                InitKinmuYoteiForm();
                InitKinmuJissekiForm();
                InitSagyoNisshiForm();
                ChangeYoteiControlsEnabled(dataChangeIsEnabled);
                ChangeJissekiControlsIsEnabled(dataChangeIsEnabled);
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// ヘッダー情報を初期化します。
        /// </summary>
        private void InitPageHeader()
        {
            try
            {
                logger.Debug(LOG_START);
                // カレンダーマスタの認証コードを取得
                calenderNinsyoCD = (NinsyoCD)int.Parse(record.CalendarMaster.NINYO_CD ?? "1");

                ViewShainCD.Text = viewShainInfo.SHAIN_CD;
                ViewShainName.Text = viewShainInfo.SHAIN_NM;
                KinmuYear.Text = viewDateTime.ToString("yyyy");
                KinmuMonth.Text = viewDateTime.ToString("MM");
                KinmuDay.Text = viewDateTime.ToString("dd");
                KinmuDayOfWeek.Text = "（" + viewDateTime.ToString("ddd") + "）";
                CalendarNinsyoCD.Text = $"[{calenderNinsyoCD.ToString()}]";
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 勤務予定を初期化します。
        /// </summary>
        private void InitKinmuYoteiForm()
        {
            try
            {
                logger.Debug(LOG_START);
                BindTimeRangeTextForm(null, TimeRangeBox.予定);
                YoteiEndNextDayFlg.Checked = false;
                if (YoteiNinshoCd.SelectedItem != null) YoteiNinshoCd.SelectedItem.Selected = false;
                ChangeYoteiControlsEnabled(dataChangeIsEnabled);
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 勤務実績を初期化します。
        /// </summary>
        private void InitKinmuJissekiForm()
        {
            try
            {
                logger.Debug(LOG_START);
                BindTimeRangeTextForm(null, TimeRangeBox.実績);
                BindTimeRangeTextForm(null, TimeRangeBox.休憩1);
                BindTimeRangeTextForm(null, TimeRangeBox.休憩2);
                BindTimeRangeTextForm(null, TimeRangeBox.休憩3);
                JissekiEndNextDayFlg.Checked = false;
                JissekiKiji.Text = "";
                jissekiNinshoMsg.Text = "";
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
        /// 作業日誌を初期化します。
        /// </summary>
        private void InitSagyoNisshiForm()
        {
            try
            {
                logger.Debug(LOG_START);
                for (int i = 0; i < Enum.GetNames(typeof(SagyoDropDownList)).Length; i++)
                {
                    BindSagyoNisshiData(null, (SagyoDropDownList)i);
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
        /// 作業日誌のプロジェクトコード欄を初期化します。
        /// </summary>
        /// <param name="pjcd"></param>
        private void InitSagyoNisshiPJCDDropDownList(ref DropDownList pjcd)
        {
            try
            {
                logger.Debug(LOG_START);
                pjcd.DataSource = kojinSettingPJCD;
                pjcd.DataTextField = "Value";
                pjcd.DataValueField = "Key";
                pjcd.DataBind();
                pjcd.Items.FindByValue("").Selected = true;
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 作業日誌の作業コード欄を初期化します。
        /// </summary>
        /// <param name="sagyocd"></param>
        private void InitSagyoNisshiSagyoCDDropDownList(ref DropDownList sagyocd)
        {
            try
            {
                logger.Debug(LOG_START);
                sagyocd.DataSource = kojinSettingSagyoCD;
                sagyocd.DataTextField = "Value";
                sagyocd.DataValueField = "Key";
                sagyocd.DataBind();
                sagyocd.Items.FindByValue("").Selected = true;
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }
        #endregion

        #region 値を画面にバインドする処理
        /// <summary>
        /// 認証コードマスターをバインドします。
        /// </summary>
        private void BindNinshoCodeMaster()
        {
            try
            {
                logger.Debug(LOG_START);
                JissekiNinshoCd.DataSource = db.GetKinmuNinsyoCDMasterALL().ToDictionary(_ => _.NINSHO_CD, _ => _.NINSHO_CD + ":" + _.NINSHO_NM);
                JissekiNinshoCd.DataTextField = "Value";
                JissekiNinshoCd.DataValueField = "Key";
                JissekiNinshoCd.DataBind();
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 勤務実績をバインドします。
        /// </summary>
        /// <param name="record"></param>
        private void BindKinmuRecordRow(KinmuRecordRow record)
        {
            try
            {
                logger.Debug(LOG_START);
                BindKinmuYoteiData(record);
                if (record.KinmuYotei.KAKN_FLG == "1" && record.KinmuJisseki.KAKN_FLG != "1" && record.KinmuYotei.YOTEI_CD != record.KinmuJisseki.NINKA_CD)
                {
                    JissekiNinshoCd.SelectedValue = record.KinmuYotei.YOTEI_CD;
                    ChangeedJissekiNinshoCd();
                }
                else
                {
                    BindKinmuJissekiData(record);
                    BindSagyoNisshiData(record);
                }
                ChangedTimeValue();
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 勤務予定データをバインドします。
        /// </summary>
        /// <param name="record">勤務情報</param>
        private void BindKinmuYoteiData(KinmuRecordRow record)
        {
            try
            {
                logger.Debug(LOG_START);
                // 勤務予定時間のバインド
                BindTimeRangeTextForm(record.KinmuYotei.GetWorkTimeRange(), TimeRangeBox.予定);

                // 列挙型で認証コードを定義したので、それを使う
                NinsyoCD yoteicd = NinsyoCD.出勤;
                if (!string.IsNullOrWhiteSpace(record.KinmuYotei.YOTEI_CD))
                {
                    yoteicd = (NinsyoCD)int.Parse(record.KinmuYotei.YOTEI_CD);
                }
                else if (!string.IsNullOrWhiteSpace(record.CalendarMaster.NINYO_CD))
                {
                    yoteicd = (NinsyoCD)int.Parse(record.CalendarMaster.NINYO_CD);
                }

                // 翌日フラグの設定
                YoteiEndNextDayFlg.Checked = record.KinmuYotei.END_Y_PAR == "1";

                // 勤務認証ラジオボタンの設定
                if (YoteiNinshoCd.SelectedItem != null) YoteiNinshoCd.SelectedItem.Selected = false;
                switch (yoteicd)
                {
                    case NinsyoCD.出勤:
                    case NinsyoCD.年休:
                    case NinsyoCD.AM半休:
                    case NinsyoCD.PM半休:
                        YoteiNinshoCd.Items.FindByValue(yoteicd.ToZeroPaddingString()).Selected = true;
                        break;
                    default:
                        YoteiNinshoCd.Items.FindByValue(NinsyoCD.出勤.ToZeroPaddingString()).Selected = true;
                        break;
                }

                // 状態の変更
                ChangeYoteiControlsEnabled(yoteicd);
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 勤務実績データをバインドします。
        /// </summary>
        /// <param name="record">勤務情報</param>
        private void BindKinmuJissekiData(KinmuRecordRow record)
        {
            try
            {
                logger.Debug(LOG_START);
                //----------------------------------------------------------------------------
                // 認証コードの設定
                //----------------------------------------------------------------------------
                string kakuteiCD = NinsyoCD.出勤.ToZeroPaddingString();
                // 予定入力済みかつ実績が未入力かつ予定が出勤以外
                if (!string.IsNullOrWhiteSpace(record.KinmuJisseki.NINKA_CD))
                {
                    // 実績が登録されているパターン
                    kakuteiCD = record.KinmuJisseki.NINKA_CD;
                }

                // 確定コードによってドロップダウンリストの値を変える
                JissekiNinshoCd.Items.FindByValue(kakuteiCD).Selected = true;

                //----------------------------------------------------------------------------
                // 時間帯コードの設定
                //----------------------------------------------------------------------------
                TimeRange work = record.KinmuJisseki.GetWorkTimeRange();
                TimeRange rest1 = record.KinmuJisseki.GetRest1TimeRange();
                TimeRange rest2 = record.KinmuJisseki.GetRest2TimeRange();
                TimeRange rest3 = record.KinmuJisseki.GetRest3TimeRange();

                // 未確認かつ出勤設定の場合はrecord.jissekiに個人設定をオーバーライド
                if (record.KinmuJisseki.KAKN_FLG != "1" && JissekiNinshoCd.Items.FindByValue(NinsyoCD.出勤.ToZeroPaddingString()).Selected)
                {
                    work = kojinSettingKinmuJisseki.GetWorkTimeRange();
                    rest1 = kojinSettingKinmuJisseki.GetRest1TimeRange();
                    rest2 = kojinSettingKinmuJisseki.GetRest2TimeRange();
                    rest3 = kojinSettingKinmuJisseki.GetRest3TimeRange();
                }

                // 時間帯のセット
                BindTimeRangeTextForm(work, TimeRangeBox.実績);
                BindTimeRangeTextForm(rest1, TimeRangeBox.休憩1);
                BindTimeRangeTextForm(rest2, TimeRangeBox.休憩2);
                BindTimeRangeTextForm(rest3, TimeRangeBox.休憩3);

                //----------------------------------------------------------------------------
                // その他フォームの設定
                //----------------------------------------------------------------------------
                // 翌日フラグ
                JissekiEndNextDayFlg.Checked = record.KinmuJisseki.END_PAR == "1";

                // 記事欄
                JissekiKiji.Text = record.KinmuJisseki.DKIJI ?? "";
                jissekiKijiMsg.Text = string.Format("{0:d}/20", JissekiKiji.Text.Length);

                // フォーム状態変更
                KNS_M04 jisskicd = db.GetKinmuNinsyoCDMasterALL().SingleOrDefault(_ => _.NINSHO_CD == kakuteiCD);
                CalcPattern jissekiPattern = (CalcPattern)int.Parse(jisskicd.CALC_PATTERN);
                ChangeJissekiControlsIsEnabled(jissekiPattern);
                BindJissekiNinshoMsg(jissekiPattern);
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 作業日誌をバインドします。
        /// </summary>
        /// <param name="record">勤務情報</param>
        private void BindSagyoNisshiData(KinmuRecordRow record)
        {
            try
            {
                logger.Debug(LOG_START);
                if (record.KinmuJisseki.KAKN_FLG != "1" && record.CalendarMaster.NINYO_CD == NinsyoCD.出勤.ToZeroPaddingString())
                {
                    // 未登録かつカレンダーマスターで出勤指定なら個人設定を読み込んでデフォルト値をバインド
                    KNS_D02 sagyo = new KNS_D02()
                    {
                        DATA_Y = viewDateTime.ToString("yyyy"),
                        DATA_M = viewDateTime.ToString("MM"),
                        DATA_D = viewDateTime.ToString("dd"),
                        PROJ_CD = kojinSettingKinmuJisseki.PROJ_CD,
                        SAGYO_CD = kojinSettingKinmuJisseki.SAGYO_CD,
                        SAGYO_MIN = kojinSettingKinmuJisseki.GetWorkingTime(),
                        SHAIN_CD = viewShainInfo.SHAIN_CD
                    };
                    BindSagyoNisshiData(sagyo, SagyoDropDownList.Sagyo1);
                }
                else
                {
                    // 登録されているデータをすべてバインドする
                    for (int i = 0; i < record.SagyoNisshi.Count; i++)
                    {
                        KNS_D02 sagyo = record.SagyoNisshi[i];
                        BindSagyoNisshiData(sagyo, (SagyoDropDownList)i);
                    }
                }
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 作業日誌をバインドします。
        /// </summary>
        /// <param name="sagyo">作業日誌データ</param>
        /// <param name="no">バインドしたいドロップダウンリスト番号</param>
        private void BindSagyoNisshiData(KNS_D02 sagyo, SagyoDropDownList no)
        {
            try
            {
                logger.Debug(LOG_START);
                string pre = "Jisseki";
                string nostr = no.ToString("d");

                // Controlを検索
                ContentPlaceHolder cph = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolderBody");
                DropDownList pjCD = (DropDownList)cph.FindControl(pre + "ProjCd_" + nostr);
                DropDownList workCD = (DropDownList)cph.FindControl(pre + "WorkCd_" + nostr);
                TextBox hour = (TextBox)cph.FindControl(pre + "WorkHour_" + nostr);
                TextBox min = (TextBox)cph.FindControl(pre + "WorkMinute_" + nostr);

                // Controlが未取得なら戻す
                if (pjCD == null || workCD == null || hour == null || min == null) return;

                // データバインド
                BindSagyoNisshiData(sagyo, ref pjCD, ref workCD, ref hour, ref min);
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 作業日誌をバインドします。
        /// </summary>
        /// <param name="sagyo">作業日誌データ</param>
        /// <param name="pjcd">PJCDドロップダウンリスト</param>
        /// <param name="work">作業CDドロップダウンリスト</param>
        /// <param name="hr">作業時間テキストボックス</param>
        /// <param name="min">作業分テキストボックス</param>
        private void BindSagyoNisshiData(KNS_D02 sagyo, ref DropDownList pjcd, ref DropDownList work, ref TextBox hr, ref TextBox min)
        {
            try
            {
                logger.Debug(LOG_START);

                // 初期化
                InitSagyoNisshiPJCDDropDownList(ref pjcd);
                InitSagyoNisshiSagyoCDDropDownList(ref work);
                hr.Text = "";
                min.Text = "";

                // 作業日誌データがない場合は返す
                if (sagyo == null) return;

                // PJコードを拾ってセレクトする
                if (kojinSettingPJCD.Any(_ => _.Key == sagyo.PROJ_CD))
                {
                    pjcd.Items.FindByValue("").Selected = false;
                    pjcd.Items.FindByValue(sagyo.PROJ_CD).Selected = true;
                }
                else
                {
                    var ex = new KinmuException("PJコードが個人設定に含まれていませんでした。");
                    SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
                }

                // 作業コードを拾ってセレクトする
                if (kojinSettingSagyoCD.Any(_ => _.Key == sagyo.SAGYO_CD))
                {
                    work.Items.FindByValue("").Selected = false;
                    work.Items.FindByValue(sagyo.SAGYO_CD).Selected = true;
                }
                else
                {
                    var ex = new KinmuException("作業コードが個人設定に含まれていませんでした。");
                    SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
                }

                // 作業時間を入れる
                int workTimeMin = sagyo.SAGYO_MIN % 60;
                int workTimeHr = sagyo.SAGYO_MIN / 60;
                hr.Text = workTimeHr.ToString("00");
                min.Text = workTimeMin.ToString("00");
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 引数に指定した<see cref="TimeRange"/>を<see cref="TextBox"/>にバインドします。
        /// </summary>
        /// <param name="tr">入力した時間帯</param>
        /// <param name="beginHour">開始時用の<see cref="TextBox"/>オブジェクト</param>
        /// <param name="beginMin">開始分用の<see cref="TextBox"/>オブジェクト</param>
        /// <param name="lastHour">終了時用の<see cref="TextBox"/>オブジェクト</param>
        /// <param name="lastMin">終了分用の<see cref="TextBox"/>オブジェクト</param>
        private void BindTimeRangeTextForm(TimeRange tr, ref TextBox beginHour, ref TextBox beginMin, ref TextBox lastHour, ref TextBox lastMin)
        {
            try
            {
                logger.Debug(LOG_START);

                // 初期化
                beginHour.Text = "";
                beginMin.Text = "";
                lastHour.Text = "";
                lastMin.Text = "";

                if (tr == null) return;

                // 0埋めしなくてもいいけど、一応ね
                // Entity側で0埋め作業はしてるんですよね
                beginHour.Text = tr.Begin.Hour.ToString("00");
                beginMin.Text = tr.Begin.Minute.ToString("00");
                lastHour.Text = tr.Last.Hour.ToString("00");
                lastMin.Text = tr.Last.Minute.ToString("00");
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// <see cref="TimeRange"/>の値を<see cref="TextBox"/>に反映させます。
        /// </summary>
        /// <param name="tr">反映させる<see cref="TimeRange"/></param>
        /// <param name="pattern">テキストボックスのパターンを指定します。</param>
        private void BindTimeRangeTextForm(TimeRange tr, TimeRangeBox pattern)
        {
            try
            {
                logger.Debug(LOG_START);
                switch (pattern)
                {
                    case TimeRangeBox.予定:
                        BindTimeRangeTextForm(
                            tr,
                            ref YoteiStartHour,
                            ref YoteiStartMinute,
                            ref YoteiEndHour,
                            ref YoteiEndMinute
                        );
                        break;

                    case TimeRangeBox.実績:
                        BindTimeRangeTextForm(
                            tr,
                            ref JissekiStartHour,
                            ref JissekiStartMinute,
                            ref JissekiEndHour,
                            ref JissekiEndMinute
                        );
                        break;

                    case TimeRangeBox.休憩1:
                        BindTimeRangeTextForm(
                            tr,
                            ref JissekiRestStartHour_1,
                            ref JissekiRestStartMinute_1,
                            ref JissekiRestEndHour_1,
                            ref JissekiRestEndMinute_1
                        );
                        break;

                    case TimeRangeBox.休憩2:
                        BindTimeRangeTextForm(
                            tr,
                            ref JissekiRestStartHour_2,
                            ref JissekiRestStartMinute_2,
                            ref JissekiRestEndHour_2,
                            ref JissekiRestEndMinute_2
                        );
                        break;


                    case TimeRangeBox.休憩3:
                        BindTimeRangeTextForm(
                            tr,
                            ref JissekiRestStartHour_3,
                            ref JissekiRestStartMinute_3,
                            ref JissekiRestEndHour_3,
                            ref JissekiRestEndMinute_3
                        );
                        break;
                }
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 認証コードに対応するヒントメッセージをバインドします。
        /// </summary>
        /// <param name="pattern"></param>
        private void BindJissekiNinshoMsg(CalcPattern pattern)
        {
            try
            {
                logger.Debug(LOG_START);
                switch (pattern)
                {
                    case CalcPattern.年休:
                    case CalcPattern.無給入力不可:
                        jissekiNinshoMsg.Text = "入力できる項目はありません。このまま登録してください。";
                        break;
                    case CalcPattern.AM半休:
                        int start = SYOTEI_ROUDO_END - SYOTEI_ROUDO_MIN + HANKYU_HUYO_TIME;
                        jissekiNinshoMsg.Text = "始業は" + MinutesToStringFormat(start) + "で入力してください。";
                        break;
                    case CalcPattern.PM半休:
                        int end = SYOTEI_ROUDO_END - HANKYU_HUYO_TIME;
                        jissekiNinshoMsg.Text = "終業は" + MinutesToStringFormat(end) + "で入力してください。";
                        break;
                    case CalcPattern.有給記事のみ可:
                    case CalcPattern.無給記事のみ:
                        jissekiNinshoMsg.Text = "記事欄のみ入力できます。";
                        break;
                    case CalcPattern.時間有給:
                        jissekiNinshoMsg.Text = "コアタイムの" + MinutesToStringFormat(CORE_TIME_BEGIN) + "～" + MinutesToStringFormat(CORE_TIME_END) + "の中での欠勤時間はみなし時間に計上されます。それ以外で勤務時間が発生した場合、その時間を入力してください。";
                        break;
                    default:
                        jissekiNinshoMsg.Text = "";
                        break;
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
        /// プロパティに設定されている時間を時間ラベルにバインドします。
        /// </summary>
        private void BindTimeLabel()
        {
            try
            {
                logger.Debug(LOG_START);
                // 予定勤務時間
                yoteiKousokuTimeMsg.CssClass = yoteiTimeMin < 0 ? "validaterError" : "validaterInfo";
                yoteiKousokuTimeMsg.Text = MinutesToStringFormat(yoteiTimeMin, "{0:00}:{1:00}");

                // 実勤務時間
                sagyoTime.CssClass = sagyoTimeMin < 0 ? "validaterError" : "validaterInfo";
                sagyoTime.Text = MinutesToStringFormat(sagyoTimeMin, "{0:00}:{1:00}");

                // 未入力時間
                nisshiSagyoTimeRemain.CssClass = nisshiSagyoTimeRemainMin < 0 ? "validaterError" : "validaterInfo";
                nisshiSagyoTimeRemain.Text = MinutesToStringFormat(nisshiSagyoTimeRemainMin, "{0:00}:{1:00}");

                // 拘束時間
                jissekiKousokuTimeMsg.CssClass = kousokuTimeMin < 0 ? "validaterError" : "validaterInfo";
                jissekiKousokuTimeMsg.Text = MinutesToStringFormat(kousokuTimeMin, "{0:00}:{1:00}");

                // 休憩時間
                jissekiRestTimeTotalMsg.CssClass = totalKyukeiTimeMin < 0 ? "validaterError" : "validaterInfo";
                jissekiRestTimeTotalMsg.Text = MinutesToStringFormat(totalKyukeiTimeMin, "{0:00}:{1:00}");
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }
        #endregion

        #region フォーム状態の変更処理
        /// <summary>
        /// 勤務予定のControlの有効無効を設定します。
        /// </summary>
        /// <param name="isEnable">有効にするならtrueを設定する</param>
        private void ChangeYoteiControlsEnabled(bool isEnable)
        {
            try
            {
                logger.Debug(LOG_START);
                YoteiEndNextDayFlg.Enabled = isEnable;
                YoteiStartHour.Enabled = isEnable;
                YoteiStartMinute.Enabled = isEnable;
                YoteiEndHour.Enabled = isEnable;
                YoteiEndMinute.Enabled = isEnable;
                YoteiUpdateButton.Enabled = isEnable;
                YoteiNinshoCd.Enabled = isEnable;

                if (record.CalendarMaster.NINYO_CD == NinsyoCD.特休.ToZeroPaddingString() || record.CalendarMaster.NINYO_CD == NinsyoCD.公休.ToZeroPaddingString())
                {
                    YoteiEndNextDayFlg.Enabled = false;
                    YoteiStartHour.Enabled = false;
                    YoteiStartMinute.Enabled = false;
                    YoteiEndHour.Enabled = false;
                    YoteiEndMinute.Enabled = false;
                    YoteiUpdateButton.Enabled = false;
                    YoteiNinshoCd.Enabled = false;
                }
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        private void ChangeYoteiControlsEnabled(NinsyoCD ninsyoCD)
        {
            try
            {
                logger.Debug(LOG_START);
                ChangeYoteiControlsEnabled(dataChangeIsEnabled);
                switch (ninsyoCD)
                {
                    case NinsyoCD.年休:
                        YoteiEndNextDayFlg.Enabled = false;
                        YoteiStartHour.Enabled = false;
                        YoteiStartMinute.Enabled = false;
                        YoteiEndHour.Enabled = false;
                        YoteiEndMinute.Enabled = false;
                        break;
                }
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 勤務実績のControlの有効無効を設定します。
        /// </summary>
        /// <param name="isEnabale">有効にするならtrueを設定する</param>
        private void ChangeJissekiControlsIsEnabled(bool isEnabale)
        {
            try
            {
                logger.Debug(LOG_START);
                foreach (var control in JissekiPanel.Controls)
                {
                    if (control.GetType() == typeof(TextBox))
                    {
                        ((TextBox)control).Enabled = isEnabale;
                    }
                    else if (control.GetType() == typeof(Button))
                    {
                        ((Button)control).Enabled = isEnabale;
                    }
                    else if (control.GetType() == typeof(DropDownList))
                    {
                        ((DropDownList)control).Enabled = isEnabale;
                    }
                }
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 勤務実績のControlの有効無効を認証コードの計算パターンに基づき設定します。
        /// </summary>
        /// <param name="pattern">勤務実績の認証コードの計算パターン</param>
        private void ChangeJissekiControlsIsEnabled(CalcPattern pattern)
        {
            try
            {
                logger.Debug(LOG_START);
                ChangeJissekiControlsIsEnabled(dataChangeIsEnabled);
                switch (pattern)
                {
                    case CalcPattern.年休:
                    case CalcPattern.無給入力不可:
                        ChangeJissekiControlsIsEnabled(false);
                        JissekiNinshoCd.Enabled = dataChangeIsEnabled;
                        JissekiUpdateButton.Enabled = dataChangeIsEnabled;
                        break;
                    case CalcPattern.有給記事のみ可:
                    case CalcPattern.無給記事のみ:
                        ChangeJissekiControlsIsEnabled(false);
                        JissekiNinshoCd.Enabled = dataChangeIsEnabled;
                        JissekiUpdateButton.Enabled = dataChangeIsEnabled;
                        JissekiKiji.Enabled = dataChangeIsEnabled;
                        break;
                    default:
                        break;
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

        #region イベントがトリガーとなる処理
        /// <summary>
        /// 実績認証コードが変更された場合に実行する処理
        /// </summary>
        private void ChangeedYoteiNinshoCd()
        {
            try
            {
                logger.Debug(LOG_START);
                // 選択された認証コードを退避
                var master = db.GetKinmuNinsyoCDMasterALL();
                KNS_M04 selectedCD = master.SingleOrDefault(_ => _.NINSHO_CD == YoteiNinshoCd.SelectedValue);
                NinsyoCD ninsyoCD = (NinsyoCD)int.Parse(selectedCD.NINSHO_CD);

                // 予定の認証コード
                KNS_M04 yotei = master.SingleOrDefault(_ => _.NINSHO_CD == record.KinmuYotei.YOTEI_CD);
                NinsyoCD yoteicd = NinsyoCD.出勤;
                if (yotei != null) yoteicd = (NinsyoCD)int.Parse(yotei.NINSHO_CD);

                // 予定の認証コードと異なるか
                bool isChanged = ninsyoCD != yoteicd;

                // 予定の確認フラグ
                bool isYoteiChecked = record.KinmuYotei.KAKN_FLG == "1";

                // 予定フォーム初期化
                InitKinmuYoteiForm();
                YoteiNinshoCd.SelectedValue = ninsyoCD.ToZeroPaddingString();

                // 計算パターンによってフォームの値を変える
                switch (ninsyoCD)
                {
                    case NinsyoCD.出勤:
                        record.KinmuYotei.YOTEI_CD = selectedCD.NINSHO_CD;
                        BindKinmuYoteiData(record);
                        break;
                    case NinsyoCD.AM半休:
                        int start = SYOTEI_ROUDO_END - SYOTEI_ROUDO_MIN + HANKYU_HUYO_TIME;
                        if (isYoteiChecked && !isChanged)
                        {
                            BindKinmuYoteiData(record);
                        }
                        else
                        {
                            BindTimeRangeTextForm(new TimeRange(start, SYOTEI_ROUDO_END), TimeRangeBox.予定);
                        }
                        break;
                    case NinsyoCD.PM半休:
                        int end = SYOTEI_ROUDO_END - HANKYU_HUYO_TIME;
                        if (isYoteiChecked && !isChanged)
                        {
                            BindKinmuYoteiData(record);
                        }
                        else
                        {
                            BindTimeRangeTextForm(new TimeRange(SYOTEI_ROUDO_BEGIN, end), TimeRangeBox.予定);
                        }
                        break;
                    case NinsyoCD.年休:
                        break;
                }

                // 予定によってControlの有効・無効を切り替える
                ChangeYoteiControlsEnabled(ninsyoCD);

                ChangedTimeValue();
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
        /// 実績認証コードが変更された場合に実行する処理
        /// </summary>
#pragma warning disable S3776
        private void ChangeedJissekiNinshoCd()
#pragma warning restore S3776
        {
            try
            {
                logger.Debug(LOG_START);
                // 選択された認証コードを退避
                var master = db.GetKinmuNinsyoCDMasterALL();
                KNS_M04 selectedCD = master.SingleOrDefault(_ => _.NINSHO_CD == JissekiNinshoCd.SelectedValue);
                CalcPattern pattern = (CalcPattern)int.Parse(selectedCD.CALC_PATTERN);

                // 実績の計算パターン
                KNS_M04 jisskicd = master.SingleOrDefault(_ => _.NINSHO_CD == record.KinmuJisseki.NINKA_CD);
                CalcPattern jissekiPattern = CalcPattern.出勤;
                if (jisskicd != null) jissekiPattern = (CalcPattern)int.Parse(jisskicd.CALC_PATTERN);

                // 実績の計算パターンと異なるか
                bool isChanged = jissekiPattern != pattern;

                // 実績の確認フラグ
                bool isJissekiChecked = record.KinmuJisseki.KAKN_FLG == "1";

                // 実績フォーム初期化
                InitKinmuJissekiForm();
                InitSagyoNisshiForm();

                // 計算パターンによってフォームの値を変える
                switch (pattern)
                {
                    case CalcPattern.出勤:
                        if (!isJissekiChecked) record.KinmuJisseki.DKIJI = "";
                        record.KinmuJisseki.NINKA_CD = selectedCD.NINSHO_CD;
                        BindKinmuJissekiData(record);
                        BindSagyoNisshiData(record);
                        break;
                    case CalcPattern.研修:
                        if (isJissekiChecked && !isChanged)
                        {
                            BindKinmuJissekiData(record);
                        }
                        else
                        {
                            BindTimeRangeTextForm(new TimeRange(9 * 60 + 20, 18 * 60 + 0), TimeRangeBox.実績);
                            BindTimeRangeTextForm(new TimeRange(12 * 60 + 30, 13 * 60 + 30), TimeRangeBox.休憩1);
                            JissekiKiji.Text = "研修";
                        }
                        BindSagyoNisshiData(record);
                        break;
                    case CalcPattern.出張:
                        if (isJissekiChecked && !isChanged)
                        {
                            BindKinmuJissekiData(record);
                        }
                        else
                        {
                            BindTimeRangeTextForm(new TimeRange(SYOTEI_ROUDO_BEGIN, SYOTEI_ROUDO_END), TimeRangeBox.実績);
                            BindTimeRangeTextForm(new TimeRange(REST_TIME_BEGIN, REST_TIME_END), TimeRangeBox.休憩1);
                        }
                        BindSagyoNisshiData(record);
                        break;
                    case CalcPattern.AM半休:
                        int start = SYOTEI_ROUDO_END - SYOTEI_ROUDO_MIN + HANKYU_HUYO_TIME;
                        if (isJissekiChecked && !isChanged)
                        {
                            BindKinmuJissekiData(record);
                        }
                        else
                        {
                            BindTimeRangeTextForm(new TimeRange(start, SYOTEI_ROUDO_END), TimeRangeBox.実績);
                        }
                        BindSagyoNisshiData(record);
                        break;
                    case CalcPattern.PM半休:
                        int end = SYOTEI_ROUDO_END - HANKYU_HUYO_TIME;
                        if (isJissekiChecked && !isChanged)
                        {
                            BindKinmuJissekiData(record);
                        }
                        else
                        {
                            BindTimeRangeTextForm(new TimeRange(SYOTEI_ROUDO_BEGIN, end), TimeRangeBox.実績);
                            BindTimeRangeTextForm(new TimeRange(REST_TIME_BEGIN, REST_TIME_END), TimeRangeBox.休憩1);
                        }
                        BindSagyoNisshiData(record);
                        break;
                    case CalcPattern.有給記事のみ可:
                    case CalcPattern.無給記事のみ:
                    case CalcPattern.時間有給:
                        if (isJissekiChecked && !isChanged)
                        {
                            BindKinmuJissekiData(record);
                        }
                        break;
                    default:
                        break;
                }

                // 認証コードのメッセージを表示
                BindJissekiNinshoMsg(pattern);

                // 実績によってControlの有効・無効を切り替える
                ChangeJissekiControlsIsEnabled(pattern);

                ChangedTimeValue();
                jissekiKijiMsg.Text = string.Format("{0:d}/20", JissekiKiji.Text.Length);
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
        /// 作業日誌フォームの内容をオブジェクトに変換します。
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        private KNS_D02 GetFormSagyoDorpDownListValue(SagyoDropDownList no)
        {
            try
            {
                logger.Debug(LOG_START);
                string pre = "Jisseki";
                string nostr = no.ToString("d");
                ContentPlaceHolder h = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolderBody");
                DropDownList pjcd = (DropDownList)h.FindControl(pre + "ProjCd_" + nostr);
                DropDownList work = (DropDownList)h.FindControl(pre + "WorkCd_" + nostr);
                TextBox hr = (TextBox)h.FindControl(pre + "WorkHour_" + nostr);
                TextBox min = (TextBox)h.FindControl(pre + "WorkMinute_" + nostr);
                int sagyo_min = 0;
                if (pjcd.SelectedItem.Value == "" && work.SelectedItem.Value == "" && hr.Text == "" && min.Text == "") return null;

                try
                {
                    if (hr.Text == "") hr.Text = "00";
                    if (min.Text == "") min.Text = "00";
                    DateTime _s = viewDateTime.Date;
                    DateTime _l = viewDateTime.Date.AddHours(int.Parse(hr.Text)).AddMinutes(int.Parse(min.Text));
                    sagyo_min = new TimeRange(_s, _l).GetRangeMinutes();
                }
                catch (FormatException ex)
                {
                    throw new KinmuException("作業時間に数値以外の値は入力できません。" + ((int)no + 1) + "番目の作業内容を見直してください", ex);
                }

                KNS_D02 a = new KNS_D02
                {
                    DATA_Y = viewDateTime.ToString("yyyy"),
                    DATA_M = viewDateTime.ToString("MM"),
                    DATA_D = viewDateTime.ToString("dd"),
                    PROJ_CD = pjcd.SelectedItem.Value,
                    SAGYO_CD = work.SelectedItem.Value,
                    SAGYO_MIN = sagyo_min,
                    SHAIN_CD = viewShainInfo.SHAIN_CD
                };

                return a;
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
        /// <see cref="TextBox"/>から<see cref="TimeRange"/>を取得します
        /// </summary>
        /// <param name="beginHour">開始時のテキストボックス</param>
        /// <param name="beginMin">開始分のテキストボックス</param>
        /// <param name="lastHour">終了時のテキストボックス</param>
        /// <param name="lastMin">終了分のテキストボックス</param>
        /// <param name="nextDay">翌日フラグ</param>
        /// <returns></returns>
        private TimeRange GetTimeRangeTextForm(ref TextBox beginHour, ref TextBox beginMin, ref TextBox lastHour, ref TextBox lastMin, bool nextDay)
        {
            try
            {
                logger.Debug(LOG_START);
                DateTime _b = viewDateTime.Date.AddHours(int.Parse(beginHour.Text)).AddMinutes(int.Parse(beginMin.Text));
                DateTime _l = viewDateTime.Date.AddHours(int.Parse(lastHour.Text)).AddMinutes(int.Parse(lastMin.Text));
                if (nextDay && _l < _b) _l = _l.AddDays(1);
                return new TimeRange(_b, _l);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 画面に表示される時刻を再計算します
        /// </summary>
        private void ChangedTimeValue()
        {
            try
            {
                logger.Debug(LOG_START);
                // 初期化
                int sumSagyoMin = 0;
                totalKyukeiTimeMin = 0;
                yoteiTimeMin = 0;

                // 作業日誌から作業時間を取得し計算
                foreach (SagyoDropDownList item in Enum.GetValues(typeof(SagyoDropDownList)))
                {
                    var a = GetFormSagyoDorpDownListValue(item);
                    if (a == null) continue;
                    sumSagyoMin += a.SAGYO_MIN;
                }

                // 各時間帯を取得し計算
                TimeRange Jisseki = GetTimeRangeTextForm(ref JissekiStartHour, ref JissekiStartMinute, ref JissekiEndHour, ref JissekiEndMinute, JissekiEndNextDayFlg.Checked);
                TimeRange Yotei = GetTimeRangeTextForm(ref YoteiStartHour, ref YoteiStartMinute, ref YoteiEndHour, ref YoteiEndMinute, YoteiEndNextDayFlg.Checked);
                TimeRange Rest1 = GetTimeRangeTextForm(ref JissekiRestStartHour_1, ref JissekiRestStartMinute_1, ref JissekiRestEndHour_1, ref JissekiRestEndMinute_1, JissekiEndNextDayFlg.Checked);
                TimeRange Rest2 = GetTimeRangeTextForm(ref JissekiRestStartHour_2, ref JissekiRestStartMinute_2, ref JissekiRestEndHour_2, ref JissekiRestEndMinute_2, JissekiEndNextDayFlg.Checked);
                TimeRange Rest3 = GetTimeRangeTextForm(ref JissekiRestStartHour_3, ref JissekiRestStartMinute_3, ref JissekiRestEndHour_3, ref JissekiRestEndMinute_3, JissekiEndNextDayFlg.Checked);

                // 予定時間を代入
                if (Yotei != null)
                {
                    yoteiTimeMin = Yotei.GetRangeMinutes();
                }

                // 値が未入力の場合は0で代入
                if ((Rest1 != null && Rest1.IsOverlap(Rest2)) ||
                    (Rest2 != null && Rest2.IsOverlap(Rest3)) ||
                    (Rest3 != null && Rest3.IsOverlap(Rest1)) ||
                    Jisseki == null)
                {
                    nisshiSagyoTimeRemainMin = 0;
                    sagyoTimeMin = 0;
                    kousokuTimeMin = 0;
                    totalKyukeiTimeMin = 0;
                    BindTimeLabel();
                    return;
                }

                // 拘束時間を代入
                kousokuTimeMin = Jisseki.GetRangeMinutes();

                // 作業時間を再計算
                if (Rest1 != null) totalKyukeiTimeMin += Rest1.GetRangeMinutes();
                if (Rest2 != null) totalKyukeiTimeMin += Rest2.GetRangeMinutes();
                if (Rest3 != null) totalKyukeiTimeMin += Rest3.GetRangeMinutes();
                sagyoTimeMin = kousokuTimeMin - totalKyukeiTimeMin;
                nisshiSagyoTimeRemainMin = sagyoTimeMin - sumSagyoMin;
                BindTimeLabel();
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
        /// 表示月日を指定した日数分移動し再度データを取得し直します。
        /// </summary>
        /// <param name="days">移動する日数</param>
        private void ChangeViewDateTime(int days)
        {
            try
            {
                logger.Debug(LOG_START);
                viewDateTime = ((DateTime)Session[SESSION_STRING_VIEW_DATETIME]).AddDays(days);
                Session[SESSION_STRING_VIEW_DATETIME] = viewDateTime;
                try
                {
                    // カレンダーマスターがない場合、はviewDateTimeをdays分戻す必要がある。
                    SetKinmuRecordRow(ref record, viewShainInfo.SHAIN_CD, viewDateTime);
                }
                catch (KinmuException ex) when (ex.Message.Contains("カレンダーマスター"))
                {
                    // 勤務実績がない場合もKinmuExceptionが発生するため、Whenで場合分けの必要。
                    SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
                    viewDateTime = ((DateTime)Session[SESSION_STRING_VIEW_DATETIME]).AddDays(-days);
                    Session[SESSION_STRING_VIEW_DATETIME] = viewDateTime;
                }
                SetDataChangeIsEnabled(ref dataChangeIsEnabled);

                // 画面の再設定
                FormInit();
                BindKinmuRecordRow(record);
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
        /// 予定の認証コードが変更された際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnChangedYoteiNinshoCd(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);
                ChangeedYoteiNinshoCd();
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
            }
        }

        /// <summary>
        /// 実績の認証コードが変更された際の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnChangedJissekiNinshoCd(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);
                ChangeedJissekiNinshoCd();
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
            }
        }

        /// <summary>
        /// 前日へ移動します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClickGotoYesterday(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);
                ChangeViewDateTime(-1);
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
            }
        }

        /// <summary>
        /// 翌日へ移動します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClickGotoTomorrow(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);
                ChangeViewDateTime(1);
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
            }
        }

        /// <summary>
        /// 残り残時間を押された時の処理です。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClickNokoriButton(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);
                string id = ((Button)sender).ID.Replace("Nokorizenbu_", "");
                string pre = "Jisseki";
                ContentPlaceHolder h = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolderBody");
                TextBox hr = (TextBox)h.FindControl(pre + "WorkHour_" + id);
                TextBox min = (TextBox)h.FindControl(pre + "WorkMinute_" + id);
                int m;
                try
                {
                    m = int.Parse(hr.Text) * 60 + int.Parse(min.Text);
                }
                catch (Exception)
                {
                    m = 0;
                }

                ChangedTimeValue();
                nisshiSagyoTimeRemainMin += m;
                if (nisshiSagyoTimeRemainMin <= 0) return;
                hr.Text = (nisshiSagyoTimeRemainMin / 60).ToString("00");
                min.Text = (nisshiSagyoTimeRemainMin % 60).ToString("00");
                nisshiSagyoTimeRemainMin = 0;
                BindTimeLabel();
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
            }
        }

        /// <summary>
        /// 時刻が変更された場合に走る処理です。非同期で更新します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnChangedTimeValue(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);
                ChangedTimeValue();
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
            }
        }

        /// <summary>
        /// 予定更新ボタン押下時にデータを更新する処理。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClickYoteiUpdateButton(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);
                string yoteicd = YoteiNinshoCd.SelectedValue;

                KNS_D13 yotei = new KNS_D13()
                {
                    DATA_Y = viewDateTime.ToString("yyyy"),
                    DATA_M = viewDateTime.ToString("MM"),
                    DATA_D = viewDateTime.ToString("dd"),
                    STR_Y_HR = YoteiStartHour.Text,
                    STR_Y_MIN = YoteiStartMinute.Text,
                    END_Y_HR = YoteiEndHour.Text,
                    END_Y_MIN = YoteiEndMinute.Text,
                    END_Y_PAR = YoteiEndNextDayFlg.Checked ? "1" : "0",
                    SHAIN_CD = viewShainInfo.SHAIN_CD,
                    KAKN_FLG = "1",
                    YOTEI_CD = yoteicd,
                    SHONIN_FLG = "0"
                };

                // 予定のDB更新
                if (!KinmuManager.ExecuteUpdate(ref yotei)) throw new KinmuException("勤務予定の更新に失敗しました。");

                SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, "勤務予定を更新しました。", InformationLevel.Highlight);

                // リバインド
                SetKinmuRecordRow(ref record, viewShainInfo.SHAIN_CD, viewDateTime);
                BindKinmuRecordRow(record);
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
            }
        }

        /// <summary>
        /// 実績更新ボタン押下時にデータを更新する処理。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClickJissekiUpdateButton(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);
                List<KNS_D02> sagyoList = new List<KNS_D02>();
                KNS_D01 jisseki = new KNS_D01()
                {
                    DATA_Y = viewDateTime.ToString("yyyy"),
                    DATA_M = viewDateTime.ToString("MM"),
                    DATA_D = viewDateTime.ToString("dd"),
                    DKIJI = JissekiKiji.Text,
                    STR_HR = JissekiStartHour.Text,
                    STR_MIN = JissekiStartMinute.Text,
                    END_HR = JissekiEndHour.Text,
                    END_MIN = JissekiEndMinute.Text,
                    END_PAR = JissekiEndNextDayFlg.Checked ? "1" : "0",
                    NINKA_CD = JissekiNinshoCd.Text,
                    // NINYO_CDはつかわないという強い意志
                    NINYO_CD = "",
                    RESTS1_HR = JissekiRestStartHour_1.Text,
                    RESTS1_MIN = JissekiRestStartMinute_1.Text,
                    RESTE1_HR = JissekiRestEndHour_1.Text,
                    RESTE1_MIN = JissekiRestEndMinute_1.Text,
                    RESTS2_HR = JissekiRestStartHour_2.Text,
                    RESTS2_MIN = JissekiRestStartMinute_2.Text,
                    RESTE2_HR = JissekiRestEndHour_2.Text,
                    RESTE2_MIN = JissekiRestEndMinute_2.Text,
                    RESTS3_HR = JissekiRestStartHour_3.Text,
                    RESTS3_MIN = JissekiRestStartMinute_3.Text,
                    RESTE3_HR = JissekiRestEndHour_3.Text,
                    RESTE3_MIN = JissekiRestEndMinute_3.Text,
                    SHAIN_CD = viewShainInfo.SHAIN_CD,
                    UPD_SHAIN_CD = loginShainInfo.SHAIN_CD
                };

                // 作業日誌をフォームから取得します。
                foreach (SagyoDropDownList item in Enum.GetValues(typeof(SagyoDropDownList)))
                {
                    KNS_D02 a = GetFormSagyoDorpDownListValue(item);
                    if (a == null) continue;
                    a.CheckValidationForForm();
                    sagyoList.Add(a);
                }

                // 重複キーの作業日誌を合算します。
                for (int i = 1; i < sagyoList.Count; i++)
                {
                    var item = sagyoList[i];
                    for (int j = 0; j < i; j++)
                    {
                        if (sagyoList[j].PROJ_CD != item.PROJ_CD || sagyoList[j].SAGYO_CD != item.SAGYO_CD) continue;
                        sagyoList[j].SAGYO_MIN += item.SAGYO_MIN;
                        sagyoList.Remove(item);
                        i--;
                        break;
                    }
                }

                // 作業日誌の作業時間の合計と実勤務時間が一致するかの判定
                KinmuManager.ValidationD01AndD02(ref jisseki, ref sagyoList);

                // 実績のDB更新
                if (!KinmuManager.ExecuteUpdate(ref jisseki)) throw new KinmuException("勤務実績の更新に失敗しました。");

                // 作業日誌のDB更新
                if (sagyoList.Any())
                {
                    KinmuManager.ExecuteUpdate(ref sagyoList);
                }
                else
                {
                    db.DeleteSagyoNisshi(db.GetSagyoNisshi(viewShainInfo.SHAIN_CD, viewDateTime.Year, viewDateTime.Month, viewDateTime.Day));
                }

                SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, "勤務実績を更新しました。", InformationLevel.Highlight);

                // リバインド
                SetKinmuRecordRow(ref record, viewShainInfo.SHAIN_CD, viewDateTime);
                BindKinmuRecordRow(record);
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
            }
        }

        /// <summary>
        /// 記事欄が変更された際に走る処理です。非同期で値を更新します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnTextChangedKiji(object sender, EventArgs e)
        {
            try
            {
                logger.Debug(LOG_START);
                jissekiKijiMsg.Text = string.Format("{0:d}/20", JissekiKiji.Text.Length);
            }
            catch (KinmuException ex)
            {
                SetInformationPanel(ref ContentsBlock, ref ContentsBlockIcon, ref ContentsBlockLabel, ex);
            }
        }
        #endregion

        #region 列挙体
        private enum TimeRangeBox
        {
            予定, 実績, 休憩1, 休憩2, 休憩3
        }

        private enum SagyoDropDownList
        {
            Sagyo1, Sagyo2, Sagyo3, Sagyo4, Sagyo5, Sagyo6, Sagyo7, Sagyo8, Sagyo9, Sagyo10
        }

        /// <summary>
        /// 勤務入力画面で使う用
        /// </summary>
        private enum CalcPattern
        {
            /// <summary>
            /// 出勤
            /// </summary>
            出勤 = 10,
            /// <summary>
            /// 研修
            /// </summary>
            研修 = 11,
            /// <summary>
            /// 出張。赴任、着任も含まれます。
            /// </summary>
            出張 = 12,
            /// <summary>
            /// 年休
            /// </summary>
            年休 = 30,
            /// <summary>
            /// AM半休
            /// </summary>
            AM半休 = 31,
            /// <summary>
            /// PM半休
            /// </summary>
            PM半休 = 32,
            /// <summary>
            /// 記事のみ入力可能なタイプの有給
            /// </summary>
            有給記事のみ可 = 40,
            /// <summary>
            /// 勤務時間を入力可能なタイプの有給
            /// </summary>
            時間有給 = 41,
            /// <summary>
            /// 何も入力できないタイプの無給
            /// </summary>
            無給入力不可 = 50,
            /// <summary>
            /// 記事のみ入力可能なタイプの無給
            /// </summary>
            無給記事のみ = 52
        }
    }
    #endregion
}