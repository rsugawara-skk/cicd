using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonLibrary;
using CommonLibrary.Models;
using System.Collections.Generic;

namespace CommonLibraryUT
{
    [TestClass]
    public class DBManagerUT
    {
        KNS_D01 d01 = new KNS_D01
        {
            SHAIN_CD = "9999999",
            DATA_Y = "2020",
            DATA_M = "4",
            DATA_D = "14",
            NINYO_CD = "01",
            NINKA_CD = "01",
            DKIJYUN_MIN = "460",
            STR_HR = "8",
            STR_MIN = "0",
            END_HR = "16",
            END_MIN = "40",
            END_PAR = "0",
            RESTS1_HR = "12",
            RESTS1_MIN = "0",
            RESTE1_HR = "13",
            RESTE1_MIN = "0",
            RESTS2_HR = null,
            RESTS2_MIN = null,
            RESTE2_HR = null,
            RESTE2_MIN = null,
            RESTS3_HR = null,
            RESTS3_MIN = null,
            RESTE3_HR = null,
            RESTE3_MIN = null,
            DKINM = 460,
            DCHOB = 0,
            DCHOD = 0,
            DYAKN = 0,
            DSHUK = 0,
            DMINA1 = 0,
            DMINA2 = 0,
            KAKN_FLG = "1",
            DKIJI = null,
            UPD_SHAIN_CD = "9999999",
        };

        KNS_D02 d02 = new KNS_D02
        {
            SHAIN_CD = "9999999",
            DATA_Y = "2020",
            DATA_M = "4",
            DATA_D = "1",
            PROJ_CD = "V00000000A",
            SAGYO_CD = "01",
            SAGYO_MIN = 460,
            UPD_DATE = new DateTime(2020, 4, 14),
        };

        KNS_D04 d04 = new KNS_D04
        {
            SHAIN_CD = "9999999",
            NINSE_CD = "01",
            STR_HR = "8",
            STR_MIN = "00",
            END_HR = "16",
            END_MIN = "40",
            END_PAR = "0",
            RESTS1_HR = "12",
            RESTS1_MIN = "0",
            RESTE1_HR = "13",
            RESTE1_MIN = "0",
            RESTS2_HR = null,
            RESTS2_MIN = null,
            RESTE2_HR = null,
            RESTE2_MIN = null,
            RESTS3_HR = null,
            RESTS3_MIN = null,
            RESTE3_HR = null,
            RESTE3_MIN = null,
            DKIJYUN_MIN = 460,
            DKINM = 460,
            FLX_FLG = "1",
            UPD_DATE = new DateTime(2020, 4, 14),
            PROJ_CD = "V00000000A",
            SAGYO_CD = "01",
        };

        KNS_D05 d05 = new KNS_D05
        {
            SHAIN_CD = "9999999",
            PROJ_CD = "V00000000A",
            UPD_DATE = new DateTime(2020, 4, 14),
            VIEW_ORDER = "1",
        };

        KNS_D06 d06 = new KNS_D06
        {
            SHAIN_CD = "9999999",
            SAGYO_CD = "01",
            UPD_DATE = new DateTime(2020, 4, 14),
            VIEW_ORDER = "1",
        };

        KNS_D13 d13 = new KNS_D13
        {
            SHAIN_CD = "9999999",
            DATA_Y = "2020",
            DATA_M = "4",
            DATA_D = "1",
            STR_Y_HR = "8",
            STR_Y_MIN = "00",
            END_Y_HR = "16",
            END_Y_MIN = "40",
            END_Y_PAR = "0",
            KAKN_FLG = "1",
            UPD_SHAIN_CD = "9999999",
            YOTEI_CD = "10",
            SHONIN_FLG = "1"
        };

        KNS_D14 d14 = new KNS_D14
        {
            SHAIN_CD = "0000006",
            LAST_LOGIN_DATE = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Hour + ":" + DateTime.Now.Minute
        };

        KNS_M05 m05 = new KNS_M05
        {
            DATA_Y = "2020",
            DATA_M = "4",
            DATA_D = "1",
            NINYO_CD = "01",
            KEITAI = "1",
            SHUKU_FLG = "0",
            UPD_DATE = new DateTime(2020, 4, 14),
        };


