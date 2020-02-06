using System;
using System.Collections.Generic;
using System.Linq;
using CommonLibrary;
using CommonLibrary.Models;
using static CommonLibrary.CommonDefine;
using static CommonLibrary.CommonDefine.NinsyoCD;

namespace KinmuLibrary
{
    /// <summary>
    /// 勤務情報を計算したりするマネージャーです。
    /// </summary>
    public class KinmuManager
    {
        private readonly DBManager dbManager = new DBManager();
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 勤務実績
        /// </summary>
        public List<KNS_D01> KinmuJisseki { get; }
        /// <summary>
        /// 勤務予定
        /// </summary>
        public List<KNS_D13> KinmuYotei { get; }
        /// <summary>
        /// カレンダーマスター
        /// </summary>
        public List<KNS_M05> CalendarMaster { get; }
        /// <summary>
        /// 認証コードマスター
        /// </summary>
        public List<KNS_M04> NinsyoCDMaster { get; }


        /// <summary>
        /// KinmuManagerが管理する対象の社員の社員コード
        /// </summary>
        public string EmployeeCD { get; }

        /// <summary>
        /// 社員区分
        /// </summary>
        public string SyainKubun { get; }

        /// <summary>
        /// KinmuManagerが管理する対象の年（年度ではなく暦日）
        /// </summary>
        public int Year { get; }

        /// <summary>
        /// KinmuManagerが管理する対象の月
        /// </summary>
        public int Month { get; }

        /// <summary>
        /// 個人の勤務実績、予定を管理するマネージャを作成します。
        /// 帳票出力や、実績入力等はこのマネージャを通して行ってください。
        /// 規定で現在時刻の年月でマネージャが作成されます。
        /// </summary>
        /// <param name="employeeCD"></param>
        public KinmuManager(string employeeCD) : this(employeeCD, DateTime.Now.Year, DateTime.Now.Month)
        {
        }

        /// <summary>
        /// 個人の勤務実績、予定を管理するマネージャを作成します。
        /// 帳票出力や、実績入力等はこのマネージャを通して行ってください。
        /// </summary>
        /// <param name="employeeCD"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public KinmuManager(string employeeCD, int year, int month)
        {
            logger.Debug("社員コード：{0},年月：{1}/{2}の作成", employeeCD, year, month);
            EmployeeCD = employeeCD;
            Year = year;
            Month = month;
            SyainKubun = dbManager.GetSyainMasterByShainCD(EmployeeCD).SHAIN_KUBUN;
            KinmuJisseki = dbManager.GetKinmuJisseki(EmployeeCD, Year, Month);
            KinmuYotei = dbManager.GetKinmuYotei(EmployeeCD, Year, Month);
            CalendarMaster = dbManager.GetCalender(Year, Month);
            NinsyoCDMaster = dbManager.GetKinmuNinsyoCDMasterALL();
            logger.Debug("社員コード：{0},年月：{1}/{2}の作成終了", employeeCD, year, month);
        }

        #region 帳票出力系
        /// <summary>
        /// 月間所定出勤日数（認証コードが出勤の日）を算出します。
        /// </summary>
        /// <returns>月間所定出勤日数</returns>
        public int CalcGekkanSyoteiNissu()
        {
            logger.Debug(LOG_START);
            // 現行では当月日数から公休・特休かつ日別確認フラグ1をひてる
            int inDay = DateTime.DaysInMonth(Year, Month);

            // 本来はカレンダーの認証コードを使用すべきだが、現行に合わせて実績の認証コードを使用している（総務部確認済み）
            return inDay - KinmuJisseki.Count(_ => _.KAKN_FLG == "1" && (_.NINKA_CD == 公休.ToZeroPaddingString() || _.NINKA_CD == 特休.ToZeroPaddingString()));
        }

        /// <summary>
        /// 月間出勤日数を算出します。
        /// </summary>
        /// <returns>月間出勤日数</returns>
        public int CalcGekkanSyukkinNissu()
        {
            logger.Debug(LOG_START);
            // 代休、年休は除く
            string[] ignoreCD = { 代休.ToZeroPaddingString(), 年休.ToZeroPaddingString() };
            // 有給パターン、無給パターンは除く
            List<KNS_M04> ignorePattern = NinsyoCDMaster.Where(cdMaster => CALC_PATTERN_YUKYU.Any(pattern => pattern == cdMaster.CALC_PATTERN) || CALC_PATTERN_MUKYU.Any(pattern => pattern == cdMaster.CALC_PATTERN)).ToList();

            // 現行では当月日数から公休、特休、代休、年休と有給パターン、無給パターンかつ日別確認フラグ1をひてる
            return CalcGekkanSyoteiNissu() - KinmuJisseki.Count(_ => ignoreCD.Any(__ => __ == _.NINKA_CD) || ignorePattern.Any(__ => __.NINSHO_CD == _.NINKA_CD));
        }

        /// <summary>
        /// 月間休日労働日数を算出します。
        /// </summary>
        /// <returns>月間休日労働日数</returns>
        public int CalcGekkanKyujitsuRoudouNissu()
        {
            logger.Debug(LOG_START);
            // （特急、公休、代休）かつ（0 < 勤務時間）なら休日労働
            string[] targetCD = { 公休.ToZeroPaddingString(), 特休.ToZeroPaddingString(), 代休.ToZeroPaddingString(), 年休.ToZeroPaddingString() };
            int day = KinmuJisseki.Count(_ => targetCD.Any(__ => __ == _.NINKA_CD) && 0 < _.DKINM + _.DMINA1);

            return day;
        }

        /// <summary>
        /// 月間－日数を算出します。なお、0固定です。
        /// </summary>
        /// <returns>月間－日数</returns>
        public int CalcBarNissu()
        {
            logger.Debug(LOG_START);
            return 0;
        }

        /// <summary>
        /// 月間代休日数を算出します。
        /// </summary>
        /// <returns>月間代休日数</returns>
        public int CalcGekkanDaikyuNissu()
        {
            logger.Debug(LOG_START);
            return KinmuJisseki.Count(_ => _.NINKA_CD == 代休.ToZeroPaddingString());
        }

#pragma warning disable S4144 // Methods should not have identical implementations
        /// <summary>
        /// 月間非番日数を計算します。
        /// </summary>
        /// <returns>月間非番日数</returns>
        public int CalcGekkanHibanNissu()
#pragma warning restore S4144 // Methods should not have identical implementations
        {
            logger.Debug(LOG_START);
            return 0;
        }

        /// <summary>
        /// 月間の有給日数を計算します。
        /// </summary>
        /// <returns>月間有給日数</returns>
        public int CalcGekkanYukyuNissu()
        {
            logger.Debug(LOG_START);
            List<KNS_M04> yukyuCD = NinsyoCDMaster.Where(cdMaster => CALC_PATTERN_YUKYU.Any(pattern => pattern == cdMaster.CALC_PATTERN)).ToList();
            return KinmuJisseki.Count(jisseki => yukyuCD.Any(_ => _.NINSHO_CD == jisseki.NINKA_CD));
        }

        /// <summary>
        /// 月間の無休日数を計算します。
        /// </summary>
        /// <returns>月間有給日数</returns>
        public int CalcGekkanMukyuNissu()
        {
            logger.Debug(LOG_START);
            List<KNS_M04> mukyuCD = NinsyoCDMaster.Where(cdMaster => CALC_PATTERN_MUKYU.Any(pattern => pattern == cdMaster.CALC_PATTERN)).ToList();
            return KinmuJisseki.Count(jisseki => mukyuCD.Any(_ => _.NINSHO_CD == jisseki.NINKA_CD));
        }

#pragma warning disable S4144 // Methods should not have identical implementations
        /// <summary>
        /// 月間の組休日数を計算します。
        /// </summary>
        /// <returns>月間組休日数</returns>
        public int CalcGekkanKumikyuNissu()
#pragma warning restore S4144 // Methods should not have identical implementations
        {
            logger.Debug(LOG_START);
            return 0;
        }

