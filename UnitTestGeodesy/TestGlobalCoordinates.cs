/* See License.md in the solution root for license information.
 * File: TestGlobalCoordinates.cs
*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Geodesy;

namespace UnitTestGeodesy
{
    [TestClass]
    public class TestGlobalCoordinates
    {
        [TestMethod]
        public void TestConstructor1()
        {
            var g = new GlobalCoordinates(45, 9);
            Assert.AreEqual(g.Latitude.Degrees, 45);
            Assert.AreEqual(g.Longitude.Degrees,9);
        }

        [TestMethod]
        public void TestConstructor2()
        {
            var g = new GlobalCoordinates(-181, 9);
            Assert.AreEqual(g.Latitude.Degrees, 1);
            Assert.AreEqual(g.Longitude.Degrees, -171);
        }

        [TestMethod]
        public void TestConstructor3()
        {
            var g = new GlobalCoordinates(-811, 0);
            Assert.AreEqual(g.Latitude.Degrees, -89);
            Assert.AreEqual(g.Longitude.Degrees, -180);
        }

        [TestMethod]
        public void TestConstructor4()
        {
            var g = new GlobalCoordinates(-0, -811);
            Assert.AreEqual(g.Latitude.Degrees, 0);
            Assert.AreEqual(g.Longitude.Degrees, -91);
        }

        [TestMethod]
        public void TestLatitudeSetter()
        {
            var a = new GlobalCoordinates(0.0, 45.0);
            a.Latitude = 45.0;
            Assert.AreEqual(a.Latitude.Degrees,45);
        }

        [TestMethod]
        public void TestLongitudeSetter()
        {
            var a = new GlobalCoordinates(0.0, 45.0);
            a.Longitude = -10.0;
            Assert.AreEqual(a.Longitude.Degrees, -10);
        }

        [TestMethod]
        public void TestCompareTo1()
        {
            var a = new GlobalCoordinates(45, 9);
            var b = new GlobalCoordinates(45, 9);
            Assert.AreEqual(a.CompareTo(b),0);
        }

        [TestMethod]
        public void TestCompareTo2()
        {
            var a = new GlobalCoordinates(45, 9);
            var b = new GlobalCoordinates(46, 9);
            Assert.AreEqual(a.CompareTo(b), -1);
        }

        [TestMethod]
        public void TestCompareTo3()
        {
            var a = new GlobalCoordinates(45, 10);
            var b = new GlobalCoordinates(45, 9);
            Assert.AreEqual(a.CompareTo(b), 1);
        }

        [TestMethod]
        public void TestCompareTo4()
        {
            var a = new GlobalCoordinates(45, 9);
            var b = new GlobalCoordinates(45, 10);
            Assert.AreEqual(a.CompareTo(b), -1);
        }

        [TestMethod]
        public void TestCompareTo5()
        {
            var a = new GlobalCoordinates(44, 9);
            var b = new GlobalCoordinates(45, 9);
            Assert.AreEqual(a.CompareTo(b), -1);
        }

        [TestMethod]
        public void TestCompareTo6()
        {
            var a = new GlobalCoordinates(45, 9);
            var b = new GlobalCoordinates(44, 9);
            Assert.AreEqual(a.CompareTo(b), 1);
        }

        [TestMethod]
        public void TestEquals()
        {
            var a = new GlobalCoordinates(45, 9);
            Assert.IsFalse(a.Equals(null));
            object s = "x";
            Assert.IsFalse(a.Equals(s));
            var b = new GlobalCoordinates(45, 9);
            Assert.IsTrue(a.Equals(b));
            b.Longitude = b.Longitude + 1;
            Assert.IsFalse(a.Equals(b));
        }

        [TestMethod]
        public void TestToString()
        {
            var a = new GlobalCoordinates(45, 9);
            Assert.AreEqual(a.ToString(),"45N;9E;");
            var b = new GlobalCoordinates(-45, -9);
            Assert.AreEqual(b.ToString(),"45S;9W;");
        }

        [TestMethod]
        public void TestGetHash()
        {
            var a = new GlobalCoordinates(45, 9);
            var b = new GlobalCoordinates(45, 9.000000001);
            Assert.AreNotEqual(a.GetHashCode(),b.GetHashCode());
        }

        [TestMethod]
        public void TestEquality()
        {
            var a = new GlobalCoordinates(45, 9);
            var b = new GlobalCoordinates(45, 9);
            Assert.IsTrue(a == b);
            b.Longitude = b.Longitude + 1e-13;
            Assert.IsTrue(a == b);
            b.Longitude = b.Longitude + 0.00001;
            Assert.IsFalse(a == b);
        }

        [TestMethod]
        public void TestInEquality()
        {
            var a = new GlobalCoordinates(45, 9);
            var b = new GlobalCoordinates(45, 9);
            Assert.IsFalse(a != b);
            b.Longitude = b.Longitude + 1e-13;
            Assert.IsFalse(a != b);
            b.Longitude = b.Longitude + 0.00001;
            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void TestGreater()
        {
            var a = new GlobalCoordinates(45, 9);
            var b = new GlobalCoordinates(45, 8);
            Assert.IsTrue(a > b);
            b.Longitude = a.Longitude + 1e-13;
            Assert.IsFalse(a > b);
            b.Longitude = a.Longitude + 0.00001;
            Assert.IsFalse(a > b);
        }

        [TestMethod]
        public void TestGreaterEqual()
        {
            var a = new GlobalCoordinates(45, 9);
            var b = new GlobalCoordinates(45, 8);
            Assert.IsTrue(a >= b);
            b.Longitude = a.Longitude + 1e-13;
            Assert.IsTrue(a >= b);
            b.Longitude = a.Longitude - 0.00001;
            Assert.IsTrue(a > b);
        }

        [TestMethod]
        public void TestLess()
        {
            var a = new GlobalCoordinates(45, 8);
            var b = new GlobalCoordinates(45, 9);
            Assert.IsTrue(a < b);
            a.Longitude = b.Longitude + 1e-13;
            Assert.IsFalse(a < b);
            a.Longitude = b.Longitude + 0.00001;
            Assert.IsFalse(a < b);
        }

        [TestMethod]
        public void TestLessEqual()
        {
            var a = new GlobalCoordinates(45, 8);
            var b = new GlobalCoordinates(45, 9);
            Assert.IsTrue(a <= b);
            a.Longitude = b.Longitude + 1e-13;
            Assert.IsTrue(a <= b);
            a.Longitude = b.Longitude + 0.00001;
            Assert.IsFalse(a <= b);
        }

        [TestMethod]
        public void TestAntipode()
        {
            var loc = new GlobalCoordinates(27.97, -82.53);
            var antiloc = new GlobalCoordinates(-27.97, 97.47);
            Assert.AreEqual(loc.Antipode, antiloc);
        }

    }
}
