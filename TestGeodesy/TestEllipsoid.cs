/* See License.md in the solution root for license information.
 * File: TestEllipsoid.cs
*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Geodesy;
using Geodesy.Extensions;

namespace UnitTestGeodesy
{
    [TestClass]
    public class TestEllipsoid
    {
        [TestMethod]
        public void TestFactory1()
        {
            Ellipsoid e = Ellipsoid.FromAAndF(100000, 0.01);
            Assert.AreEqual(e.SemiMinorAxis,(1-0.01)*100000);
            Assert.AreEqual(e.InverseFlattening,100);
            Assert.AreEqual(e.SemiMajorAxis,100000);
            Assert.AreEqual(e.Ratio,1.0-0.01);
        }

        [TestMethod]
        public void TestFactory2()
        {
            Ellipsoid e = Ellipsoid.FromAAndInverseF(100000, 100);
            Assert.AreEqual(e.SemiMinorAxis, (1 - 0.01) * 100000);
            Assert.AreEqual(e.Flattening, 0.01);
            Assert.AreEqual(e.SemiMajorAxis, 100000);
            Assert.AreEqual(e.Ratio, 1.0 - 0.01);
        }

        [TestMethod]
        public void TestEquality()
        {
            Ellipsoid e1 = Ellipsoid.FromAAndInverseF(100000, 100);
            Ellipsoid e2 = Ellipsoid.FromAAndInverseF(100000, 100);
            Ellipsoid e3 = Ellipsoid.FromAAndInverseF(100000, 101);
            Ellipsoid e4 = Ellipsoid.FromAAndInverseF(100000, 100 + 1e-13);
            Ellipsoid e5 = Ellipsoid.FromAAndInverseF(99000, 100);
            Assert.IsTrue(e1 == e2);
            Assert.IsFalse(e1 == e3);
            Assert.IsTrue(e1 == e4);
            Assert.IsFalse(e1 == e5);
        }

        [TestMethod]
        public void TestInEquality()
        {
            Ellipsoid e1 = Ellipsoid.FromAAndInverseF(100000, 100);
            Ellipsoid e2 = Ellipsoid.FromAAndInverseF(100000, 100);
            Ellipsoid e3 = Ellipsoid.FromAAndInverseF(100000, 101);
            Ellipsoid e4 = Ellipsoid.FromAAndInverseF(100000, 100 + 1e-13);
            Assert.IsFalse(e1 != e2);
            Assert.IsTrue(e1 != e3);
            Assert.IsFalse(e1 != e4);
        }

        [TestMethod]
        public void TestEquals()
        {
            Ellipsoid e1 = Ellipsoid.FromAAndInverseF(100000, 100);
            Ellipsoid e2 = Ellipsoid.FromAAndInverseF(100000, 100);
            Ellipsoid e3 = Ellipsoid.FromAAndInverseF(100000, 101);
            Ellipsoid e4 = Ellipsoid.FromAAndInverseF(100000, 100 + 1e-13);
            object s = "123";
            Assert.IsTrue(e1.Equals(e2));
            Assert.IsFalse(e1.Equals(e3));
            Assert.IsTrue(e1.Equals(e4));
            Assert.IsFalse(e1.Equals(null));
            Assert.IsFalse(e1.Equals(s));
        }

        [TestMethod]
        public void TestHashCode()
        {
            Ellipsoid e1 = Ellipsoid.FromAAndInverseF(100000, 100);
            Ellipsoid e2 = Ellipsoid.FromAAndInverseF(100000, 101);
            Assert.AreNotEqual(e1.GetHashCode(),e2.GetHashCode());
        }

        [TestMethod]
        public void TestEccentricity()
        {
            var e = Ellipsoid.WGS84;
            Assert.IsTrue(e.Eccentricity.IsApproximatelyEqual(0.081819190842621));
        }
    }
}
