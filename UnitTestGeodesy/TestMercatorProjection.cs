/* See License.md in the solution root for license information.
 * File: TestMercatorProjection.cs
*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Geodesy;

namespace UnitTestGeodesy
{
    [TestClass]
    public class TestMercatorProjection
    {
        [TestMethod]
        public void TestLoxodromeToOffice()
        {
            var proj = new EllipticalMercatorProjection();
            double mercatorRhumDistance;
            Angle bearing;
            var path = proj.CalculatePath(
                Constants.MyHome, 
                Constants.MyOffice,
                out mercatorRhumDistance,
                out bearing,3);
            // The reference values are computed with 
            // http://onboardintelligence.com/RL_Lat1Long1Lat2Long2.aspx
            // (Set the high precision options in the dialog!)
            Assert.AreEqual(Math.Round(mercatorRhumDistance),43233);
            Assert.AreEqual(Math.Round(bearing.Degrees*10)/10,342.2);
        }
    }
}
