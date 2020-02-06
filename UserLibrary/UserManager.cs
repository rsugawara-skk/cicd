using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.Models;
using System.DirectoryServices;
using System.Configuration;


namespace UserLibrary
{
    /// <summary>
    /// ユーザの個人設定データを扱うマネージャーです。
    /// </summary>
    public class UserManager
    {
        // DBマネージャのインスタンスを生成
        private readonly DBManager dbManager = new DBManager();

        // 各種マスタデータの保持
        private readonly List<KNS_M02> projCDMstList;
        private readonly List<KNS_M03> sagyoCDMstList;

        /// <summary>
        /// ユーザマネージャのコンストラクタ
        /// </summary>
        public UserManager()
        {
            projCDMstList = GetProjCDMst();
            sagyoCDMstList = GetSagyoCDMst();
        }

        /// <summary>
        /// プロジェクトコード一覧をDBから取得する。
        /// </summary>
        /// <returns>プロジェクトコードのエンティティのリスト</returns>
        private List<KNS_M02> GetProjCDMst()
        {
            // プロジェクトコード一覧をDBから取得
            List<KNS_M02> ProjCDMstList = dbManager.GetPJCDMasterAll();

            return ProjCDMstList;
        }

        /// <summary>
        /// 作業コード一覧をDBから取得する。
        /// </summary>
        /// <returns>作業コードのエンティティのリスト</returns>
        private List<KNS_M03> GetSagyoCDMst()
        {
            // 作業コード一覧をDBから取得
            return dbManager.GetSagyoCDMasterAll();
        }

        /// <summary>
        /// 指定した社員の勤務種別コードをDBから取得する。
        /// </summary>
        /// <param name="_employeeCD">対象社員コード</param>
        /// <returns>勤務種別コード</returns>
        public Dictionary<string, string> GetKinmuShubetsuByShainCD(string _employeeCD)
        {
            // 引数の社員に設定された勤務種別コードを選択し返却
            Dictionary<string, string> kinmuShubetsuCD = new Dictionary<string, string>();

            // 社員コードをDBより取得
            KNS_M01 userData = dbManager.GetSyainMasterByShainCD(_employeeCD);

            // 比較用種別コード
            string NIKKIN_1 = "1";
            string NIKKIN_2 = "2";
            string SAN_KOTAI = "3";

            // 勤務種別によって返却するリストを変更
            if (userData.KINSHUBETSU == NIKKIN_1)
            {
                kinmuShubetsuCD.Add("01", "01:出勤");
            }
            else if (userData.KINSHUBETSU == NIKKIN_2)
            {
                kinmuShubetsuCD.Add("51", "51:日２");
            }
            else if (userData.KINSHUBETSU == SAN_KOTAI)
            {
                kinmuShubetsuCD.Add("51", "51:日２");
                kinmuShubetsuCD.Add("61", "61:交昼");
                kinmuShubetsuCD.Add("62", "62:交夜");
            }
            else
            {
                kinmuShubetsuCD.Add("01", "01:出勤");
            }

            return kinmuShubetsuCD;
        }


        /// <summary>
        /// 指定した社員の個人設定情報をDBから取得する。
        /// </summary>
        /// <param name="_shainCD">対象社員コード</param>
        /// <returns>指定した社員の個人設定エンティティ</returns>
        public UserSetting GetUserSettingByShainCD(string _shainCD)
        {
            // ログインユーザの個人設定をDBより取得
            UserSetting currentUserSetting = dbManager.GetUserSetting(_shainCD);

            return currentUserSetting;
        }

        /// <summary>
        /// 社員の個人設定エンティティをDBに反映させる。
        /// </summary>
        /// <param name="_userSetting">個人設定エンティティ</param>
        /// <returns>メッセージパネル表示文字列</returns>
        public string UpdateUserSetting(UserSetting _userSetting)
        {
            // 返却メッセージ変数
            string displayMessage = "CM0002:DB更新が正常に終了しました。";

            // ログインユーザの個人設定をDBより取得
            dbManager.SetUserSetting(_userSetting);

            return displayMessage;
        }

        /// <summary>
        ///  aliasをもとに社員CDを取得する。
        /// </summary>
        /// <param name="_alias">エイリアス文字列</param>
        /// <returns>レコード</returns>
        public KNS_M01 GetShainCDInfoByAlias(string _alias)
        {
            // 社員コードをDBより取得
            KNS_M01 result;
            try
            {
                result = dbManager.GetSyainMasterByAlias(_alias);
            }
            catch
            {
                return null;
            }
            return result;
        }


