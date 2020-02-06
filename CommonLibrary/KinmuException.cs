using System;
using NLog;

namespace CommonLibrary
{
    /// <summary>
    /// 業務ロジックで発生したエラーです。
    /// </summary>
    public class KinmuException : Exception
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static DateTime __startdate;
        private static int __errorCount;

        /// <summary>
        /// このインスタンスのエラーシリアルナンバーです
        /// </summary>
        public string Serial { get; }

        /// <summary>
        /// 業務ロジック例外エラーです。
        /// </summary>
        public KinmuException() : base() { }

        /// <summary>
        /// 業務ロジック例外エラーです。
        /// </summary>
        /// <param name="_message">エラーメッセージ</param>
        /// <param name="_innerException">包含するException</param>
        public KinmuException(string _message, Exception _innerException) : base(_message, _innerException)
        {
            Serial = "###" + ErrorSerial + "###";
            logger.Error(Serial + " " + _message);
            logger.Error(Environment.NewLine + _innerException.StackTrace);
        }

        /// <summary>
        /// 業務ロジック例外エラーです。
        /// </summary>
        /// <param name="_message">エラーメッセージ</param>
        public KinmuException(string _message) : base(_message)
        {
            Serial = "###" + ErrorSerial + "###";
            logger.Error(Serial + " " + _message);
        }

        private string ErrorSerial
        {
            get { return StartDate.ToString("yyyyMMdd") + "-" + ErrorCount.ToString("00000000"); }
        }

        private static DateTime StartDate
        {
            get
            {
                if (__startdate.Date != DateTime.Now.Date)
                {
                    __startdate = DateTime.Now.Date;
                    __errorCount = 0;
                }
                return __startdate;
            }
        }

        private static int ErrorCount
        {
            get
            {
                __errorCount++;
                return __errorCount;
            }
        }
    }
}
