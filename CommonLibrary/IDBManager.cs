using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.Models;

namespace CommonLibrary
{
    /// <summary>
    /// Entitiy Flameworkの操作部分のインターフェースです。
    /// DBからのデータ取得や登録の根幹となる部分を担っています。
    /// </summary>
    public interface IDBManager
    {
        /// <summary>
        /// 全社員の当月分の勤務実績を取得するインターフェースです。
        /// </summary>
        /// <returns>勤務実績リスト</returns>
        List<KNS_D01> GetKinmuJisseki();

        /// <summary>
        /// 全社員の指定月分の勤務実績を取得するインターフェースです。
        /// </summary>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <returns>勤務実績リスト</returns>
        List<KNS_D01> GetKinmuJisseki(int _year, int _month);

        /// <summary>
        /// 全社員の指定日分の勤務実績を取得するインターフェースです。
        /// </summary>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <param name="_day">取得対象の日</param>
        /// <returns>勤務実績リスト</returns>
        List<KNS_D01> GetKinmuJisseki(int _year, int _month, int _day);

        /// <summary>
        /// 指定社員の当月分の勤務実績を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <returns>勤務実績リスト</returns>
        List<KNS_D01> GetKinmuJisseki(string _employeeCD);

        /// <summary>
        /// 指定社員の指定月分の勤務実績を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <returns>勤務実績リスト</returns>
        List<KNS_D01> GetKinmuJisseki(string _employeeCD, int _year, int _month);

        /// <summary>
        /// 指定社員の指定日分の勤務実績を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <param name="_day">取得対象の日</param>
        /// <returns>勤務実績リスト</returns>
        List<KNS_D01> GetKinmuJisseki(string _employeeCD, int _year, int _month, int _day);

        /// <summary>
        /// 勤務実績を追加するインターフェースです。
        /// </summary>
        /// <param name="_d01s">追加する勤務実績リスト</param>
        /// <returns>更新レコード件数</returns>
        int InsertKinmuJisseki(List<KNS_D01> _d01s);

        /// <summary>
        /// 勤務実績を更新するインターフェースです。
        /// </summary>
        /// <param name="_d01s">更新する勤務実績リスト</param>
        /// <returns>更新レコード件数</returns>
        int UpdateKinmuJisseki(List<KNS_D01> _d01s);

        /// <summary>
        /// 勤務実績を削除するインターフェースです。
        /// </summary>
        /// <param name="_d01s">削除する勤務実績リスト</param>
        /// <returns>削除レコード件数</returns>
        int DeleteKinmuJisseki(List<KNS_D01> _d01s);

        /// <summary>
        /// 全社員の当月分の勤務予定を取得するインターフェースです。
        /// </summary>
        /// <returns>勤務予定リスト</returns>
        List<KNS_D13> GetKinmuYotei();

        /// <summary>
        /// 全社員の指定月分の勤務予定を取得するインターフェースです。
        /// </summary>
        /// <param name="_year">指定年</param>
        /// <param name="_month">指定月</param>
        /// <returns>勤務予定リスト</returns>
        List<KNS_D13> GetKinmuYotei(int _year, int _month);

        /// <summary>
        /// 全社員の指定日分の勤務予定を取得するインターフェースです。
        /// </summary>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <param name="_day">取得対象の日</param>
        /// <returns>勤務予定リスト</returns>
        List<KNS_D13> GetKinmuYotei(int _year, int _month, int _day);

        /// <summary>
        /// 指定社員の当月分の勤務予定を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <returns>勤務予定リスト</returns>
        List<KNS_D13> GetKinmuYotei(string _employeeCD);

        /// <summary>
        /// 指定社員の当月分の勤務予定を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <returns>勤務予定リスト</returns>
        List<KNS_D13> GetKinmuYotei(string _employeeCD, int _year, int _month);

        /// <summary>
        /// 指定社員の指定日分の勤務予定を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <param name="_year">取得対象の年</param>
        /// <param name="_month">取得対象の月</param>
        /// <param name="_day">取得対象の日</param>
        /// <returns>勤務予定リスト</returns>
        List<KNS_D13> GetKinmuYotei(string _employeeCD, int _year, int _month, int _day);

        /// <summary>
        /// 勤務予定を追加するインターフェースです。
        /// </summary>
        /// <param name="_d13s">追加する勤務予定リスト。1件でもList化してください。</param>
        /// <returns>更新レコード件数</returns>
        int InsertKinmuYotei(List<KNS_D13> _d13s);