        /// <summary>
        /// 月間の公休労働日数を計算します。
        /// </summary>
        /// <returns>月間公休日数</returns>
        public int CalcGekkanKokyuRoudouNissu()
        {
            logger.Debug(LOG_START);
            List<KNS_D01> kokyu;

            // 公休日の勤務実績を取得する。
            try { kokyu = KinmuJisseki.Where(_ => _.NINKA_CD == 公休.ToZeroPaddingString()).ToList(); }
            catch (ArgumentNullException) { return 0; }

            // みなし1を入れないのは出張中の休日は取得したものとみなすため。
            int kokyuRoudou = kokyu.Count(_ => 0 < (_.DKINM ?? 0));

            // 前日の翌日フラグを確認する
            foreach (KNS_D01 item in kokyu)
            {
                KNS_D01 yesterday;

                // そもそも勤務時間があったらカウント済み
                if (0 < item.DKINM) { continue; }

                // 前日日付を取得（1日が公休日の場合は前月に遡るが、入力の段階で翌日フラグを立てないようにするのであまり考慮しない）
                DateTime date = new DateTime(Year, Month, int.Parse(item.DATA_D)).AddDays(-1);
                try { yesterday = KinmuJisseki.First(_ => _.DATA_D == date.Day.ToString(PADDING_ZERO_2)); }
                catch (Exception) { continue; }

                // 翌日フラグがなければ次へ
                if (yesterday.END_PAR != "1") { continue; }

                // 終業が0:00なら公休日労働にはしない。
                if (yesterday.END_HR == "00" && yesterday.END_MIN == "00") { continue; }

                // めでたく公休日労働です。
                kokyuRoudou++;
            }

            return kokyuRoudou;
        }

        /// <summary>
        /// 月間の所定労働日数を計算します。所定日数はカレンダー基準で計算するのに対し、
        /// 本メソッドは勤務実績を考慮して計算します。
        /// </summary>
        /// <returns>所定労働日数</returns>
        public int CalcGekkanSyoteiRoudoNissu()
        {
            logger.Debug(LOG_START);

            // まずは月に含まれる日数を取得する
            int workDays = DateTime.DaysInMonth(Year, Month);

            // 特休、公休、代休のコード配列
            string[] ignoreCDs = new string[] { 特休.ToZeroPaddingString(), 公休.ToZeroPaddingString(), 代休.ToZeroPaddingString() };

            // 無給休暇のコード配列
            IEnumerable<KNS_M04> mukyuCDs = NinsyoCDMaster.Where(cdMaster => CALC_PATTERN_MUKYU.Any(_ => _ == cdMaster.CALC_PATTERN));
            IEnumerable<KNS_M04> yukyuCDs = NinsyoCDMaster.Where(cdMaster => CALC_PATTERN_YUKYU.Any(_ => _ == cdMaster.CALC_PATTERN) || CALC_PATTERN_NENKYU.Any(_ => _ == cdMaster.CALC_PATTERN));

            KinmuJisseki.ForEach(_ =>
            {
                // 確認フラグが立っていなければ次へ
                if (_.KAKN_FLG != "1") return;

                bool isKyuka = ignoreCDs.Any(cd => cd == _.NINKA_CD);
                bool isMukyu = (_.DKINM ?? 0) == 0 && mukyuCDs.Any(cd => cd.NINSHO_CD == _.NINKA_CD);
                bool isYukyu = yukyuCDs.Any(cd => cd.NINSHO_CD == _.NINKA_CD);
                if (isKyuka || isMukyu)
                {
                    // 特休、公休、代休の場合は所定労働日数に含めない
                    // 無給休暇かつ勤務時間が0の場合は所定労働日数に含めない
                    workDays--;
                    return;
                }

                TimeRange tmpCoreTime = new TimeRange(_.GetDate() + TimeSpan.FromMinutes(CORE_TIME_BEGIN), _.GetDate() + TimeSpan.FromMinutes(CORE_TIME_END));
                if (!tmpCoreTime.IsOverlap(_.GetWorkTimeRange()) && !isYukyu)
                {
                    // コアタイムの全部を欠勤した場合は所定労働日数に含めない
                    workDays--;
                }
            });

            return workDays;
        }

        /// <summary>
        /// 月間の労働日数を計算します。
        /// </summary>
        /// <returns>月間の実労働日数</returns>
        public int CalcGekkanRoudouNissu()
        {
            logger.Debug(LOG_START);
            var cd = NinsyoCDMaster.Where(_ => CALC_PATTERN_SKK_ROUDO_NISSU.Any(__ => __ == _.CALC_PATTERN));
            return KinmuJisseki.Count(_ =>
                (
                    // 認証コードが一致して、確認フラグが１で、勤務時間が０
                    cd.Any(__ => __.NINSHO_CD == _.NINKA_CD) &&
                    _.KAKN_FLG == "1" &&
                    (_.DKINM ?? 0) == 0
                ) ||
                0 < (_.DKINM ?? 0)
            );
        }

        /// <summary>
        /// 月間の特休予定日数を計算します。
        /// </summary>
        /// <returns>特休予定日数</returns>
        public int CalcGekkanTokkyuYoteiNissu()
        {
            logger.Debug(LOG_START);
            return CalendarMaster.Count(_ => _.NINYO_CD == 特休.ToZeroPaddingString());
        }

        /// <summary>
        /// 月間の公休予定日数を計算します。
        /// </summary>
        /// <returns>公休予定日数</returns>
        public int CalcGekkanKoukyuYoteiNissu()
        {
            logger.Debug(LOG_START);
            return CalendarMaster.Count(_ => _.NINYO_CD == 公休.ToZeroPaddingString());
        }

        /// <summary>
        /// 月間の特休確定日数を計算します。
        /// </summary>
        /// <returns>特休確定日数</returns>
        public int CalcGekkanTokkyuKakuteiNissu()
        {
            logger.Debug(LOG_START);
            return KinmuJisseki.Count(_ => _.NINKA_CD == 特休.ToZeroPaddingString() && _.KAKN_FLG == "1");
        }

        /// <summary>
        /// 月間の公休確定日数を計算します。
        /// </summary>
        /// <returns>公休確定日数</returns>
        public int CalcGekkanKoukyuKakuteiNissu()
        {
            logger.Debug(LOG_START);
            return KinmuJisseki.Count(_ => _.NINKA_CD == 公休.ToZeroPaddingString() && _.KAKN_FLG == "1");
        }

        /// <summary>
        /// 月間の総実労働時間（勤務時間＋みなし1）を計算します。
        /// </summary>
        /// <returns>月間の総実労働時間</returns>
        public int CalcGekkanTotalJitsuRoudoJikan()
        {
            logger.Debug(LOG_START);
            int time = 0;
            KinmuJisseki.ForEach(_ =>
            {
                time += _.DKINM ?? 0;
                if (_.KAKN_FLG == "1") time += _.DMINA1 ?? 0;
            });

            return time;
        }

