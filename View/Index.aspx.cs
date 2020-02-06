using System;
using System.Web.UI;
using static CommonLibrary.CommonDefine;

namespace KinmuSystem.View
{
    public partial class Index : Page
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Debug(LOG_START);

            // デフォルトではLogin.aspxに飛ばす
            string url = "Login.aspx";

            if (!string.IsNullOrWhiteSpace((string)Session[SESSION_STRING_REDIRECT_URL]))
            {
                // リダイレクト先が設定されている場合はURLを上書きする
                url = (string)Session[SESSION_STRING_REDIRECT_URL];
                Session[SESSION_STRING_REDIRECT_URL] = "";
            }

            // リダイレクトする
            Response.Redirect(url, false);
        }
    }
}