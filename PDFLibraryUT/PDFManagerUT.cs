using CommonLibrary;
using CommonLibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDFLibrary;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PDFLibraryUT
{
    [TestClass]
    public class PDFManagerUT
    {

        [TestMethod]
        public void PDFManagerTest()
        {
            PDFManager pdfManager = new PDFManager("9017812", 2016, 10);
            Assert.IsTrue(pdfManager != null, "正常系エラー：インスタンスが生成されていません");
        }

        [TestMethod]
        public void ExistYoteiTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //勤務予定が入っている日
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "03");

            PrivateObject po = new PrivateObject(pdfManager);

            bool b = (bool)po.Invoke("ExistsYotei", krr.KinmuYotei);

            Assert.IsTrue(b, "正常系エラー");
        }

        [TestMethod]
        public void ExistYoteiTest2()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //勤務予定が入っていない日（特休）
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "01");

            PrivateObject po = new PrivateObject(pdfManager);

            bool b = (bool)po.Invoke("ExistsYotei", krr.KinmuYotei);

            Assert.IsFalse(b, "正常系エラー");
        }

        [TestMethod]
        public void ExistYoteiTest3()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //勤務予定が入っていない日（公休）
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "02");

            PrivateObject po = new PrivateObject(pdfManager);

            bool b = (bool)po.Invoke("ExistsYotei", krr.KinmuYotei);

            Assert.IsFalse(b, "正常系エラー");
        }

        [TestMethod]
        public void ExistYoteiTest4()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //勤務予定が入っていない日（年休）
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "11");

            PrivateObject po = new PrivateObject(pdfManager);

            bool b = (bool)po.Invoke("ExistsYotei", krr.KinmuYotei);

            Assert.IsFalse(b, "正常系エラー");
        }

        [TestMethod]
        public void ExistYoteiTest5()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 7;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //勤務予定が入っていない日（着任）
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "01");

            PrivateObject po = new PrivateObject(pdfManager);

            bool b = (bool)po.Invoke("ExistsYotei", krr.KinmuYotei);

            Assert.IsFalse(b, "正常系エラー");
        }

        [TestMethod]
        public void ExistYoteiTest6()
        {
            string employeeCode = "9017812";
            int year = 2017;
            int month = 5;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //勤務予定が入っていない日（確定欄が特休）
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "25");

            PrivateObject po = new PrivateObject(pdfManager);

            bool b = (bool)po.Invoke("ExistsYotei", krr.KinmuYotei);

            Assert.IsFalse(b, "正常系エラー");
        }

        [TestMethod]
        public void ExistJissekiTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //勤務実績が入っている日
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "03");

            PrivateObject po = new PrivateObject(pdfManager);

            bool b = (bool)po.Invoke("ExistsJisseki", krr.KinmuJisseki);

            Assert.IsTrue(b, "正常系エラー");
        }

        [TestMethod]
        public void ExistJissekiTest2()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //勤務実績が入っていない日（特休）
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "01");

            PrivateObject po = new PrivateObject(pdfManager);

            bool b = (bool)po.Invoke("ExistsJisseki", krr.KinmuJisseki);

            Assert.IsFalse(b, "正常系エラー");
        }

        [TestMethod]
        public void ExistJissekiTest3()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //勤務実績が入っていない日（公休）
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "02");

            PrivateObject po = new PrivateObject(pdfManager);

            bool b = (bool)po.Invoke("ExistsJisseki", krr.KinmuJisseki);

            Assert.IsFalse(b, "正常系エラー");
        }

        [TestMethod]
        public void ExistJissekiTest4()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //勤務実績が入っていない日（年休）
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "11");

            PrivateObject po = new PrivateObject(pdfManager);

            bool b = (bool)po.Invoke("ExistsJisseki", krr.KinmuJisseki);

            Assert.IsFalse(b, "正常系エラー");
        }

        [TestMethod]
        public void ExistMinashiTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //みなしが入っている日
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "11");

            PrivateObject po = new PrivateObject(pdfManager);

            bool b = (bool)po.Invoke("ExistsMinashi", krr.KinmuJisseki);

            Assert.IsTrue(b, "正常系エラー");
        }

        [TestMethod]
        public void ExistMinashiTest2()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //みなしが入っていない日
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "03");

            PrivateObject po = new PrivateObject(pdfManager);

            bool b = (bool)po.Invoke("ExistsMinashi", krr.KinmuJisseki);

            Assert.IsFalse(b, "正常系エラー");
        }

        [TestMethod]
        public void GetWeekDayTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //通常の曜日
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "01");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetWeekDay", krr.CalendarMaster);

            Assert.IsTrue(s == "土", "正常系エラー");

        }

        [TestMethod]
        public void GetWeekDayTest2()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //祝日
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "10");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetWeekDay", krr.CalendarMaster);

            Assert.IsTrue(s == "月　祝", "正常系エラー");

        }

        [TestMethod]
        public void GetWeekDayTest3()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //祝日前日フラグ
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "09");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetWeekDay", krr.CalendarMaster);

            Assert.IsTrue(s == "日　*", "正常系エラー");

        }

        [TestMethod]
        public void GetNinshoNameTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //出勤
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "03");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetNinshoName", krr.KinmuJisseki.NINYO_CD);

            Assert.IsTrue(s == "出勤", "正常系エラー");

        }

        [TestMethod]
        public void GetNinshoNameTest2()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //特休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "01");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetNinshoName", krr.KinmuJisseki.NINYO_CD);

            Assert.IsTrue(s == "特休", "正常系エラー");

        }

        [TestMethod]
        public void GetNinshoNameTest3()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //公休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "02");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetNinshoName", krr.KinmuJisseki.NINYO_CD);

            Assert.IsTrue(s == "公休", "正常系エラー");

        }

        [TestMethod]
        public void GetNinshoNameTest4()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //年休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "11");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetNinshoName", krr.KinmuJisseki.NINKA_CD);

            Assert.IsTrue(s == "年休", "正常系エラー");

        }

        [TestMethod]
        public void GetOpeningTimeToClosingTimeScheduleTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //平日
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "03");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetOpeningTimeToClosingTimeSchedule", krr.KinmuYotei);

            Assert.IsTrue(s == "09:20　～　18:00", "正常系エラー");

        }

        [TestMethod]
        public void GetOpeningTimeToClosingTimeScheduleTest2()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //特休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "01");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetOpeningTimeToClosingTimeSchedule", krr.KinmuYotei);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetOpeningTimeToClosingTimeScheduleTest3()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //公休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "02");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetOpeningTimeToClosingTimeSchedule", krr.KinmuYotei);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetOpeningTimeToClosingTimeScheduleTest4()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //年休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "11");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetOpeningTimeToClosingTimeSchedule", krr.KinmuYotei);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetOpeningTimeToClosingTimeResultTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //平日
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "03");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetOpeningTimeToClosingTimeResult", krr.KinmuJisseki);

            Assert.IsTrue(s == "09:20　～　18:00", "正常系エラー");

        }

        [TestMethod]
        public void GetOpeningTimeToClosingTimeResultTest2()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //特休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "01");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetOpeningTimeToClosingTimeResult", krr.KinmuJisseki);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetOpeningTimeToClosingTimeResultTest3()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //公休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "02");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetOpeningTimeToClosingTimeResult", krr.KinmuJisseki);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetOpeningTimeToClosingTimeResultTest4()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //年休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "11");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetOpeningTimeToClosingTimeResult", krr.KinmuJisseki);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetNextDayFlagTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //翌日フラグなし
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "03");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetNextDayFlag", krr.KinmuJisseki);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetNextDayFlagTest2()
        {
            string employeeCode = "9017812";
            int year = 2017;
            int month = 4;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //翌日フラグあり
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "28");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetNextDayFlag", krr.KinmuJisseki);

            Assert.IsTrue(s == "*", "正常系エラー");

        }


        [TestMethod]
        public void GetRestTimeTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //平日
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "03");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetRestTime", krr.KinmuJisseki);

            Assert.IsTrue(s == "1:00", "正常系エラー");

        }

        [TestMethod]
        public void GetRestTimeTest2()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //特休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "01");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetRestTime", krr.KinmuJisseki);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetRestTimeTest3()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //公休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "02");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetRestTime", krr.KinmuJisseki);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetRestTimeTest4()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //年休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "11");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetRestTime", krr.KinmuJisseki);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetRestTimeTest5()
        {
            string employeeCode = "9017812";
            int year = 2017;
            int month = 4;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //休憩2時間の日
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "20");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetRestTime", krr.KinmuJisseki);

            Assert.IsTrue(s == "2:00", "正常系エラー");

        }

        [TestMethod]
        public void GetRestTimeTest6()
        {
            string employeeCode = "9017812";
            int year = 2017;
            int month = 5;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //確定欄が特休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "25");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetRestTime", krr.KinmuJisseki);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetWorkTimeTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //平日
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "03");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetWorkTime", krr.KinmuJisseki);

            Assert.IsTrue(s == "7:40", "正常系エラー");

        }

        [TestMethod]
        public void GetWorkTimeTest2()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //特休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "01");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetWorkTime", krr.KinmuJisseki);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetWorkTimeTest3()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //公休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "02");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetWorkTime", krr.KinmuJisseki);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetWorkTimeTest4()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //年休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "11");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetWorkTime", krr.KinmuJisseki);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetWorkTimeTest5()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 7;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //10時間超え（2桁）
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "28");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetWorkTime", krr.KinmuJisseki);

            Assert.IsTrue(s == "10:40", "正常系エラー");

        }

        [TestMethod]
        public void GetMinashiTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //みなし勤務なし
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "03");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetMinashi", krr.KinmuJisseki);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetMinashiTest2()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //みなし勤務あり（年休）
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "11");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetMinashi", krr.KinmuJisseki);

            Assert.IsTrue(s == "7:40", "正常系エラー");

        }

        [TestMethod]
        public void GetMinashiTest3()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 7;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //みなし勤務あり（着任）
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "01");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetMinashi", krr.KinmuJisseki);

            Assert.IsTrue(s == "7:40", "正常系エラー");

        }

        [TestMethod]
        public void GetMinashiTest4()
        {
            string employeeCode = "9017812";
            int year = 2017;
            int month = 5;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //確定欄が特休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "25");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetMinashi", krr.KinmuJisseki);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetYoteiArticleTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //平日、空欄
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "03");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetYoteiArticle", krr.KinmuJisseki, krr.KinmuYotei);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetYoteiArticleTest2()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //平日、入力あり
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "07");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetYoteiArticle", krr.KinmuJisseki, krr.KinmuYotei);

            Assert.IsTrue(s == "GICT：G車準備_リファクタリング", "正常系エラー");

        }

        [TestMethod]
        public void GetYoteiArticleTest3()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //特休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "01");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetYoteiArticle", krr.KinmuJisseki, krr.KinmuYotei);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetYoteiArticleTest4()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //公休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "02");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetYoteiArticle", krr.KinmuJisseki, krr.KinmuYotei);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetYoteiArticleTest5()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //年休
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "11");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetYoteiArticle", krr.KinmuJisseki, krr.KinmuYotei);

            Assert.IsTrue(s == "", "正常系エラー");

        }

        [TestMethod]
        public void GetYoteiArticleTest6()
        {
            string employeeCode = "9017812";
            int year = 2017;
            int month = 5;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);
            DBManager dbManager = new DBManager();

            List<KinmuRecordRow> kinmuRecordRowList = dbManager.GetKinmuRecordRow(employeeCode, year, month);

            //記事が書いてある休日
            KinmuRecordRow krr = kinmuRecordRowList.Find(_ => _.CalendarMaster.DATA_D == "25");

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetYoteiArticle", krr.KinmuJisseki, krr.KinmuYotei);

            Assert.IsTrue(s == "5月3日(水)の振替", "正常系エラー");

        }

        [TestMethod]
        public void GetProjectNameTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetProjectName", "V00671010A");

            Assert.IsTrue(s == "グループ会社向け提案", "正常系エラー");

        }


        [TestMethod]
        public void GetProjectNameTest2()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetProjectName", "あああ");

            Assert.IsTrue(s == "(PJマスタに存在しません)", "正常系エラー");

        }

        [TestMethod]
        public void GetSagyoNameTest1()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetSagyoName", "06");

            Assert.IsTrue(s == "プログラム作成", "正常系エラー");

        }


        [TestMethod]
        public void GetSagyotNameTest2()
        {
            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);

            PrivateObject po = new PrivateObject(pdfManager);

            string s = (string)po.Invoke("GetSagyoName", "あああ");

            Assert.IsTrue(s == "(作業コードマスタに存在しません)", "正常系エラー");

        }

        [TestMethod]
        public void GetWorkDiaryListTest1()
        {
            //件数のみ確認する。内容については帳票を出力して目視確認する。

            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);

            List<object> list = pdfManager.GetWorkDiaryList();


            Assert.IsTrue(list.Count == 36, "正常系エラー");

        }

        [TestMethod]
        public void GetWorkDiaryListTest2()
        {
            //該当作業日誌が存在しない場合

            string employeeCode = "9017812";
            int year = 1000;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);

            List<object> list = pdfManager.GetWorkDiaryList();


            Assert.IsTrue(list.Count == 0, "正常系エラー");

        }

        [TestMethod]
        public void GetOverTimeListTest1()
        {
            //件数のみ確認する。内容については帳票を出力して目視確認する。

            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);

            List<object> list = pdfManager.GetOverTimeList();

            Assert.IsTrue(list.Count == 8, "正常系エラー");

        }

        [TestMethod]
        public void GetOverTimeListTest2()
        {
            //該当勤務予定が存在しない場合

            string employeeCode = "9017812";
            int year = 1000;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);

            List<object> list = pdfManager.GetOverTimeList();

            Assert.IsTrue(list.Count == 8, "正常系エラー");

        }

        [TestMethod]
        public void GetCheck36ListTest1()
        {
            //件数のみ確認する。内容については帳票を出力して目視確認する。

            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);

            List<object> list = pdfManager.GetCheck36List();

            Assert.IsTrue(list.Count == 4, "正常系エラー");

        }


        [TestMethod]
        public void GetCheck36ListTest2()
        {
            //該当勤務予定が存在しない場合

            string employeeCode = "9017812";
            int year = 1000;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);

            List<object> list = pdfManager.GetCheck36List();

            Assert.IsTrue(list.Count == 4, "正常系エラー");

        }

        [TestMethod]
        public void GetHoteiRoudoCheckMessageTest1()
        {
            //作業時間が法定労働時間を超過している場合

            string employeeCode = "9017812";
            int year = 2017;
            int month = 4;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);

            string message = pdfManager.GetHoteiRoudoCheckMessage();

            Assert.IsTrue(message == "※作業時間が法定労働時間を超過しました。", "正常系エラー");

        }

        [TestMethod]
        public void GetHoteiRoudoCheckMessageTest2()
        {
            //作業時間が法定労働時間を超過していない場合

            string employeeCode = "9017812";
            int year = 2016;
            int month = 10;

            PDFManager pdfManager = new PDFManager(employeeCode, year, month);

            string message = pdfManager.GetHoteiRoudoCheckMessage();

            Assert.IsTrue(message == "", "正常系エラー");

        }


    }
}