        //勤務実績の試験
        [TestMethod]
        public void KinmuJissekiTest()
        {
            DeleteKinmuJissekiTest();
            InsertKinmuJissekiTest();
            UpdateKinmuJissekiTest();
            GetKinmuJissekiTest1();
            GetKinmuJissekiTest2();
        }


        //勤務予定の試験
        [TestMethod]
        public void KinmuYoteiTest()
        {
            DeleteKinmuYoteiTest();
            InsertKinmuYoteiTest();
            UpdateKinmuYoteiTest();
            GetKinmuYoteiTest1();
            GetKinmuYoteiTest2();
        }

        //作業日誌の試験
        [TestMethod]
        public void SagyoNisshiTest()
        {
            DeleteSagyoNisshiTest();
            InsertSagyoNisshiTest();
            UpdateSagyoNisshiTest();
            GetSagyoNisshiTest1();
            GetSagyoNisshiTest2();
        }

        //個人設定の試験
        [TestMethod]
        public void KojinSetteiTest()
        {
            DeleteKojinSetteiKinmuJissekiTest();
            InsertKojinSetteiKinmuJissekiTest();
            UpdateKojinSetteiKinmuJissekiTest();
            GetKojinSetteiKinmuJissekiTest();

            DeleteKojinSetteiPJCDTest();
            InsertKojinSetteiPJCDTest();
            UpdateKojinSetteiPJCDTest();
            GetKojinSetteiPJCDTest();

            DeleteKojinSetteiSagyoCDTest();
            InsertKojinSetteiSagyoCDTest();
            UpdateKojinSetteiSagyoCDTest();
            GetKojinSetteiSagyoCDTest();
        }

        //メッセージ・休暇・年休・最終ログインの試験
        [TestMethod]
        public void MessageTest()
        {
            //InsertLastLoginTestは、d14のSHAIN_CDが重複するためここでは実施しません
            //InsertLastLoginTest();
            UpdateLastLoginTest();
            GetLastLoginTest();

            GetMessageAllTest();
            GetTokkyuKokyuDataTest();
            GetNenkyuDataTest();
        }

        //マスタの試験
        [TestMethod]
        public void MasterTest()
        {
            GetSyainMasterAllTest();
            GetSyainMasterByShainCDTest();
            GetSyainMasterByAliasTest();
            GetSyainMasterByShoninShainCDTest();

            GetPJCDMasterAllTest();
            GetSagyoCDMasterAllTest();
            GetKinmuNinsyoCDMasterALLTest();

            GetCalenderTest1();
            GetCalenderTest2();
            GetCalenderTest3();

            GetSyuyakubiMasterTest1();
            GetSyuyakubiMasterTest2();
        }

        //ユーティティの試験
        [TestMethod]
        public void UtilityTest()
        {
            GetKinmuRecordRowTest1();
            GetKinmuRecordRowTest2();
            SetKinmuRecordRowTest();

            SetKinmuJissekiTest();
            SetKinmuYoteiTest();

            SetSagyoNisshiTest();

            GetUserSettingTest();
            SetUserSettingTest();

            SetKojinSetteiKinumuJissekiTest();
            SetKojinSetteiPJCDTest();
            SetKojinSetteiSagyoCDTest();

            GetNenkyuZanTest();
            GetTokkyuKoukyuZanTest();

            PJCDToStringTest1();
            PJCDToStringTest2();

            SagyoCDToStringTest1();
            SagyoCDToStringTest2();
            SetLastLoginAtNowTest();

            GetShuyakuYMTest();
            UpdateShoninFlagTest();

            GetLatestMessageTest();
            GetHouteigaiRuikeiTest();
            GetTyoukinRuikeiTest();
        }

        //勤務実績の試験
        public void GetKinmuJissekiTest1()
        {
            DBManager manager = new DBManager();
            List<KNS_D01> res = manager.GetKinmuJisseki("9999999", 2020, 4, 14);
            Assert.IsTrue(res.Count == 1, "Getに失敗" + res + "件");
        }