        /// <summary>
        ///  対象社員の最終ログイン時間を更新する。
        /// </summary>
        /// <param name="_shainCD">社員番号</param>
        /// <returns>レコードの更新件数</returns>
        public int SetLastLoginAtNow(string _shainCD)
        {
            // ログインユーザの社員コードをDBより取得
            int result = dbManager.SetLastLoginAtNow(_shainCD);

            return result;
        }

        /// <summary>
        /// 最終ログイン日時を取得します。
        /// </summary>
        /// <param name="_shaincd"></param>
        /// <returns></returns>
        public string GetLastLogin(string _shaincd)
        {
            try
            {
                return dbManager.GetLastLogin(_shaincd).LAST_LOGIN_DATE;
            }
            catch (Exception)
            {
                return DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            }
        }

        /// <summary>
        ///  プロジェクトコードマスタから取得したListをDictionary形式に変換。
        /// </summary>
        /// <returns>プロジェクトコードマスタ全量のDictionary</returns>
        public Dictionary<string, string> GetProjCDMstDictionary()
        {
            // 表示用辞書データ
            Dictionary<string, string> displayDic = new Dictionary<string, string>();

            foreach (KNS_M02 currentProj in projCDMstList)
            {
                // マスタデータを表示用に整形して格納
                displayDic.Add(currentProj.PROJ_CD, currentProj.PROJ_CD + ":" + currentProj.PROJ_NM);
            }
            return displayDic;
        }

        /// <summary>
        ///  作業コードマスタから取得したListをDictionary形式に変換。
        /// </summary>
        /// <returns>作業コードマスタ全量のDictionary</returns>
        public Dictionary<string, string> GetSagyoCDMstDictionary()
        {
            // 表示用辞書データ
            Dictionary<string, string> displayDic = new Dictionary<string, string>();

            foreach (KNS_M03 currentSagyo in sagyoCDMstList)
            {
                // マスタデータを表示用に整形して格納
                displayDic.Add(currentSagyo.SAGYO_CD, currentSagyo.SAGYO_CD + ":" + currentSagyo.SAGYO_NM);
            }
            return displayDic;
        }

        /// <summary>
        ///  設定済みプロジェクトコードのリストを表示用のDictionary形式に変換する
        /// </summary>
        /// <param name="_projList">設定済みのプロジェクトコード</param>
        /// <returns>設定済みプロジェクトコードリストの表示用のDictionary</returns>
        public Dictionary<string, string> GetProjCDListDictionary(List<KNS_D05> _projList)
        {
            // 表示用辞書データ
            Dictionary<string, string> displayDic = new Dictionary<string, string>();

            // 表示順でソート
            _projList.Sort((a, b) => Int32.Parse(a.VIEW_ORDER) - Int32.Parse(b.VIEW_ORDER));

            foreach (KNS_D05 currentProj in _projList)
            {
                // 設定値のあるPJコードがマスタに存在するか確認
                // マスタから削除済みであれば画面に反映させない
                if (!projCDMstList.Exists(project => project.PROJ_CD == currentProj.PROJ_CD)) continue;

                // 一致するレコードを取得
                KNS_M02 targetProj = projCDMstList.Find(project => project.PROJ_CD == currentProj.PROJ_CD);

                // マスタデータを表示用に整形して格納
                displayDic.Add(currentProj.PROJ_CD, currentProj.PROJ_CD + ":" + targetProj.PROJ_NM);
            }
            return displayDic;
        }

        /// <summary>
        ///  設定済み作業コードのリストを表示用のDictionary形式に変換する
        /// </summary>
        /// <param name="_sagyojList">設定済みの作業コード</param>
        /// <returns>設定済み作業コードリストの表示用のDictionary</returns>
        public Dictionary<string, string> GetSagyoCDListDictionary(List<KNS_D06> _sagyojList)
        {
            // 表示用辞書データ
            Dictionary<string, string> displayDic = new Dictionary<string, string>();

            // 表示順でソート
            _sagyojList.Sort((a, b) => Int32.Parse(a.VIEW_ORDER) - Int32.Parse(b.VIEW_ORDER));

            foreach (KNS_D06 currentSagyo in _sagyojList)
            {
                // 設定値のあるPJコードがマスタに存在するか確認
                if (!sagyoCDMstList.Exists(sagyo => sagyo.SAGYO_CD == currentSagyo.SAGYO_CD)) continue;
                // 一致するレコードを取得
                KNS_M03 targetSagyo = sagyoCDMstList.Find(sagyo => sagyo.SAGYO_CD == currentSagyo.SAGYO_CD);
                // マスタデータを表示用に整形して格納
                displayDic.Add(currentSagyo.SAGYO_CD, currentSagyo.SAGYO_CD + ":" + targetSagyo.SAGYO_NM);
            }
            return displayDic;
        }