        /// <summary>
        /// 勤務予定を更新するインターフェースです。
        /// </summary>
        /// <param name="_d13s">更新する勤務予定リスト。1件でもList化してください。</param>
        /// <returns>更新レコード件数</returns>
        int UpdateKinmuYotei(List<KNS_D13> _d13s);

        /// <summary>
        /// 勤務予定を削除するインターフェースです。
        /// </summary>
        /// <param name="_d13s">削除する勤務予定リスト。1件でもList化してください。</param>
        /// <returns>削除レコード件数</returns>
        int DeleteKinmuYotei(List<KNS_D13> _d13s);

        /// <summary>
        /// 指定日の作業日誌を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <param name="_year">対象年</param>
        /// <param name="_month">対象月</param>
        /// <returns>作業日誌リスト</returns>
        List<KNS_D02> GetSagyoNisshi(string _employeeCD, int _year, int _month);

        /// <summary>
        /// 指定日の作業日誌を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <param name="_year">対象年</param>
        /// <param name="_month">対象月</param>
        /// <param name="_day">対象日</param>
        /// <returns>作業日誌リスト</returns>
        List<KNS_D02> GetSagyoNisshi(string _employeeCD, int _year, int _month, int _day);

        /// <summary>
        /// 作業日誌を追加するインターフェースです。
        /// </summary>
        /// <param name="_d02s">追加対象の作業日誌</param>
        /// <returns>更新レコード件数</returns>
        int InsertSagyoNisshi(List<KNS_D02> _d02s);

        /// <summary>
        /// 作業日誌を更新するインターフェースです。
        /// </summary>
        /// <param name="_d02s">更新対象の作業日誌</param>
        /// <returns>更新レコード件数</returns>
        int UpdateSagyoNisshi(List<KNS_D02> _d02s);

        /// <summary>
        /// 作業日誌を削除するインターフェースです。
        /// </summary>
        /// <param name="_d02s">削除対象の作業日誌</param>
        /// <returns>削除レコード件数</returns>
        int DeleteSagyoNisshi(List<KNS_D02> _d02s);

        /// <summary>
        /// 対象社員の個人設定（勤務実績）を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">対象の社員コード</param>
        /// <returns>個人設定（勤務実績）リスト</returns>
        List<KNS_D04> GetKojinSetteiKinmuJisseki(string _employeeCD);

        /// <summary>
        /// 個人設定（勤務実績）を追加するインターフェースです。
        /// </summary>
        /// <param name="_d04s">追加対象の個人設定（勤務実績）</param>
        /// <returns>更新レコード件数</returns>
        int InsertKojinSetteiKinmuJisseki(List<KNS_D04> _d04s);

        /// <summary>
        /// 個人設定（勤務実績）を更新するインターフェースです。
        /// </summary>
        /// <param name="_d04s">更新対象の個人設定（勤務実績）</param>
        /// <returns>更新レコード件数</returns>
        int UpdateKojinSetteiKinmuJisseki(List<KNS_D04> _d04s);

        /// <summary>
        /// 個人設定（勤務実績）を削除するインターフェースです。
        /// </summary>
        /// <param name="_d04s">削除対象の個人設定（勤務実績）</param>
        /// <returns>削除レコード件数</returns>
        int DeleteKojinSetteiKinmuJisseki(List<KNS_D04> _d04s);

        /// <summary>
        /// 個人設定（PJコード）を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">対象社員コード</param>
        /// <returns>個人設定（PJコード）リスト</returns>
        List<KNS_D05> GetKojinSetteiPJCD(string _employeeCD);

        /// <summary>
        /// 個人設定（PJコード）を追加するインターフェースです。
        /// </summary>
        /// <param name="_d05s">追加する個人設定（PJコード）</param>
        /// <returns>更新レコード件数</returns>
        int InsertKojinSetteiPJCD(List<KNS_D05> _d05s);

        /// <summary>
        /// 個人設定（PJコード）を更新するインターフェースです。
        /// </summary>
        /// <param name="_d05s">更新する個人設定（PJコード）</param>
        /// <returns>更新レコード件数</returns>
        int UpdateKojinSetteiPJCD(List<KNS_D05> _d05s);

        /// <summary>
        /// 個人設定（PJコード）を削除するインターフェースです。
        /// </summary>
        /// <param name="_d05s">削除する個人設定（PJコード）</param>
        /// <returns>削除レコード件数</returns>
        int DeleteKojinSetteiPJCD(List<KNS_D05> _d05s);

