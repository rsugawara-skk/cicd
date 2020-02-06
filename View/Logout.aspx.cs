using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Web;
using System.Web.UI;
using static CommonLibrary.CommonDefine;

namespace KinmuSystem.View
{
    public partial class Logout : Page
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);
            Signout();
        }

        private void Signout()
        {
            logger.Debug(LOG_START);
            // ユーザ情報入りのセッションをクリア
            Session.Clear();
            // MSサインアウト画面の表示
            HttpContext.Current.GetOwinContext().Authentication.SignOut(
                OpenIdConnectAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
        }
    }
}