        /// <summary>
        ///  追加用プロジェクトコード一覧の生成
        /// </summary>
        /// <param name="_projList">設定済みのプロジェクトコード</param>
        /// <returns>設定済みプロジェクトコードリストの表示用のDictionary</returns>
        public Dictionary<string, string> GetAddProjCDDictionary(List<KNS_D05> _projList)
        {
            // 表示用辞書データ
            Dictionary<string, string> displayDic = GetProjCDMstDictionary();
            // 全量のマスタから設定済みの項目を削除
            foreach (KNS_D05 currentProj in _projList)
            {
                displayDic.Remove(currentProj.PROJ_CD);
            }
            return displayDic;
        }

        /// <summary>
        ///  追加用作業コード一覧の生成
        /// </summary>
        /// <param name="_sagyojList">設定済みの作業コード</param>
        /// <returns>設定済み作業コードリストの表示用のDictionary</returns>
        public Dictionary<string, string> GetAddSagyoCDDictionary(List<KNS_D06> _sagyojList)
        {
            // 表示用辞書データ
            Dictionary<string, string> displayDic = GetSagyoCDMstDictionary();
            // 全量のマスタから設定済みの項目を削除
            foreach (KNS_D06 currentSagyo in _sagyojList)
            {
                displayDic.Remove(currentSagyo.SAGYO_CD);
            }
            return displayDic;
        }

        /// <summary>
        /// LDAP(オンプレミス)認証結果を返却する
        /// </summary>
        /// <param name="_username">ユーザ名</param>
        /// <param name="_pwd">パスワード</param>
        /// <returns>LDAP認証結果</returns>
        public AuthResult AuthLdap(string _username, string _pwd)
        {
            // 返却用インスタンスの定義
            AuthResult resultEntity = new AuthResult();

            //ADサーバの接続先取得
            string domain = ConfigurationManager.AppSettings["DOMAIN"];
            string lserver = ConfigurationManager.AppSettings["ADSERVER"];

            // 照会用文字列の整形
            string domainAndUsername = domain + @"\" + _username;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(lserver, domainAndUsername, _pwd);

                DirectorySearcher search = new DirectorySearcher(entry)
                {
                    Filter = "(SAMAccountName=" + _username + ")"
                };

                // 表示名の取得
                search.PropertiesToLoad.Add("displayName");
                // 役職の取得
                search.PropertiesToLoad.Add("title");
                // 部署の取得
                search.PropertiesToLoad.Add("department");
                // 所属グループの取得
                search.PropertiesToLoad.Add("memberOf");

                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return resultEntity;
                }

                DirectoryEntry resultEntry = result.GetDirectoryEntry();

                // 画面表示名の格納
                resultEntity.DisplayName = (string)resultEntry.Properties["displayName"].Value;
                // 役職の格納
                resultEntity.Title = (string)resultEntry.Properties["title"].Value;
                // 部署の格納
                resultEntity.Department = (string)resultEntry.Properties["department"].Value;
                // 所属グループの格納
                foreach (string currentGroup in result.Properties["memberOf"])
                {
                    // グループの格納
                    resultEntity.GroupList.Add(currentGroup);
                }

                // 認証成功ステータスの格納
                resultEntity.LoginResult = true;
            }
            catch (Exception ex)
            {
                // 画面表示用のエラーメッセージの格納
                resultEntity.Message = ex.Message;
                return resultEntity;
            }
            return resultEntity;
        }

        /// <summary>
        /// LDAP認証返却用クラス
        /// </summary>
        public class AuthResult
        {
            public bool LoginResult { get; set; }
            public string DisplayName { get; set; }
            public string Title { get; set; }
            public string Department { get; set; }
            public List<string> GroupList { get; set; }
            public string Message { get; set; }

            public AuthResult()
            {
                LoginResult = false;
                DisplayName = string.Empty;
                Title = string.Empty;
                Department = string.Empty;
                GroupList = new List<string>();
                Message = string.Empty;
            }
        }
    }
}