        /// <summary>
        /// 個人設定（作業コード）を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">取得対象社員コード</param>
        /// <returns>個人設定（作業コード）リスト</returns>
        List<KNS_D06> GetKojinSetteiSagyoCD(string _employeeCD);

        /// <summary>
        /// 個人設定（作業コード）を追加するインターフェースです。
        /// </summary>
        /// <param name="_d06s">追加する個人設定（作業コード）</param>
        /// <returns>更新レコード件数</returns>
        int InsertKojinSetteiSagyoCD(List<KNS_D06> _d06s);

        /// <summary>
        /// 個人設定（作業コード）を更新するインターフェースです。
        /// </summary>
        /// <param name="_d06s">更新する個人設定（作業コード）</param>
        /// <returns>更新レコード件数</returns>
        int UpdateKojinSetteiSagyoCD(List<KNS_D06> _d06s);

        /// <summary>
        /// 個人設定（作業コード）を削除するインターフェースです。
        /// </summary>
        /// <param name="_d06s">削除する個人設定（作業コード）</param>
        /// <returns>削除レコード件数</returns>
        int DeleteKojinSetteiSagyoCD(List<KNS_D06> _d06s);

        /// <summary>
        /// お知らせメッセージの全量を取得するインターフェースです。
        /// </summary>
        /// <returns>HYOJI_FLGで昇順されたメッセージリスト</returns>
        List<KNS_D08> GetMessageAll();

        /// <summary>
        /// 対象社員、年度の特休・公休データを取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">対象社員コード</param>
        /// <param name="_nendo">対象年度</param>
        /// <returns>特休・公休データ</returns>
        KNS_D11 GetTokkyuKokyuData(string _employeeCD, int _nendo);

        /// <summary>
        /// 対象社員、年度の年休データを取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">対象社員</param>
        /// <param name="_nendo">対象年度</param>
        /// <returns>年休データ</returns>
        KNS_D12 GetNenkyuData(string _employeeCD, int _nendo);

        /// <summary>
        /// システムへの最終ログイン時刻を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">取得対象社員コード</param>
        /// <returns>最終ログインレコード</returns>
        KNS_D14 GetLastLogin(string _employeeCD);

        /// <summary>
        /// システムへの最終ログイン時刻を追加するインターフェースです。
        /// </summary>
        /// <param name="_d14">最終ログインレコード</param>
        /// <returns>更新件数（1件になる見込み）</returns>
        int InsertLastLogin(KNS_D14 _d14);

        /// <summary>
        /// システムへの最終ログイン時刻を更新するインターフェースです。
        /// </summary>
        /// <param name="_d14">最終ログインレコード</param>
        /// <returns>更新件数（1件になる見込み）</returns>
        int UpdateLastLogin(KNS_D14 _d14);

        /// <summary>
        /// 有効な全社員のマスタデータを取得するインターフェースです。
        /// </summary>
        /// <returns>社員マスタリスト</returns>
        List<KNS_M01> GetSyainMasterAll();

        /// <summary>
        /// 社員マスタから一致する社員コードのデータを取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">取得対象社員コード</param>
        /// <returns>社員マスタレコード</returns>
        KNS_M01 GetSyainMasterByShainCD(string _employeeCD);

        /// <summary>
        /// 社員マスタから一致するエイリアス（ドメインアカウント名）のデータを取得するインターフェースです。
        /// </summary>
        /// <param name="_alias">取得対象のエイリアス</param>
        /// <returns>社員マスタレコード</returns>
        KNS_M01 GetSyainMasterByAlias(string _alias);

        /// <summary>
        /// 社員マスタから一致する承認社員コードのデータを取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">承認社員（GMや部長など）の社員コード</param>
        /// <returns>社員マスタリスト</returns>
        List<KNS_M01> GetSyainMasterByShoninShainCD(string _employeeCD);

        /// <summary>
        /// PJコードの全量を取得するインターフェースです。
        /// </summary>
        /// <returns>PJコードリスト</returns>
        List<KNS_M02> GetPJCDMasterAll();

        /// <summary>
        /// 作業コードの全量を取得するインターフェースです。
        /// </summary>
        /// <returns>作業コードリスト</returns>
        List<KNS_M03> GetSagyoCDMasterAll();

