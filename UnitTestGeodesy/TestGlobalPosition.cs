/* See License.md in the solution root for license information.
 * File: TestGlobalPosition.cs
*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Geodesy;

namespace UnitTestGeodesy
{
    [TestClass]
    public class TestGlobalPosition
    {
        GlobalCoordinates c1 = new GlobalCoordinates(45, 9);
        GlobalCoordinates c2 = new GlobalCoordinates(45,10);
        GlobalCoordinates c3 = new GlobalCoordinates(46, 9);
        GlobalCoordinates c4 = new GlobalCoordinates(44, 9);

        [TestMethod]
        public void TestConstructor1()
        {
            var a = new GlobalPosition();
            Assert.AreEqual(a.Latitude,0);
            Assert.AreEqual(a.Longitude,0);
            Assert.AreEqual(a.Elevation,0);
        }

        [TestMethod]
        public void TestConstructor2()
        {
            var a = new GlobalPosition(c1, 100);
            Assert.AreEqual(a.Coordinates,c1);
            Assert.AreEqual(a.Elevation,100);
        }

        [TestMethod]
        public void TestConstructor3()
        {
            var a = new GlobalPosition(c1);
            Assert.AreEqual(a.Coordinates, c1);
            Assert.AreEqual(a.Elevation, 0);
        }

        [TestMethod]
        public void TestCoordSetter()
        {
            var a = new GlobalPosition(c1);
            Assert.AreEqual(a.Coordinates, c1);
            Assert.AreEqual(a.Elevation, 0);
            a.Coordinates = c2;
            Assert.AreEqual(a.Coordinates,c2);
        }

        [TestMethod]
        public void TestLatSetter()
        {
            var a = new GlobalPosition(c1);
            Assert.AreEqual(a.Coordinates, c1);
            Assert.AreEqual(a.Elevation, 0);
            a.Latitude = 46;
            Assert.AreEqual(a.Coordinates, c3);
        }

        [TestMethod]
        public void TestLongSetter()
        {
            var a = new GlobalPosition(c1);
            Assert.AreEqual(a.Coordinates, c1);
            Assert.AreEqual(a.Elevation, 0);
            a.Longitude = 10;
            Assert.AreEqual(a.Coordinates, c2);
        }

        [TestMethod]
        public void TestElevSetter()
        {
            var a = new GlobalPosition(c1);
            Assert.AreEqual(a.Coordinates, c1);
            Assert.AreEqual(a.Elevation, 0);
            a.Elevation = -100;
            Assert.AreEqual(a.Elevation, -100);
        }

        [TestMethod]
        public void TestCompareTo1()
        {
            var a = new GlobalPosition(c1);
            var b = new GlobalPosition(c1);
            Assert.AreEqual(a.CompareTo(b),0);
            b.Elevation = b.Elevation + 1e-13;
            Assert.AreEqual(a.CompareTo(b),0);
            b.Elevation = 100;
            Assert.AreEqual(a.CompareTo(b),-1);
            b.Elevation = -100;
            Assert.AreEqual(a.CompareTo(b),1);
        }

        [TestMethod]
        public void TestEquals()
        {
            var a = new GlobalPosition(c1);
            Assert.IsFalse(a.Equals(null));
            object s = "x";
            Assert.IsFalse(a.Equals(s));
            var b = new GlobalPosition(c1);
            Assert.IsTrue(a.Equals(b));
            b.Elevation = b.Elevation + 1;
            Assert.IsFalse(a.Equals(b));
        }

        [TestMethod]
        public void TestToString()
        {
            var a = new GlobalPosition(c1, 200);
            Assert.AreEqual(a.ToString(),"45N;9E;200m");
        }

        [TestMethod]
        public void TestGetHash()
        {
            var a = new GlobalPosition(c1);
            var b = new GlobalPosition(c2);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [TestMethod]
        public void TestGetHash2()
        {
            var a = new GlobalPosition(c1,100);
            var b = new GlobalPosition(c2,-100);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [TestMethod]
        public void TestEquality()
        {
            var a = new GlobalPosition(c1);
            var b = new GlobalPosition(c1);
            Assert.IsTrue(a == b);
            b.Elevation = b.Elevation + 1e-13;
            Assert.IsTrue(a == b);
            b.Elevation = b.Elevation + 0.00001;
            Assert.IsFalse(a == b);
        }

        [TestMethod]
        public void TestInEquality()
        {
            var a = new GlobalPosition(c1);
            var b = new GlobalPosition(c1);
            Assert.IsFalse(a != b);
            b.Elevation = b.Elevation + 1e-13;
            Assert.IsFalse(a != b);
            b.Elevation = b.Elevation + 0.00001;
            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void TestGreater()
        {
            var a = new GlobalPosition(c1);
            var b = new GlobalPosition(c4);
            Assert.IsTrue(a > b);
            b.Latitude = a.Latitude + 1e-13;
            Assert.IsFalse(a > b);
            a.Elevation = 100;
            Assert.IsTrue(a > b);
            b.Latitude = a.Latitude + 0.00001;
            Assert.IsFalse(a > b);
        }

        [TestMethod]
        public void TestGreaterEqual()
        {
            var a = new GlobalPosition(c1);
            var b = new GlobalPosition(c4);
            Assert.IsTrue(a >= b);
            b.Latitude = a.Latitude + 1e-13;
            Assert.IsTrue(a >= b);
            a.Elevation = 100;
            Assert.IsTrue(a >= b);
            b.Latitude = a.Latitude - 0.00001;
            Assert.IsTrue(a >= b);
        }

        [TestMethod]
        public void TestLess()
        {
            var a = new GlobalPosition(c1);
            var b = new GlobalPosition(c4);
            Assert.IsTrue(b < a);
            b.Latitude = a.Latitude + 1e-13;
            Assert.IsFalse(b < a);
            a.Elevation = 100;
            Assert.IsTrue(b < a);
            b.Latitude = a.Latitude + 0.00001;
            Assert.IsFalse(b < a);
        }

        [TestMethod]
        public void TestLessEqual()
        {
            var a = new GlobalPosition(c1);
            var b = new GlobalPosition(c4);
            Assert.IsTrue(b <= a);
            b.Latitude = a.Latitude + 1e-13;
            Assert.IsTrue(b <= a);
            a.Elevation = 100;
            Assert.IsTrue(b <= a);
            b.Latitude = a.Latitude - 0.00001;
            Assert.IsTrue(b <= a);
        }
    }
}
