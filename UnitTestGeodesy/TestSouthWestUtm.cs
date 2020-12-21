/* See License.md in the solution root for license information.
 * File: TestUtmGrid.cs
*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Geodesy;

namespace UnitTestGeodesy
{
    [TestClass]
    public class TestSouthWestUtm
    {
        UtmProjection utm = new UtmProjection();

        private void validateCorners(UtmGrid g)
        {
            Assert.AreEqual(g.ToString(), new UtmGrid(utm, g.LowerRightCorner).ToString());
            Assert.AreEqual(g.ToString(), new UtmGrid(utm, g.UpperLeftCorner).ToString());
            Assert.AreEqual(g.ToString(), new UtmGrid(utm, g.UpperRightCorner).ToString());
        }

        [TestMethod]
        public void TestCorners()
        {
            var lon = -54.45576937839619;
            var lat = -34.6821341916981;
            var point = new GlobalCoordinates(lat,lon);

            var meshsize = 500;
            var mesh = new GlobalMesh(meshsize);
            
            var meshnumber = mesh.MeshNumber(point);
            var utmOrig = mesh.Projection.ToEuclidian(point);
            var utmCenter = mesh.CenterOf(meshnumber);
            var coord2 = utmCenter.Projection.FromEuclidian(utmCenter);
            var dist = mesh.Projection.EuclidianDistance(utmOrig, utmCenter);
            var maxdist = Math.Sqrt(2.0) * meshsize / 2.0;
            Assert.IsTrue(dist <= maxdist);
        }
    }

}
