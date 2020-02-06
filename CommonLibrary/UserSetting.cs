using CommonLibrary.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    /// <summary>
    /// 個人設定のエンティティ
    /// </summary>
    public class UserSetting
    {
        public string EmployeeCD { get; set; }
        public List<KNS_D04> KinmuJissekiMasterList { get; set; }
        public List<KNS_D05> PJMasterList { get; set; }
        public List<KNS_D06> SagyoCDMasterList { get; set; }

        /// <summary>
        /// 個人設定のコンストラクタ
        /// </summary>
        public UserSetting(string _EmployeeCD, List<KNS_D04> _KinmuJissekiMasterList, List<KNS_D05> _PJMasterList, List<KNS_D06> _SagyoCDMasterList)
        {
            EmployeeCD = _EmployeeCD;
            KinmuJissekiMasterList = _KinmuJissekiMasterList;
            PJMasterList = _PJMasterList;
            SagyoCDMasterList = _SagyoCDMasterList;
        }

        /// <summary>
        /// 更新用個人設定のバリデーションチェック
        /// </summary>
        public void CheckValidation()
        {
            // 勤務時間系のバリデーションチェック
            KinmuJissekiMasterList[0].CheckValidationForForm();

            // 更新データのチェック処理
            // 更新後のプロジェクトコードが20件以上の場合、DB更新処理を実施しない
            if (this.PJMasterList.Count > 20)
            {
                //プロジェクトコードが20件以上になってしまうメッセージを表示
                throw new KinmuException("KS0003:これ以上追加できません。（最大20件です）");
            }

            // 更新後の作業コードが20件以上の場合、DB更新処理を実施しない
            if (this.SagyoCDMasterList.Count > 20)
            {
                // 作業コードが20件以上になってしまうメッセージを表示
                throw new KinmuException("KS0003:これ以上追加できません。（最大20件です）");
            }

            // 更新後のプロジェクトコードが0件の場合、DB更新処理を実施しない
            if (this.PJMasterList.Count < 1)
            {
                // プロジェクトコードが0件になってしまうメッセージを表示
                throw new KinmuException("KS0004:プロジェクトコードが0件です、1件以上追加してください。");
            }

            // 更新後の作業コードが0件の場合、DB更新処理を実施しない
            if (this.SagyoCDMasterList.Count < 1)
            {
                // 作業コードが0件になってしまうメッセージを表示
                throw new KinmuException("KS0004:作業コードが0件です、1件以上追加してください。");
            }
        }
    }
}