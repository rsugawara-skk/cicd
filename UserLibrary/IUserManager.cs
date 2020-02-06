using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.Models;

namespace UserLibrary
{
    interface IUserManager
    {

        /// <summary>
        /// プロジェクトコード一覧をDBから取得する。
        /// </summary>
        /// <returns>プロジェクトコードのエンティティのリスト</returns>
        List<KNS_M02> GetProjCDMst();

        /// <summary>
        /// 作業コード一覧をDBから取得する。
        /// </summary>
        /// <returns>作業コードのエンティティのリスト</returns>
        List<KNS_M03> GetSagyoCDMst();

        /// <summary>
        /// 指定した社員の勤務種別コードをDBから取得する。
        /// </summary>
        /// <param name="_shainCD">対象社員コード</param>
        /// <returns>勤務種別コード</returns>
        string GetKinmuShubetsuByShainCD(string _shainCD);

        /// <summary>
        /// 指定した社員の個人設定情報をDBから取得する。
        /// </summary>
        /// <param name="_shainCD">対象社員コード</param>
        /// <returns>指定した社員の個人設定エンティティ</returns>
        UserSetting GetUserSettingByShainCD(string _shainCD);

        /// <summary>
        /// 社員の個人設定エンティティをDBに反映させる。
        /// </summary>
        /// <param name="_userSetting">個人設定エンティティ</param>
        /// <returns>レコードの更新件数</returns>
        int UpdateUserSetting(UserSetting _userSetting);

        /// <summary>
        ///  aliasをもとに社員CDを取得する。
        /// </summary>
        /// <param name="_alias">エイリアス文字列</param>
        /// <returns>レコードの更新件数</returns>
        string GetShainCDInfoByAlias(string _alias);

        /// <summary>
        ///  対象社員の最終ログイン時間を更新する。
        /// </summary>
        /// <param name="_shainCD">社員番号</param>
        /// <returns>レコードの更新件数</returns>
        int SetLastLoginAtNow(string _shainCD);


        /// <summary>
        /// LDAP認証結果を返却する
        /// </summary>
        /// <param name="_username">ユーザ名</param>
        /// <param name="_pwd">パスワード</param>
        /// <returns>LDAP認証結果</returns>
        AuthResult AuthLdap(string _username, string _pwd);
    }


    /// <summary>
    /// LDAP認証返却用クラス
    /// </summary>
    class AuthResult { };
}
