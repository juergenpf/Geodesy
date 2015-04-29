/* See License.md in the solution root for license information.
 * File: TestUtmGrid.cs
*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Geodesy;

namespace UnitTestGeodesy
{
    [TestClass]
    public class TestUtmGrid
    {
        UtmProjection utm = new UtmProjection();

        private void validateCorners(UtmGrid g)
        {
            Assert.AreEqual(g.ToString(),new UtmGrid(utm,g.LowerRightCorner).ToString());
            Assert.AreEqual(g.ToString(), new UtmGrid(utm, g.UpperLeftCorner).ToString());
            Assert.AreEqual(g.ToString(), new UtmGrid(utm, g.UpperRightCorner).ToString());
        }

        [TestMethod]
        public void TestConstructor1()
        {
            var g = new UtmGrid(utm, 1, 'C');
            Assert.AreEqual(g.LowerLeftCorner.Longitude,-180);
            Assert.AreEqual(g.LowerLeftCorner.Latitude,utm.MinLatitude);
            Assert.AreEqual(g.Width,6.0);
            validateCorners(g);
        }

        [TestMethod]
        public void TestConstructor2()
        {
            var g = new UtmGrid(utm, 1, 'X');
            Assert.AreEqual(g.LowerLeftCorner.Longitude, -180);
            Assert.AreEqual(g.LowerLeftCorner.Latitude, utm.MaxLatitude - g.Height);
            Assert.AreEqual(g.Height,12.0);
            Assert.AreEqual(g.Width,6.0);
            validateCorners(g);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructor3()
        {
            var loc = new GlobalCoordinates(utm.MaxLatitude + 1.0, 0);
            var g = new UtmGrid(utm, loc);
        }

        [TestMethod]
        public void TestConstructor4()
        {
            var loc = new GlobalCoordinates(utm.MaxLatitude, 0);
            var g = new UtmGrid(utm, loc);
            Assert.IsTrue(true);
            validateCorners(g);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructor5()
        {
            var g = new UtmGrid(utm, 0, 'C');
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructor6()
        {
            var g = new UtmGrid(utm, 1, 'A');
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor7()
        {
            var g = new UtmGrid(null, 1, 'C');
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructor8()
        {
            var g = new UtmGrid(utm, UtmGrid.NumberOfGrids + 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructor_32X()
        {
            var g = new UtmGrid(utm, 32, 'X');
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructor_34X()
        {
            var g = new UtmGrid(utm, 34, 'X');
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructor_36X()
        {
            var g = new UtmGrid(utm, 36, 'X');
        }

        [TestMethod]
        public void TestConstructor_32V()
        {
            var g = new UtmGrid(utm, 32, 'V');
            Assert.AreEqual(g.Width,9.0);
            validateCorners(g);
        }

        [TestMethod]
        public void TestConstructor_31V()
        {
            var g = new UtmGrid(utm, 31, 'V');
            Assert.AreEqual(g.Width, 3.0);
            var l = g.LowerLeftCorner;
            // 4 degrees east of lower left is normally in the same grid
            // but not so in 31V
            l.Longitude += 4.0;
            var g2 = new UtmGrid(utm, l);
            Assert.AreEqual(g2.ToString(),"32V");
            validateCorners(g);
            validateCorners(g2);
        }

        [TestMethod]
        public void TestConstructor_31X()
        {
            var g = new UtmGrid(utm, 31, 'X');
            Assert.AreEqual(g.Width, 9.0);
            var l = g.LowerLeftCorner;
            // Going a little more than width should bring us 
            // into the next zone 32, but not in this band
            l.Longitude += g.Width + 1.0;
            var g2 = new UtmGrid(utm, l);
            Assert.AreEqual(g2.ToString(),"33X");
            validateCorners(g);
            validateCorners(g2);
        }

        [TestMethod]
        public void TestConstructor_37X()
        {
            var g = new UtmGrid(utm, 37, 'X');
            Assert.AreEqual(g.Width, 9.0);
            validateCorners(g);
        }

        [TestMethod]
        public void TestConstructor_33X()
        {
            var g = new UtmGrid(utm, 33, 'X');
            Assert.AreEqual(g.Width, 12.0);
            var l = g.LowerRightCorner;
            l.Longitude += 1.0;
            var g2 = new UtmGrid(utm, l);
            Assert.AreEqual(g2.ToString(),"35X");
            validateCorners(g);
            validateCorners(g2);
        }

        [TestMethod]
        public void TestConstructor_35X()
        {
            var g = new UtmGrid(utm, 35, 'X');
            Assert.AreEqual(g.Width, 12.0);
            var l = g.LowerRightCorner;
            l.Longitude += 1.0;
            var g2 = new UtmGrid(utm, l);
            Assert.AreEqual(g2.ToString(), "37X");
            validateCorners(g);
            validateCorners(g2);
        }

        [TestMethod]
        public void TestCorners()
        {
            var g = new UtmGrid(utm, 32, 'U');
            var glrc = new UtmGrid(utm, g.LowerRightCorner);
            Assert.AreEqual(glrc.ToString(),"32U");
            var gulc = new UtmGrid(utm, g.UpperLeftCorner);
            Assert.AreEqual(gulc.ToString(), "32U");
            var gurc = new UtmGrid(utm, g.UpperRightCorner);
            Assert.AreEqual(gurc.ToString(), "32U");
            validateCorners(g);
        }

        [TestMethod]
        public void TestOrdinal()
        {
            var g = new UtmGrid(utm, 32, 'U');
            int ord = g.Ordinal;
            var g2 = new UtmGrid(utm, ord);
            Assert.AreEqual(g,g2);
            validateCorners(g);
            validateCorners(g2);
        }

        [TestMethod]
        public void TestOrdinal2()
        {
            for (int ord = 0; ord < 1200; ord++)
            {
                if (UtmGrid.IsValidOrdinal(ord))
                {
                    var g = new UtmGrid(utm, ord);
                    Assert.AreEqual(g.Ordinal, ord);
                }
            }
        }

        [TestMethod]
        public void TestBandSetter()
        {
            var g0 = new UtmGrid(utm, 32, 'U');
            var g = new UtmGrid(utm, 32, 'U');
            g.Band = 'V';
            Assert.AreEqual(g.ToString(),"32V");
            Assert.AreEqual(g.LowerLeftCorner.Latitude - g0.LowerLeftCorner.Latitude,g0.Height);
            validateCorners(g);
        }

        [TestMethod]
        public void TestZoneSetter()
        {
            var g0 = new UtmGrid(utm, 32, 'U');
            var g = new UtmGrid(utm, 32, 'U');
            g.Zone = 33;
            Assert.AreEqual(g.ToString(), "33U");
            Assert.AreEqual(g.LowerLeftCorner.Longitude - g0.LowerLeftCorner.Longitude, g0.Width);
            validateCorners(g);
        }

        [TestMethod]
        public void TestOriginCompute()
        {
            var g = new UtmGrid(utm, 32, 'U');
            var o = g.Origin;
            Assert.AreEqual(o.Projection,utm);
            Assert.AreEqual(o.Grid.ToString(),"32U");
            Assert.IsTrue(Math.Abs(o.ScaleFactor - 1.0) < 0.001);
            validateCorners(g);
        }

        [TestMethod]
        public void ValidateAllGridsCorners()
        {
            double maxHeight = double.MinValue;
            double maxWidth = double.MinValue;
            double minWidth = double.MaxValue;
            double minHeight = double.MaxValue;

            UtmGrid G = new UtmGrid(utm,1,'C');
            for (int zone = 1; zone <= UtmGrid.NumberOfZones; zone++)
            {
                for (int band = 0; band < UtmGrid.NumberOfBands; band++)
                {
                    bool valid = true;
                    try
                    {
                        G = new UtmGrid(utm, zone, band);
                    }
                    catch (Exception)
                    { valid = false; }
                    if (valid)
                    {
                        validateCorners(G);
                        minWidth = Math.Min(minWidth, G.MapWidth);
                        maxWidth = Math.Max(maxWidth, G.MapWidth);
                        minHeight = Math.Min(minHeight, G.MapHeight);
                        maxHeight = Math.Max(maxHeight, G.MapHeight);
                    }
                }
            }
            Assert.IsTrue(maxHeight >= minHeight && minHeight > 0.0);
            Assert.IsTrue(maxWidth >= minWidth && minWidth > 0.0);
        }

        [TestMethod]
        public void TestEquals1()
        {
            string s = "123";
            var g = new UtmGrid(utm, 1, 'C');
            Assert.IsFalse(g.Equals(s));
        }

        [TestMethod]
        public void TestEquals2()
        {
            var g1 = new UtmGrid(utm, 1, 'C');
            var g2 = new UtmGrid(utm, 1, 'D');
            g2.Band = 'C';
            Assert.IsTrue(g1.Equals(g2));
        }

        [TestMethod]
        public void TestEquality()
        {
            var g1 = new UtmGrid(utm, 1, 'C');
            var g2 = new UtmGrid(utm, 1, 'D');
            g2.Band = 'C';
            Assert.IsTrue(g1==g2);
        }

        [TestMethod]
        public void TestInEquality()
        {
            var g1 = new UtmGrid(utm, 1, 'C');
            var g2 = new UtmGrid(utm, 1, 'D');
            Assert.IsTrue(g1 != g2);
        }
    }
}
