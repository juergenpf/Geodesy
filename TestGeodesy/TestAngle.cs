/* See License.md in the solution root for license information.
 * File: TestAngle.cs
*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Geodesy;

namespace UnitTestGeodesy
{
    [TestClass]
    public class TestAngle
    {
        [TestMethod]
        public void TestConstructor1()
        {
            Angle a = new Angle(90);
            Assert.AreEqual(a.Degrees,90);
        }

        [TestMethod]
        public void TestConstructor2()
        {
            Angle a = new Angle(45,30);
            Assert.AreEqual(a.Degrees,45.5);
        }

        [TestMethod]
        public void TestConstructor3()
        {
            Angle a = new Angle(-45, 30);
            Assert.AreEqual(a.Degrees, -45.5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructor4()
        {
            Angle a = new Angle(45, 60);
        }

        [TestMethod]
        public void TestConstructor5()
        {
            Angle a = new Angle(45, 30,30);
            Assert.AreEqual(a.Degrees, 45.5+(1.0/120.0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructor6()
        {
            Angle a = new Angle(45, 30, 60);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructor7()
        {
            Angle a = new Angle(45, 60, 30);
        }

        [TestMethod]
        public void TestConstructor8()
        {
            Angle a = new Angle(-45, 30, 30);
            Assert.AreEqual(a.Degrees,-45.5-(1.0/120.0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructor9()
        {
            Angle a = new Angle(45, -1, 30);
        }

        [TestMethod]
        public void TestDegreeSetter()
        {
            Angle a = new Angle(0);
            Assert.AreEqual(a.Degrees,0);
            a.Degrees = 90.0;
            Assert.AreEqual(a.Degrees,90);
        }

        [TestMethod]
        public void TestRadiansGetter()
        {
            Angle a = new Angle(180);
            Assert.AreEqual(a.Radians,Math.PI);
        }

        [TestMethod]
        public void TestRadiansSetter()
        {
            Angle a = new Angle(0);
            Assert.AreEqual(a.Degrees,0);
            a.Radians = Math.PI;
            Assert.AreEqual(a.Degrees,180);
        }

        [TestMethod]
        public void TestAbs()
        {
            Angle a = new Angle(-180);
            Assert.AreEqual(a.Degrees,-180);
            Angle b = a.Abs();
            Assert.AreEqual(b.Degrees,180);
        }

        [TestMethod]
        public void TestCompareTo1()
        {
            double x = 180.0;
            double y = 180.00000001;
            Angle a = new Angle(x);
            Angle b = new Angle(y);
            Assert.AreEqual(a.CompareTo(a),0);
            Assert.AreEqual(a.CompareTo(b),-1);
            Assert.AreEqual(b.CompareTo(a),1);
        }

        [TestMethod]
        public void TestCompareTo2()
        {
            double x = 180.0;
            double y = x + 1e-13;
            Angle a = new Angle(x);
            Angle b = new Angle(y);
            Assert.AreEqual(a.CompareTo(a), 0);
            Assert.AreEqual(a.CompareTo(b), 0);
            Assert.AreEqual(b.CompareTo(a), 0);
        }

        [TestMethod]
        public void TestEquals1()
        {
            double x = 180.0;
            double y = 180.00000001;
            Angle a = new Angle(x);
            Angle b = new Angle(y);
            Assert.IsFalse(a.Equals(b));
            b = new Angle(x);
            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void TestEquals2()
        {
            Angle a = new Angle(180);
            object s = "180";
            Assert.IsFalse(a.Equals(null));
            Assert.IsFalse(a.Equals(s));
        }

        [TestMethod]
        public void TestToString()
        {
            Angle a = new Angle(45);
            string s = a.ToString();
            Assert.AreEqual(s,"45");
        }

        [TestMethod]
        public void TestLessEqual1()
        {
            double x = 180.0;
            double y = 180.00000001;
            Angle a = new Angle(x);
            Angle b = new Angle(y);
            Assert.IsTrue(a <= b);
        }

        [TestMethod]
        public void TestLessEqual2()
        {
            double x = 180.0;
            double y = 180.0 + 1e-13;
            Angle a = new Angle(x);
            Angle b = new Angle(y);
            Assert.IsTrue(a <= b);
        }

        [TestMethod]
        public void TestGreaterEqual1()
        {
            double x = 180.0;
            double y = 180.00000001;
            Angle a = new Angle(x);
            Angle b = new Angle(y);
            Assert.IsTrue(b >= a);
        }

        [TestMethod]
        public void TestGreaterEqual2()
        {
            double x = 180.0;
            double y = 180.0 + 1e-13;
            Angle a = new Angle(x);
            Angle b = new Angle(y);
            Assert.IsTrue(b >= a);
        }

        [TestMethod]
        public void TestOperatorAdd()
        {
            var a = new Angle(45);
            var b = new Angle(60);
            var c = a + b;
            Assert.AreEqual(c.Degrees,105);
        }

        [TestMethod]
        public void TestOperatorSub()
        {
            var a = new Angle(45);
            var b = new Angle(60);
            var c = a - b;
            Assert.AreEqual(c.Degrees, -15);
        }

        [TestMethod]
        public void TestOperatorMul()
        {
            var a = new Angle(45);
            var b = 2 * a;
            var c = a * 3;
            Assert.AreEqual(b.Degrees, 90);
            Assert.AreEqual(c.Degrees, 135);
        }

        [TestMethod]
        public void TestGreaterThan1()
        {
            double x = 180.00000001;
            double y = 180.0;
            Angle a = new Angle(x);
            Angle b = new Angle(y);
            Assert.IsTrue(a > b);
        }

        [TestMethod]
        public void TestGreaterThan2()
        {
            double x = 180.0 + 1e-12;
            double y = 180.0;
            Angle a = new Angle(x);
            Angle b = new Angle(y);
            Assert.IsFalse(a > b);
        }

        [TestMethod]
        public void TestNotEqual1()
        {
            double x = 180.00000001;
            double y = 180.0;
            Angle a = new Angle(x);
            Angle b = new Angle(y);
            Assert.IsTrue(a != b);

        }

        [TestMethod]
        public void TestNotEqual2()
        {
            double x = 180.0 + 1e-12;
            double y = 180.0;
            Angle a = new Angle(x);
            Angle b = new Angle(y);
            Assert.IsFalse(a != b);

        }

        [TestMethod]
        public void TestNegate()
        {
            var a = new Angle(180);
            var b = -a;
            Assert.AreEqual(b.Degrees,-180);
        }

        [TestMethod]
        public void TestImplicitConversion()
        {
            Angle a = 44.0;
            Assert.AreEqual(a.Degrees,44);
        }

        [TestMethod]
        public void TestDeg2Rad()
        {
            var a = Angle.DegToRad(90);
            Assert.AreEqual(a,Math.PI/2.0);
        }

        [TestMethod]
        public void TestRadToDeg()
        {
            var a = Angle.RadToDeg(Math.PI);
            Assert.AreEqual(a,180);
        }

        [TestMethod]
        public void TestHashCode()
        {
            var a = new Angle(0);
            var b = new Angle(0.000000001);
            Assert.IsTrue(a.GetHashCode()!=b.GetHashCode());
        }
    }
}
