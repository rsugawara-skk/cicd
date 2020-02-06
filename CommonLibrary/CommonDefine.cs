using System.Collections.Generic;

namespace CommonLibrary
{
    /// <summary>
    /// 共通定義ライブラリ
    /// </summary>
    public static class CommonDefine
    {
        /// <summary>
        /// SKK労働日数に含める計算パターンの一覧
        /// </summary>
        public static List<string> CALC_PATTERN_SKK_ROUDO_NISSU => new List<string>() { "10", "11", "12", "30", "31", "32", "40", "41", "42" };

        /// <summary>
        /// 有給休暇に該当する計算パターンの一覧
        /// </summary>
        public static List<string> CALC_PATTERN_YUKYU => new List<string>() { "40", "41", "42" };

        /// <summary>
        /// 年休に該当する計算パターンの一覧
        /// </summary>
        public static List<string> CALC_PATTERN_NENKYU => new List<string>() { "30", "31", "32" };

        /// <summary>
        /// 無給休暇に該当する計算パターンの一覧
        /// </summary>
        public static List<string> CALC_PATTERN_MUKYU => new List<string>() { "50", "51", "52" };

        /// <summary>
        /// 所定労働時間
        /// </summary>
        public static int SYOTEI_ROUDO_MIN => 7 * 60 + 40;

        /// <summary>
        /// 所定勤務開始時刻（分単位）　9:00開始
        /// 就業規則第50条 第1項
        /// </summary>
        public static int SYOTEI_ROUDO_BEGIN => 9 * 60 + 0;

        /// <summary>
        /// 所定勤務終了時刻（分単位）　17:40終了
        /// 就業規則第50条 第1項
        /// </summary>
        public static int SYOTEI_ROUDO_END => 17 * 60 + 40;

        /// <summary>
        /// ゼロ4桁埋め用
        /// </summary>
        public static string PADDING_ZERO_4 => "0000";

        /// <summary>
        /// ゼロ2桁埋め用
        /// </summary>
        public static string PADDING_ZERO_2 => "00";

        /// <summary>
        /// コアタイム開始時刻（分単位）　10:30開始
        /// </summary>
        public static int CORE_TIME_BEGIN => 10 * 60 + 30;

        /// <summary>
        /// コアタイム終了時刻（分単位）　15:00終了
        /// </summary>
        public static int CORE_TIME_END => 15 * 60 + 0;

        /// <summary>
        /// 一斉休憩時間開始時刻（分単位）　12:00開始
        /// </summary>
        public static int REST_TIME_BEGIN => 12 * 60 + 0;

        /// <summary>
        /// 一斉休憩時間終了時刻（分単位）　13:00終了
        /// </summary>
        public static int REST_TIME_END => 13 * 60 + 0;

        /// <summary>
        /// 半休付与時間（分単位）　4時間
        /// 就業規則第62条 第2項
        /// </summary>
        public static int HANKYU_HUYO_TIME => 4 * 60 + 0;

        /// <summary>
        /// セッション文字列：最終ログイン時刻
        /// </summary>
        public static string SESSION_STRING_LAST_LOGIN_TIME => "LastLoginTime";

        /// <summary>
        /// セッション文字列：ログインしている社員情報
        /// </summary>
        public static string SESSION_STRING_LOGIN_SHAIN_INFO => "LoginShainInfo";

        /// <summary>
        /// セッション文字列：表示している社員情報
        /// </summary>
        public static string SESSION_STRING_VIEW_SHAIN_INFO => "ViewShainInfo";

        /// <summary>
        /// セッション文字列：リダイレクト用のURL
        /// </summary>
        public static string SESSION_STRING_REDIRECT_URL => "RedirectURL";

        /// <summary>
        /// セッション文字列：情報表示用の時刻データ
        /// </summary>
        public static string SESSION_STRING_VIEW_DATETIME => "ViewDateTime";

        /// <summary>
        /// 開始時、開始分、終了時、終了分の4引数を必要とするStringFormat
        /// </summary>
        public static string STRING_FORMAT_TIME_RANGE => "{0}：{1}　～　{2}：{3}";

