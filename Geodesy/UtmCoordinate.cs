/* See License.md in the solution root for license information.
 * File: UtmCoordinates.cs
*/
using System;
using System.Globalization;

namespace Geodesy
{
    /// <summary>
    /// UTM Coordinates need additionally the zone and the band info
    /// to identify the grid to which the X/Y values belong
    /// </summary>
    public class UtmCoordinate : EuclidianCoordinate
    {
        private const double DefaultPrecision = 0.01;
        private bool _computed;
        private double _scaleFactor;
        private double _meridianConvergence; // stored in radians

        /// <summary>
        /// The UTM Grid this point belongs to.
        /// </summary>
        public UtmGrid Grid { get; private set; }

        public override bool IsSameProjection(EuclidianCoordinate other)
        {
            var utmOther = other as UtmCoordinate;
            if (null != utmOther)
            {
                return utmOther.Grid.Equals(Grid);
            }
            return false;
        }

        private void Compute()
        {
            var c = Grid.Projection.FromEuclidian(this, out _scaleFactor, out _meridianConvergence);
            _computed = true;
        }

        /// <summary>
        /// The scale factor at this location
        /// </summary>
        public double ScaleFactor
        {
            get
            {
                if (!_computed)
                    Compute();
                return _scaleFactor;
            }
            private set
            {
                _scaleFactor = value;
            }
        }

        /// <summary>
        /// The meridian convergence at this location
        /// </summary>
        public Angle MeridianConvergence
        {
            get
            {
                if (!_computed)
                    Compute();
                return Angle.RadToDeg(_meridianConvergence);
            }
            private set { _meridianConvergence = value.Radians; }
        }

        /// <summary>
        /// Instantiate a new point on an UTM map
        /// </summary>
        /// <param name="grid">The UTM grid</param>
        /// <param name="easting">The X value</param>
        /// <param name="northing">The Y value</param>
        public UtmCoordinate(
            UtmGrid grid, 
            double easting, 
            double northing) : base(grid.Projection, easting, northing)
        {
            Grid = grid;
            _computed = false;
        }

        internal UtmCoordinate(UtmGrid grid,
            double easting,
            double northing,
            double scaleFactor,
            double meridianConvergence) : this(grid, easting, northing)
        {
            _scaleFactor = scaleFactor;
            _meridianConvergence = meridianConvergence;
            _computed = true;
        }

        public override double DistanceTo(EuclidianCoordinate other)
        {
            var obj = other as UtmCoordinate;
            if (obj==null || !Grid.Equals(obj.Grid))
                throw new ArgumentException();
            return base.DistanceTo(other);
        }

        public override bool Equals(object obj)
        {
            var other = obj as UtmCoordinate;

            return (other != null && other.Grid.Equals(Grid) && 
                IsApproximatelyEqual(other,DefaultPrecision));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// The culture invariant string representation of the UTM coordinate
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var x = (long)Math.Floor(X);
            var y = (long)Math.Floor(Y);
            return Grid.ToString() + " " 
                + x.ToString(NumberFormatInfo.InvariantInfo) + " " 
                + y.ToString(NumberFormatInfo.InvariantInfo);
        }
    }
}
