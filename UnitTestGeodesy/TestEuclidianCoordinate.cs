/* See License.md in the solution root for license information.
 * File: TestEuclidianCoordinate.cs
*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Geodesy;

namespace UnitTestGeodesy
{
    [TestClass]
    public class TestEuclidianCoordinate
    {
        private GlobalMercatorProjection projection = new SphericalMercatorProjection();

        [TestMethod]
        public void TestConstructor1()
        {
            var e = new EuclidianCoordinate(projection, -1, -2);
            Assert.AreEqual(e.X,-1);
            Assert.AreEqual(e.Y,-2);
        }

        [TestMethod]
        public void TestConstructor2()
        {
            var e = new EuclidianCoordinate(projection, new double[] {-3, -4});
            Assert.AreEqual(e.X,-3);
            Assert.AreEqual(e.Y,-4);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestConstructor3()
        {
            var e = new EuclidianCoordinate(projection, new double[] { -3, -4, -5 });
        }

        [TestMethod]
        public void TestEquals1()
        {
            var e1 = new EuclidianCoordinate(projection, -3, -4);
            var e2 = new EuclidianCoordinate(projection, -3, -4);
            Assert.AreEqual(e1,e2);
        }

        [TestMethod]
        public void TestEquals2()
        {
            var e1 = new EuclidianCoordinate(projection, -3, -4);
            var e2 = "123";
            Assert.AreNotEqual(e1, e2);
        }

        [TestMethod]
        public void TestEquals3()
        {
            var e1 = new EuclidianCoordinate(projection, -3, -4);
            var e2 = new EuclidianCoordinate(new EllipticalMercatorProjection(), -3, -4);
            Assert.AreNotEqual(e1, e2);
        }

        [TestMethod]
        public void TestEquals4()
        {
            var e1 = new EuclidianCoordinate(projection, -3, -4);
            var e2 = new EuclidianCoordinate(projection, -3+1e-13, -4);
            Assert.AreEqual(e1, e2);
        }

        [TestMethod]
        public void TestEquals5()
        {
            var e1 = new EuclidianCoordinate(projection, -3, -4);
            var e2 = new EuclidianCoordinate(projection, -3, -4 + 1e-13);
            Assert.AreEqual(e1, e2);
        }

        [TestMethod]
        public void TestHash()
        {
            var e1 = new EuclidianCoordinate(projection, -3, -4);
            var e2 = new EuclidianCoordinate(projection, -3, -4 + 1e-13);
            Assert.AreNotEqual(e1.GetHashCode(), e2.GetHashCode());
        }
    }
}
