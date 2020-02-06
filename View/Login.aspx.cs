using CommonLibrary.Models;
using System;
using System.Security.Claims;
using System.Web.UI;
using UserLibrary;
using static CommonLibrary.CommonDefine;

namespace KinmuSystem.View
{
    public partial class Login : Page
    {
        // ログ出力用
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);

#if !DEBUG
            // ログイン済みでない場合は何もしない
            // (MasterページからMSの認証画面を呼び出す)
            if (!Request.IsAuthenticated)
            {
                KinmuSystem.SignIn();
            }
#endif
            // ユーザマネージャのインスタンスを生成
            UserManager userManager = new UserManager();
#if DEBUG
            string userName = "rokamoto";
            if (Request.QueryString["user"] != null)
            {
                userName = Request.QueryString["user"];
            }
#else
            // ユーザ情報の取得
            ClaimsIdentity userClaims = User.Identity as ClaimsIdentity;

            // ADに登録されたユーザ名（メールアドレス）を取得(alias@j-skk.com)
            string userName = userClaims?.FindFirst(System.IdentityModel.Claims.ClaimTypes.Name)?.Value;
#endif

            // aliasをもとに社員CDをDBから取得する
            KNS_M01 shainInfo = userManager.GetShainCDInfoByAlias(userName);

            // AD認証はできるがDBにユーザが登録されていない場合
            if (shainInfo == null)
            {
                resultLabel.Text = "ログインは成功しましたが、DBにユーザ情報が登録されていません。";
                return;
            }

            // 最終ログイン時間の更新
            Session[SESSION_STRING_LAST_LOGIN_TIME] = userManager.GetLastLogin(shainInfo.SHAIN_CD);
            int updateNum = userManager.SetLastLoginAtNow(shainInfo.SHAIN_CD);
            if (updateNum < 1)
            {
                resultLabel.Text = "ログインは成功しましたが、対象ユーザの最終ログイン時間が更新できませんでした。";
                return;
            }

            // ユーザ名（メールアドレス）から社員コードを取得
            Session[SESSION_STRING_LOGIN_SHAIN_INFO] = shainInfo;
            Session[SESSION_STRING_VIEW_SHAIN_INFO] = shainInfo;
            Session[SESSION_STRING_VIEW_DATETIME] = DateTime.Now;

            // 最終アクセスURLへ飛ばす
            string url = "List.aspx";
            if (Session[SESSION_STRING_REDIRECT_URL] != null)
            {
                url = (string)Session[SESSION_STRING_REDIRECT_URL];
                Session[SESSION_STRING_REDIRECT_URL] = null;
            }
            logger.Debug("リダイレクト先：" + url);
            Response.Redirect(url);
        }

        /// <summary>
        /// ログアウトボタン押下時に、セッションをクリアしMSサインアウト画面の表示
        /// </summary>
        protected void OnClickLogoutButton(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            // ログアウトページに遷移する
            Response.Redirect("Logout.aspx");
        }
    }
}