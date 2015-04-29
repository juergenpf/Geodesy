/* See License.md in the solution root for license information.
 * File: TestUtmProjection.cs
*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Geodesy;

namespace UnitTestGeodesy
{
    [TestClass]
    public class TestUtmProjection
    {
        private UtmProjection utm = new UtmProjection();

        [TestMethod]
        public void TestMyHome()
        {
            var e = (UtmCoordinate)utm.ToEuclidian(Constants.MyHome);
            // Reference Computation from http://www.earthpoint.us/Convert.aspx
            Assert.AreEqual(e.ToString(), "32U 485577 5521521");
        }

        [TestMethod]
        public void TestInversion()
        {
            var e = (UtmCoordinate)utm.ToEuclidian(Constants.MyHome);
            var c = utm.FromEuclidian(e); 
            Assert.IsTrue(c.IsApproximatelyEqual(Constants.MyHome, 0.000000001));
        }

        [TestMethod]
        public void TestEquals1()
        {
            string s = "123";
            Assert.IsFalse(utm.Equals(s));
        }

        [TestMethod]
        public void TestEquals2()
        {
            UtmProjection utm2 = new UtmProjection();
            
        }
    }
}
