using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonLibrary.Models;
using KinmuLibrary;
using System.Collections.Generic;

namespace KinmuLibraryUT
{
    [TestClass]
    public class KinmuLibraryUT
    {
        //基本的には以下2つのデータで試験実施（Manager1のデータはDBに作成）
        //Manager2の試験をする際、DBにデータがなかったらデータが存在する社員コードを使用
        //終盤のテストはManager1にデータがないので0で返ってくるか試験している
        readonly KinmuManager manager1 = new KinmuManager("9017812", 2018, 11);
        readonly KinmuManager manager2 = new KinmuManager("9017812", 2016, 7);

        readonly KNS_D01 d01 = new KNS_D01
        {
            SHAIN_CD = "9999999",
            DATA_Y = "2018",
            DATA_M = "11",
            DATA_D = "1",
            NINYO_CD = "01",
            NINKA_CD = "01",
            DKIJYUN_MIN = "460",
            STR_HR = "9",
            STR_MIN = "20",
            END_HR = "19",
            END_MIN = "00",
            END_PAR = "0",
            RESTS1_HR = "12",
            RESTS1_MIN = "0",
            RESTE1_HR = "12",
            RESTE1_MIN = "30",
            RESTS2_HR = "",
            RESTS2_MIN = "",
            RESTE2_HR = "",
            RESTE2_MIN = "",
            RESTS3_HR = null,
            RESTS3_MIN = null,
            RESTE3_HR = null,
            RESTE3_MIN = null,
            DKINM = 0,
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

        readonly KNS_D01 d02 = new KNS_D01
        {
            SHAIN_CD = "9999999",
            DATA_Y = "2018",
            DATA_M = "11",
            DATA_D = "1",
            NINYO_CD = "01",
            NINKA_CD = "01",
            DKIJYUN_MIN = "460",
            STR_HR = "9",
            STR_MIN = "20",
            END_HR = "22",
            END_MIN = "00",
            END_PAR = "0",
            RESTS1_HR = "12",
            RESTS1_MIN = "0",
            RESTE1_HR = "13",
            RESTE1_MIN = "0",
            RESTS2_HR = "16",
            RESTS2_MIN = "0",
            RESTE2_HR = "17",
            RESTE2_MIN = "0",
            RESTS3_HR = "21",
            RESTS3_MIN = "30",
            RESTE3_HR = "22",
            RESTE3_MIN = "30",
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


        readonly KNS_D01 d03 = new KNS_D01
        {
            SHAIN_CD = "9999999",
            DATA_Y = "2018",
            DATA_M = "11",
            DATA_D = "1",
            NINYO_CD = "01",
            NINKA_CD = "01",
            DKIJYUN_MIN = "460",
            STR_HR = "9",
            STR_MIN = "20",
            END_HR = "1",
            END_MIN = "00",
            END_PAR = "1",
            RESTS1_HR = "12",
            RESTS1_MIN = "0",
            RESTE1_HR = "13",
            RESTE1_MIN = "0",
            RESTS2_HR = "16",
            RESTS2_MIN = "0",
            RESTE2_HR = "17",
            RESTE2_MIN = "0",
            RESTS3_HR = "23",
            RESTS3_MIN = "30",
            RESTE3_HR = "0",
            RESTE3_MIN = "30",
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

        [TestMethod]
        public void CalcGekkanSyoteiNissuTest()
        {
            int res1 = manager1.CalcGekkanSyoteiNissu();
            Assert.IsTrue(res1 == 23, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanSyoteiNissu();
            Assert.IsTrue(res2 == 20, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanSyukkinNissuTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanSyukkinNissu();
            Assert.IsTrue(res1 == 14, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanSyukkinNissu();
            Assert.IsTrue(res2 == 20, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanKyujitsuRoudouNissuTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanKyujitsuRoudouNissu();
            Assert.IsTrue(res1 == 3, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanKyujitsuRoudouNissu();
            Assert.IsTrue(res2 == 0, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcBarNissuTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcBarNissu();
            Assert.IsTrue(res1 == 0, "失敗1：res = " + res1);
            int res2 = manager2.CalcBarNissu();
            Assert.IsTrue(res2 == 0, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanDaikyuNissuTest()
        {
            int res1 = manager1.CalcGekkanDaikyuNissu();
            Assert.IsTrue(res1 == 1, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9017812", 2018, 11);
            int res2 = manager3.CalcGekkanDaikyuNissu();
            Assert.IsTrue(res2 == 1, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanHibanNissuTest()
        {
            int res1 = manager1.CalcGekkanHibanNissu();
            Assert.IsTrue(res1 == 0, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanHibanNissu();
            Assert.IsTrue(res2 == 0, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanYukyuNissuTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanYukyuNissu();
            Assert.IsTrue(res1 == 2, "失敗1：res = " + res1);
            //DBに有給のデータがあればmanager2もテストできる
            //int res2 = manager3.CalcGekkanYukyuNissu();
            //Assert.IsTrue(res2 != 0, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanMukyuNissuTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanMukyuNissu();
            Assert.IsTrue(res1 == 3, "失敗1：res = " + res1);
            //DBに無給のデータがあればmanager2もテストできる
            //int res2 = manager2.CalcGekkanMukyuNissu();
            //Assert.IsTrue(res2 != 0, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanKumikyuNissuTest()
        {
            int res1 = manager1.CalcGekkanKumikyuNissu();
            Assert.IsTrue(res1 == 0, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanKumikyuNissu();
            Assert.IsTrue(res2 == 0, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanKokyuRoudouNissuTest1()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanKokyuRoudouNissu();
            Assert.IsTrue(res1 == 2, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9017812", 2017, 4);
            int res2 = manager3.CalcGekkanKokyuRoudouNissu();
            Assert.IsTrue(res2 == 0, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanSyoteiRoudoNissuTest()
        {
            int res1 = manager1.CalcGekkanSyoteiRoudoNissu();
            Assert.IsTrue(res1 == 23, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanSyoteiRoudoNissu();
            Assert.IsTrue(res2 == 20, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanRoudouNissuTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanRoudouNissu();
            Assert.IsTrue(res1 == 20, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanRoudouNissu();
            Assert.IsTrue(res2 == 20, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanTokkyuYoteiNissuTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanTokkyuYoteiNissu();
            Assert.IsTrue(res1 == 5, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanTokkyuYoteiNissu();
            Assert.IsTrue(res2 == 6, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanKoukyuYoteiNissuTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanKoukyuYoteiNissu();
            Assert.IsTrue(res1 == 4, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanKoukyuYoteiNissu();
            Assert.IsTrue(res2 == 5, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanTokkyuKakuteiNissuTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanTokkyuKakuteiNissu();
            Assert.IsTrue(res1 == 5, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanTokkyuKakuteiNissu();
            Assert.IsTrue(res2 == 6, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanKoukyuKakuteiNissuTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanKoukyuKakuteiNissu();
            Assert.IsTrue(res1 == 4, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanKoukyuKakuteiNissu();
            Assert.IsTrue(res2 == 5, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanTotalJitsuRoudoJikanTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanTotalJitsuRoudoJikan();
            Assert.IsTrue(res1 == 11390, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanTotalJitsuRoudoJikan();
            Assert.IsTrue(res2 == 9820, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanYukyuJikanTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanYukyuJikan();
            Assert.IsTrue(res1 == 2060, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9017812", 2016, 11);
            int res2 = manager3.CalcGekkanYukyuJikan();
            Assert.IsTrue(res2 == 460, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanTankaAJikanTest()
        {
            int res1 = manager1.CalcGekkanTankaAJikan();
            Assert.IsTrue(res1 == 0, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanTankaAJikan();
            Assert.IsTrue(res2 == 0, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanTankaBJikanTest()
        {
            //KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            //int res1 = manager1.CalcGekkanTankaBJikan();
            //Assert.IsTrue(res1 == 2850, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9017812", 2017, 4);
            int res2 = manager3.CalcGekkanTankaBJikan();
            Assert.IsTrue(res2 == 3500, "失敗2：res = " + res2);
            KinmuManager manager4 = new KinmuManager("9017812", 2016, 7);
            int res3 = manager4.CalcGekkanTankaBJikan();
            Assert.IsTrue(res3 == 620, "失敗3：res = " + res3);
            KinmuManager manager5 = new KinmuManager("9400346", 2013, 2);
            int res4 = manager5.CalcGekkanTankaBJikan();
            Assert.IsTrue(res4 == 4200, "失敗4：res = " + res4);
            KinmuManager manager6 = new KinmuManager("9016617", 2014, 9);
            int res5 = manager6.CalcGekkanTankaBJikan();
            Assert.IsTrue(res5 == 4050, "失敗5：res = " + res5);
        }

        [TestMethod]
        public void CalcGekkanTankaTokkyuDJikanTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanTankaTokkyuDJikan();
            Assert.IsTrue(res1 == 1370, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9400346", 2013, 2);
            int res2 = manager3.CalcGekkanTankaTokkyuDJikan();
            Assert.IsTrue(res2 == 590, "失敗2：res = " + res2);
            KinmuManager manager4 = new KinmuManager("9017812", 2017, 4);
            int res3 = manager4.CalcGekkanTankaTokkyuDJikan();
            Assert.IsTrue(res3 == 50, "失敗3：res = " + res3);
        }

        [TestMethod]
        public void CalcGekkanTankaKoukyuDJikanTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanTankaKoukyuDJikan();
            Assert.IsTrue(res1 == 520, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9400346", 2013, 2);
            int res2 = manager3.CalcGekkanTankaKoukyuDJikan();
            Assert.IsTrue(res2 == 970, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanTankaDJikanTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanTankaDJikan();
            Assert.IsTrue(res1 == 1400, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanTankaDJikan();
            Assert.IsTrue(res2 == 0, "失敗2：res = " + res2);
            KinmuManager manager3 = new KinmuManager("9400346", 2013, 2);
            int res3 = manager3.CalcGekkanTankaDJikan();
            Assert.IsTrue(res3 == 1560, "失敗3：res = " + res3);
            KinmuManager manager4 = new KinmuManager("9017812", 2017, 4);
            int res4 = manager4.CalcGekkanTankaDJikan();
            Assert.IsTrue(res4 == 50, "失敗4：res = " + res4);
        }

        [TestMethod]
        public void CalcGekkanTankaCJikanTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanTankaCJikan();
            Assert.IsTrue(res1 == 0, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9013373", 2014, 9);
            int res2 = manager3.CalcGekkanTankaCJikan();
            Assert.IsTrue(res2 == 30, "失敗2：res = " + res2);
            KinmuManager manager4 = new KinmuManager("9017812", 2017, 4);
            int res3 = manager4.CalcGekkanTankaCJikan();
            Assert.IsTrue(res3 == 270, "失敗3：res = " + res3);
            KinmuManager manager5 = new KinmuManager("9400346", 2013, 2);
            int res4 = manager5.CalcGekkanTankaCJikan();
            Assert.IsTrue(res4 == 170, "失敗4：res = " + res4);
        }

        [TestMethod]
        public void CalcGekkanKoujyoAJikanTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanKoujyoAJikan();
            Assert.IsTrue(res1 == 460, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9017812", 2018, 11);
            int res2 = manager3.CalcGekkanKoujyoAJikan();
            Assert.IsTrue(res2 == 460, "失敗2：res = " + res2);
            KinmuManager manager4 = new KinmuManager("1201519", 2017, 2);
            int res3 = manager4.CalcGekkanKoujyoAJikan();
            Assert.IsTrue(res3 == 920, "失敗3：res = " + res3);
        }

        [TestMethod]
        public void CalcGekkanTankaEJikanTest()
        {
            //KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            //int res1 = manager1.CalcGekkanTankaEJikan();
            //Assert.IsTrue(res1 == 650, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanTankaEJikan();
            Assert.IsTrue(res2 == 0, "失敗2：res = " + res2);
            KinmuManager manager3 = new KinmuManager("9400346", 2013, 2);
            int res3 = manager3.CalcGekkanTankaEJikan();
            Assert.IsTrue(res3 == 1190, "失敗3：res = " + res3);
        }

        [TestMethod]
        public void CalcGekkanGengakuAjikanTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanGengakuAjikan();
            Assert.IsTrue(res1 == 0, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9017812", 2016, 4);
            int res2 = manager3.CalcGekkanGengakuAjikan();
            Assert.IsTrue(res2 == 9200, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanJikangaiRoudoJikanTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanJikangaiRoudoJikan();
            Assert.IsTrue(res1 == 1105, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9016617", 2014, 9);
            int res2 = manager3.CalcGekkanJikangaiRoudoJikan();
            Assert.IsTrue(res2 == 3695, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanKeieiKokyuRoudoJikanTest()
        {
            int res1 = manager2.CalcGekkanKeieiKokyuRoudoJikan();
            Assert.IsTrue(res1 == 0, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("7843293", 2016, 7);
            int res2 = manager3.CalcGekkanKeieiKokyuRoudoJikan();
            Assert.IsTrue(res2 == 240, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanHouTeiRoudoJikanTest()
        {
            int res1 = manager1.CalcGekkanHouTeiRoudoJikan();
            Assert.IsTrue(res1 == 10285, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanHouTeiRoudoJikan();
            Assert.IsTrue(res2 == 10628, "失敗2：res = " + res2);
            KinmuManager manager3 = new KinmuManager("9016617", 2014, 9);
            int res3 = manager3.CalcGekkanHouTeiRoudoJikan();
            Assert.IsTrue(res3 == 10285, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanSyoteiRoudoJikanTest()
        {
            int res1 = manager1.CalcGekkanSyoteiRoudoJikan();
            Assert.IsTrue(res1 == 10580, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanSyoteiRoudoJikan();
            Assert.IsTrue(res2 == 9200, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanMinashi1JikanTest()
        {
            int res1 = manager1.CalcGekkanMinashi1Jikan();
            Assert.IsTrue(res1 == 0, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanMinashi1Jikan();
            Assert.IsTrue(res2 == 460, "失敗2：res = " + res2);
            KinmuManager manager3 = new KinmuManager("7743391", 2016, 2);
            int res3 = manager3.CalcGekkanMinashi1Jikan();
            Assert.IsTrue(res3 == 920, "失敗3：res = " + res3);
        }

        [TestMethod]
        public void CalcGekkanSyukujituRoudoJikanTest()
        {
            int res1 = manager1.CalcGekkanSyukujituRoudoJikan();
            Assert.IsTrue(res1 == 0, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9016617", 2014, 9);
            int res2 = manager3.CalcGekkanSyukujituRoudoJikan();
            Assert.IsTrue(res2 == 730, "失敗2：res = " + res2);
            KinmuManager manager4 = new KinmuManager("9012859", 2015, 9);
            int res3 = manager4.CalcGekkanSyukujituRoudoJikan();
            Assert.IsTrue(res3 == 700, "失敗3：res = " + res3);
        }

        [TestMethod]
        public void CalcGekkanNenkyuTest()
        {
            int res1 = manager1.CalcGekkanNenkyu();
            Assert.IsTrue(res1 == 1, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9017812", 2018, 11);
            int res2 = manager3.CalcGekkanNenkyu();
            Assert.IsTrue(res2 == 1, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CaclGekkanAMHankyuTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CaclGekkanAMHankyu();
            Assert.IsTrue(res1 == 1, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9017812", 2018, 11);
            int res2 = manager3.CaclGekkanAMHankyu();
            Assert.IsTrue(res2 == 1, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CaclGekkanPMHankyuTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CaclGekkanPMHankyu();
            Assert.IsTrue(res1 == 3, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9017812", 2018, 11);
            int res2 = manager3.CaclGekkanPMHankyu();
            Assert.IsTrue(res2 == 1, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcGekkanKyukeiJikanTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcGekkanKyukeiJikan();
            Assert.IsTrue(res1 == 960, "失敗1：res = " + res1);
            int res2 = manager2.CalcGekkanKyukeiJikan();
            Assert.IsTrue(res2 == 1200, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcTounenRuikeiTyoukinJissekiJikanTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcTounenRuikeiTyoukinJissekiJikan();
            Assert.IsTrue(res1 == 1730, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9017812", 2017, 6);
            int res2 = manager3.CalcTounenRuikeiTyoukinJissekiJikan();
            Assert.IsTrue(res2 == 3550, "失敗2：res = " + res2);
        }

        [TestMethod]
        public void CalcTounenRuikeiHouteigaiJissekiJikanTest()
        {
            KinmuManager manager1 = new KinmuManager("9999999", 2018, 11);
            int res1 = manager1.CalcTounenRuikeiHouteigaiJissekiJikan();
            Assert.IsTrue(res1 == 1105, "失敗1：res = " + res1);
            KinmuManager manager3 = new KinmuManager("9017812", 2017, 6);
            int res2 = manager3.CalcTounenRuikeiHouteigaiJissekiJikan();
            Assert.IsTrue(res2 == 2465, "失敗2：res = " + res2);
        }

        //[TestMethod]
        //public void ToMinutesTest()
        //{
        //    int res1 = KinmuUtil.ToMinutes("1", "30") ?? 0;
        //    Assert.IsTrue(res1 == 90, "失敗1：res = " + res1);
        //    int res2 = KinmuUtil.ToMinutes(1, 30);
        //    Assert.IsTrue(res2 == 90, "失敗2：res = " + res2);
        //}

        //[TestMethod]
        //public void GetTimeValueInRangeTest1()
        //{
        //    //res1,res2は正常系
        //    int res1 = KinmuUtil.GetTimeValueInRange(560, 1080);
        //    Assert.IsTrue(res1 == 520, "失敗1：res = " + res1);
        //    int res2 = KinmuUtil.GetTimeValueInRange(560, 1080, 0, 1200);
        //    Assert.IsTrue(res2 == 520, "失敗2：res = " + res2);
        //}

        //[TestMethod]
        //public void GetTimeValueInRangeTest2()
        //{
        //    //res3：開始値が終了値より大きい（startMin, endMin）
        //    int res3 = KinmuUtil.GetTimeValueInRange(560, 540, 0, 1200);
        //    Assert.IsTrue(res3 == 0, "失敗3：res = " + res3);
        //}

        //[TestMethod]
        //public void GetTimeValueInRangeTest3()
        //{
        //    //res4：開始値が終了値より大きい（startRange, endRange）
        //    int res4 = KinmuUtil.GetTimeValueInRange(560, 1080, 1400, 1200);
        //    Assert.IsTrue(res4 == 0, "失敗4：res = " + res4);
        //}

        //[TestMethod]
        //public void GetTimeValueInRangeTest4()
        //{
        //    //res5：開始時刻範囲内？→ダメパターン
        //    int res5 = KinmuUtil.GetTimeValueInRange(560, 1080, 600, 1200);
        //    Assert.IsTrue(res5 == 0, "失敗5：res = " + res5);
        //}

        //[TestMethod]
        //public void GetTimeValueInRangeTest5()
        //{
        //    //res6：終了時刻範囲内？→ダメパターン
        //    int res6 = KinmuUtil.GetTimeValueInRange(560, 1300, 0, 1200);
        //    Assert.IsTrue(res6 == 0, "失敗6：res = " + res6);
        //}

        //[TestMethod]
        //public void GetTimeValueInRangeTest6()
        //{
        //    //res7：負数なら0にするよ
        //    int res7 = KinmuUtil.GetTimeValueInRange(560, 100, 150, 200);
        //    Assert.IsTrue(res7 == 0, "失敗7：res = " + res7);
        //}

        [TestMethod]
        public void SetKinmuJissekiParameterTest1()
        {
            KNS_D01 _d01 = new KNS_D01
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "1",
                NINYO_CD = "01",
                NINKA_CD = "10",
                STR_HR = "9",
                STR_MIN = "20",
                END_HR = "18",
                END_MIN = "0",
                END_PAR = "0",
                RESTS1_HR = "12",
                RESTS1_MIN = "00",
                RESTE1_HR = "13",
                RESTE1_MIN = "0",
            };
            bool res = KinmuManager.ExecuteUpdate(ref _d01);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void SetKinmuYoteiParameterTest1()
        {
            KNS_D13 _d13 = new KNS_D13
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "1",
                YOTEI_CD = "01",
                STR_Y_HR = "9",
                STR_Y_MIN = "20",
                END_Y_HR = "18",
                END_Y_MIN = "0",
                END_Y_PAR = "0",
                KAKN_FLG = "0",
                SHONIN_FLG = "0"
            };
            bool res = KinmuManager.ExecuteUpdate(ref _d13);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void SetSagyoNisshiParameterTest1()
        {
            List<KNS_D02> _d02s = new List<KNS_D02>();
            KNS_D02 _d02 = new KNS_D02
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "1",
                PROJ_CD = "V00000000A",
                SAGYO_CD = "01",
                SAGYO_MIN = 460
            };
            _d02s.Add(_d02);
            bool res = KinmuManager.ExecuteUpdate(ref _d02s);
            Assert.IsTrue(res);
        }

        // 2/2(土)460時間パターン
        [TestMethod]
        public void CalcTankaDTest1()
        {
            KNS_D01 _d01 = new KNS_D01
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "2",
                NINYO_CD = "01",
                NINKA_CD = "10",
                STR_HR = "9",
                STR_MIN = "20",
                END_HR = "18",
                END_MIN = "0",
                END_PAR = "0",
                RESTS1_HR = "12",
                RESTS1_MIN = "00",
                RESTE1_HR = "13",
                RESTE1_MIN = "0",
            };
            int res = KinmuManager.CalcTankaD(ref _d01);
            Assert.IsTrue(res == 460, "失敗：res = " + res);
        }

        // 2/3(日)460時間パターン
        [TestMethod]
        public void CalcTankaDTest2()
        {
            KNS_D01 _d01 = new KNS_D01
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "3",
                NINYO_CD = "01",
                NINKA_CD = "10",
                STR_HR = "9",
                STR_MIN = "20",
                END_HR = "18",
                END_MIN = "0",
                END_PAR = "0",
                RESTS1_HR = "12",
                RESTS1_MIN = "00",
                RESTE1_HR = "13",
                RESTE1_MIN = "0",
            };
            int res = KinmuManager.CalcTankaD(ref _d01);
            Assert.IsTrue(res == 460, "失敗：res = " + res);
        }

        // 2/1(金)→2/2(土)820時間パターン
        [TestMethod]
        public void CalcTankaDTest3()
        {
            KNS_D01 _d01 = new KNS_D01
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "1",
                NINYO_CD = "01",
                NINKA_CD = "01",
                STR_HR = "9",
                STR_MIN = "20",
                END_HR = "1",
                END_MIN = "0",
                END_PAR = "1",
                RESTS1_HR = "12",
                RESTS1_MIN = "00",
                RESTE1_HR = "13",
                RESTE1_MIN = "0",
                RESTS2_HR = "22",
                RESTS2_MIN = "30",
                RESTE2_HR = "23",
                RESTE2_MIN = "30"

            };
            int res = KinmuManager.CalcTankaD(ref _d01);
            Assert.IsTrue(res == 60, "失敗：res = " + res);
        }

        // 2/2(土)→2/3(日)820時間パターン
        [TestMethod]
        public void CalcTankaDTest4()
        {
            KNS_D01 _d01 = new KNS_D01
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "2",
                NINYO_CD = "01",
                NINKA_CD = "10",
                STR_HR = "9",
                STR_MIN = "20",
                END_HR = "1",
                END_MIN = "0",
                END_PAR = "1",
                RESTS1_HR = "12",
                RESTS1_MIN = "00",
                RESTE1_HR = "13",
                RESTE1_MIN = "0",
                RESTS2_HR = "22",
                RESTS2_MIN = "30",
                RESTE2_HR = "23",
                RESTE2_MIN = "30"
            };
            int res = KinmuManager.CalcTankaD(ref _d01);
            Assert.IsTrue(res == 820, "失敗：res = " + res);
        }

        // 2/3(日)→2/4(月)820時間パターン
        [TestMethod]
        public void CalcTankaDTest5()
        {
            KNS_D01 _d01 = new KNS_D01
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "3",
                NINYO_CD = "01",
                NINKA_CD = "10",
                STR_HR = "9",
                STR_MIN = "20",
                END_HR = "1",
                END_MIN = "0",
                END_PAR = "1",
                RESTS1_HR = "12",
                RESTS1_MIN = "00",
                RESTE1_HR = "13",
                RESTE1_MIN = "0",
                RESTS2_HR = "22",
                RESTS2_MIN = "30",
                RESTE2_HR = "23",
                RESTE2_MIN = "30"
            };
            int res = KinmuManager.CalcTankaD(ref _d01);
            Assert.IsTrue(res == 760, "失敗：res = " + res);
        }

        //当日祝日
        [TestMethod]
        public void CalcSyukujituCTest1()
        {
            KNS_D01 _d01 = new KNS_D01
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "11",
                NINYO_CD = "01",
                NINKA_CD = "10",
                STR_HR = "9",
                STR_MIN = "20",
                END_HR = "18",
                END_MIN = "0",
                END_PAR = "0",
                RESTS1_HR = "12",
                RESTS1_MIN = "00",
                RESTE1_HR = "13",
                RESTE1_MIN = "0",
            };
            int res = KinmuManager.CalcSyukujituC(ref _d01);
            Assert.IsTrue(res == 460, "失敗：res = " + res);
        }

        //当日は平日で、翌日が祝日
        [TestMethod]
        public void CalcSyukujituCTest2()
        {
            KNS_D01 _d01 = new KNS_D01
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "10",
                NINYO_CD = "01",
                NINKA_CD = "10",
                STR_HR = "9",
                STR_MIN = "20",
                END_HR = "1",
                END_MIN = "0",
                END_PAR = "1",
                RESTS1_HR = "12",
                RESTS1_MIN = "00",
                RESTE1_HR = "13",
                RESTE1_MIN = "0",
                RESTS2_HR = "22",
                RESTS2_MIN = "30",
                RESTE2_HR = "23",
                RESTE2_MIN = "30"
            };
            int res = KinmuManager.CalcSyukujituC(ref _d01);
            Assert.IsTrue(res == 60, "失敗：res = " + res);
        }

        //当日が祝日で、翌日は平日
        [TestMethod]
        public void CalcSyukujituCTest3()
        {
            KNS_D01 _d01 = new KNS_D01
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "11",
                NINYO_CD = "01",
                NINKA_CD = "10",
                STR_HR = "9",
                STR_MIN = "20",
                END_HR = "1",
                END_MIN = "0",
                END_PAR = "1",
                RESTS1_HR = "12",
                RESTS1_MIN = "00",
                RESTE1_HR = "13",
                RESTE1_MIN = "0",
                RESTS2_HR = "22",
                RESTS2_MIN = "30",
                RESTE2_HR = "23",
                RESTE2_MIN = "30"
            };
            int res = KinmuManager.CalcSyukujituC(ref _d01);
            Assert.IsTrue(res == 760, "失敗：res = " + res);
        }

        //期待値は後日計算
        [TestMethod]
        public void ValidationD01AndD02Test1()
        {
            KNS_D01 _d01 = new KNS_D01
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "1",
                NINYO_CD = "01",
                NINKA_CD = "10",
                STR_HR = "9",
                STR_MIN = "20",
                END_HR = "18",
                END_MIN = "0",
                END_PAR = "0",
                RESTS1_HR = "12",
                RESTS1_MIN = "00",
                RESTE1_HR = "13",
                RESTE1_MIN = "0",
                //RESTS2_HR = "24",
                //RESTS2_MIN = "30",
                //RESTE2_HR = "25",
                //RESTE2_MIN = "0"
            };

            List<KNS_D02> _d02s = new List<KNS_D02>();
            KNS_D02 _d02 = new KNS_D02
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "1",
                PROJ_CD = "V00303010A",
                SAGYO_CD = "01",
                SAGYO_MIN = 460
            };
            _d02s.Add(_d02);
            KinmuManager.ValidationD01AndD02(ref _d01, ref _d02s);
        }

        [TestMethod]
        public void CalcTankaCTest1()
        {
            KNS_D01 _d01 = new KNS_D01
            {
                SHAIN_CD = "9999999",
                DATA_Y = "2019",
                DATA_M = "2",
                DATA_D = "1",
                NINYO_CD = "01",
                NINKA_CD = "10",
                STR_HR = "9",
                STR_MIN = "20",
                END_HR = "1",
                END_MIN = "0",
                END_PAR = "1",
                RESTS1_HR = "12",
                RESTS1_MIN = "00",
                RESTE1_HR = "13",
                RESTE1_MIN = "0",
                RESTS2_HR = "22",
                RESTS2_MIN = "30",
                RESTE2_HR = "23",
                RESTE2_MIN = "30"
            };
            int res = KinmuManager.CalcTankaC(ref _d01);
            Assert.IsTrue(res == 120, "失敗：res = " + res);
        }
    }
}