        /// <summary>
        /// NLog用文字列
        /// </summary>
        public static string LOG_START => "================================< START >================================";
        /// <summary>
        /// 認証コード
        /// </summary>
        public enum NinsyoCD
        {
            /// <summary>
            /// 基本の認証コードです。
            /// </summary>
            出勤 = 1,
            /// <summary>
            /// 勤務箇所を離れて勤務する場合このコードを使用します。就業規則第70条
            /// </summary>
            出張 = 2,
            /// <summary>
            /// 研修全般にこのコードを使用します。
            /// </summary>
            研修 = 3,
            /// <summary>
            /// フレックスタイム制勤務取扱規程第12条の2
            /// </summary>
            非番 = 4,
            /// <summary>
            /// 就業規則第30条
            /// </summary>
            赴任 = 5,
            /// <summary>
            /// 就業規則第30条
            /// </summary>
            着任 = 6,
            /// <summary>
            /// 就業規則第51条（原則日曜日）
            /// </summary>
            公休 = 10,
            /// <summary>
            /// 就業規則第52条（原則土曜日、祝日、年末年始、お盆前後、JEIS創社記念日（11/24））
            /// </summary>
            特休 = 11,
            /// <summary>
            /// 就業規則第53条、フレックスタイム制勤務取扱規程第12条
            /// </summary>
            代休 = 12,
            /// <summary>
            /// 就業規則第62条第1項
            /// </summary>
            年休 = 19,
            /// <summary>
            /// 就業規則第62条第2～3項
            /// </summary>
            AM半休 = 20,
            /// <summary>
            /// 就業規則第62条第2～3項
            /// </summary>
            PM半休 = 21,
            /// <summary>
            /// 就業規則第66条
            /// </summary>
            選挙 = 22,
            /// <summary>
            /// 就業規則第66条
            /// </summary>
            り災 = 23,
            /// <summary>
            /// 就業規則第66条
            /// </summary>
            忌引 = 24,
            /// <summary>
            /// 就業規則第66条
            /// </summary>
            障害 = 25,
            /// <summary>
            /// 就業規則第66条
            /// </summary>
            召喚 = 26,
            /// <summary>
            /// 就業規則第66条
            /// </summary>
            育児 = 27,
            /// <summary>
            /// 就業規則第66条
            /// </summary>
            静養Ａ = 28,
            /// <summary>
            /// 就業規則第66条
            /// </summary>
            移転 = 29,
            /// <summary>
            /// 就業規則第66条
            /// </summary>
            結婚 = 30,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            業災 = 31,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            通災 = 32,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            病気 = 33,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            公職 = 35,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            出産 = 36,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            診査 = 37,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            団交 = 38,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            経協 = 39,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            組休 = 40,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            苦情 = 41,
            /// <summary>
            /// 
            /// </summary>
            待機 = 42,
            /// <summary>
            /// 
            /// </summary>
            不参 = 43,
            /// <summary>
            /// 
            /// </summary>
            事故 = 45,
            /// <summary>
            /// 
            /// </summary>
            欠務 = 46,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            静養Ｂ = 47,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            立候補 = 48,
            /// <summary>
            /// 
            /// </summary>
            日２ = 51,
            /// <summary>
            /// 
            /// </summary>
            出停 = 52,
            /// <summary>
            /// 
            /// </summary>
            制限 = 53,
            /// <summary>
            /// 就業規則第31条
            /// </summary>
            休職 = 54,
            /// <summary>
            /// 
            /// </summary>
            否認 = 55,
            /// <summary>
            /// 
            /// </summary>
            専従休職 = 56,
            /// <summary>
            /// 
            /// </summary>
            争議 = 57,
            /// <summary>
            /// 
            /// </summary>
            閉鎖 = 58,
            /// <summary>
            /// 
            /// </summary>
            拒否 = 59,
            /// <summary>
            /// 
            /// </summary>
            交昼 = 61,
            /// <summary>
            /// 
            /// </summary>
            交夜 = 62,
            /// <summary>
            /// 
            /// </summary>
            免除 = 70,
            /// <summary>
            /// 就業規則第66条
            /// </summary>
            通院 = 71,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            指導 = 72,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            養育 = 73,
            /// <summary>
            /// 就業規則第67条
            /// </summary>
            介護 = 74,
            /// <summary>
            /// 
            /// </summary>
            看護 = 75,
            /// <summary>
            /// 就業規則第66条
            /// </summary>
            裁員 = 78,
            /// <summary>
            /// 
            /// </summary>
            退職 = 98,
            /// <summary>
            /// 
            /// </summary>
            その他 = 99,
        }

        /// <summary>
        /// 認証コードを2桁の数値文字列に変換します。
        /// </summary>
        /// <param name="cd"></param>
        /// <returns></returns>
        public static string ToZeroPaddingString(this NinsyoCD cd)
        {
            return ((int)cd).ToString(PADDING_ZERO_2);
        }
    }
}