        public void GetKinmuJissekiTest2()
        {
            DBManager manager = new DBManager();
            List<KNS_D01> res = manager.GetKinmuJisseki(null, 2013, 12, 0);
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void InsertKinmuJissekiTest()
        {
            var a = new List<KNS_D01>();
            a.Add(d01);
            DBManager manager = new DBManager();
            int res = manager.InsertKinmuJisseki(a);
            Assert.IsTrue(res == 1, "Insertに失敗" + res + "件");
        }

        public void UpdateKinmuJissekiTest()
        {
            var a = new List<KNS_D01>();
            a.Add(d01);
            DBManager manager = new DBManager();
            int res = manager.UpdateKinmuJisseki(a);
            Assert.IsTrue(res == 1, "Updateに失敗" + res + "件");
        }

        public void DeleteKinmuJissekiTest()
        {
            var a = new List<KNS_D01>();
            a.Add(d01);
            DBManager manager = new DBManager();
            int res = manager.DeleteKinmuJisseki(a);
            Assert.IsTrue(res == 1, "Deleteに失敗" + res + "件");
        }

        //勤務予定の試験
        public void GetKinmuYoteiTest1()
        {
            DBManager manager = new DBManager();
            List<KNS_D13> res = manager.GetKinmuYotei("1201301", 2013, 12, 3);
            Assert.IsTrue(res.Count == 1, "Getに失敗" + res + "件");
        }

        public void GetKinmuYoteiTest2()
        {
            DBManager manager = new DBManager();
            List<KNS_D13> res = manager.GetKinmuYotei(null, 2013, 12, 0); ;
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void InsertKinmuYoteiTest()
        {
            var a = new List<KNS_D13>();
            a.Add(d13);
            DBManager manager = new DBManager();
            int res = manager.InsertKinmuYotei(a);
            Assert.IsTrue(res == 1, "Insertに失敗" + res + "件");
        }

        public void UpdateKinmuYoteiTest()
        {
            var a = new List<KNS_D13>();
            a.Add(d13);
            DBManager manager = new DBManager();
            int res = manager.UpdateKinmuYotei(a);
            Assert.IsTrue(res == 1, "Updateに失敗" + res + "件");
        }

        public void DeleteKinmuYoteiTest()
        {
            var a = new List<KNS_D13>();
            a.Add(d13);
            DBManager manager = new DBManager();
            int res = manager.DeleteKinmuYotei(a);
            Assert.IsTrue(res == 1, "Deleteに失敗" + res + "件");
        }

        //作業日誌の試験
        public void GetSagyoNisshiTest1()
        {
            DBManager manager = new DBManager();
            List<KNS_D02> res = manager.GetSagyoNisshi("9999999", 2020, 4);
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void GetSagyoNisshiTest2()
        {
            DBManager manager = new DBManager();
            List<KNS_D02> res = manager.GetSagyoNisshi("9999999", 2020, 4, 14);
            Assert.IsTrue(res.Count == 1, "Getに失敗" + res + "件");
        }

        public void InsertSagyoNisshiTest()
        {
            var a = new List<KNS_D02>();
            a.Add(d02);
            DBManager manager = new DBManager();
            int res = manager.InsertSagyoNisshi(a);
            Assert.IsTrue(res == 1, "Insertに失敗" + res + "件");
        }

        public void UpdateSagyoNisshiTest()
        {
            var a = new List<KNS_D02>();
            a.Add(d02);
            DBManager manager = new DBManager();
            int res = manager.UpdateSagyoNisshi(a);
            Assert.IsTrue(res == 1, "Updateに失敗" + res + "件");
        }

        public void DeleteSagyoNisshiTest()
        {
            var a = new List<KNS_D02>();
            a.Add(d02);
            DBManager manager = new DBManager();
            int res = manager.DeleteSagyoNisshi(a);
            Assert.IsTrue(res == 1, "Deleteに失敗" + res + "件");
        }

        //個人設定の試験
        public void GetKojinSetteiKinmuJissekiTest()
        {
            DBManager manager = new DBManager();
            List<KNS_D04> res = manager.GetKojinSetteiKinmuJisseki("9999999");
            Assert.IsTrue(res.Count == 1, "Getに失敗" + res + "件");
        }

        public void InsertKojinSetteiKinmuJissekiTest()
        {
            var a = new List<KNS_D04>();
            a.Add(d04);
            DBManager manager = new DBManager();
            int res = manager.InsertKojinSetteiKinmuJisseki(a);
            Assert.IsTrue(res == 1, "Insertに失敗" + res + "件");
        }

        public void UpdateKojinSetteiKinmuJissekiTest()
        {
            var a = new List<KNS_D04>();
            a.Add(d04);
            DBManager manager = new DBManager();
            int res = manager.UpdateKojinSetteiKinmuJisseki(a);
            Assert.IsTrue(res == 1, "Updateに失敗" + res + "件");
        }

        public void DeleteKojinSetteiKinmuJissekiTest()
        {
            var a = new List<KNS_D04>();
            a.Add(d04);
            DBManager manager = new DBManager();
            int res = manager.DeleteKojinSetteiKinmuJisseki(a);
            Assert.IsTrue(res == 1, "Deleteに失敗" + res + "件");
        }

        public void GetKojinSetteiPJCDTest()
        {
            DBManager manager = new DBManager();
            List<KNS_D05> res = manager.GetKojinSetteiPJCD("9999999");
            Assert.IsTrue(res.Count == 1, "Getに失敗" + res + "件");
        }

        public void InsertKojinSetteiPJCDTest()
        {
            var a = new List<KNS_D05>();
            a.Add(d05);
            DBManager manager = new DBManager();
            int res = manager.InsertKojinSetteiPJCD(a);
            Assert.IsTrue(res == 1, "Insertに失敗" + res + "件");
        }

        public void UpdateKojinSetteiPJCDTest()
        {
            var a = new List<KNS_D05>();
            a.Add(d05);
            DBManager manager = new DBManager();
            int res = manager.UpdateKojinSetteiPJCD(a);
            Assert.IsTrue(res == 1, "Updateに失敗" + res + "件");
        }

        public void DeleteKojinSetteiPJCDTest()
        {
            var a = new List<KNS_D05>();
            a.Add(d05);
            DBManager manager = new DBManager();
            int res = manager.DeleteKojinSetteiPJCD(a);
            Assert.IsTrue(res == 1, "Deleteに失敗" + res + "件");
        }

        public void GetKojinSetteiSagyoCDTest()
        {
            DBManager manager = new DBManager();
            List<KNS_D06> res = manager.GetKojinSetteiSagyoCD("9999999");
            Assert.IsTrue(res.Count == 1, "Getに失敗" + res + "件");
        }

        public void InsertKojinSetteiSagyoCDTest()
        {
            var a = new List<KNS_D06>();
            a.Add(d06);
            DBManager manager = new DBManager();
            int res = manager.InsertKojinSetteiSagyoCD(a);
            Assert.IsTrue(res == 1, "Insertに失敗" + res + "件");
        }

        public void UpdateKojinSetteiSagyoCDTest()
        {
            var a = new List<KNS_D06>();
            a.Add(d06);
            DBManager manager = new DBManager();
            int res = manager.UpdateKojinSetteiSagyoCD(a);
            Assert.IsTrue(res == 1, "Updateに失敗" + res + "件");
        }

        public void DeleteKojinSetteiSagyoCDTest()
        {
            var a = new List<KNS_D06>();
            a.Add(d06);
            DBManager manager = new DBManager();
            int res = manager.DeleteKojinSetteiSagyoCD(a);
            Assert.IsTrue(res == 1, "Deleteに失敗" + res + "件");
        }

        //メッセージ・休暇・年休・最終ログインの試験
        public void GetMessageAllTest()
        {
            DBManager manager = new DBManager();
            List<KNS_D08> res = manager.GetMessageAll();
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void GetTokkyuKokyuDataTest()
        {
            DBManager manager = new DBManager();
            KNS_D11 res = manager.GetTokkyuKokyuData("9017812", 2016);
            Assert.IsTrue(res != null, "Getに失敗" + res + "件");
        }

        public void GetNenkyuDataTest()
        {
            DBManager manager = new DBManager();
            KNS_D12 res = manager.GetNenkyuData("9017812", 2016);
            Assert.IsTrue(res != null, "Getに失敗" + res + "件");
        }

        public void GetLastLoginTest()
        {
            DBManager manager = new DBManager();
            KNS_D14 res = manager.GetLastLogin("9017812");
            Assert.IsTrue(res != null, "Getに失敗" + res + "件");
        }

        public void InsertLastLoginTest()
        {
            var a = new KNS_D14();
            a = d14;
            DBManager manager = new DBManager();
            int res = manager.InsertLastLogin(a);
            Assert.IsTrue(res == 1, "Insertに失敗" + res + "件");
        }

        public void UpdateLastLoginTest()
        {
            var a = new KNS_D14();
            a = d14;
            DBManager manager = new DBManager();
            int res = manager.UpdateLastLogin(a);
            Assert.IsTrue(res == 1, "Updateに失敗" + res + "件");
        }

        //マスタの試験
        public void GetSyainMasterAllTest()
        {
            DBManager manager = new DBManager();
            List<KNS_M01> res = manager.GetSyainMasterAll();
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void GetSyainMasterByShainCDTest()
        {
            DBManager manager = new DBManager();
            KNS_M01 res = manager.GetSyainMasterByShainCD("9017812");
            Assert.IsTrue(res != null, "Getに失敗" + res + "件");
        }

        public void GetSyainMasterByAliasTest()
        {
            DBManager manager = new DBManager();
            KNS_M01 res = manager.GetSyainMasterByAlias("rokamoto");
            Assert.IsTrue(res != null, "Getに失敗" + res + "件");
        }

        public void GetSyainMasterByShoninShainCDTest()
        {
            DBManager manager = new DBManager();
            List<KNS_M01> res = manager.GetSyainMasterByShoninShainCD("9012758");
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void GetPJCDMasterAllTest()
        {
            DBManager manager = new DBManager();
            List<KNS_M02> res = manager.GetPJCDMasterAll();
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void GetSagyoCDMasterAllTest()
        {
            DBManager manager = new DBManager();
            List<KNS_M03> res = manager.GetSagyoCDMasterAll();
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void GetKinmuNinsyoCDMasterALLTest()
        {
            DBManager manager = new DBManager();
            List<KNS_M04> res = manager.GetKinmuNinsyoCDMasterALL();
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void GetCalenderTest1()
        {
            DBManager manager = new DBManager();
            List<KNS_M05> res = manager.GetCalender(2016);
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void GetCalenderTest2()
        {
            DBManager manager = new DBManager();
            List<KNS_M05> res = manager.GetCalender(2016, 4);
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void GetCalenderTest3()
        {
            DBManager manager = new DBManager();
            List<KNS_M05> res = manager.GetCalender(2016, 4, 1);
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void GetSyuyakubiMasterTest1()
        {
            DBManager manager = new DBManager();
            List<KNS_M07> res = manager.GetSyuyakubiMaster(2016);
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void GetSyuyakubiMasterTest2()
        {
            DBManager manager = new DBManager();
            List<KNS_M07> res = manager.GetSyuyakubiMaster(2016, 4);
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        //ユーティティの試験
        public void GetKinmuRecordRowTest1()
        {
            DBManager manager = new DBManager();
            List<KinmuRecordRow> res = manager.GetKinmuRecordRow("9017812", 2016, 7, 1);
            Assert.IsTrue(res.Count == 1, "Getに失敗" + res + "件");
        }

        public void GetKinmuRecordRowTest2()
        {
            DBManager manager = new DBManager();
            List<KinmuRecordRow> res = manager.GetKinmuRecordRow();
            Assert.IsTrue(res.Count == 0, "Getに失敗" + res.Count + "件");
        }

        public void SetKinmuRecordRowTest()
        {
            var a = new List<KNS_D02>();
            a.Add(d02);
            var b = new List<KinmuRecordRow>();
            KinmuRecordRow kinmu = new KinmuRecordRow("9999999", d01, d13, a, m05);
            b.Add(kinmu);
            DBManager manager = new DBManager();
            int res = manager.SetKinmuRecordRow(b);
            Assert.IsTrue(res != 0, "Setに失敗" + res + "件");
        }

        public void SetKinmuJissekiTest()
        {
            var a = new List<KNS_D01>();
            a.Add(d01);
            DBManager manager = new DBManager();
            int res = manager.SetKinmuJisseki(a);
            Assert.IsTrue(res != 0, "Setに失敗" + res + "件");
        }

        public void SetKinmuYoteiTest()
        {
            var a = new List<KNS_D13>();
            a.Add(d13);
            DBManager manager = new DBManager();
            int res = manager.SetKinmuYotei(a);
            Assert.IsTrue(res != 0, "Setに失敗" + res + "件");
        }

        public void SetSagyoNisshiTest()
        {
            var a = new List<KNS_D02>();
            a.Add(d02);
            DBManager manager = new DBManager();
            int res = manager.SetSagyoNisshi(a);
            Assert.IsTrue(res != 0, "Setに失敗" + res + "件");
        }

        public void GetUserSettingTest()
        {
            DBManager manager = new DBManager();
            UserSetting res = manager.GetUserSetting("9999999");
            Assert.IsTrue(res != null, "Getに失敗" + res + "件");
        }

        public void SetUserSettingTest()
        {
            var a = new List<KNS_D04>();
            a.Add(d04);
            var b = new List<KNS_D05>();
            b.Add(d05);
            var c = new List<KNS_D06>();
            c.Add(d06);
            var d = new UserSetting("9999999", a, b, c);
            DBManager manager = new DBManager();
            int res = manager.SetUserSetting(d);
            Assert.IsTrue(res != 0, "Setに失敗" + res + "件");
        }

        public void SetKojinSetteiKinumuJissekiTest()
        {
            var a = new List<KNS_D04>();
            a.Add(d04);
            DBManager manager = new DBManager();
            int res = manager.SetKojinSetteiKinumuJisseki(a);
            Assert.IsTrue(res == 1, "Setに失敗" + res + "件");
        }

        public void SetKojinSetteiPJCDTest()
        {
            var a = new List<KNS_D05>();
            a.Add(d05);
            DBManager manager = new DBManager();
            int res = manager.SetKojinSetteiPJCD(a);
            Assert.IsTrue(res == 1, "Setに失敗" + res + "件");
        }

        public void SetKojinSetteiSagyoCDTest()
        {
            var a = new List<KNS_D06>();
            a.Add(d06);
            DBManager manager = new DBManager();
            int res = manager.SetKojinSetteiSagyoCD(a);
            Assert.IsTrue(res == 1, "Setに失敗" + res + "件");
        }

        public void GetNenkyuZanTest()
        {
            DBManager manager = new DBManager();
            double res = manager.GetNenkyuZan("9017812");
            Assert.IsTrue(res != 0, "Getに失敗" + res + "件");
        }

        public void GetTokkyuKoukyuZanTest()
        {
            DBManager manager = new DBManager();
            double[] res = manager.GetTokkyuKoukyuZan("9017812", 2016);
            Assert.IsTrue(res != null, "Getに失敗" + res + "件");
        }

        public void PJCDToStringTest1()
        {
            DBManager manager = new DBManager();
            Dictionary<string, string> res = manager.PJCDToString();
            Assert.IsTrue(res != null, "Getに失敗" + res + "件");
        }

        public void PJCDToStringTest2()
        {
            DBManager manager = new DBManager();
            string res = manager.PJCDToString("V00504520A");
            Assert.IsTrue(res != null, "Getに失敗" + res + "件");
        }

        public void SagyoCDToStringTest1()
        {
            DBManager manager = new DBManager();
            Dictionary<string, string> res = manager.SagyoCDToString();
            Assert.IsTrue(res != null, "Getに失敗" + res + "件");
        }

        public void SagyoCDToStringTest2()
        {
            DBManager manager = new DBManager();
            string res = manager.SagyoCDToString("01");
            Assert.IsTrue(res != null, "Getに失敗" + res + "件");
        }

        public void SetLastLoginAtNowTest()
        {
            DBManager manager = new DBManager();
            int res = manager.SetLastLoginAtNow("9017812");
            Assert.IsTrue(res == 1, "Setに失敗" + res + "件");
        }

        public void GetShuyakuYMTest()
        {
            DBManager manager = new DBManager();
            int[] res = manager.GetShuyakuYM();
            Assert.IsTrue(res != null, "Getに失敗" + res + "件");
        }

        public void UpdateShoninFlagTest()
        {
            DBManager manager = new DBManager();
            int res = manager.UpdateShoninFlag("9012758", "9017812", 2016, 7);
            Assert.IsTrue(res != 0, "Updateに失敗" + res + "件");
        }

        public void GetLatestMessageTest()
        {
            DBManager manager = new DBManager();
            List<string> res = manager.GetLatestMessage();
            Assert.IsTrue(res.Count != 0, "Getに失敗" + res + "件");
        }

        public void GetHouteigaiRuikeiTest()
        {
            DBManager manager = new DBManager();
            int res = manager.GetHouteigaiRuikei("9017812", 2017, 5);
            Assert.IsTrue(res != 0, "Getに失敗" + res + "件");
        }

        public void GetTyoukinRuikeiTest()
        {
            DBManager manager = new DBManager();
            int res = manager.GetTyoukinRuikei("9017812", 2016, 7);
            Assert.IsTrue(res != 0, "Getに失敗" + res + "件");
        }
    }
}