        /// <summary>
        /// 月間の有給時間（みなし2）を計算します。
        /// </summary>
        /// <returns>月間の有給時間</returns>
        public int CalcGekkanYukyuJikan()
        {
            logger.Debug(LOG_START);
            int time = 0;
            KinmuJisseki.ForEach(_ => time += (_.DMINA2 ?? 0));
            return time;
        }

#pragma warning disable S4144 // Methods should not have identical implementations
        /// <summary>
        /// 月間の超勤A時間を計算します。
        /// </summary>
        /// <returns>超勤A時間</returns>
        public int CalcGekkanTankaAJikan()
#pragma warning restore S4144 // Methods should not have identical implementations
        {
            logger.Debug(LOG_START);
            // 代休のときに7:40を返していたが、休んでいるのに勤務時間がつくのはおかしいので0で返す
            // 時短勤務が出た場合はこのメソッドを改修する
            return 0;
        }

        /// <summary>
        /// 月間の超勤B時間を計算します。
        /// </summary>
        /// <returns>超勤B時間</returns>
        public int CalcGekkanTankaBJikan()
        {
            logger.Debug(LOG_START);

            int time = CalcGekkanTotalJitsuRoudoJikan() + CalcGekkanYukyuJikan() - CalcGekkanSyoteiRoudoJikan() - CalcGekkanTankaTokkyuDJikan() - CalcGekkanTankaKoukyuDJikan();
            return 0 < time ? time : 0;
        }

        /// <summary>
        /// 月間の超勤D（特休）時間を計算します。
        /// </summary>
        /// <returns>超勤D（特休）時間</returns>
        public int CalcGekkanTankaTokkyuDJikan()
        {
            logger.Debug(LOG_START);

            int time = 0;
            for (int index = 0; index < KinmuJisseki.Count; index++)
            {
                KNS_D01 today = KinmuJisseki[index];

                // 未確認、もしくは特休・代休ではない場合次の勤務実績を見る
                if (today.KAKN_FLG != "1") continue;
                if (today.NINKA_CD != 特休.ToZeroPaddingString() && today.NINKA_CD != 代休.ToZeroPaddingString()) continue;

                // 当日の時間帯を作成
                DateTime day = today.GetDate();
                TimeRange tr = new TimeRange(day, day + TimeSpan.FromDays(1));

                // 当日（特休）に重複する勤務時間を加算、重複する休憩時間を減算
                time += tr.GetOverlapMinutes(today);

                // 前日が日付跨ぎだった場合は前日の勤務も考慮する。
                if (0 < index && KinmuJisseki[index - 1].END_PAR == "1")
                {
                    KNS_D01 yesterday = KinmuJisseki[index - 1];
                    time += tr.GetOverlapMinutes(yesterday);
                }
            }

            return time;
        }

        /// <summary>
        /// 月間の超勤D（公休）時間を計算します
        /// </summary>
        /// <returns>超勤D（公休）時間</returns>
        public int CalcGekkanTankaKoukyuDJikan()
        {
            logger.Debug(LOG_START);
            return CalcGekkanTankaKoukyuDJikan(SyainKubun == "1");
        }

        /// <summary>
        /// 月間の超勤D（公休）時間を計算します
        /// </summary>
        /// <returns>超勤D（公休）時間</returns>
        public int CalcGekkanTankaKoukyuDJikan(bool isEnable)
        {
            logger.Debug(LOG_START);
            if (!isEnable) return 0;

            int time = 0;
            for (int index = 0; index < KinmuJisseki.Count; index++)
            {
                KNS_D01 today = KinmuJisseki[index];

                // 未確認、もしくは公休ではない場合次の勤務実績を見る
                if (today.KAKN_FLG != "1") continue;
                if (today.NINKA_CD != 公休.ToZeroPaddingString()) continue;

                // 当日の時間帯を作成
                DateTime day = today.GetDate();
                TimeRange tr = new TimeRange(day, day + TimeSpan.FromDays(1));

                // 当日（公休）に重複する勤務時間を加算、重複する休憩時間を減算
                time += tr.GetOverlapMinutes(today);

                // 前日が日付跨ぎだった場合は前日の勤務も考慮する。
                if (0 < index && KinmuJisseki[index - 1].END_PAR == "1")
                {
                    KNS_D01 yesterday = KinmuJisseki[index - 1];
                    time += tr.GetOverlapMinutes(yesterday);
                }
            }

            return time;
        }

        /// <summary>
        /// 月間の超勤D時間を計算します
        /// </summary>
        /// <returns>超勤D時間</returns>
        public int CalcGekkanTankaDJikan()
        {
            logger.Debug(LOG_START);
            return KinmuJisseki.Sum(_ => (_.DCHOD ?? 0));
        }

        /// <summary>
        /// 月間の超勤C時間（夜勤）を計算します
        /// </summary>
        /// <returns>超勤C時間</returns>
        public int CalcGekkanTankaCJikan()
        {
            logger.Debug(LOG_START);
            return KinmuJisseki.Sum(_ => (_.DYAKN ?? 0));
        }

        /// <summary>
        /// 月間の控除A時間を計算します
        /// </summary>
        /// <returns>控除A時間</returns>
        public int CalcGekkanKoujyoAJikan()
        {
            logger.Debug(LOG_START);
            return CalcGekkanDaikyuNissu() * SYOTEI_ROUDO_MIN;
        }

        /// <summary>
        /// 月間の超勤E時間を計算します
        /// </summary>
        /// <returns>超勤E時間</returns>
        public int CalcGekkanTankaEJikan()
        {
            logger.Debug(LOG_START);
            int e = CalcGekkanTankaBJikan() + CalcGekkanTankaTokkyuDJikan() - 60 * 60;
            return 0 < e ? e : 0;
        }

        /// <summary>
        /// 月間の減額A時間を計算します
        /// </summary>
        /// <returns>減額A時間</returns>
        public int CalcGekkanGengakuAjikan()
        {
            logger.Debug(LOG_START);
            int a = CalcGekkanSyoteiRoudoNissu() * SYOTEI_ROUDO_MIN - CalcGekkanTotalJitsuRoudoJikan() - CalcGekkanYukyuJikan();

            return 0 < a ? a : 0;
        }

        /// <summary>
        /// 時間外労働時間を計算します。
        /// </summary>
        /// <returns>時間外労働時間</returns>
        public int CalcGekkanJikangaiRoudoJikan()
        {
            logger.Debug(LOG_START);
            int d = CalcGekkanTotalJitsuRoudoJikan() - CalcGekkanHouTeiRoudoJikan();

            return 0 < d ? d : 0;
        }

        /// <summary>
        /// 経営社員の公休労働時間を計算します。
        /// </summary>
        /// <returns>公休時間労働</returns>
        public int CalcGekkanKeieiKokyuRoudoJikan()
        {
            logger.Debug(LOG_START);
            return CalcGekkanTankaKoukyuDJikan(SyainKubun == "2");
        }

        /// <summary>
        /// 法定労働時間　月の暦日数 / 7日 * 40時間 * 60分 
        /// </summary>
        /// <returns></returns>
        public int CalcGekkanHouTeiRoudoJikan()
        {
            logger.Debug(LOG_START);
            return DateTime.DaysInMonth(Year, Month) * 40 * 60 / 7;
        }

        /// <summary>
        /// 月間の所定労働時間を計算します。　出勤日 * 所定労働時間
        /// </summary>
        /// <returns></returns>
        public int CalcGekkanSyoteiRoudoJikan()
        {
            logger.Debug(LOG_START);
            return CalcGekkanSyoteiRoudoNissu() * SYOTEI_ROUDO_MIN;
        }

        /// <summary>
        /// 月間のみなし1（出張等）を計算します。
        /// </summary>
        /// <returns></returns>
        public int CalcGekkanMinashi1Jikan()
        {
            logger.Debug(LOG_START);
            return KinmuJisseki.Where(_ => _.KAKN_FLG == "1").Sum(_ => _.DMINA1 ?? 0);
        }

