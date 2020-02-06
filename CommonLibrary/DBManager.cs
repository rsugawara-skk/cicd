using System;
using System.Collections.Generic;
using System.Linq;
using CommonLibrary.Models;
using CommonLibrary;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Globalization;
using static CommonLibrary.CommonDefine;
using static CommonLibrary.CommonDefine.NinsyoCD;

namespace CommonLibrary
{
    /// <summary>
    /// Entity Flameworkで作成したEntityを操作するマネージャーです。
    /// DBへのアクセスはこのマネージャーを経由して行ってください。
    /// なお、例外処理は行っていないため、呼び出し側のライブラリ、またはViewで適切に処理してください。
    /// </summary>
    public class DBManager : IDBManager, IDBManagerUtility
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        #region 勤務実績
        /// <summary>
        /// 全社員の当月分の勤務実績を取得します。処理は<see cref="GetKinmuJisseki(string, int, int, int)"/>に委譲しています。
        /// 認証コードなどはコードで返されるため、別途マスターを読み込み、プログラム側で値と名称を紐づけてください。
        /// </summary>
        /// <returns>勤務実績リスト</returns>
        public List<KNS_D01> GetKinmuJisseki()
        {
            logger.Debug(LOG_START);
            return GetKinmuJisseki(null, new DateTime().Year, new DateTime().Month, 0);
        }

        /// <summary>
        /// 全社員の指定月分の勤務実績を取得します。処理は<see cref="GetKinmuJisseki(string, int, int, int)"/>に委譲しています。
        /// 認証コードなどはコードで返されるため、別途マスターを読み込み、プログラム側で値と名称を紐づけてください。
        /// </summary>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <returns>勤務実績リスト</returns>
        public List<KNS_D01> GetKinmuJisseki(int _year, int _month)
        {
            logger.Debug(LOG_START);
            return GetKinmuJisseki(null, _year, _month, 0);
        }

        /// <summary>
        /// 全社員の指定日分の勤務実績を取得します。処理は<see cref="GetKinmuJisseki(string, int, int, int)"/>に委譲しています。
        /// 認証コードなどはコードで返されるため、別途マスターを読み込み、プログラム側で値と名称を紐づけてください。
        /// </summary>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <param name="_day">取得対象の日</param>
        /// <returns>勤務実績リスト</returns>
        public List<KNS_D01> GetKinmuJisseki(int _year, int _month, int _day)
        {
            logger.Debug(LOG_START);
            return GetKinmuJisseki(null, _year, _month, _day);
        }

        /// <summary>
        /// 指定社員の当月分の勤務実績を取得します。処理は<see cref="GetKinmuJisseki(string, int, int, int)"/>に委譲しています。
        /// 認証コードなどはコードで返されるため、別途マスターを読み込み、プログラム側で値と名称を紐づけてください。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <returns>勤務実績リスト</returns>
        public List<KNS_D01> GetKinmuJisseki(string _employeeCD)
        {
            logger.Debug(LOG_START);
            return GetKinmuJisseki(_employeeCD, new DateTime().Year, new DateTime().Month, 0);
        }

        /// <summary>
        /// 指定社員の指定月分の勤務実績を取得します。処理は<see cref="GetKinmuJisseki(string, int, int, int)"/>に委譲しています。
        /// 認証コードなどはコードで返されるため、別途マスターを読み込み、プログラム側で値と名称を紐づけてください。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <returns>勤務実績リスト</returns>
        public List<KNS_D01> GetKinmuJisseki(string _employeeCD, int _year, int _month)
        {
            logger.Debug(LOG_START);
            return GetKinmuJisseki(_employeeCD, _year, _month, 0);
        }

        /// <summary>
        /// 指定社員の指定日分の勤務実績を取得します。ほかにオーバーライドしているメソッドはすべてこのメソッドに処理を委譲しています。
        /// 認証コードなどはコードで返されるため、別途マスターを読み込み、プログラム側で値と名称を紐づけてください。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード。nullを指定すると全社員の勤務実績リストを取得します。</param>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <param name="_day">取得対象の日。0を指定すると指定年月のすべての勤務実績リストを取得します。</param>
        /// <returns>勤務実績リスト</returns>
        public List<KNS_D01> GetKinmuJisseki(string _employeeCD, int _year, int _month, int _day)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                // ゼロ埋め対応
                string y = _year.ToString(PADDING_ZERO_4);
                string m = _month.ToString(PADDING_ZERO_2);
                string d = _day.ToString(PADDING_ZERO_2);

                // 年月は共通条件なので、まずはそれで引っ掛ける
                IQueryable<KNS_D01> d01 = DB.KNS_D01.Where(_d01 => _d01.DATA_Y == y && _d01.DATA_M == m);

                // 社員コードの指定があれば、さらにそれで絞り込む
                if (_employeeCD != null) { d01 = d01.Where(_d01 => _d01.SHAIN_CD == _employeeCD); }

                // 日付指定があれば、さらにそれで絞り込む
                if (_day != 0) { d01 = d01.Where(_d01 => _d01.DATA_D == d); }

                // 社員コード→年→月→日の順で昇順にソート
                d01 = d01.OrderBy(_ => _.SHAIN_CD).ThenBy(_ => _.DATA_Y).ThenBy(_ => _.DATA_M).ThenBy(_ => _.DATA_D);

