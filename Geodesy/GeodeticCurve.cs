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
using Geodesy.Extensions;

namespace Geodesy
{
    /// <summary>
    ///     This is the outcome of a geodetic calculation.  It represents the path and
    ///     ellipsoidal distance between two GlobalCoordinates for a specified reference
    ///     ellipsoid.
    /// </summary>
    public readonly struct GeodeticCurve : IEquatable<GeodeticCurve>
    {
        /// <summary>Ellipsoidal distance (in meters).</summary>
        public double EllipsoidalDistance { get; }

        /// <summary>
        ///     The calculator used to compute this curve
        /// </summary>
        public GeodeticCalculator Calculator { get; }

        /// <summary>
        ///     Get the azimuth.  This is angle from north from start to end.
        /// </summary>
        public Angle Azimuth { get; }

        /// <summary>
        ///     Create a new GeodeticCurve.
        /// </summary>
        /// <param name="geoCalculator">The calculator used to compute this curve</param>
        /// <param name="ellipsoidalDistance">ellipsoidal distance in meters</param>
        /// <param name="azimuth">azimuth in degrees</param>
        internal GeodeticCurve(
            GeodeticCalculator geoCalculator,
            double ellipsoidalDistance,
            Angle azimuth)
        {
            Calculator = geoCalculator;
            EllipsoidalDistance = ellipsoidalDistance;
            Azimuth = azimuth;
        }

        /// <summary>
        ///     Get the reverse azimuth.  This is angle from north from end to start.
        /// </summary>
        public Angle ReverseAzimuth
        {
            get
            {
                if (double.IsNaN(Azimuth.Degrees))
                    return double.NaN;
                if (Azimuth.Degrees < 180.0)
                    return Azimuth + 180.0;
                return Azimuth - 180.0;
            }
        }

        /// <summary>
        ///     Get curve as a culture invariant string.
        /// </summary>
        /// <returns>The parameters describing the curve as culture invariant string</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append("s=");
            builder.Append(EllipsoidalDistance.ToString(NumberFormatInfo.InvariantInfo));
            builder.Append(";a12=");
            builder.Append(Azimuth);
            builder.Append(";a21=");
            builder.Append(ReverseAzimuth);
            builder.Append(";");

            return builder.ToString();
        }

        /// <summary>
        ///     Test another curve for equality
        /// </summary>
        /// <param name="other">The other geodetic curve</param>
        /// <returns>True if they are the same</returns>
        public bool Equals(GeodeticCurve other)
        {
            return EllipsoidalDistance.IsApproximatelyEqual(other.EllipsoidalDistance) && Azimuth.Equals(other.Azimuth) &&
                   Calculator.Equals(other.Calculator);
        }
    }
}