        /// <summary>
        /// 勤務認証コードの全量を取得するインターフェースです。
        /// </summary>
        /// <returns>勤務認証コードリスト</returns>
        List<KNS_M04> GetKinmuNinsyoCDMasterALL();

        /// <summary>
        /// カレンダーマスタを取得するインターフェースです。
        /// </summary>
        /// <param name="_year">対象年</param>
        /// <returns>カレンダーマスタリスト</returns>
        List<KNS_M05> GetCalender(int _year);

        /// <summary>
        /// カレンダーマスタを取得するインターフェースです。
        /// </summary>
        /// <param name="_year">対象年</param>
        /// <param name="_month">対象月</param>
        /// <returns>カレンダーマスタリスト</returns>
        List<KNS_M05> GetCalender(int _year, int _month);

        /// <summary>
        /// カレンダーマスタを取得するインターフェースです。
        /// </summary>
        /// <param name="_year">対象年</param>
        /// <param name="_month">対象月</param>
        /// <param name="_day">対象日</param>
        /// <returns>カレンダーマスタ</returns>
        List<KNS_M05> GetCalender(int _year, int _month, int _day);

        /// <summary>
        /// 集約日マスタを取得するインターフェースです。
        /// </summary>
        /// <param name="_year">対象年</param>
        /// <returns>集約日マスタリスト</returns>
        List<KNS_M07> GetSyuyakubiMaster(int _year);

        /// <summary>
        /// 集約日マスタを取得するインターフェースです。
        /// </summary>
        /// <param name="_year">対象年</param>
        /// <param name="_month">対象月</param>
        /// <returns>集約日マスタリスト</returns>
        List<KNS_M07> GetSyuyakubiMaster(int _year, int _month);
    }

    /// <summary>
    /// <see cref="IDBManager"/>を使用したユーティリティインターフェースです。
    /// ピンポイントなデータの取得、コード変換、フラグ更新がここに定義されています。
    /// </summary>
    public interface IDBManagerUtility
    {
        /// <summary>
        /// 全社員の当月分の勤務情報のリストを取得するインターフェースです。
        /// </summary>
        /// <returns>全社員の当月分の勤務情報リスト</returns>
        List<KinmuRecordRow> GetKinmuRecordRow();

        /// <summary>
        /// 全社員の指定月分の勤務情報のリストを取得するインターフェースです。
        /// </summary>
        /// <param name="_year">指定年</param>
        /// <param name="_month">指定月</param>
        /// <returns>全社員の指定月分の勤務情報リスト</returns>
        List<KinmuRecordRow> GetKinmuRecordRow(int _year, int _month);

        /// <summary>
        /// 指定社員の当月分の勤務情報リストを取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">対象の社員コード</param>
        /// <returns>勤務情報リスト</returns>
        List<KinmuRecordRow> GetKinmuRecordRow(string _employeeCD);

        /// <summary>
        /// 指定社員の指定月分の勤務情報リストを取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">対象の社員コード</param>
        /// <param name="_year">指定年</param>
        /// <param name="_month">指定月</param>
        /// <returns>勤務情報リスト</returns>
        List<KinmuRecordRow> GetKinmuRecordRow(string _employeeCD, int _year, int _month);

        /// <summary>
        /// 指定社員の指定月分の勤務情報リストを取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">対象の社員コード</param>
        /// <param name="_year">指定年</param>
        /// <param name="_month">指定月</param>
        /// <param name="_day">指定日。0を指定すると指定月の全量を対象とします。</param>
        /// <returns></returns>
        List<KinmuRecordRow> GetKinmuRecordRow(string _employeeCD, int _year, int _month, int _day);

        /// <summary>
        /// 勤務情報を更新または追加するインターフェースです。
        /// </summary>
        /// <param name="_rows">対象の勤務情報リスト</param>
        /// <returns>全テーブルの更新件数</returns>
        int SetKinmuRecordRow(List<KinmuRecordRow> _rows);

        /// <summary>
        /// 勤務実績を更新または追加するインターフェースです。
        /// </summary>
        /// <param name="_d01s">勤務実績リスト</param>
        /// <returns>更新レコード件数</returns>
        int SetKinmuJisseki(List<KNS_D01> _d01s);

        /// <summary>
        /// 勤務予定を更新または追加するインターフェースです。
        /// </summary>
        /// <param name="_d13s">勤務予定リスト</param>
        /// <returns>更新レコード件数</returns>
        int SetKinmuYotei(List<KNS_D13> _d13s);

