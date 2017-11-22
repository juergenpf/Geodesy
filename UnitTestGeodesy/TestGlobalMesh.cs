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

        [TestMethod]
        public void TestBoundingBox()
        {
            var theMesh = new GlobalMesh(1000);
            var nr = theMesh.MeshNumber(Constants.MyHome);
            var ll = theMesh.LowerLeft(nr);
            var lr = theMesh.LowerRight(nr);
            var ul = theMesh.UpperLeft(nr);
            var ur = theMesh.UpperRight(nr);
            Assert.AreEqual(ll.X,ul.X);
            Assert.AreEqual(lr.X,ur.X);
            Assert.AreEqual(ll.Y,lr.Y);
            Assert.AreEqual(ul.Y,ur.Y);
        }

        [TestMethod]
        public void TestNeighborHood()
        {
            var theMesh = new GlobalMesh(1000);
            var nr = theMesh.MeshNumber(Constants.MyHome);
            var n0 = theMesh.Neighborhood(nr,0);
            Assert.AreEqual(n0.Count,1);
            Assert.AreEqual(n0[0],nr);
            var n1 = theMesh.Neighborhood(nr, 1);
            Assert.AreEqual(n1.Count,8);
            var n2 = theMesh.Neighborhood(nr, 2);
            Assert.AreEqual(n2.Count,16);
            var n3 = theMesh.Neighborhood(nr, 3);
            Assert.AreEqual(n3.Count,24);
        }

        [TestMethod]
        public void TestMeshSizeInMetersValidation()
        {
            try
            {
                new GlobalMesh(0);
            }
            catch (ArgumentOutOfRangeException)
            { }

            try
            {
                new GlobalMesh(1);
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.Fail();
            }

            try
            {
                new GlobalMesh(2);
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.Fail();
            }
        }
    }
}
