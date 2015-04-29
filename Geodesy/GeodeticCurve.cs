/* This code has been slightly modified and adapted by Jürgen Pfeifer
 * The orginal work is by:
 * 
 * Gavaghan.Geodesy by Mike Gavaghan
 * 
 * http://www.gavaghan.org/blog/free-source-code/geodesy-library-vincentys-formula/
 * 
 * This code may be freely used and modified on any personal or professional
 * project.  It comes with no warranty.
 *
 */

using System;
using System.Globalization;
using System.Text;

namespace Geodesy
{
    /// <summary>
    /// This is the outcome of a geodetic calculation.  It represents the path and
    /// ellipsoidal distance between two GlobalCoordinates for a specified reference
    /// ellipsoid.
    /// </summary>
    public struct GeodeticCurve
    {
        /// <summary>Ellipsoidal distance (in meters).</summary>
        private readonly double _mEllipsoidalDistance;

        /// <summary>Azimuth (degrees from north).</summary>
        private readonly Angle _mAzimuth;

        /// <summary>
        /// The calculator used to compute this curve.
        /// </summary>
        private readonly GeodeticCalculator _mCalculator;

        /// <summary>
        /// Create a new GeodeticCurve.
        /// </summary>
        /// <param name="geoCalculator">The calculator used to compute this curve</param>
        /// <param name="ellipsoidalDistance">ellipsoidal distance in meters</param>
        /// <param name="azimuth">azimuth in degrees</param>
        internal GeodeticCurve(
            GeodeticCalculator geoCalculator,
            double ellipsoidalDistance, 
            Angle azimuth)
        {
            _mCalculator = geoCalculator;
            _mEllipsoidalDistance = ellipsoidalDistance;
            _mAzimuth = azimuth;
        }

        /// <summary>Ellipsoidal distance (in meters).</summary>
        public double EllipsoidalDistance
        {
            get { return _mEllipsoidalDistance; }
        }

        /// <summary>
        /// Get the azimuth.  This is angle from north from start to end.
        /// </summary>
        public Angle Azimuth
        {
            get { return _mAzimuth; }
        }

        /// <summary>
        /// Get the reverse azimuth.  This is angle from north from end to start.
        /// </summary>
        public Angle ReverseAzimuth
        {
            get
            {
                if (Double.IsNaN(_mAzimuth.Degrees))
                    return double.NaN;
                else
                {
                    if (_mAzimuth.Degrees < 180.0)
                        return _mAzimuth + 180.0;
                    else
                        return _mAzimuth - 180.0;
                }
            }
        }

        /// <summary>
        /// The calculator used to compute this curve
        /// </summary>
        public GeodeticCalculator Calculator
        {
            get { return _mCalculator; }
        }

        /// <summary>
        /// Get curve as a culture invariant string.
        /// </summary>
        /// <returns>The parameters describing the curve as culture invariant string</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("s=");
            builder.Append(_mEllipsoidalDistance.ToString(NumberFormatInfo.InvariantInfo));
            builder.Append(";a12=");
            builder.Append(_mAzimuth.ToString());
            builder.Append(";a21=");
            builder.Append(ReverseAzimuth.ToString());
            builder.Append(";");

            return builder.ToString();
        }
    }
}