        /// <summary>
        /// 月間の祝日労働時間を計算します。
        /// </summary>
        /// <returns></returns>
        public int CalcGekkanSyukujituRoudoJikan()
        {
            logger.Debug(LOG_START);
            return KinmuJisseki.Sum(_ => _.DSHUK ?? 0);
        }

        /// <summary>
        /// 月間取得年休日数を計算します。
        /// </summary>
        /// <returns>月間取得年休日数</returns>
        public int CalcGekkanNenkyu()
        {
            logger.Debug(LOG_START);
            return KinmuJisseki.Count(_ => _.NINKA_CD == 年休.ToZeroPaddingString());
        }

        /// <summary>
        /// 月間AM半休日数を計算します。
        /// </summary>
        /// <returns>AM半休日数</returns>
        public int CaclGekkanAMHankyu()
        {
            logger.Debug(LOG_START);
            return KinmuJisseki.Count(_ => _.NINKA_CD == AM半休.ToZeroPaddingString());
        }

        /// <summary>
        /// 月間PM半休日数を計算します。
        /// </summary>
        /// <returns>PM半休日数</returns>
        public int CaclGekkanPMHankyu()
        {
            logger.Debug(LOG_START);
            return KinmuJisseki.Count(_ => _.NINKA_CD == PM半休.ToZeroPaddingString());
        }

        /// <summary>
        /// 月間休憩時間の合計を計算します。
        /// </summary>
        /// <returns>休憩時間の合計</returns>
        public int CalcGekkanKyukeiJikan()
        {
            logger.Debug(LOG_START);
            int totalRestTime = 0;
            KinmuJisseki.ForEach(_ =>
            {
                // よくよく考えたら勤務実績って割とデータが大きいので参照渡しのほうがいいよね。
                totalRestTime += _.GetTotalRestTime();
            });

            return totalRestTime;
        }

        /// <summary>
        /// 当年の累計超勤時間を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcTounenRuikeiTyoukinJissekiJikan()
        {
            logger.Debug(LOG_START);
            int nendo = Month < 4 ? Year - 1 : Year;
            return dbManager.GetTyoukinRuikei(EmployeeCD, nendo, Month);
        }

        /// <summary>
        /// 当年の累計法定外労働時間を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcTounenRuikeiHouteigaiJissekiJikan()
        {
            logger.Debug(LOG_START);
            int nendo = Month < 4 ? Year - 1 : Year;
            return dbManager.GetHouteigaiRuikei(EmployeeCD, nendo, Month);
        }

        /// <summary>
        /// 月間の労働日数（土日以外の日数）を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcGekkanHouteiNissu()
        {
            logger.Debug(LOG_START);
            int inMonth = DateTime.DaysInMonth(Year, Month);
            int holiday = 0;

            for (int day = 1; day <= inMonth; day++)
            {
                DayOfWeek week = DateTime.Parse($"{Year}/{Month}/{day}").DayOfWeek;
                switch (week)
                {
                    case DayOfWeek.Sunday:
                    case DayOfWeek.Saturday:
                        holiday++;
                        break;
                }
            }

            return inMonth - holiday;
        }


        ///勤務予定表用

        /// <summary>
        /// 月間の超勤B時間（予測）を計算します。
        /// </summary>
        /// <returns>超勤B時間（予測）</returns>
        public int CalcGekkanTankaBJikanYosoku()
        {
            logger.Debug("start");

            int time = 0;

            time += CalcGekkanTankaBJikan();

            CalendarMaster.ForEach(d02 =>
            {
                try
                {
                    if (!KinmuJisseki.Any(jisseki => jisseki.DATA_D == d02.DATA_D) && KinmuYotei.Any(yotei => yotei.DATA_D == d02.DATA_D))
                    {
                        KNS_D13 d13 = KinmuYotei.Single(yotei => yotei.DATA_D == d02.DATA_D);
                        if (d13.GetWorkTimeRange() == null) return;
                        int result = d13.GetWorkingTime() - SYOTEI_ROUDO_MIN - 60;
                        if (0 < result) time += result;
                    }
                }
                catch (Exception e)
                {
                    logger.Warn(e);
                }
            });

            return time;
        }

        /// <summary>
        /// 月間の超勤D（特休）時間（予測）を計算します。
        /// </summary>
        /// <returns>超勤D（特休）時間（予測）</returns>
        public int CalcGekkanTankaTokkyuDJikanYosoku()
        {
            logger.Debug("start");

            int time = 0;

            time += CalcGekkanTankaTokkyuDJikan();

            CalendarMaster.ForEach(d02 =>
            {
                try
                {
                    if (!KinmuJisseki.Any(jisseki => jisseki.DATA_D == d02.DATA_D) && KinmuYotei.Any(yotei => yotei.DATA_D == d02.DATA_D))
                    {
                        KNS_D13 d13 = KinmuYotei.Single(yotei => yotei.DATA_D == d02.DATA_D);
                        if (d13.YOTEI_CD != 特休.ToZeroPaddingString() && d13.YOTEI_CD != 代休.ToZeroPaddingString()) return;
                        if (d13.GetWorkTimeRange() == null) return;
                        int result = d13.GetWorkingTime() - SYOTEI_ROUDO_MIN - 60;
                        if (0 < result) time += result;
                    }
                }
                catch (Exception e)
                {
                    logger.Warn(e);
                }
            });

            return time;
        }

        /// <summary>
        /// 月間の超勤D（公休）時間（予測）を計算します
        /// </summary>
        /// <returns>超勤D（公休）時間（予測）</returns>
        public int CalcGekkanTankaKoukyuDJikanYosoku()
        {
            logger.Debug("start");

            int time = 0;

            time += CalcGekkanTankaKoukyuDJikan();

            CalendarMaster.ForEach(d02 =>
            {
                try
                {
                    if (!KinmuJisseki.Any(jisseki => jisseki.DATA_D == d02.DATA_D) && KinmuYotei.Any(yotei => yotei.DATA_D == d02.DATA_D))
                    {
                        KNS_D13 d13 = KinmuYotei.Single(yotei => yotei.DATA_D == d02.DATA_D);
                        if (d13.YOTEI_CD != 公休.ToZeroPaddingString()) return;
                        if (d13.GetWorkTimeRange() == null) return;
                        int result = d13.GetWorkingTime() - SYOTEI_ROUDO_MIN - 60;
                        if (0 < result) time += result;
                    }
                }
                catch (Exception e)
                {
                    logger.Warn(e);
                }
            });

            return time;
        }

        /// <summary>
        /// 月間の超勤E時間（予測）を計算します
        /// <returns>超勤E時間（予測）</returns>
        /// </summary>
        public int CalcGekkanTankaEJikanYosoku()
        {
            logger.Debug("start");
            int e = CalcGekkanTankaBJikanYosoku() + CalcGekkanTankaTokkyuDJikanYosoku() - 60 * 60;
            return 0 < e ? e : 0;
        }

        /// <summary>
        /// 当月の累計超勤時間（予測）を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcToutsukiRuikeiTyoukinJissekiJikanYosoku()
        {
            logger.Debug("start");
            int ruikei = CalcGekkanTankaBJikanYosoku() + CalcGekkanTankaTokkyuDJikanYosoku() + CalcGekkanTankaKoukyuDJikanYosoku();
            return ruikei;
        }

        /// <summary>
        /// 当年の累計超勤時間（予測）を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcTounenRuikeiTyoukinJissekiJikanYosoku()
        {
            logger.Debug("start");
            int nendo = Month < 4 ? Year - 1 : Year;
            int ruikei = CalcToutsukiRuikeiTyoukinJissekiJikanYosoku();
            switch (Month)
            {
                case 1:
                    ruikei += dbManager.GetTyoukinRuikei(EmployeeCD, nendo, 12);
                    break;
                case 4:
                    break;
                default:
                    ruikei += dbManager.GetTyoukinRuikei(EmployeeCD, nendo, Month - 1);
                    break;
            }
            return ruikei;
        }

