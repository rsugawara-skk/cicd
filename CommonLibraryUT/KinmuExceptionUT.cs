using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonLibrary;

namespace CommonLibraryUT
{
    /// <summary>
    /// KinmuExceptionUT の概要の説明
    /// </summary>
    [TestClass]
    public class KinmuExceptionUT
    {
        [TestMethod]
        public void KinmuExceptionThrowTest1()
        {
            Assert.ThrowsException<KinmuException>(() => TryCatchTest1());
        }

        [TestMethod]
        public void KinmuExceptionThrowTest2()
        {
            Assert.ThrowsException<KinmuException>(() => ThrowTest1());
        }

        [TestMethod]
        public void KinmuExceptionThrowTest3()
        {
            Assert.ThrowsException<KinmuException>(() => ThrowTest2());
        }

        private void InnerKinmuExceptionTest()
        {
            int[] a = { 1 };

            int b = a[1];
        }

        private void TryCatchTest1()
        {
            try
            {
                InnerKinmuExceptionTest();
            }
            catch (Exception e)
            {
                throw new KinmuException("エラーが発生", e);
            }
        }

        private void ThrowTest1()
        {
            throw new KinmuException("エラーが発生：ThrowTest1");
        }

        private void ThrowTest2()
        {
            throw new KinmuException();
        }
    }
}
