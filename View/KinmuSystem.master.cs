using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Web.UI;
using System;
using System.Web;
using static CommonLibrary.CommonDefine;
using CommonLibrary.Models;
using System.Web.SessionState;

namespace KinmuSystem.View
{
    public partial class KinmuSystem : MasterPage
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ページ読み込み時処理（最終ログイン日時の設定）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);

            if (string.IsNullOrWhiteSpace((string)Session[SESSION_STRING_LAST_LOGIN_TIME]))
            {
                LatestLogin.Text = "";
            }
            else
            {
                LatestLogin.Text = "最終ログイン日時：" + (string)Session[SESSION_STRING_LAST_LOGIN_TIME];
            }
        }

        /// <summary>
        /// MSサインイン画面の表示
        /// </summary>
        public static void SignIn()
        {
            logger.Debug(LOG_START);
            // MicroSoftのログインチャレンジを表示
            // ログイン成功時にログイン画面へ遷移（ユーザ情報取得後自動でList.aspxにリダイレクト）
            HttpContext.Current.GetOwinContext().Authentication.Challenge(
                new AuthenticationProperties { RedirectUri = "/" },
                OpenIdConnectAuthenticationDefaults.AuthenticationType);
        }

        public void SetSessionData(ref KNS_M01 loginShainInfo, ref KNS_M01 viewShainInfo, ref DateTime viewDateTime)
        {
            logger.Debug(LOG_START);
            SetSessionData(ref loginShainInfo, ref viewShainInfo, ref viewDateTime, Response, Request, Session);
        }

        public static void SetSessionData(ref KNS_M01 loginShainInfo, ref KNS_M01 viewShainInfo, ref DateTime viewDateTime, HttpResponse Response, HttpRequest Request, HttpSessionState Session)
        {
            logger.Debug(LOG_START);
#if !DEBUG
            // ログイン済みでない場合は処理はせず、MasterPageからログイン情報を取得する
            if (!Request.IsAuthenticated)
            {
                SignIn();
            }
#endif
            loginShainInfo = (KNS_M01)Session[SESSION_STRING_LOGIN_SHAIN_INFO];
            viewShainInfo = (KNS_M01)Session[SESSION_STRING_VIEW_SHAIN_INFO];
            viewDateTime = (DateTime?)Session[SESSION_STRING_VIEW_DATETIME] ?? DateTime.Now;

            // セッションから社員コードを取得できない場合
            // ログイン画面に遷移させ、各種ユーザ情報を取得する
            if (loginShainInfo == null)
            {
                logger.Debug("ログイン社員セッション情報が取得できませんでした。");
                Session[SESSION_STRING_REDIRECT_URL] = Request.RawUrl;
                Response.Redirect("Login.aspx");
            }
        }
    }
}