        /// <summary>
        /// 当月の累計法定外労働時間（予測）を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcToutsukiRuikeiHouteigaiJissekiJikanYosoku()
        {
            int time = 0;

            time += CalcGekkanTotalJitsuRoudoJikan();

            CalendarMaster.ForEach(d02 =>
            {
                try
                {
                    if (!KinmuJisseki.Any(jisseki => jisseki.DATA_D == d02.DATA_D) && KinmuYotei.Any(yotei => yotei.DATA_D == d02.DATA_D))
                    {
                        KNS_D13 d13 = KinmuYotei.Single(yotei => yotei.DATA_D == d02.DATA_D);
                        if (d13.GetWorkTimeRange() == null) return;
                        int result = d13.GetWorkingTime() - 60;
                        if (0 < result) time += result;
                    }
                }
                catch (Exception e)
                {
                    logger.Warn(e);
                }
            });

            time -= CalcGekkanHouTeiRoudoJikan();

            return 0 < time ? time : 0;
        }

        /// <summary>
        /// 当年の累計法定外労働時間（予測）を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcTounenRuikeiHouteigaiJissekiJikanYosoku()
        {
            logger.Debug("start");
            int nendo = Month < 4 ? Year - 1 : Year;
            int ruikei = CalcToutsukiRuikeiHouteigaiJissekiJikanYosoku();
            switch (Month)
            {
                case 1:
                    ruikei += dbManager.GetHouteigaiRuikei(EmployeeCD, nendo, 12);
                    break;
                case 4:
                    break;
                default:
                    ruikei += dbManager.GetHouteigaiRuikei(EmployeeCD, nendo, Month - 1);
                    break;
            }
            return ruikei;
        }


        //36協定チェック欄

        /// <summary>
        /// 当月の累計超勤時間（36協定チェック欄・実績）を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcToutsukiRuikeiTyoukinJissekiJikan36Jisseki()
        {
            logger.Debug("start");
            int ruikei = CalcGekkanTankaBJikan() + CalcGekkanTankaTokkyuDJikan();
            return ruikei;
        }

        /// <summary>
        /// 当月の累計超勤時間（36協定チェック欄・予測）を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcToutsukiRuikeiTyoukinJissekiJikan36Yosoku()
        {
            logger.Debug("start");
            int ruikei = CalcGekkanTankaBJikanYosoku() + CalcGekkanTankaTokkyuDJikanYosoku();
            return ruikei;
        }

        /// <summary>
        /// 当年の累計超勤時間（36協定チェック欄・実績）を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcTounenRuikeiTyoukinJissekiJikan36Jisseki()
        {
            logger.Debug("start");
            int nendo = Month < 4 ? Year - 1 : Year;
            return dbManager.GetTyoukinRuikei36(EmployeeCD, nendo, Month);
        }

        /// <summary>
        /// 当年の累計超勤時間（36協定チェック欄・予測）を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcTounenRuikeiTyoukinJissekiJikan36Yosoku()
        {
            logger.Debug("start");
            int nendo = Month < 4 ? Year - 1 : Year;
            int ruikei = CalcToutsukiRuikeiTyoukinJissekiJikan36Yosoku();
            switch (Month)
            {
                case 1:
                    ruikei += dbManager.GetTyoukinRuikei36(EmployeeCD, nendo, 12);
                    break;
                case 4:
                    break;
                default:
                    ruikei += dbManager.GetTyoukinRuikei36(EmployeeCD, nendo, Month - 1);
                    break;
            }
            return ruikei;
        }

        /// <summary>
        /// 当月の累計法定外労働時間（36協定チェック欄・実績）を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcToutsukiRuikeiHouteigaiJissekiJikan36Jisseki()
        {
            int time = CalcGekkanTotalJitsuRoudoJikan() - CalcGekkanTankaKoukyuDJikan() - CalcGekkanHouTeiRoudoJikan();
            return 0 < time ? time : 0;
        }

        /// <summary>
        /// 当月の累計法定外労働時間（36協定チェック欄・予測）を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcToutsukiRuikeiHouteigaiJissekiJikan36Yosoku()
        {
            int time = 0;

            time += CalcGekkanTotalJitsuRoudoJikan();

            CalendarMaster.ForEach(d02 =>
            {
                try
                {
                    if (!KinmuJisseki.Any(jisseki => jisseki.DATA_D == d02.DATA_D) && KinmuYotei.Any(yotei => yotei.DATA_D == d02.DATA_D))
                    {
                        KNS_D13 d13 = KinmuYotei.Single(yotei => yotei.DATA_D == d02.DATA_D);
                        if (d13.GetWorkTimeRange() == null) return;
                        if (d13.YOTEI_CD == 公休.ToZeroPaddingString()) return;
                        int result = d13.GetWorkingTime() - 60;
                        if (0 < result) time += result;
                    }
                }
                catch (Exception e)
                {
                    logger.Warn(e);
                }
            });

            time -= CalcGekkanHouTeiRoudoJikan();

            return 0 < time ? time : 0;
        }

        /// <summary>
        /// 当年の累計法定外労働時間（36協定チェック欄・実績）を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcTounenRuikeiHouteigaiJissekiJikan36Jisseki()
        {
            logger.Debug("start");
            int nendo = Month < 4 ? Year - 1 : Year;
            return dbManager.GetHouteigaiRuikei36(EmployeeCD, nendo, Month);
        }

        /// <summary>
        /// 当年の累計法定外労働時間（36協定チェック欄・予測）を算出します。
        /// </summary>
        /// <returns></returns>
        public int CalcTounenRuikeiHouteigaiJissekiJikan36Yosoku()
        {
            logger.Debug("start");
            int nendo = Month < 4 ? Year - 1 : Year;
            int ruikei = CalcToutsukiRuikeiHouteigaiJissekiJikan36Yosoku();
            switch (Month)
            {
                case 1:
                    ruikei += dbManager.GetHouteigaiRuikei36(EmployeeCD, nendo, 12);
                    break;
                case 4:
                    break;
                default:
                    ruikei += dbManager.GetHouteigaiRuikei36(EmployeeCD, nendo, Month - 1);
                    break;
            }
            return ruikei;

        }

        #endregion