        /// <summary>
        /// 作業日誌を削除し追加するインターフェースです。
        /// </summary>
        /// <param name="_d02s">作業日誌リスト</param>
        /// <returns>更新レコード件数</returns>
        int SetSagyoNisshi(List<KNS_D02> _d02s);

        /// <summary>
        /// 個人設定情報を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">取得対象の社員コード</param>
        /// <returns>個人設定情報</returns>
        UserSetting GetUserSetting(string _employeeCD);

        /// <summary>
        /// 個人設定情報を更新または追加するインターフェースです。
        /// </summary>
        /// <param name="_setting">個人設定情報</param>
        /// <returns>更新レコード件数</returns>
        int SetUserSetting(UserSetting _setting);

        /// <summary>
        /// 個人設定（勤務実績）を更新または追加するインターフェースです。
        /// </summary>
        /// <param name="_d04s">個人設定（勤務実績）リスト</param>
        /// <returns>更新レコード件数</returns>
        int SetKojinSetteiKinumuJisseki(List<KNS_D04> _d04s);

        /// <summary>
        /// 個人設定（PJコード）を更新または追加するインターフェースです。
        /// </summary>
        /// <param name="_d05s">個人設定（PJコード）リスト</param>
        /// <returns>更新レコード件数</returns>
        int SetKojinSetteiPJCD(List<KNS_D05> _d05s);

        /// <summary>
        /// 個人設定（作業コード）を更新または追加するインターフェースです。
        /// </summary>
        /// <param name="_d06s">個人設定（作業コード）リスト</param>
        /// <returns>更新レコード件数</returns>
        int SetKojinSetteiSagyoCD(List<KNS_D06> _d06s);

        /// <summary>
        /// 年休の残り日数を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">対象の社員コード</param>
        /// <returns>年休の残日数</returns>
        double GetNenkyuZan(string _employeeCD);

        /// <summary>
        /// 特休と公休の残日数を取得するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">対象社員コード</param>
        /// <param name="_nendo">対象年度</param>
        /// <returns>[0]が特休残日数、[1]が公休残日数</returns>
        double[] GetTokkyuKoukyuZan(string _employeeCD, int _nendo);

        /// <summary>
        /// PJコードとPJ名称をDictionary型で返すインターフェースです。
        /// </summary>
        /// <returns>KeyがPJコード、ValueがPJ名称のDictionary型</returns>
        Dictionary<string, string> PJCDToString();

        /// <summary>
        /// 指定したPJコードに対応するPJ名称を取得するインターフェースです。
        /// </summary>
        /// <param name="_pjcd">変換対象のPJコード</param>
        /// <returns>対応するPJ名称</returns>
        string PJCDToString(string _pjcd);

        /// <summary>
        /// 作業認証コードと作業認証名称をDictionary型で返すインターフェースです。
        /// </summary>
        /// <returns>Keyが作業認証コード、Valueが作業認証名称のDictionary型</returns>
        Dictionary<string, string> SagyoCDToString();

        /// <summary>
        /// 指定した作業認証コードに対応するPJ名称を取得するインターフェースです。
        /// </summary>
        /// <param name="_sagyocd">変換対象の作業認証コード</param>
        /// <returns>対応する作業認証名称</returns>
        string SagyoCDToString(string _sagyocd);

        /// <summary>
        /// 現在時刻で最終ログイン日時を更新するインターフェースです。
        /// </summary>
        /// <param name="_employeeCD">更新対象の社員コード</param>
        /// <returns>更新レコード件数（基本的に1）</returns>
        int SetLastLoginAtNow(string _employeeCD);

        /// <summary>
        /// 直近の集約年月を取得するインターフェースです。
        /// </summary>
        /// <returns>[0]が年、[1]が月</returns>
        int[] GetShuyakuYM();

        /// <summary>
        /// 勤務予定の承認フラグを1に変更するインターフェースです。
        /// </summary>
        /// <param name="_updater">承認者（GMや部長など）の社員コード</param>
        /// <param name="_target">対象者の社員コード</param>
        /// <param name="_year">指定年</param>
        /// <param name="_month">指定日</param>
        /// <returns>更新レコード件数</returns>
        int UpdateShoninFlag(string _updater, string _target, int _year, int _month);

        /// <summary>
        /// お知らせ用メッセージを取得するインターフェースです。
        /// </summary>
        /// <returns>メッセージリスト</returns>
        List<string> GetLatestMessage();
    }
}
