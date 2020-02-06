using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using static CommonLibrary.CommonDefine;

namespace CommonLibrary
{
    /// <summary>
    /// 全機能共通して使うユーティリティを記載しています。
    /// </summary>
    public static class Util
    {
        // ログ出力用
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 分を整形文字列に変換します
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string MinutesToStringFormat(int value, string format)
        {
            logger.Debug(LOG_START);
            string _format = value < 0 ? "-" + format : format;
            int hour = Math.Abs(value) / 60;
            int min = Math.Abs(value) % 60;
            try
            {
                return string.Format(_format, hour, min);
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        /// <summary>
        /// 分を整形文字列（0:00）に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string MinutesToStringFormat(int value)
        {
            logger.Debug(LOG_START);
            return MinutesToStringFormat(value, "{0}:{1:00}");
        }

        /// <summary>
        /// 情報パネルへメッセージ及びレベルに応じたCSSを適用します。
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="icon"></param>
        /// <param name="label"></param>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public static void SetInformationPanel(ref Panel panel, ref Label icon, ref Label label, string message, InformationLevel level)
        {
            logger.Debug(LOG_START);
            string IconCssClass = "ui-icon floatLeft ";
            string PanelCssClass = "ui-corner-all ";

            switch (level)
            {
                case InformationLevel.None:
                    // 表示させない
                    panel.Visible = false;
                    break;

                case InformationLevel.Highlight:
                    // 情報パネル
                    label.Text = message;
                    icon.CssClass = IconCssClass + "ui-icon-info";
                    panel.CssClass = PanelCssClass + "ui-state-highlight";
                    panel.Visible = true;
                    break;

                case InformationLevel.Error:
                    // エラーパネル
                    label.Text = message;
                    icon.CssClass = IconCssClass + "ui-icon-alert";
                    panel.CssClass = PanelCssClass + "ui-state-error";
                    panel.Visible = true;
                    break;

                default:
                    // デフォルトは非表示
                    panel.Visible = false;
                    break;
            }
        }

        /// <summary>
        /// エラー情報表示を行います。
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="icon"></param>
        /// <param name="label"></param>
        /// <param name="exception"></param>
        public static void SetInformationPanel(ref Panel panel, ref Label icon, ref Label label, KinmuException exception)
        {
            logger.Debug(LOG_START);
            logger.Error(Environment.NewLine + exception.StackTrace);
            SetInformationPanel(ref panel, ref icon, ref label, exception.Message + " " + exception.Serial, InformationLevel.Error);
        }

        /// <summary>
        /// 情報パネルを初期化します。
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="icon"></param>
        /// <param name="label"></param>
        public static void InitInformationPanel(ref Panel panel, ref Label icon, ref Label label)
        {
            logger.Debug(LOG_START);
            SetInformationPanel(ref panel, ref icon, ref label, string.Empty, InformationLevel.None);
        }

        /// <summary>
        /// エラーや操作に対してのメッセージ出力のLevelです。
        /// </summary>
        public enum InformationLevel
        {
            /// <summary>
            /// 非表示
            /// </summary>
            None,
            /// <summary>
            /// 情報（主に成功メッセージ）
            /// </summary>
            Highlight,
            /// <summary>
            /// エラー（主にExceptionメッセージ）
            /// </summary>
            Error
        }
    }
}