        #region 勤務入力系
#pragma warning disable S3776
        /// <summary>
        /// 勤務実績を更新します
        /// </summary>
        /// <param name="ptr_d01"></param>
        /// <returns></returns>
        /// <exception cref="KinmuException"></exception>
        public static Boolean ExecuteUpdate(ref KNS_D01 ptr_d01)
#pragma warning restore S3776
        {
            logger.Debug(LOG_START);
            ptr_d01.CheckValidationForForm();

            DBManager db = new DBManager();
            DateTime today = ptr_d01.GetDate();
            DateTime yesterday = today.AddDays(-1);
            DateTime tomorrow = today.AddDays(1);
            KNS_D01 y, t;

            try { y = db.GetKinmuJisseki(ptr_d01.SHAIN_CD, yesterday.Year, yesterday.Month, yesterday.Day).First(); }
            catch (Exception) { y = null; }
            try { t = db.GetKinmuJisseki(ptr_d01.SHAIN_CD, tomorrow.Year, tomorrow.Month, tomorrow.Day).First(); }
            catch (Exception) { t = null; }

            // 認証コード（確定）があるか確認する。
            if (!IsExistNinsyoCD(ptr_d01.NINKA_CD))
            {
                throw new KinmuException("指定された認証確定コードは認証コードマスタに存在しませんでした。");
            }

            // 前後日で勤務実績の重複がないか確認
            CheckOverlapWorkTime(ptr_d01, y);
            CheckOverlapWorkTime(ptr_d01, t);

            // 年休が暦日で取られているか確認
            CheckNenkyuRule(ptr_d01, y);
            CheckNenkyuRule(ptr_d01, t);

            // 日別勤務時間
            ptr_d01.DKIJYUN_MIN = ptr_d01.GetWorkTimeRange() == null ? "0" : SYOTEI_ROUDO_MIN.ToString();

            // 必要休憩時間の割り出し
            TimeRange tr = ptr_d01.GetWorkTimeRange();
            int minRestTime = tr == null ? 0 : GetMinimumRequiredRestTime(tr.GetRangeMinutes());

            // 入力された休憩全てが労基法以下の休憩時間の場合エラー
            if (ptr_d01.GetTotalRestTime() < minRestTime)
            {
                throw new KinmuException("現在の勤務時間の場合、最低でも" + minRestTime + "分の休憩が必要です。");
            }

            // 勤務時間
            ptr_d01.DKINM = ptr_d01.GetWorkingTime();

            // みなし1
            ptr_d01.DMINA1 = 0;
            // 02：出張、05：赴任、06：着任
            if (ptr_d01.NINKA_CD == 出張.ToZeroPaddingString() || ptr_d01.NINKA_CD == 赴任.ToZeroPaddingString() || ptr_d01.NINKA_CD == 着任.ToZeroPaddingString())
            {
                if (ptr_d01.DKINM < 460) { throw new KinmuException("勤務時間の合計が7時間40分になるように入力してください。"); }
                ptr_d01.DMINA1 = 460;
                ptr_d01.DKINM -= 460;
            }

            // みなし2
            ptr_d01.DMINA2 = CalcMinashi2(ref ptr_d01);

            // 超勤D
            ptr_d01.DCHOD = CalcTankaD(ref ptr_d01);

            // 超勤B
            int? _b = ptr_d01.DKINM - ptr_d01.DCHOD;
            ptr_d01.DCHOB = _b;

            // 超勤C（夜勤）
            ptr_d01.DYAKN = CalcTankaC(ref ptr_d01);

            // 祝日時間
            ptr_d01.DSHUK = CalcSyukujituC(ref ptr_d01);

            // 日別確認フラグ=1
            ptr_d01.KAKN_FLG = "1";

            // 更新時刻
            ptr_d01.UPD_DATE = DateTime.Now;

            // 更新者
            if (String.IsNullOrWhiteSpace(ptr_d01.UPD_SHAIN_CD))
            {
                ptr_d01.UPD_SHAIN_CD = ptr_d01.SHAIN_CD;
            }

            try
            {
                bool isSuccess = 0 < db.SetKinmuJisseki(new List<KNS_D01>() { ptr_d01 });

                // 本日の更新が成功かつ昨日の勤務が日付跨ぎの場合
                if (isSuccess && y != null && y.END_PAR == "1")
                {
                    // 前月末日が日付跨がりの場合エラーとする（当日の勤務は更新するが、前日の勤務を更新しない。）
                    if (today.Day == 1)
                    {
                        throw new KinmuException("勤務実績を更新しました。前月末日が日付跨がりの勤務のため勤務認証を既定から変更している場合、単価計算に矛盾が発生している可能性があります。");
                    }
                    return ExecuteUpdate(ref y);
                }
                return isSuccess;
            }
            catch (KinmuException)
            {
                throw;
            }
            catch (Exception e)
            {
                string mes = e.Message;
                Exception tmp = e;
                for (int i = 0; i < 5; i++)
                {
                    mes = tmp.Message;
                    if (tmp.InnerException == null) break;
                    tmp = tmp.InnerException;
                }
                logger.Error(mes);
                throw new KinmuException("勤務実績の更新時にエラーが発生しました。", e);
            }
        }

        /// <summary>
        /// 2つの勤務実績のの重複を確認します。重複の場合は例外が発生します。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <exception cref="KinmuException"></exception>
        public static void CheckOverlapWorkTime(KNS_D01 source, KNS_D01 target)
        {
            logger.Debug(LOG_START);
            if (source == null || target == null) return;

            TimeRange b_wtr = source.GetWorkTimeRange();
            TimeRange n_wtr = target.GetWorkTimeRange();

            if (IsOverlapWorkTime(source, target))
            {
                string mes = string.Format(
                    "{0}と{1}の勤務時間が重複しています。",
                    b_wtr.ToString("{0:M/d H:mm}～{1:H:mm}"),
                    n_wtr.ToString("{0:M/d H:mm}～{1:H:mm}")
                );
                throw new KinmuException(mes);
            }
        }

        /// <summary>
        /// 2つの勤務実績で年休が正しく取得されているかを確認します。暦日で年休が取得されていない場合は例外が発生します。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <exception cref="KinmuException"></exception>
        public static void CheckNenkyuRule(KNS_D01 source, KNS_D01 target)
        {
            logger.Debug(LOG_START);
            if (source == null || target == null) return;
#pragma warning disable S2234 // Parameters should be passed in the correct order
            if (target.GetDate() < source.GetDate()) CheckNenkyuRule(target, source);
#pragma warning restore S2234 // Parameters should be passed in the correct order

            TimeRange b_wtr = source.GetWorkTimeRange();

            if (b_wtr != null && source.END_PAR == "1" && target.NINKA_CD == 年休.ToZeroPaddingString() && b_wtr.Last.ToString("HHmm") != "0000")
            {
                string mes = string.Format(
                    "年休は原則暦日で取得しなければなりません。年休日：{0:M/d}、重複勤務実績{1}",
                    target.GetDate(),
                    b_wtr.ToString("{0:M/d H:mm}～{1:H:mm}")
                );
                throw new KinmuException(mes);
            }
        }

