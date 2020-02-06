using NLog;
using System;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace CommonLibrary.Models
{

    /// <summary>
    /// 勤務システムDBのORマッパー
    /// </summary>
    public partial class KinmuSystemDB : DbContext
    {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
#if DEBUG
        /// <summary>
        /// 接続文字列KinmuSystemDBを使用してインスタンスを作成します。
        /// また、NLogを使用してSQLログを吐き出します。
        /// </summary>
        public KinmuSystemDB() : base("name=KinmuSystemDBdev")
        {
            Database.Log = log =>
            {
                if (!string.IsNullOrWhiteSpace(log))
                {
                    logger.Trace(log.Replace(Environment.NewLine, " ").Replace("  ", " "));
                }
            };
        }
#else
        public KinmuSystemDB() : base("name=KinmuSystemDB") { }
#endif

        /// <summary>
        /// サニタイズしゼロ2桁埋めを行います。
        /// </summary>
        /// <param name="value">サニタイズする文字列</param>
        /// <returns></returns>
        internal static string SanitizeAndZeroPaddingToIntString(string value)
        {
            return SanitizeAndZeroPaddingToIntString(value, 2);
        }

        /// <summary>
        /// サニタイズしゼロ桁埋めを行います。数値以外の場合は例外が発生します
        /// </summary>
        /// <param name="value">サニタイズする文字列</param>
        /// <param name="digit">ゼロ埋めする桁数</param>
        /// <exception cref="KinmuException"></exception>
        /// <returns></returns>
        internal static string SanitizeAndZeroPaddingToIntString(string value, int digit)
        {
            // nullは空文字で返す
            if (value == null) return "";

            // 空白文字の削除
            value = value.Trim();

            // 空文字の場合空文字で返す
            if (value == "") return "";

            // 数値以外が入っていたら例外を出す
            if (!Regex.IsMatch(value, "^[0-9]+$")) throw new KinmuException("数値以外の値は入力できません。入力値：" + value);

            // ゼロ埋めして返す
            return value.PadLeft(digit, '0');
        }

        /// <summary>
        /// 年月日の妥当性を判断します
        /// </summary>
        /// <param name="_year">年</param>
        /// <param name="_month">月</param>
        /// <param name="_day">日</param>
        internal static void ValidationDate(string _year, string _month, string _day)
        {
            try
            {
                DateTime test = DateTime.Parse($"{_year}/{_month}/{_day}");
            }
            catch (Exception)
            {
                throw new KinmuException($"日付文字列が不正です。入力された値：{_year}/{_month}/{_day}");
            }
        }

        /// <summary>
        /// 日別勤務実績データ
        /// </summary>
        public virtual DbSet<KNS_D01> KNS_D01 { get; set; }

        /// <summary>
        /// 日別作業日誌データ
        /// </summary>
        public virtual DbSet<KNS_D02> KNS_D02 { get; set; }

        /// <summary>
        /// 日別諸手当データ
        /// </summary>
        public virtual DbSet<KNS_D03> KNS_D03 { get; set; }

        /// <summary>
        /// 個人PJマスタ（勤務実績）
        /// </summary>
        public virtual DbSet<KNS_D04> KNS_D04 { get; set; }

        /// <summary>
        /// 個人PJマスタ（プロジェクト）
        /// </summary>
        public virtual DbSet<KNS_D05> KNS_D05 { get; set; }

        /// <summary>
        /// 個人PJマスタ（作業コード）
        /// </summary>
        public virtual DbSet<KNS_D06> KNS_D06 { get; set; }

        /// <summary>
        /// 勤務管理マスタ
        /// </summary>
        public virtual DbSet<KNS_D07> KNS_D07 { get; set; }

        /// <summary>
        /// メッセージ管理
        /// </summary>
        public virtual DbSet<KNS_D08> KNS_D08 { get; set; }

        /// <summary>
        /// パスワード管理
        /// </summary>
        public virtual DbSet<KNS_D09> KNS_D09 { get; set; }

        /// <summary>
        /// 月別勤務実績データ
        /// </summary>
        public virtual DbSet<KNS_D10> KNS_D10 { get; set; }

        /// <summary>
        /// 特休・公休データ
        /// </summary>
        public virtual DbSet<KNS_D11> KNS_D11 { get; set; }

        /// <summary>
        /// 年休データ
        /// </summary>
        public virtual DbSet<KNS_D12> KNS_D12 { get; set; }

        /// <summary>
        /// 日別勤務予定データ
        /// </summary>
        public virtual DbSet<KNS_D13> KNS_D13 { get; set; }

        /// <summary>
        /// 最終ログインデータ
        /// </summary>
        public virtual DbSet<KNS_D14> KNS_D14 { get; set; }

        /// <summary>
        /// 社員マスタ
        /// </summary>
        public virtual DbSet<KNS_M01> KNS_M01 { get; set; }

        /// <summary>
        /// プロジェクトマスタ
        /// </summary>
        public virtual DbSet<KNS_M02> KNS_M02 { get; set; }

        /// <summary>
        /// 作業コードマスタ
        /// </summary>
        public virtual DbSet<KNS_M03> KNS_M03 { get; set; }

        /// <summary>
        /// 勤務認証マスタ
        /// </summary>
        public virtual DbSet<KNS_M04> KNS_M04 { get; set; }

        /// <summary>
        /// カレンダーマスタ
        /// </summary>
        public virtual DbSet<KNS_M05> KNS_M05 { get; set; }

        /// <summary>
        /// 手当マスタ
        /// </summary>
        public virtual DbSet<KNS_M06> KNS_M06 { get; set; }

        /// <summary>
        /// 集約日マスタ
        /// </summary>
        public virtual DbSet<KNS_M07> KNS_M07 { get; set; }

        /// <summary>
        /// 年休付与マスタ
        /// </summary>
        public virtual DbSet<KNS_M08> KNS_M08 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.DATA_Y)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.DATA_M)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.DATA_D)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.NINYO_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.NINKA_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.DKIJYUN_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.STR_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.STR_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.END_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.END_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.END_PAR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.RESTS1_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.RESTS1_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.RESTE1_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.RESTE1_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.RESTS2_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.RESTS2_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.RESTE2_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.RESTE2_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.RESTS3_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.RESTS3_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.RESTE3_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.RESTE3_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.KAKN_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.DKIJI)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D01>()
                .Property(e => e.UPD_SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D02>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D02>()
                .Property(e => e.DATA_Y)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D02>()
                .Property(e => e.DATA_M)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D02>()
                .Property(e => e.DATA_D)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D02>()
                .Property(e => e.PROJ_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D02>()
                .Property(e => e.SAGYO_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D03>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D03>()
                .Property(e => e.DATA_Y)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D03>()
                .Property(e => e.DATA_M)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D03>()
                .Property(e => e.DATA_D)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D03>()
                .Property(e => e.TEATE_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D03>()
                .Property(e => e.DTEATE_CNT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.NINSE_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.STR_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.STR_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.END_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.END_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.END_PAR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.RESTS1_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.RESTS1_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.RESTE1_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.RESTE1_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.RESTS2_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.RESTS2_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.RESTE2_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.RESTE2_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.RESTS3_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.RESTS3_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.RESTE3_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.RESTE3_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.FLX_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.PROJ_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D04>()
                .Property(e => e.SAGYO_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D05>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D05>()
                .Property(e => e.PROJ_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D05>()
                .Property(e => e.VIEW_ORDER)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D06>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D06>()
                .Property(e => e.SAGYO_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D06>()
                .Property(e => e.VIEW_ORDER)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D07>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D07>()
                .Property(e => e.F1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D07>()
                .Property(e => e.F2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D07>()
                .Property(e => e.F3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D07>()
                .Property(e => e.F4)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D07>()
                .Property(e => e.F5)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D07>()
                .Property(e => e.F6)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D07>()
                .Property(e => e.F7)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D07>()
                .Property(e => e.F8)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D07>()
                .Property(e => e.F9)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D07>()
                .Property(e => e.F10)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.HYOJI_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.F1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.F2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.F3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.DB_MSG1)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.DB_MSG2)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.DB_MSG3)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.DB_MSG4)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.DB_MSG5)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.DB_MSG6)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.DB_MSG7)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.DB_MSG8)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.DB_MSG9)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D08>()
                .Property(e => e.DB_MSG10)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D09>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D09>()
                .Property(e => e.PASSWORD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D10>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D10>()
                .Property(e => e.SHUYAKU_Y)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D10>()
                .Property(e => e.SHUYAKU_M)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D11>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D11>()
                .Property(e => e.NENDO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D11>()
                .Property(e => e.SHUYAKUY)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D11>()
                .Property(e => e.SHUYAKUM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D12>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D12>()
                .Property(e => e.NENDO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D12>()
                .Property(e => e.SHUYAKUY)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D12>()
                .Property(e => e.SHUYAKUM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D13>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D13>()
                .Property(e => e.DATA_Y)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D13>()
                .Property(e => e.DATA_M)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D13>()
                .Property(e => e.DATA_D)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D13>()
                .Property(e => e.STR_Y_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D13>()
                .Property(e => e.STR_Y_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D13>()
                .Property(e => e.END_Y_HR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D13>()
                .Property(e => e.END_Y_MIN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D13>()
                .Property(e => e.END_Y_PAR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D13>()
                .Property(e => e.KAKN_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D13>()
                .Property(e => e.UPD_SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D13>()
                .Property(e => e.YOTEI_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D13>()
                .Property(e => e.SHONIN_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D14>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_D14>()
                .Property(e => e.LAST_LOGIN_DATE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.HATSUREI_Y)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.HATSUREI_M)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.HATSUREI_D)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.SHAIN_NM)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.IK_KUBUN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.KINSHUBETSU)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.MIBUN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.FLX_KUBUN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.GYOMU_KUBUN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.ALIAS)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.MANAGER)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.SHAIN_KUBUN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.SHONIN_SHAIN_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M01>()
                .Property(e => e.PC_NAME)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M02>()
                .Property(e => e.PROJ_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M02>()
                .Property(e => e.PROJ_NM)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M03>()
                .Property(e => e.SAGYO_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M03>()
                .Property(e => e.SAGYO_NM)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M04>()
                .Property(e => e.NINSHO_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M04>()
                .Property(e => e.NINSHO_NM)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M04>()
                .Property(e => e.REDIRECT_PATTERN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M04>()
                .Property(e => e.CALC_PATTERN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M05>()
                .Property(e => e.DATA_Y)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M05>()
                .Property(e => e.DATA_M)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M05>()
                .Property(e => e.DATA_D)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M05>()
                .Property(e => e.NINYO_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M05>()
                .Property(e => e.KEITAI)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M05>()
                .Property(e => e.SHUKU_FLG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M06>()
                .Property(e => e.TEATE_CD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M06>()
                .Property(e => e.TEATE_NM)
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M07>()
                .Property(e => e.SHUYAKUY)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M07>()
                .Property(e => e.SHUYAKUM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M08>()
                .Property(e => e.MIBUN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KNS_M08>()
                .Property(e => e.HATSUREI_M)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