                // LINQを高速に動かすため、最後にList化
                return d01.ToList();
            }
        }

        /// <summary>
        /// 勤務実績を追加します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d01s">追加する勤務実績リスト。1件でもList化してください。</param>
        /// <returns>更新レコード件数</returns>
        public int InsertKinmuJisseki(List<KNS_D01> _d01s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D01 item in _d01s)
                {
                    DB.KNS_D01.Add(item);
                    DB.Entry(item).State = EntityState.Added;
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 勤務実績を更新します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d01s">更新する勤務実績リスト。1件でもList化してください。</param>
        /// <returns>更新レコード件数</returns>
        public int UpdateKinmuJisseki(List<KNS_D01> _d01s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D01 item in _d01s)
                {
                    DB.KNS_D01.Attach(item);
                    DB.Entry(item).State = EntityState.Modified;
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 勤務実績を削除します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d01s">削除する勤務実績リスト。1件でもList化してください。</param>
        /// <returns>削除レコード件数</returns>
        public int DeleteKinmuJisseki(List<KNS_D01> _d01s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D01 item in _d01s)
                {
                    // 主キーで検索をかけているので、基本は1件（=Single()でOK）である
                    KNS_D01 d01 = DB.KNS_D01.Single(_d01 => _d01.SHAIN_CD == item.SHAIN_CD && _d01.DATA_Y == item.DATA_Y && _d01.DATA_M == item.DATA_M && _d01.DATA_D == item.DATA_D);
                    DB.KNS_D01.Remove(d01);
                }
                return DB.SaveChanges();
            }
        }
        #endregion

        #region 勤務予定
        /// <summary>
        /// 全社員の当月分の勤務予定を取得します。処理は<see cref="GetKinmuYotei(string, int, int, int)"/>に委譲しています。
        /// 認証コードなどはコードで返されるため、別途マスターを読み込み、プログラム側で値と名称を紐づけてください。
        /// </summary>
        /// <returns>勤務予定リスト</returns>
        public List<KNS_D13> GetKinmuYotei()
        {
            logger.Debug(LOG_START);
            return GetKinmuYotei(null, new DateTime().Year, new DateTime().Month, 0);
        }

        /// <summary>
        /// 全社員の指定月分の勤務予定を取得します。処理は<see cref="GetKinmuYotei(string, int, int, int)"/>に委譲しています。
        /// 認証コードなどはコードで返されるため、別途マスターを読み込み、プログラム側で値と名称を紐づけてください。
        /// </summary>
        /// <param name="_year">指定年</param>
        /// <param name="_month">指定月</param>
        /// <returns>勤務予定リスト</returns>
        public List<KNS_D13> GetKinmuYotei(int _year, int _month)
        {
            logger.Debug(LOG_START);
            return GetKinmuYotei(null, _year, _month, 0);
        }

        /// <summary>
        /// 全社員の指定日分の勤務予定を取得します。処理は<see cref="GetKinmuYotei(string, int, int, int)"/>に委譲しています。
        /// 認証コードなどはコードで返されるため、別途マスターを読み込み、プログラム側で値と名称を紐づけてください。
        /// </summary>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <param name="_day">取得対象の日</param>
        /// <returns>勤務予定リスト</returns>
        public List<KNS_D13> GetKinmuYotei(int _year, int _month, int _day)
        {
            logger.Debug(LOG_START);
            return GetKinmuYotei(null, _year, _month, _day);
        }

        /// <summary>
        /// 指定社員の当月分の勤務予定を取得します。処理は<see cref="GetKinmuYotei(string, int, int, int)"/>に委譲しています。
        /// 認証コードなどはコードで返されるため、別途マスターを読み込み、プログラム側で値と名称を紐づけてください。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <returns>勤務予定リスト</returns>
        public List<KNS_D13> GetKinmuYotei(string _employeeCD)
        {
            logger.Debug(LOG_START);
            return GetKinmuYotei(_employeeCD, new DateTime().Year, new DateTime().Month, 0);
        }

        /// <summary>
        /// 指定社員の当月分の勤務予定を取得します。処理は<see cref="GetKinmuYotei(string, int, int, int)"/>に委譲しています。
        /// 認証コードなどはコードで返されるため、別途マスターを読み込み、プログラム側で値と名称を紐づけてください。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <returns>勤務予定リスト</returns>
        public List<KNS_D13> GetKinmuYotei(string _employeeCD, int _year, int _month)
        {
            logger.Debug(LOG_START);
            return GetKinmuYotei(_employeeCD, _year, _month, 0);
        }

        /// <summary>
        /// 指定社員の指定日分の勤務予定を取得します。ほかにオーバーライドしているメソッドはすべてこのメソッドに処理を委譲しています。
        /// 認証コードなどはコードで返されるため、別途マスターを読み込み、プログラム側で値と名称を紐づけてください。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード。nullを指定すると全社員の勤務予定リストを取得します。</param>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <param name="_day">取得対象の日。0を指定すると指定年月のすべての勤務予定リストを取得します。</param>
        /// <returns>勤務予定リスト</returns>
        public List<KNS_D13> GetKinmuYotei(string _employeeCD, int _year, int _month, int _day)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                // ゼロ埋め対応
                string y = _year.ToString(PADDING_ZERO_4);
                string m = _month.ToString(PADDING_ZERO_2);
                string d = _day.ToString(PADDING_ZERO_2);

                // 共通条件の年月で引っ掛ける
                IQueryable<KNS_D13> d13 = DB.KNS_D13.Where(_d13 => _d13.DATA_Y == y && _d13.DATA_M == m);

                // 社員コードの指定があれば、それで絞り込む
                if (_employeeCD != null) { d13 = d13.Where(_d13 => _d13.SHAIN_CD == _employeeCD); }

                // 日付の指定があれば、それで絞り込む
                if (_day != 0) { d13 = d13.Where(_d13 => _d13.DATA_D == d); }

                // 社員コード→年→月→日の順で昇順にソート
                d13 = d13.OrderBy(_ => _.SHAIN_CD).OrderBy(_ => _.DATA_Y).OrderBy(_ => _.DATA_M).OrderBy(_ => _.DATA_D);

                // LINQを高速に動かすため、最後にList化する。
                return d13.ToList();
            }
        }

        /// <summary>
        /// 勤務予定を追加します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d13s">追加する勤務予定リスト。1件でもList化してください。</param>
        /// <returns>更新レコード件数</returns>
        public int InsertKinmuYotei(List<KNS_D13> _d13s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D13 item in _d13s)
                {
                    DB.KNS_D13.Add(item);
                    DB.Entry(item).State = EntityState.Added;
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 勤務予定を更新します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d13s">更新する勤務予定リスト。1件でもList化してください。</param>
        /// <returns>更新レコード件数</returns>
        public int UpdateKinmuYotei(List<KNS_D13> _d13s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D13 item in _d13s)
                {
                    DB.KNS_D13.Attach(item);
                    DB.Entry(item).State = EntityState.Modified;
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 勤務予定を削除します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d13s">削除する勤務予定リスト。1件でもList化してください。</param>
        /// <returns>削除レコード件数</returns>
        public int DeleteKinmuYotei(List<KNS_D13> _d13s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D13 item in _d13s)
                {
                    // 主キーで検索をかけているので基本は1件（=Single()でOK）
                    KNS_D13 d13 = DB.KNS_D13.Single(_d13 => _d13.SHAIN_CD == item.SHAIN_CD && _d13.DATA_Y == item.DATA_Y && _d13.DATA_M == item.DATA_M && _d13.DATA_D == item.DATA_D);
                    DB.KNS_D13.Remove(d13);
                }
                return DB.SaveChanges();
            }
        }
        #endregion

        #region 作業日誌
        /// <summary>
        /// 指定日の作業日誌を取得します。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <param name="_year">対象年</param>
        /// <param name="_month">対象月</param>
        /// <returns>作業日誌リスト。DBに何もない場合は空のリストが返されます。</returns>
        public List<KNS_D02> GetSagyoNisshi(string _employeeCD, int _year, int _month)
        {
            logger.Debug(LOG_START);
            return GetSagyoNisshi(_employeeCD, _year, _month, 0);
        }

        /// <summary>
        /// 指定日の作業日誌を取得します。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <param name="_year">対象年</param>
        /// <param name="_month">対象月</param>
        /// <param name="_day">対象日</param>
        /// <returns>作業日誌リスト。DBに何もない場合は空のリストが返されます。</returns>
        public List<KNS_D02> GetSagyoNisshi(string _employeeCD, int _year, int _month, int _day)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                // ゼロ埋め対応
                string y = _year.ToString(PADDING_ZERO_4);
                string m = _month.ToString(PADDING_ZERO_2);
                string d = _day.ToString(PADDING_ZERO_2);
                if (_employeeCD == null && _day == 0)
                {
                    return DB.KNS_D02.Where(d02 => d02.DATA_Y == y && d02.DATA_M == m).ToList();
                }
                else if (_employeeCD == null)
                {
                    return DB.KNS_D02.Where(d02 => d02.DATA_Y == y && d02.DATA_M == m && d02.DATA_D == d).ToList();
                }
                else if (_day == 0)
                {
                    return DB.KNS_D02.Where(d02 => d02.SHAIN_CD == _employeeCD && d02.DATA_Y == y && d02.DATA_M == m).ToList();
                }
                else
                {
                    return DB.KNS_D02.Where(d02 => d02.SHAIN_CD == _employeeCD && d02.DATA_Y == y && d02.DATA_M == m && d02.DATA_D == d).ToList();
                }
            }
        }

        /// <summary>
        /// 作業日誌を追加します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d02s">追加対象の作業日誌</param>
        /// <returns>更新レコード件数</returns>
        public int InsertSagyoNisshi(List<KNS_D02> _d02s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D02 item in _d02s)
                {
                    DB.KNS_D02.Add(item);
                    DB.Entry(item).State = EntityState.Added;
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 作業日誌を更新します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d02s">更新対象の作業日誌</param>
        /// <returns>更新レコード件数</returns>
        public int UpdateSagyoNisshi(List<KNS_D02> _d02s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D02 item in _d02s)
                {
                    DB.KNS_D02.Attach(item);
                    DB.Entry(item).State = EntityState.Modified;
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 作業日誌を削除します。1件であってもList化してメソッドを呼び出してください。
        /// 削除レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d02s">削除対象の作業日誌</param>
        /// <returns>削除レコード件数</returns>
        public int DeleteSagyoNisshi(List<KNS_D02> _d02s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D02 item in _d02s)
                {
                    // 主キーで検索しているので基本は1件(=Single()でOK)
                    KNS_D02 d02 = DB.KNS_D02.Single(_d02 => _d02.SHAIN_CD == item.SHAIN_CD && _d02.DATA_Y == item.DATA_Y && _d02.DATA_M == item.DATA_M && _d02.DATA_D == item.DATA_D && _d02.PROJ_CD == item.PROJ_CD && _d02.SAGYO_CD == item.SAGYO_CD);
                    DB.KNS_D02.Remove(d02);
                }
                return DB.SaveChanges();
            }
        }
        #endregion

        #region 個人設定
        /// <summary>
        /// 対象社員の個人設定（勤務実績）を取得します。
        /// 1件でもListで返されます。0件でも空のListで返されます。
        /// </summary>
        /// <param name="_employeeCD">対象の社員コード</param>
        /// <returns>個人設定（勤務実績）リスト</returns>
        public List<KNS_D04> GetKojinSetteiKinmuJisseki(string _employeeCD)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                return DB.KNS_D04.Where(_d04 => _d04.SHAIN_CD == _employeeCD).ToList();
            }
        }

        /// <summary>
        /// 個人設定（勤務実績）を追加します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d04s">追加対象の個人設定（勤務実績）</param>
        /// <returns>更新レコード件数</returns>
        public int InsertKojinSetteiKinmuJisseki(List<KNS_D04> _d04s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D04 item in _d04s)
                {
                    DB.KNS_D04.Add(item);
                    DB.Entry(item).State = EntityState.Added;
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 個人設定（勤務実績）を更新します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d04s">更新対象の個人設定（勤務実績）</param>
        /// <returns>更新レコード件数</returns>
        public int UpdateKojinSetteiKinmuJisseki(List<KNS_D04> _d04s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D04 item in _d04s)
                {
                    DB.KNS_D04.Attach(item);
                    DB.Entry(item).State = EntityState.Modified;
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 個人設定（勤務実績）を削除します。1件であってもList化してメソッドを呼び出してください。
        /// 削除レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d04s">削除対象の個人設定（勤務実績）</param>
        /// <returns>削除レコード件数</returns>
        public int DeleteKojinSetteiKinmuJisseki(List<KNS_D04> _d04s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D04 item in _d04s)
                {
                    // 主キーで検索しているので基本は1件（=Single()でOK）
                    KNS_D04 d04 = DB.KNS_D04.Single(_d04 => _d04.SHAIN_CD == item.SHAIN_CD && _d04.NINSE_CD == item.NINSE_CD);
                    DB.KNS_D04.Remove(d04);
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 個人設定（PJコード）を取得します。
        /// 1件でもListで返されます。0件でも空のListで返されます。
        /// </summary>
        /// <param name="_employeeCD">対象社員コード</param>
        /// <returns>個人設定（PJコード）リスト</returns>
        public List<KNS_D05> GetKojinSetteiPJCD(string _employeeCD)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                return DB.KNS_D05.Where(_d05 => _d05.SHAIN_CD == _employeeCD).ToList();
            }
        }

        /// <summary>
        /// 個人設定（PJコード）を追加します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d05s">追加する個人設定（PJコード）</param>
        /// <returns>更新レコード件数</returns>
        public int InsertKojinSetteiPJCD(List<KNS_D05> _d05s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D05 item in _d05s)
                {
                    DB.KNS_D05.Add(item);
                    DB.Entry(item).State = EntityState.Added;
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 個人設定（PJコード）を更新します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d05s">更新する個人設定（PJコード）</param>
        /// <returns>更新レコード件数</returns>
        public int UpdateKojinSetteiPJCD(List<KNS_D05> _d05s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D05 item in _d05s)
                {
                    DB.KNS_D05.Attach(item);
                    DB.Entry(item).State = EntityState.Modified;
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 個人設定（PJコード）を削除します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d05s">削除する個人設定（PJコード）</param>
        /// <returns>削除レコード件数</returns>
        public int DeleteKojinSetteiPJCD(List<KNS_D05> _d05s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D05 item in _d05s)
                {
                    // 主キーで検索しているので基本は1件（=Single()でOK）
                    KNS_D05 d05 = DB.KNS_D05.Single(_d05 => _d05.SHAIN_CD == item.SHAIN_CD && _d05.PROJ_CD == item.PROJ_CD && _d05.VIEW_ORDER == item.VIEW_ORDER);
                    DB.KNS_D05.Remove(d05);
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 個人設定（作業コード）を取得します。
        /// 1件でもListで返されます。0件でも空のListで返されます。
        /// </summary>
        /// <param name="_employeeCD">取得対象社員コード</param>
        /// <returns>個人設定（作業コード）リスト</returns>
        public List<KNS_D06> GetKojinSetteiSagyoCD(string _employeeCD)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                return DB.KNS_D06.Where(_d06 => _d06.SHAIN_CD == _employeeCD).ToList();
            }
        }

        /// <summary>
        /// 個人設定（作業コード）を追加します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d06s">追加する個人設定（作業コード）</param>
        /// <returns>更新レコード件数</returns>
        public int InsertKojinSetteiSagyoCD(List<KNS_D06> _d06s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D06 item in _d06s)
                {
                    DB.KNS_D06.Add(item);
                    DB.Entry(item).State = EntityState.Added;
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 個人設定（作業コード）を更新します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d06s">更新する個人設定（作業コード）</param>
        /// <returns>更新レコード件数</returns>
        public int UpdateKojinSetteiSagyoCD(List<KNS_D06> _d06s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D06 item in _d06s)
                {
                    DB.KNS_D06.Attach(item);
                    DB.Entry(item).State = EntityState.Modified;
                }
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// 個人設定（作業コード）を削除します。1件であってもList化してメソッドを呼び出してください。
        /// 更新レコードの件数確認は行いませんので、妥当性は呼び出し側で判断してください。
        /// </summary>
        /// <param name="_d06s">削除する個人設定（作業コード）</param>
        /// <returns>削除レコード件数</returns>
        public int DeleteKojinSetteiSagyoCD(List<KNS_D06> _d06s)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                foreach (KNS_D06 item in _d06s)
                {
                    KNS_D06 d06 = DB.KNS_D06.Single(_d06 => _d06.SHAIN_CD == item.SHAIN_CD && _d06.SAGYO_CD == item.SAGYO_CD && _d06.VIEW_ORDER == item.VIEW_ORDER);
                    DB.KNS_D06.Remove(d06);
                }
                return DB.SaveChanges();
            }
        }
        #endregion

        #region メッセージ・休暇・年休・最終ログイン
        /// <summary>
        /// お知らせメッセージの全量を取得します。
        /// </summary>
        /// <returns>HYOJI_FLGで昇順されたメッセージリスト</returns>
        public List<KNS_D08> GetMessageAll()
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                return DB.KNS_D08.OrderBy(_d08 => _d08.HYOJI_FLG).ToList();
            }
        }

        /// <summary>
        /// 対象社員、年度の特休・公休データを取得します。
        /// </summary>
        /// <param name="_employeeCD">対象社員コード</param>
        /// <param name="_nendo">対象年度</param>
        /// <returns>特休・公休データ</returns>
        public KNS_D11 GetTokkyuKokyuData(string _employeeCD, int _nendo)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                return DB.KNS_D11.Single(_d11 => _d11.SHAIN_CD == _employeeCD && _d11.NENDO == _nendo.ToString());
            }
        }

        /// <summary>
        /// 対象社員、年度の年休データを取得します。
        /// </summary>
        /// <param name="_employeeCD">対象社員</param>
        /// <param name="_nendo">対象年度</param>
        /// <returns>年休データ</returns>
        public KNS_D12 GetNenkyuData(string _employeeCD, int _nendo)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                return DB.KNS_D12.Single(_d12 => _d12.SHAIN_CD == _employeeCD && _d12.NENDO == _nendo.ToString());
            }
        }

        /// <summary>
        /// システムへの最終ログイン時刻を取得します。
        /// </summary>
        /// <param name="_employeeCD">取得対象社員コード</param>
        /// <returns>最終ログインレコード</returns>
        public KNS_D14 GetLastLogin(string _employeeCD)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                return DB.KNS_D14.Single(_d14 => _d14.SHAIN_CD == _employeeCD);
            }
        }

        /// <summary>
        /// システムへの最終ログイン時刻を追加します。
        /// </summary>
        /// <param name="_d14">最終ログインレコード</param>
        /// <returns>更新件数（1件になる見込み）</returns>
        public int InsertLastLogin(KNS_D14 _d14)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                DB.KNS_D14.Add(_d14);
                DB.Entry(_d14).State = EntityState.Added;
                return DB.SaveChanges();
            }
        }

        /// <summary>
        /// システムへの最終ログイン時刻を更新します。
        /// </summary>
        /// <param name="_d14">最終ログインレコード</param>
        /// <returns>更新件数（1件になる見込み）</returns>
        public int UpdateLastLogin(KNS_D14 _d14)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                DB.KNS_D14.Attach(_d14);
                DB.Entry(_d14).State = EntityState.Modified;
                return DB.SaveChanges();
            }
        }
        #endregion

        #region マスタ
        /// <summary>
        /// 有効な全社員のマスタデータを取得します。
        /// </summary>
        /// <returns>社員マスタリスト</returns>
        public List<KNS_M01> GetSyainMasterAll()
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                return DB.KNS_M01.Where(_m01 => _m01.IK_KUBUN == "1").ToList();
            }
        }

        /// <summary>
        /// 社員マスタから一致する社員コードのデータを取得します。
        /// </summary>
        /// <param name="_employeeCD">取得対象社員コード</param>
        /// <returns>社員マスタレコード</returns>
        public KNS_M01 GetSyainMasterByShainCD(string _employeeCD)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                return DB.KNS_M01.Single(_m01 => _m01.SHAIN_CD == _employeeCD);
            }
        }

        /// <summary>
        /// 社員マスタから一致するエイリアス（ドメインアカウント名）のデータを取得します。
        /// </summary>
        /// <param name="_alias">取得対象のエイリアス</param>
        /// <returns>社員マスタレコード</returns>
        public KNS_M01 GetSyainMasterByAlias(string _alias)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                // 主キーではないが、重複したら困る（業務例外エラーとする）
                return DB.KNS_M01.Single(_m01 => _m01.ALIAS == _alias);
            }
        }

        /// <summary>
        /// 社員マスタから一致する承認社員コードのデータを取得します。
        /// </summary>
        /// <param name="_employeeCD">承認社員（GMや部長など）の社員コード</param>
        /// <returns>社員マスタリスト</returns>
        public List<KNS_M01> GetSyainMasterByShoninShainCD(string _employeeCD)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                return DB.KNS_M01.Where(_m01 => _m01.SHONIN_SHAIN_CD == _employeeCD).ToList();
            }
        }

        /// <summary>
        /// PJコードの全量を取得します。
        /// </summary>
        /// <returns>PJコードリスト</returns>
        public List<KNS_M02> GetPJCDMasterAll()
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                return DB.KNS_M02.ToList();
            }
        }

        /// <summary>
        /// 作業コードの全量を取得します。
        /// </summary>
        /// <returns>作業コードリスト</returns>
        public List<KNS_M03> GetSagyoCDMasterAll()
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                return DB.KNS_M03.ToList();
            }
        }

        /// <summary>
        /// 勤務認証コードの全量を取得します。
        /// </summary>
        /// <returns>勤務認証コードリスト</returns>
        public List<KNS_M04> GetKinmuNinsyoCDMasterALL()
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                return DB.KNS_M04.ToList();
            }
        }

        /// <summary>
        /// 指定年度1年間のカレンダーマスタを取得します。
        /// </summary>
        /// <param name="_nendo">対象年</param>
        /// <exception cref="KinmuException"></exception>
        /// <returns>カレンダーマスタリスト</returns>
        public List<KNS_M05> GetNendoCalender(int _nendo)
        {
            logger.Debug(LOG_START);

            string yyyymmstart = (_nendo * 100 + 4).ToString();
            string yyyymmend = (_nendo * 100 + 103).ToString();

            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                List<KNS_M05> list = DB.KNS_M05
                    // 年度開始日以降でる
                    .Where(_ => 0 <= (_.DATA_Y + _.DATA_M).CompareTo(yyyymmstart))
                    // 指定月以前で絞る
                    .Where(_ => 0 >= (_.DATA_Y + _.DATA_M).CompareTo(yyyymmend))
                    .ToList();
                return list;
            }
        }

        /// <summary>
        /// カレンダーマスタを取得します。内部では<see cref="GetCalender(int, int, int)"/>を実行しています。
        /// </summary>
        /// <param name="_year">対象年</param>
        /// <exception cref="KinmuException"></exception>
        /// <returns>カレンダーマスタリスト</returns>
        public List<KNS_M05> GetCalender(int _year)
        {
            logger.Debug(LOG_START);
            return GetCalender(_year, 0, 0);
        }

        /// <summary>
        /// カレンダーマスタを取得します。内部では<see cref="GetCalender(int, int, int)"/>を実行しています。
        /// </summary>
        /// <param name="_year">対象年</param>
        /// <exception cref="KinmuException"></exception>
        /// <param name="_month">対象月</param>
        /// <returns>カレンダーマスタリスト</returns>
        public List<KNS_M05> GetCalender(int _year, int _month)
        {
            logger.Debug(LOG_START);
            return GetCalender(_year, _month, 0);
        }

        /// <summary>
        /// カレンダーマスタを取得します。
        /// </summary>
        /// <param name="_year">対象年</param>
        /// <param name="_month">対象月（0を指定すると検索条件に含めません）</param>
        /// <param name="_day">対象日（0を指定すると検索条件に含めません。ただし、対象月に0が指定された場合は、値に依らず検索条件に含めません）</param>
        /// <exception cref="KinmuException"></exception>
        /// <returns>カレンダーマスタ</returns>
        public List<KNS_M05> GetCalender(int _year, int _month, int _day)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                // ゼロ埋め対応
                string y = _year.ToString(PADDING_ZERO_4);
                string m = _month.ToString(PADDING_ZERO_2);
                string d = _day.ToString(PADDING_ZERO_2);

                // 効率性を考えて、引数によって条件式をそれぞれ変えている。
                List<KNS_M05> list;
                string errmes;
                if (_month == 0)
                {
                    list = DB.KNS_M05.Where(_m05 => _m05.DATA_Y == y).OrderBy(_ => _.DATA_M).OrderBy(_ => _.DATA_D).ToList();
                    errmes = y + "年";
                }
                else if (_day == 0)
                {
                    list = DB.KNS_M05.Where(_m05 => _m05.DATA_Y == y && _m05.DATA_M == m).OrderBy(_ => _.DATA_D).ToList();
                    errmes = y + "年"+m + "月";
                }
                else
                {
                    list = DB.KNS_M05.Where(_m05 => _m05.DATA_Y == y && _m05.DATA_M == m && _m05.DATA_D == d).ToList();
                    errmes = y + "年" + m + "月" + d + "日";
                }
                if (!list.Any()) throw new KinmuException("カレンダーマスターに" + errmes +"のデータがありませんでした。");

                return list;
            }
        }

        /// <summary>
        /// 集約日マスタを取得します。内部では<see cref="GetSyuyakubiMaster(int, int)"/>を実行しています。
        /// </summary>
        /// <param name="_year">対象年</param>
        /// <returns>集約日マスタリスト</returns>
        public List<KNS_M07> GetSyuyakubiMaster(int _year)
        {
            logger.Debug(LOG_START);
            return GetSyuyakubiMaster(_year, 0);
        }

        /// <summary>
        /// 集約日マスタを取得します。
        /// </summary>
        /// <param name="_year">対象年</param>
        /// <param name="_month">対象月（0を指定すると検索条件に含めません）</param>
        /// <returns>集約日マスタリスト</returns>
        public List<KNS_M07> GetSyuyakubiMaster(int _year, int _month)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                // ゼロ埋め対応
                string y = _year.ToString(PADDING_ZERO_4);
                string m = _month.ToString(PADDING_ZERO_2);

                if (_month == 0) return DB.KNS_M07.Where(m07 => m07.SHUYAKUY == y).ToList();
                return DB.KNS_M07.Where(m07 => m07.SHUYAKUY == y && m07.SHUYAKUM == m).ToList();
            }
        }
        #endregion

        #region ユーティリティ
        /// <summary>
        /// 全社員の当月分の勤務情報のリストを取得します。
        /// 内部では<see cref="GetKinmuRecordRow(string, int, int, int)"/>を実行しています。
        /// </summary>
        /// <returns>全社員の当月分の勤務情報リスト</returns>
        public List<KinmuRecordRow> GetKinmuRecordRow()
        {
            logger.Debug(LOG_START);
            return GetKinmuRecordRow(null, new DateTime().Year, new DateTime().Month, 0);
        }

        /// <summary>
        /// 全社員の指定月分の勤務情報のリストを取得します。
        /// 内部では<see cref="GetKinmuRecordRow(string, int, int, int)"/>を実行しています。
        /// </summary>
        /// <param name="_year">指定年</param>
        /// <param name="_month">指定月</param>
        /// <returns>全社員の指定月分の勤務情報リスト</returns>
        public List<KinmuRecordRow> GetKinmuRecordRow(int _year, int _month)
        {
            logger.Debug(LOG_START);
            return GetKinmuRecordRow(null, _year, _month, 0);
        }

        /// <summary>
        /// 指定社員の当月分の勤務情報リストを取得します。
        /// 内部では<see cref="GetKinmuRecordRow(string, int, int, int)"/>を実行しています。
        /// </summary>
        /// <param name="_employeeCD">対象の社員コード</param>
        /// <returns>勤務情報リスト</returns>
        public List<KinmuRecordRow> GetKinmuRecordRow(string _employeeCD)
        {
            logger.Debug(LOG_START);
            return GetKinmuRecordRow(_employeeCD, new DateTime().Year, new DateTime().Month, 0);
        }

        /// <summary>
        /// 指定社員の指定月分の勤務情報リストを取得します。
        /// 内部では<see cref="GetKinmuRecordRow(string, int, int, int)"/>を実行しています。
        /// </summary>
        /// <param name="_employeeCD">対象の社員コード</param>
        /// <param name="_year">指定年</param>
        /// <param name="_month">指定月</param>
        /// <returns>勤務情報リスト</returns>
        public List<KinmuRecordRow> GetKinmuRecordRow(string _employeeCD, int _year, int _month)
        {
            logger.Debug(LOG_START);
            return GetKinmuRecordRow(_employeeCD, _year, _month, 0);
        }

        /// <summary>
        /// 指定社員の指定月分の勤務情報リストを取得します。
        /// 他のオーバーロードしているメソッドの本体となります。
        /// </summary>
        /// <param name="_employeeCD">対象の社員コード。nullを指定すると全社員を対象とします。</param>
        /// <param name="_year">指定年</param>
        /// <param name="_month">指定月</param>
        /// <param name="_day">指定日。0を指定すると指定月の全量を対象とします。</param>
        /// <returns></returns>
        public List<KinmuRecordRow> GetKinmuRecordRow(string _employeeCD, int _year, int _month, int _day)
        {
            logger.Debug(LOG_START);
            // 取得対象社員の社員コードリスト
            List<string> empList = new List<string>();
            // 取得した勤務リスト
            List<KinmuRecordRow> kinmuList = new List<KinmuRecordRow>();
            // 取得したカレンダーマスタ
            List<KNS_M05> calendar = GetCalender(_year, _month, _day);

            // 全社員か、単一社員か
            if (_employeeCD == null) { GetSyainMasterAll().ForEach(_ => empList.Add(_.SHAIN_CD)); }
            else { empList.Add(_employeeCD); }

            List<KNS_D01> jisseki = GetKinmuJisseki(_employeeCD, _year, _month, _day);
            List<KNS_D13> yotei = GetKinmuYotei(_employeeCD, _year, _month, _day);
            List<KNS_D02> sagyo = GetSagyoNisshi(_employeeCD, _year, _month, _day);

            // 社員単位で回す
            foreach (string cd in empList)
            {
                // 日付毎に回す
                foreach (KNS_M05 m05 in calendar)
                {
                    kinmuList.Add(new KinmuRecordRow(
                        cd,
                        // 日付で取得した実績を検索し、なければNullを返す（はず
                        jisseki.SingleOrDefault(_ => _.SHAIN_CD == cd && _.DATA_Y == m05.DATA_Y && _.DATA_M == m05.DATA_M && _.DATA_D == m05.DATA_D),
                        // 日付で取得した予定を検索し、なければNullを返す（はず
                        yotei.SingleOrDefault(_ => _.SHAIN_CD == cd && _.DATA_Y == m05.DATA_Y && _.DATA_M == m05.DATA_M && _.DATA_D == m05.DATA_D),
                        sagyo.Where(_ => _.SHAIN_CD == cd && _.DATA_Y == m05.DATA_Y && _.DATA_M == m05.DATA_M && _.DATA_D == m05.DATA_D).ToList(),
                        m05
                        ));
                }
            }
            return kinmuList;

        }

        /// <summary>
        /// 勤務情報を更新または追加します。
        /// 内部では<see cref="SetKinmuJisseki(List{KNS_D01})"/>、<see cref="SetSagyoNisshi(List{KNS_D02})"/>、<see cref="SetKinmuYotei(List{KNS_D13})"/>を実行しています。
        /// </summary>
        /// <param name="_rows">対象の勤務情報リスト</param>
        /// <returns>全テーブルの更新件数</returns>
        public int SetKinmuRecordRow(List<KinmuRecordRow> _rows)
        {
            logger.Debug(LOG_START);
            int cnt = 0;
            List<KNS_D01> d01s = new List<KNS_D01>();
            List<KNS_D02> d02s = new List<KNS_D02>();
            List<KNS_D13> d13s = new List<KNS_D13>();

            foreach (KinmuRecordRow r in _rows)
            {
                d01s.Add(r.KinmuJisseki);
                // 作業日誌はList型なのでConcatで連結
                d02s = d02s.Concat(r.SagyoNisshi).ToList();
                d13s.Add(r.KinmuYotei);
            }

            cnt += SetKinmuJisseki(d01s);
            cnt += SetSagyoNisshi(d02s);
            cnt += SetKinmuYotei(d13s);

            return cnt;
        }

        /// <summary>
        /// 勤務実績を更新または追加します。
        /// </summary>
        /// <param name="_d01s">勤務実績リスト</param>
        /// <returns>更新レコード件数</returns>
        public int SetKinmuJisseki(List<KNS_D01> _d01s)
        {
            logger.Debug(LOG_START);
            List<KNS_D01> uJisseki = new List<KNS_D01>();
            List<KNS_D01> iJisseki = new List<KNS_D01>();
            foreach (KNS_D01 d01 in _d01s)
            {
                if (GetKinmuJisseki(d01.SHAIN_CD, int.Parse(d01.DATA_Y), int.Parse(d01.DATA_M), int.Parse(d01.DATA_D)).Count == 0) iJisseki.Add(d01);
                else uJisseki.Add(d01);
            }
            return UpdateKinmuJisseki(uJisseki) + InsertKinmuJisseki(iJisseki);
        }

        /// <summary>
        /// 勤務予定を更新または追加します。
        /// </summary>
        /// <param name="_d13s">勤務予定リスト</param>
        /// <returns>更新レコード件数</returns>
        public int SetKinmuYotei(List<KNS_D13> _d13s)
        {
            logger.Debug(LOG_START);
            List<KNS_D13> uYotei = new List<KNS_D13>();
            List<KNS_D13> iYotei = new List<KNS_D13>();
            foreach (KNS_D13 d13 in _d13s)
            {
                if (GetKinmuYotei(d13.SHAIN_CD, int.Parse(d13.DATA_Y), int.Parse(d13.DATA_M), int.Parse(d13.DATA_D)).Count == 0) iYotei.Add(d13);
                else uYotei.Add(d13);
            }
            return UpdateKinmuYotei(uYotei) + InsertKinmuYotei(iYotei);
        }

        /// <summary>
        /// 作業日誌を削除し追加します。削除要件は社員コード、年、月、日が一致するレコードです。
        /// これらを一旦全て削除してから引数に指定した作業日誌リストをデータベースに追加します。
        /// </summary>
        /// <param name="_d02s">作業日誌リスト</param>
        /// <returns>更新レコード件数</returns>
        public int SetSagyoNisshi(List<KNS_D02> _d02s)
        {
            logger.Debug(LOG_START);
            foreach (KNS_D02 d02 in _d02s)
            {
                List<KNS_D02> dlist = GetSagyoNisshi(d02.SHAIN_CD, int.Parse(d02.DATA_Y), int.Parse(d02.DATA_M), int.Parse(d02.DATA_D));
                if (dlist.Any()) DeleteSagyoNisshi(dlist);
            }
            return InsertSagyoNisshi(_d02s);
        }

        /// <summary>
        /// 個人設定情報を取得します。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <returns>個人設定情報</returns>
        public UserSetting GetUserSetting(string _employeeCD)
        {
            logger.Debug(LOG_START);
            return new UserSetting(
                _employeeCD,
                GetKojinSetteiKinmuJisseki(_employeeCD),
                GetKojinSetteiPJCD(_employeeCD),
                GetKojinSetteiSagyoCD(_employeeCD)
                );
        }

        /// <summary>
        /// 個人設定情報を削除して追加します。
        /// 内部では<see cref="SetKojinSetteiKinumuJisseki(List{KNS_D04})"/>、<see cref="SetKojinSetteiPJCD(List{KNS_D05})"/>、<see cref="SetKojinSetteiSagyoCD(List{KNS_D06})"/>を実行しています。
        /// </summary>
        /// <param name="_setting">個人設定情報</param>
        /// <returns>更新レコード件数</returns>
        public int SetUserSetting(UserSetting _setting)
        {
            logger.Debug(LOG_START);
            int cnt = 0;
            cnt += SetKojinSetteiKinumuJisseki(_setting.KinmuJissekiMasterList);
            cnt += SetKojinSetteiPJCD(_setting.PJMasterList);
            cnt += SetKojinSetteiSagyoCD(_setting.SagyoCDMasterList);

            return cnt;
        }

        /// <summary>
        /// 個人設定（勤務実績）を削除して追加します。
        /// </summary>
        /// <param name="_d04s">個人設定（勤務実績）リスト</param>
        /// <returns>更新レコード件数</returns>
        public int SetKojinSetteiKinumuJisseki(List<KNS_D04> _d04s)
        {
            logger.Debug(LOG_START);
            List<string> deletedList = new List<string>();

            _d04s.ForEach(_ =>
            {
                // 削除済みリストに社員コードがなければ実行
                if (!deletedList.Contains(_.SHAIN_CD))
                {
                    // 削除するデータの取得
                    List<KNS_D04> a = GetKojinSetteiKinmuJisseki(_.SHAIN_CD);

                    // 削除
                    DeleteKojinSetteiKinmuJisseki(a);

                    // 削除リストに社員コードを追加
                    deletedList.Add(_.SHAIN_CD);
                }
            });

            // 全量追加
            return InsertKojinSetteiKinmuJisseki(_d04s);
        }

        /// <summary>
        /// 個人設定（PJコード）を更新または追加します。
        /// </summary>
        /// <param name="_d05s">個人設定（PJコード）リスト</param>
        /// <returns>更新レコード件数</returns>
        public int SetKojinSetteiPJCD(List<KNS_D05> _d05s)
        {
            logger.Debug(LOG_START);
            List<string> deletedList = new List<string>();

            _d05s.ForEach(_ =>
            {
                // 削除済みリストに社員コードがなければ実行
                if (!deletedList.Contains(_.SHAIN_CD))
                {
                    // 削除するデータの取得
                    List<KNS_D05> a = GetKojinSetteiPJCD(_.SHAIN_CD);

                    // 削除
                    DeleteKojinSetteiPJCD(a);

                    // 削除リストに社員コードを追加
                    deletedList.Add(_.SHAIN_CD);
                }
            });

            // 全量追加
            return InsertKojinSetteiPJCD(_d05s);
        }

        /// <summary>
        /// 個人設定（作業コード）を更新または追加します。
        /// </summary>
        /// <param name="_d06s">個人設定（作業コード）リスト</param>
        /// <returns>更新レコード件数</returns>
        public int SetKojinSetteiSagyoCD(List<KNS_D06> _d06s)
        {
            logger.Debug(LOG_START);
            List<string> deletedList = new List<string>();

            _d06s.ForEach(_ =>
            {
                // 削除済みリストに社員コードがなければ実行
                if (!deletedList.Contains(_.SHAIN_CD))
                {
                    // 削除するデータの取得
                    List<KNS_D06> a = GetKojinSetteiSagyoCD(_.SHAIN_CD);

                    // 削除
                    DeleteKojinSetteiSagyoCD(a);

                    // 削除リストに社員コードを追加
                    deletedList.Add(_.SHAIN_CD);
                }
            });

            // 全量追加
            return InsertKojinSetteiSagyoCD(_d06s);
        }

        /// <summary>
        /// 年休の残り日数を取得します。半休（0.5日）もあるため、double型（DB仕様準拠）で返します。
        /// </summary>
        /// <param name="_employeeCD">対象の社員コード</param>
        /// <returns>年休の残日数</returns>
        public double GetNenkyuZan(string _employeeCD)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                // 集約日を取得する
                KNS_M07 syuyaku = DB.KNS_M07.OrderByDescending(_ => _.SHUYAKUY).ThenByDescending(_ => _.SHUYAKUM).First();
                string dateString = syuyaku.SHUYAKUY + "/" + syuyaku.SHUYAKUM + "/01";
                DateTime syuyakubi = DateTime.Parse(dateString);

                // 年休を取得する
                try
                {
                    return DB.KNS_D12.Where(d12 => d12.SHAIN_CD == _employeeCD && syuyakubi <= d12.YUKOD_TO).Sum(d12 => d12.NENZAN);
                }
                catch (Exception e)
                {
                    logger.Error("社員：{0}　エラー：{1}", _employeeCD, e.Message);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 特休と公休の残日数を取得します。
        /// </summary>
        /// <param name="_employeeCD">対象社員コード</param>
        /// <param name="_nendo">対象年度</param>
        /// <returns>[0]が特休残日数、[1]が公休残日数</returns>
        public double[] GetTokkyuKoukyuZan(string _employeeCD, int _nendo)
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                try
                {
                    var a = DB.KNS_D11.Where(d11 => d11.SHAIN_CD == _employeeCD && d11.NENDO == "" + _nendo + "").Single();
                    return new double[] { a.TOKUZAN, a.KOUZAN };
                }
                catch (Exception e)
                {
                    logger.Error("社員：{0}　年度：{1} エラー：{2}", _employeeCD, _nendo, e.Message);
                    return new double[] { 0.0, 0.0 };
                }
            }
        }

        /// <summary>
        /// PJコードとPJ名称をDictionary型で返します。
        /// </summary>
        /// <returns>KeyがPJコード、ValueがPJ名称のDictionary型</returns>
        public Dictionary<string, string> PJCDToString()
        {
            logger.Debug(LOG_START);
            return GetPJCDMasterAll().ToDictionary(_ => _.PROJ_CD, _ => _.PROJ_NM);
        }

        /// <summary>
        /// 指定したPJコードに対応するPJ名称を取得します。
        /// </summary>
        /// <param name="_pjcd">変換対象のPJコード</param>
        /// <returns>対応するPJ名称</returns>
        public string PJCDToString(string _pjcd)
        {
            logger.Debug(LOG_START);
            return GetPJCDMasterAll().Single(_ => _.PROJ_CD == _pjcd).PROJ_NM;
        }

        /// <summary>
        /// 作業認証コードと作業認証名称をDictionary型で返します。
        /// </summary>
        /// <returns>Keyが作業認証コード、Valueが作業認証名称のDictionary型</returns>
        public Dictionary<string, string> SagyoCDToString()
        {
            logger.Debug(LOG_START);
            return GetSagyoCDMasterAll().ToDictionary(_ => _.SAGYO_CD, _ => _.SAGYO_NM);
        }

        /// <summary>
        /// 指定した作業認証コードに対応するPJ名称を取得します。
        /// </summary>
        /// <param name="_sagyocd">変換対象の作業認証コード</param>
        /// <returns>対応する作業認証名称</returns>
        public string SagyoCDToString(string _sagyocd)
        {
            logger.Debug(LOG_START);
            return GetSagyoCDMasterAll().Single(_ => _.SAGYO_CD == _sagyocd).SAGYO_NM;
        }

        /// <summary>
        /// 現在時刻で最終ログイン日時を更新します。
        /// </summary>
        /// <param name="_employeeCD">更新対象の社員コード</param>
        /// <returns>更新レコード件数（基本的に1）</returns>
        public int SetLastLoginAtNow(string _employeeCD)
        {
            logger.Debug(LOG_START);
            KNS_D14 d14 = new KNS_D14
            {
                SHAIN_CD = _employeeCD,
                LAST_LOGIN_DATE = DateTime.Now.ToString("yyyy/MM/dd HH:mm")
            };

            try
            {
                GetLastLogin(_employeeCD);
                return UpdateLastLogin(d14);
            }
            catch (InvalidOperationException)
            {
                // データがなければGetLastLoginでInvalidOperationExceptionが発生するので、
                // それをキャッチし、データの追加を行う。
                return InsertLastLogin(d14);
            }
        }

        /// <summary>
        /// 直近の集約年月を取得します。
        /// </summary>
        /// <exception cref="KinmuException"></exception>
        /// <returns>[0]が年、[1]が月</returns>
        public int[] GetShuyakuYM()
        {
            logger.Debug(LOG_START);
            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                IOrderedQueryable<KNS_M07> a = DB.KNS_M07.OrderByDescending(m07 => new { m07.SHUYAKUY, m07.SHUYAKUM });
                if (a.Any())
                {
                    return new int[] { int.Parse(a.First().SHUYAKUY), int.Parse(a.First().SHUYAKUM) };
                }
                else
                {
                    throw new KinmuException("集約日の取得ができませんでした。");
                }
            }
        }

        /// <summary>
        /// 勤務予定の承認フラグを1に変更します。
        /// </summary>
        /// <param name="_updater">承認者（GMや部長など）の社員コード</param>
        /// <param name="_target">対象者の社員コード</param>
        /// <param name="_year">指定年</param>
        /// <param name="_month">指定日</param>
        /// <returns>更新レコード件数</returns>
        public int UpdateShoninFlag(string _updater, string _target, int _year, int _month)
        {
            logger.Debug(LOG_START);
            // 承認者に対象社員がいるか確認する。いなければエラー
            if (!GetSyainMasterByShoninShainCD(_updater).Any(_ => _.SHAIN_CD == _target)) throw new KinmuException($"社員コード：{_updater}は社員コード{_target}の承認者ではありません");
            List<KNS_D13> updatelist = GetKinmuYotei(_target, _year, _month);
            updatelist.ForEach(_ =>
            {
                _.SHONIN_FLG = "1";
            });
            return UpdateKinmuYotei(updatelist);
        }

        /// <summary>
        /// お知らせ用メッセージを取得します。
        /// DB内KNS_D08テーブルのHYOJI_FLGが一番小さいレコードを取得します。
        /// </summary>
        /// <returns>メッセージリスト</returns>
        public List<string> GetLatestMessage()
        {
            logger.Debug(LOG_START);
            try
            {
                KNS_D08 d08 = GetMessageAll().First();

                List<string> messages = new List<string>
                {
                    d08.DB_MSG1,
                    d08.DB_MSG2,
                    d08.DB_MSG3,
                    d08.DB_MSG4,
                    d08.DB_MSG5,
                    d08.DB_MSG6,
                    d08.DB_MSG7,
                    d08.DB_MSG8,
                    d08.DB_MSG9,
                    d08.DB_MSG10
                };

                return messages;
            }
            catch (Exception ex)
            {
                throw new KinmuException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 指定年度の4月～指定月までの累計を計算します。
        /// 各月ごとに実労働時間（DKINM）の合計値-法定労働時間を計算します。
        /// 計算後、値がマイナスなら0とし合算します。
        /// </summary>
        /// <param name="_employeeCD">対象社員コード</param>
        /// <param name="_nendo">対象年度</param>
        /// <param name="_month">指定月（1～12を指定します）</param>
        /// <exception cref="ArgumentOutOfRangeException">1～12以外が指定されたときに例外が発生します。</exception>
        /// <returns>法定労働時間外の超勤の合計値</returns>
        public int GetHouteigaiRuikei(string _employeeCD, int _nendo, int _month)
        {
            logger.Debug(LOG_START);
            if (_month <= 0 || 12 < _month)
            {
                throw new KinmuException("指定された月が範囲外でした。1～12を指定してください。値：" + _month);
            }

            // 戻り値
            int res = 0;
            List<SumDkinm> sum;

            sum = GroupByMonthSumDKINMForHouteigai(_employeeCD, _nendo, _month);

            // 要素0件ならすぐ返す。
            if (!sum.Any()) return res;

            // 月ごとに実労働時間と法定労働時間の差分を計算する
            foreach (var item in sum)
            {
                int mm = int.Parse(item.Month);
                int yyyy = mm < 4 ? _nendo + 1 : _nendo;
                int houteiroudo = DateTime.DaysInMonth(yyyy, mm) * 40 * 60 / 7;
                int jiturou = item.Sum;
                res += 0 < (jiturou - houteiroudo) ? jiturou - houteiroudo : 0;
            }

            return res;
        }

        /// <summary>
        /// 指定年度の4月～指定月までの累計を計算します。
        /// 各月ごとに実労働時間（③補正：DKINM-公休D）の合計値-法定労働時間を計算します。
        /// 計算後、値がマイナスなら0とし合算します。
        /// </summary>
        /// <param name="_employeeCD">対象社員コード</param>
        /// <param name="_nendo">対象年度</param>
        /// <param name="_month">指定月（1～12を指定します）</param>
        /// <exception cref="ArgumentOutOfRangeException">1～12以外が指定されたときに例外が発生します。</exception>
        /// <returns>法定労働時間外（③補正：DKINM-公休D）の超勤の合計値</returns>
        public int GetHouteigaiRuikei36(string _employeeCD, int _nendo, int _month)
        {
            logger.Trace("--------------------start--------------------");
            if (_month <= 0 || 12 < _month)
            {
                throw new KinmuException("指定された月が範囲外でした。1～12を指定してください。値：" + _month);
            }

            // 戻り値
            int res = 0;
            List<SumDkinm> sum;

            sum = GroupByMonthSumDKINMForHouteigai(_employeeCD, _nendo, _month);

            // 要素0件ならすぐ返す。
            if (!sum.Any()) return res;

            // 月ごとに実労働時間と法定労働時間の差分を計算する
            foreach (var item in sum)
            {
                int mm = int.Parse(item.Month);
                int yyyy = mm < 4 ? _nendo + 1 : _nendo;
                int houteiroudo = DateTime.DaysInMonth(yyyy, mm) * 40 * 60 / 7;
                int jiturou = item.Sum;
                res += 0 < (jiturou - houteiroudo) ? jiturou - houteiroudo : 0;
            }

            return res;
        }

        /// <summary>
        /// 指定年度の4月～指定月までの累計を計算します。
        /// 各月ごとに実労働時間（DKINM）の合計値-所定労働時間を計算します。
        /// 計算後、値がマイナスなら0とし合算します。
        /// </summary>
        /// <param name="_employeeCD">対象社員コード</param>
        /// <param name="_nendo">対象年度</param>
        /// <param name="_month">指定月（1～12を指定します）</param>
        /// <exception cref="KinmuException">1～12以外が指定されたときに例外が発生します。</exception>
        /// <returns>所定労働時間外の超勤の合計値</returns>
        public int GetTyoukinRuikei(string _employeeCD, int _nendo, int _month)
        {
            logger.Debug(LOG_START);
            if (_month <= 0 || 12 < _month)
            {
                throw new KinmuException("指定された月が範囲外でした。1～12を指定してください。値：" + _month);
            }

            // 戻り値
            int res = 0;
            List<SumDkinm> sum;

            sum = GroupByMonthSumDKINM(_employeeCD, _nendo, _month);

            // 要素0件ならすぐ返す。
            if (!sum.Any()) return res;

            // カレンダーマスターを年度単位で取得する
            List<KNS_M05> calendar = GetNendoCalender(_nendo);

            // 月ごとに実労働時間と法定労働時間の差分を計算する
            foreach (SumDkinm item in sum)
            {
                int syoteiroudo = calendar.Count(_ => _.DATA_M == item.Month && _.NINYO_CD == 出勤.ToZeroPaddingString()) * SYOTEI_ROUDO_MIN;
                int jiturou = item.Sum;
                res += 0 < (jiturou - syoteiroudo) ? jiturou - syoteiroudo : 0;
            }

            return res;
        }

        /// <summary>
        /// 指定年度の4月～指定月までの累計を計算します。
        /// 各月ごとに実労働時間（③補正：DKINM-公休D）の合計値-所定労働時間を計算します。
        /// 計算後、値がマイナスなら0とし合算します。
        /// </summary>
        /// <param name="_employeeCD">対象社員コード</param>
        /// <param name="_nendo">対象年度</param>
        /// <param name="_month">指定月（1～12を指定します）</param>
        /// <exception cref="KinmuException">1～12以外が指定されたときに例外が発生します。</exception>
        /// <returns>所定労働時間外の超勤（③補正：DKINM-公休D）の合計値</returns>
        public int GetTyoukinRuikei36(string _employeeCD, int _nendo, int _month)
        {
            logger.Trace("--------------------start--------------------");
            if (_month <= 0 || 12 < _month)
            {
                throw new KinmuException("指定された月が範囲外でした。1～12を指定してください。値：" + _month);
            }

            // 戻り値
            int res = 0;
            List<SumDkinm> sum;

            sum = GroupByMonthSumDKINM(_employeeCD, _nendo, _month);

            int syoteiroudo = 0;
            int jiturou = 0;

            // 要素0件ならすぐ返す。
            if (!sum.Any()) return res;

            // カレンダーマスターから取得
            List<KNS_M05> calendar = GetNendoCalender(_nendo);

            // 月ごとに実労働時間と法定労働時間の差分を計算する
            foreach (SumDkinm item in sum)
            {
                syoteiroudo = calendar.Count(_ => _.DATA_M == item.Month && _.NINYO_CD == 出勤.ToZeroPaddingString()) * SYOTEI_ROUDO_MIN;
                jiturou = item.Sum;
                res += 0 < (jiturou - syoteiroudo) ? jiturou - syoteiroudo : 0;
            }

            return res;
        }

        /// <summary>
        /// 認証コードを取得します。
        /// 実績→予定→カレンダーの順で照会していき、認証コードが設定されていれば返します。設定がない場合エラーとなります。
        /// </summary>
        /// <param name="_employeeCD">指定社員コード</param>
        /// <param name="_year">指定年</param>
        /// <param name="_month">指定月</param>
        /// <param name="_day">指定日</param>
        /// <returns>取得した勤務コード</returns>
        /// <exception cref="KinmuException"></exception>
        public static string GetNiinsyoCD(string _employeeCD, int _year, int _month, int _day)
        {
            logger.Debug(LOG_START);
            DBManager DB = new DBManager();
            KinmuRecordRow _kinmu = DB.GetKinmuRecordRow(_employeeCD, _year, _month, _day).FirstOrDefault() ?? throw new KinmuException("勤務レコードを取得できませんでした。");
            string cd;
            if (_kinmu.KinmuJisseki != null && !string.IsNullOrWhiteSpace(_kinmu.KinmuJisseki.NINKA_CD))
            {
                cd = _kinmu.KinmuJisseki.NINKA_CD;
            }
            else if (_kinmu.KinmuYotei != null && !string.IsNullOrWhiteSpace(_kinmu.KinmuYotei.YOTEI_CD))
            {
                cd = _kinmu.KinmuYotei.YOTEI_CD;
            }
            else if (!string.IsNullOrWhiteSpace(_kinmu.CalendarMaster.NINYO_CD))
            {
                cd = _kinmu.CalendarMaster.NINYO_CD;
            }
            else
            {
                throw new KinmuException($"{_year}年{_month}月{_day}日の勤務認証コードを取得できませんでした。");
            }

            return cd;
        }

        /// <summary>
        /// 祝日であるか確認します。祝日の場合trueが返ります。
        /// </summary>
        /// <param name="_year">年</param>
        /// <param name="_month">月</param>
        /// <param name="_day">日</param>
        /// <returns></returns>
        public static bool IsHoliday(int _year, int _month, int _day)
        {
            logger.Debug(LOG_START);
            DBManager DB = new DBManager();
            try
            {
                return DB.GetCalender(_year, _month, _day).First().SHUKU_FLG == "1";
            }
            catch (InvalidOperationException e)
            {
                throw new KinmuException("カレンダーマスターの取得でエラーが発生しました。", e);
            }
        }

        /// <summary>
        /// 年度内の実労働時間を月ごとに合計します。
        /// </summary>
        /// <param name="_employeeCD">対象社員名</param>
        /// <param name="_nendo">対象念堂</param>
        /// <param name="_month">ここで指定した月まで（指定した月を含む）を合計します。</param>
        /// <returns>月ごとの合計値</returns>
        private List<SumDkinm> GroupByMonthSumDKINM(string _employeeCD, int _nendo, int _month)
        {
            logger.Debug(LOG_START);
            if (_month <= 0 || 12 < _month)
            {
                throw new KinmuException("指定された月が範囲外でした。1～12を指定してください。値：" + _month);
            }
            string yyyymmstart = (_nendo * 100 + 4).ToString();
            string yyyymmend = (_month < 4 ? (_nendo + 1) * 100 + _month : _nendo * 100 + _month).ToString();

            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                // 社員コードで絞る
                return DB.KNS_D01.Where(_ => _.SHAIN_CD == _employeeCD)
                    // 年度開始日以降で絞る
                    .Where(_ => 0 <= (_.DATA_Y + _.DATA_M).CompareTo(yyyymmstart))
                    // 指定月以前で絞る
                    .Where(_ => 0 >= (_.DATA_Y + _.DATA_M).CompareTo(yyyymmend))
                    // グループ化して合計値を出す
                    .GroupBy(_ => _.DATA_M)
                    .Select(_ => new SumDkinm { Month = _.Key, Sum = _.Sum(__ => (__.DKINM ?? 0) + (__.DMINA1 ?? 0) + (__.DMINA2 ?? 0)) })
                    // リスト化（SQL発行）
                    .ToList();
            }
        }

        /// <summary>
        /// 年度内の実労働時間を月ごとに合計します。
        /// </summary>
        /// <param name="_employeeCD">対象社員名</param>
        /// <param name="_nendo">対象念堂</param>
        /// <param name="_month">ここで指定した月まで（指定した月を含む）を合計します。</param>
        /// <returns>月ごとの合計値</returns>
        private List<SumDkinm> GroupByMonthSumDKINMForHouteigai(string _employeeCD, int _nendo, int _month)
        {
            logger.Debug(LOG_START);
            if (_month <= 0 || 12 < _month)
            {
                throw new KinmuException("指定された月が範囲外でした。1～12を指定してください。値：" + _month);
            }
            string yyyymmstart = (_nendo * 100 + 4).ToString();
            string yyyymmend = (_month < 4 ? (_nendo + 1) * 100 + _month : _nendo * 100 + _month).ToString();

            using (KinmuSystemDB DB = new KinmuSystemDB())
            {
                // 社員コードで絞る
                return DB.KNS_D01.Where(_ => _.SHAIN_CD == _employeeCD)
                    // 年度開始日以降で絞る
                    .Where(_ => 0 <= (_.DATA_Y + _.DATA_M).CompareTo(yyyymmstart))
                    // 指定月以前で絞る
                    .Where(_ => 0 >= (_.DATA_Y + _.DATA_M).CompareTo(yyyymmend))
                    // グループ化して合計値を出す
                    .GroupBy(_ => _.DATA_M)
                    .Select(_ => new SumDkinm { Month = _.Key, Sum = _.Sum(__ => (__.DKINM ?? 0) + (__.DMINA1 ?? 0)) })
                    // リスト化（SQL発行）
                    .ToList();
            }
        }

        /// <summary>
        /// 特休と公休の初期値をデータベースに登録します。
        /// </summary>
        /// <param name="_employeeCD"></param>
        /// <param name="_year"></param>
        /// <param name="_month"></param>
        /// <exception cref="KinmuException"></exception>
        /// <returns></returns>
        public int InitTokkyuKoukyuKinmuRecordRow(string _employeeCD, int _year, int _month)
        {
            List<KNS_M05> calendarList = GetCalender(_year, _month);
            List<KNS_M05> weekend = calendarList.Where(_ => _.NINYO_CD == 公休.ToZeroPaddingString() || _.NINYO_CD == 特休.ToZeroPaddingString()).ToList();
            List<KNS_D13> insertYoteiList = new List<KNS_D13>();
            List<KNS_D01> insertJissekiList = new List<KNS_D01>();
            List<KNS_D13> yoteiList = GetKinmuYotei(_employeeCD, _year, _month);
            List<KNS_D01> jissekiList = GetKinmuJisseki(_employeeCD, _year, _month);

            foreach (KNS_M05 calendar in weekend)
            {
                if (!yoteiList.Any(_ => _.DATA_D == calendar.DATA_D))
                {
                    insertYoteiList.Add(new KNS_D13
                    {
                        DATA_Y = calendar.DATA_Y,
                        DATA_M = calendar.DATA_M,
                        DATA_D = calendar.DATA_D,
                        SHAIN_CD = _employeeCD,
                        YOTEI_CD = calendar.NINYO_CD,
                        UPD_DATE = DateTime.Now,
                        UPD_SHAIN_CD = _employeeCD,
                        KAKN_FLG = "1",
                        SHONIN_FLG = "1",
                        END_Y_PAR = "0"
                    });
                }

                if (!jissekiList.Any(_ => _.DATA_D == calendar.DATA_D))
                {
                    insertJissekiList.Add(new KNS_D01
                    {
                        DATA_Y = calendar.DATA_Y,
                        DATA_M = calendar.DATA_M,
                        DATA_D = calendar.DATA_D,
                        SHAIN_CD = _employeeCD,
                        NINKA_CD = calendar.NINYO_CD,
                        UPD_DATE = DateTime.Now,
                        UPD_SHAIN_CD = _employeeCD,
                        KAKN_FLG = "1",
                        END_PAR = "0",
                    });
                }
            }

            try
            {
                return InsertKinmuJisseki(insertJissekiList) + InsertKinmuYotei(insertYoteiList);
            }
            catch (Exception e)
            {
                throw new KinmuException(e.Message, e);
            }
        }

        private class SumDkinm
        {
            public string Month { get; set; }
            public int Sum { get; set; }
        }
        #endregion
    }
}