        /// <summary>
        /// 引数の<see cref="KNS_D01"/>に対応したみなし2を計算します。
        /// </summary>
        /// <param name="ptr_d01"></param>
        /// <returns></returns>
        public static int CalcMinashi2(ref KNS_D01 ptr_d01)
        {
            logger.Debug(LOG_START);
            DateTime b, l, today = ptr_d01.GetDate();
            TimeRange tr;
            TimeRange coretr = new TimeRange(today.AddMinutes(CORE_TIME_BEGIN), today.AddMinutes(CORE_TIME_END));
            TimeRange resttr = new TimeRange(today.AddMinutes(REST_TIME_BEGIN), today.AddMinutes(REST_TIME_END));

            switch ((NinsyoCD)int.Parse(ptr_d01.NINKA_CD))
            {
                case 年休:
                case 忌引:
                case 静養Ａ:
                case 移転:
                case 結婚:
                case 裁員:
                case 免除:
                    return SYOTEI_ROUDO_MIN;

                case AM半休:
                    b = today.AddMinutes(SYOTEI_ROUDO_BEGIN);
                    l = b.AddMinutes(HANKYU_HUYO_TIME + 60);
                    tr = new TimeRange(b, l);
                    if (0 < tr.GetOverlapMinutes(ptr_d01.GetWorkTimeRange())) throw new KinmuException("AM半休時間と勤務時間が重複しています。");
                    return HANKYU_HUYO_TIME;

                case PM半休:
                    l = today.AddMinutes(SYOTEI_ROUDO_END);
                    b = l.AddMinutes(-HANKYU_HUYO_TIME);
                    tr = new TimeRange(b, l);
                    if (0 < tr.GetOverlapMinutes(ptr_d01.GetWorkTimeRange())) throw new KinmuException("PM半休時間と勤務時間が重複しています。");
                    return HANKYU_HUYO_TIME;

                case り災:
                case 障害:
                case 召喚:
                case 選挙:
                    // 日単位の付与パターン（選挙は日単位の付与はない）
                    if (ptr_d01.GetWorkTimeRange() == null && ptr_d01.NINKA_CD != 選挙.ToZeroPaddingString())
                    {
                        return SYOTEI_ROUDO_MIN;
                    }
                    // 必要時間付与パターン
                    // コアタイムから重複している昼休憩と拘束時間を引き、昼休憩と拘束時間の重複分を足す
                    // <-----------ｺｱﾀｲﾑ----------->                <= coretr.GetRangeMinutes()
                    //            <-昼->                            <= coretr.GetOverlapMinutes(resttr)
                    //               <------拘束時間------>
                    //               <------------>                 <= coretr.GetOverlapMinutes(ptr_d01.GetWorkTimeRange())
                    //               <->                            <= resttr.GetOverlapMinutes(ptr_d01.GetWorkTimeRange())
                    // <-みなし2->
                    return coretr.GetRangeMinutes() - coretr.GetOverlapMinutes(resttr) - coretr.GetOverlapMinutes(ptr_d01.GetWorkTimeRange()) + resttr.GetOverlapMinutes(ptr_d01.GetWorkTimeRange());

                case 育児:
                    // 第66条(6)によると1日1時間なので現行の所定労働時間を返す仕様は誤り
                    // 本来であれば1時間or30分×2回の判定をするべきだが、今回はしない。
                    return 60;

                case 通院:
                    // 1日最大4時間、そして週に1回という規定であるが
                    // ここではコアタイム内の4時間までの判定のみ対応
                    int tmp = coretr.GetRangeMinutes() - coretr.GetOverlapMinutes(resttr) - coretr.GetOverlapMinutes(ptr_d01.GetWorkTimeRange()) + resttr.GetOverlapMinutes(ptr_d01.GetWorkTimeRange());
                    if (240 < tmp) tmp = 240;
                    return tmp;

                default:
                    return 0;
            }
        }

        /// <summary>
        /// 勤務実績の重複を確認します
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Boolean IsOverlapWorkTime(KNS_D01 source, KNS_D01 target)
        {
            logger.Debug(LOG_START);
            if (source == null || target == null) return false;
            TimeRange str = source.GetWorkTimeRange();
            if (str == null) return false;
            TimeRange ttr = target.GetWorkTimeRange();
            return str.IsOverlap(ttr);
        }

