/* See License.md in the solution root for license information.
 * File: TestGlobalMesh.cs
*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Geodesy;

namespace UnitTestGeodesy
{
    [TestClass]
    public class TestGlobalMesh
    {
        [TestMethod]
        public void TestAllCenters()
        {
            var theMesh = new GlobalMesh(25000);
            for (long mesh = 0; mesh < theMesh.Count; mesh++)
            {
                var center = theMesh.CenterOf(mesh);
                var verify = theMesh.MeshNumber(center);
                Assert.AreEqual(mesh, verify);
            }
        }
    }
}
