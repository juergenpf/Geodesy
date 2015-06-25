/* See License.md in the solution root for license information.
 * File: TestGeodeticCalculator.cs
*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Geodesy;

namespace UnitTestGeodesy
{
    [TestClass]
    public class TestGeodeticCalculator
    {
        private GeodeticCalculator calc = new GeodeticCalculator(Ellipsoid.WGS84);


        [TestMethod]
        public void TestCurve()
        {
            var curve = calc.CalculateGeodeticCurve(Constants.MyHome, Constants.MyOffice);
            // Reference values computed with 
            // http://williams.best.vwh.net/gccalc.htm
            Assert.AreEqual(Math.Round(1000 * curve.EllipsoidalDistance) / 1000,
                43232.317);
            Assert.AreEqual(Math.Round(1000000 * curve.Azimuth.Degrees) / 1000000,
                342.302315);
            Assert.AreEqual(curve.Calculator,calc);
        }

        [TestMethod]
        public void TestCurve2()
        {
            GlobalCoordinates target = new GlobalCoordinates(
                Constants.MyHome.Latitude, Constants.MyHome.Longitude - 1.0);
            var curve = calc.CalculateGeodeticCurve(Constants.MyHome, target);
            // Reference values computed with 
            // http://williams.best.vwh.net/gccalc.htm
            Assert.AreEqual(Math.Round(100000*curve.Azimuth.Degrees)/100000, 270.382160);
            Assert.AreEqual(curve.Calculator, calc);
        }

        [TestMethod]
        public void TestMeasurement()
        {
            var start = new GlobalPosition(Constants.MyHome, 200);
            var end = new GlobalPosition(Constants.MyOffice, 240);
            var m = calc.CalculateGeodeticMeasurement(start, end);
            var c = calc.CalculateGeodeticCurve(Constants.MyHome, Constants.MyOffice);
            Assert.IsTrue(m.EllipsoidalDistance > c.EllipsoidalDistance);
        }

        [TestMethod]
        public void TestNearAntipodicCurve()
        {
            var loc = new GlobalCoordinates(0, 10); // on the equator
            var aloc = loc.Antipode;
            aloc.Latitude = aloc.Latitude * 0.99999998;
            var curve = calc.CalculateGeodeticCurve(loc, aloc);
            Assert.IsTrue(double.IsNaN(curve.Azimuth.Degrees));
            Assert.AreEqual(curve.Calculator, calc);
        }

        [TestMethod]
        public void TestEnding()
        {
            var curve = calc.CalculateGeodeticCurve(Constants.MyHome, Constants.MyOffice);
            var final = calc.CalculateEndingGlobalCoordinates(
                Constants.MyHome,
                curve.Azimuth,
                curve.EllipsoidalDistance);
            Assert.AreEqual(final, Constants.MyOffice);
            Assert.AreEqual(curve.Calculator, calc);
        }

        [TestMethod]
        public void TestPath1()
        {
            var path = calc.CalculateGeodeticPath(Constants.MyHome, Constants.MyOffice, 2);
            Assert.AreEqual(path[0], Constants.MyHome);
            Assert.AreEqual(path[1], Constants.MyOffice);
            Assert.AreEqual(calc.ReferenceGlobe, Ellipsoid.WGS84);
        }

        [TestMethod]
        public void TestPath2()
        {
            var path = calc.CalculateGeodeticPath(Constants.MyHome, Constants.MyOffice);
            Assert.AreEqual(path[0], Constants.MyHome);
            Assert.AreEqual(path[path.Length-1], Constants.MyOffice);
            Assert.AreEqual(calc.ReferenceGlobe, Ellipsoid.WGS84);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestPath3()
        {
            var path = calc.CalculateGeodeticPath(Constants.MyHome, Constants.MyOffice,1);
        }

        [TestMethod]
        public void TestPath4()
        {
            var path = calc.CalculateGeodeticPath(Constants.MyHome, Constants.MyHome, 10);
            Assert.AreEqual(path.Length,2);
            Assert.AreEqual(path[0], Constants.MyHome);
            Assert.AreEqual(path[1], Constants.MyHome);
        }
    }
}