        /// <summary>
        /// 勤務予定を更新します
        /// </summary>
        /// <param name="ptr_d13"></param>
        /// <returns></returns>
        public static Boolean ExecuteUpdate(ref KNS_D13 ptr_d13)
        {
            logger.Debug(LOG_START);
            ptr_d13.CheckValidationForForm();

            // 認証コード（予定）があるか確認する。
            if (!IsExistNinsyoCD(ptr_d13.YOTEI_CD))
            {
                throw new KinmuException("指定された認証予定コードは認証コードマスタに存在しませんでした。");
            }

            // 更新時刻
            ptr_d13.UPD_DATE = DateTime.Now;

            // 更新者
            if (String.IsNullOrWhiteSpace(ptr_d13.UPD_SHAIN_CD))
            {
                ptr_d13.UPD_SHAIN_CD = ptr_d13.SHAIN_CD;
            }

            try
            {
                DBManager _db = new DBManager();
                return 0 < _db.SetKinmuYotei(new List<KNS_D13>() { ptr_d13 });
            }
            catch (Exception e)
            {
                string mes = e.Message;
                Exception tmp = e;
                for (int i = 0; i < 5; i++)
                {
                    mes = tmp.Message;
                    if (tmp.InnerException == null) break;
                    tmp = tmp.InnerException;
                }
                logger.Error(mes);
                throw new KinmuException("勤務予定の更新時にエラーが発生しました。", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr_d02s"></param>
        /// <returns></returns>
        public static Boolean ExecuteUpdate(ref List<KNS_D02> ptr_d02s)
        {
            logger.Debug(LOG_START);
            if (!ptr_d02s.Any()) return true;

            DBManager db = new DBManager();
            UserSetting __userSetting = db.GetUserSetting(ptr_d02s.First().SHAIN_CD);

            foreach (KNS_D02 d02 in ptr_d02s)
            {
                d02.CheckValidationForForm();
                string __pjcd = d02.PROJ_CD;
                string __sgcd = d02.SAGYO_CD;

                if (__userSetting.EmployeeCD != d02.SHAIN_CD) __userSetting = db.GetUserSetting(d02.SHAIN_CD);

                // 個人設定にコードが存在するか確認する。
                // エンティティ側では個人設定の値と比較できないため、マネージャ側で処理をする。
                if (!__userSetting.PJMasterList.Any(_ => _.PROJ_CD == __pjcd)) throw new KinmuException("指定されたPJコードは個人設定に存在しませんでした。");
                if (!__userSetting.SagyoCDMasterList.Any(_ => _.SAGYO_CD == __sgcd)) throw new KinmuException("指定された作業コードは個人設定に存在しませんでした。");
            }

            // 更新時刻を上書き
            ptr_d02s.ForEach(_ => _.UPD_DATE = DateTime.Now);

            try
            {
                DBManager _db = new DBManager();
                return 0 < _db.SetSagyoNisshi(ptr_d02s);
            }
            catch (Exception e)
            {
                string mes = e.Message;
                Exception tmp = e;
                for (int i = 0; i < 5; i++)
                {
                    mes = tmp.Message;
                    if (tmp.InnerException == null) break;
                    tmp = tmp.InnerException;
                }
                logger.Error(mes);
                throw new KinmuException("作業日誌の更新時にエラーが発生しました。", e);
            }
        }
        #endregion

        #region staticメソッド
        /// <summary>
        /// 超勤Dの計算を行います
        /// </summary>
        /// <param name="ptr_d01"></param>
        /// <returns></returns>
        public static int CalcTankaD(ref KNS_D01 ptr_d01)
        {
            logger.Debug(LOG_START);
            int tankaD = 0;
            TimeRange today, tomorrow;

            // 当日と翌日の時間帯を作成する
            try
            {
                today = new TimeRange(ptr_d01.GetDate(), ptr_d01.GetDate() + TimeSpan.FromDays(1));
                tomorrow = new TimeRange(ptr_d01.GetDate() + TimeSpan.FromDays(1), ptr_d01.GetDate() + TimeSpan.FromDays(2));
            }
            catch (FormatException e)
            {
                throw new KinmuException("勤務実績の日付が正しい文字列ではないようです。", e);
            }

            // 当日の認証コードが特休or公休or代休なら0:00～24:00で働いた時間がD時間
            if (ptr_d01.NINKA_CD == 代休.ToZeroPaddingString() || ptr_d01.NINKA_CD == 公休.ToZeroPaddingString() || ptr_d01.NINKA_CD == 特休.ToZeroPaddingString())
            {
                tankaD += today.GetOverlapMinutes(ptr_d01);
            }

            // 翌日フラグ==1かつ翌日の認証コードが特休or公休or代休なら24:00～48:00で働いた時間がD時間
            if (ptr_d01.END_PAR == "1")
            {
                string _nextCD = DBManager.GetNiinsyoCD(ptr_d01.SHAIN_CD, tomorrow.Begin.Year, tomorrow.Begin.Month, tomorrow.Begin.Day);
                if (_nextCD == 代休.ToZeroPaddingString() || _nextCD == 公休.ToZeroPaddingString() || _nextCD == 特休.ToZeroPaddingString())
                {
                    tankaD += tomorrow.GetOverlapMinutes(ptr_d01);
                }
            }

            return tankaD;
        }

        /// <summary>
        /// 祝日C時間を計算します。
        /// </summary>
        /// <param name="ptr_d01"></param>
        /// <returns></returns>
        public static int CalcSyukujituC(ref KNS_D01 ptr_d01)
        {
            logger.Debug(LOG_START);
            int syukujitsu = 0;
            TimeRange today, tomorrow;

            // 当日と翌日の時間帯を作成する
            try
            {
                today = new TimeRange(ptr_d01.GetDate(), ptr_d01.GetDate() + TimeSpan.FromDays(1));
                tomorrow = new TimeRange(ptr_d01.GetDate() + TimeSpan.FromDays(1), ptr_d01.GetDate() + TimeSpan.FromDays(2));
            }
            catch (FormatException e)
            {
                throw new KinmuException("勤務実績の日付が正しい文字列ではないようです。", e);
            }

            // 当日が祝日なら0:00～24:00で働いた時間がD時間
            if (DBManager.IsHoliday(today.Begin.Year, today.Begin.Month, today.Begin.Day))
            {
                syukujitsu += today.GetOverlapMinutes(ptr_d01);
            }

            // 翌日フラグ==1かつ翌日が祝日なら24:00～48:00で働いた時間がD時間
            if (ptr_d01.END_PAR == "1" && DBManager.IsHoliday(tomorrow.Begin.Year, tomorrow.Begin.Month, tomorrow.Begin.Day))
            {
                syukujitsu += tomorrow.GetOverlapMinutes(ptr_d01);
            }

            return syukujitsu;
        }

        /// <summary>
        /// 勤務実績の勤務時間と作業日誌の勤務時間が一致するか確認します。
        /// </summary>
        /// <param name="ptr_d01">入力された勤務実績</param>
        /// <param name="ptr_d02">入力された作業日誌</param>
        /// <exception cref="KinmuException"></exception>
        public static void ValidationD01AndD02(ref KNS_D01 ptr_d01, ref List<KNS_D02> ptr_d02)
        {
            logger.Debug(LOG_START);
            int sagyoutime = 0;
            foreach (var d02 in ptr_d02)
            {
                sagyoutime += d02.SAGYO_MIN;
            }

            if (ptr_d01.GetWorkingTime() != sagyoutime)
            {
                throw new KinmuException("勤務時間と作業時間の合計値が一致しませんでした。");
            }
        }

        /// <summary>
        /// 夜勤C時間を計算します。
        /// </summary>
        /// <param name="ptr_d01"></param>
        /// <returns></returns>
        public static int CalcTankaC(ref KNS_D01 ptr_d01)
        {
            logger.Debug(LOG_START);
            DateTime _thisDay = ptr_d01.GetDate();
            int _yakin = 0;

            TimeRange[] _cRanges = {
                new TimeRange(_thisDay + TimeSpan.FromHours(0) , _thisDay + TimeSpan.FromHours(5)),
                new TimeRange(_thisDay + TimeSpan.FromHours(22), _thisDay + TimeSpan.FromHours(29)),
                new TimeRange(_thisDay + TimeSpan.FromHours(46), _thisDay + TimeSpan.FromHours(48))
            };

            for (int i = 0; i < _cRanges.Length; i++)
            {
                // 勤務時間分を夜勤に追加
                _yakin += _cRanges[i].GetOverlapMinutes(ptr_d01);
            }

            return _yakin;
        }

        /// <summary>
        /// 単価B時間の計算を高速で行います。
        /// </summary>
        /// <param name="d01s"></param>
        /// <returns></returns>
        public static int CalcFastGekkanTankaBJikan(List<KNS_D01> d01s)
        {
            logger.Debug(LOG_START);
            int time = 0;
            d01s.ForEach(_ =>
            {
                time += (_.DKINM ?? 0) + (_.DMINA1 ?? 0) - (_.DCHOD ?? 0);
                time += _.GetWorkTimeRange() != null ? (_.DMINA2 ?? 0) : 0;
                if (!string.IsNullOrWhiteSpace(_.DKIJYUN_MIN))
                {
                    time -= int.Parse(_.DKIJYUN_MIN);
                }
            });
            return time;
        }

        /// <summary>
        /// 認証コードがマスターに存在するか確認します。存在した場合はtrue、存在しない場合はfalseを返します。
        /// </summary>
        /// <param name="_cd"></param>
        /// <returns></returns>
        public static bool IsExistNinsyoCD(string _cd)
        {
            logger.Debug(LOG_START);
            DBManager DB = new DBManager();
            return DB.GetKinmuNinsyoCDMasterALL().Any(_ => _.NINSHO_CD == _cd);
        }

        /// <summary>
        /// 法定で定められた必要最低限の休憩時間を返します。
        /// </summary>
        /// <param name="_workTime"></param>
        /// <returns></returns>
        public static int GetMinimumRequiredRestTime(int _workTime)
        {
            logger.Debug(LOG_START);
            if (8 * 60 < _workTime)
            {
                return 60;
            }
            else if (6 * 60 < _workTime)
            {
                return 45;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 超勤Bを高速で計算する際に渡すオブジェクトを作成します。
        /// </summary>
        /// <param name="kinmuRecords"></param>
        /// /// <returns></returns>
        public static List<KNS_D01> CallB(List<KinmuRecordRow> kinmuRecords)
        {
            logger.Debug(LOG_START);
            List<KNS_D01> d01s = new List<KNS_D01>();

            foreach (var item in kinmuRecords)
            {
                if (item.KinmuJisseki.DATA_Y != null)
                {
                    d01s.Add(item.KinmuJisseki);
                    continue;
                }
                if (item.KinmuYotei.DATA_Y == null)
                {
                    KNS_D01 jisseki = new KNS_D01
                    {
                        DATA_Y = item.CalendarMaster.DATA_Y,
                        DATA_M = item.CalendarMaster.DATA_M,
                        DATA_D = item.CalendarMaster.DATA_D,

                        DKINM = 460,
                        DKIJYUN_MIN = "460"
                    };

                    d01s.Add(jisseki);
                }
                else
                {
                    KNS_D01 jisseki = new KNS_D01
                    {
                        DATA_Y = item.CalendarMaster.DATA_Y,
                        DATA_M = item.CalendarMaster.DATA_M,
                        DATA_D = item.CalendarMaster.DATA_D,

                        STR_HR = item.KinmuYotei.STR_Y_HR,
                        STR_MIN = item.KinmuYotei.STR_Y_MIN,
                        END_HR = item.KinmuYotei.END_Y_HR,
                        END_MIN = item.KinmuYotei.END_Y_MIN,

                        RESTS1_HR = (REST_TIME_BEGIN / 60).ToString(),
                        RESTS1_MIN = (REST_TIME_BEGIN % 60).ToString(),
                        RESTE1_HR = (REST_TIME_END / 60).ToString(),
                        RESTE1_MIN = (REST_TIME_END % 60).ToString(),

                        DKIJYUN_MIN = "460"
                    };

                    if (item.KinmuYotei.YOTEI_CD == 年休.ToZeroPaddingString())
                    {
                        jisseki.DMINA2 = 460;
                        jisseki.DKIJYUN_MIN = "0";
                    }
                    else if (item.KinmuYotei.YOTEI_CD == AM半休.ToZeroPaddingString() || item.KinmuYotei.YOTEI_CD == PM半休.ToZeroPaddingString())
                    {
                        jisseki.DMINA2 = 240;
                        jisseki.DKINM = jisseki.GetWorkingTime();
                    }
                    else
                    {
                        jisseki.DKINM = jisseki.GetWorkingTime();
                    }

                    d01s.Add(jisseki);
                }
            }

            return d01s;
        }
        #endregion
    }
}
