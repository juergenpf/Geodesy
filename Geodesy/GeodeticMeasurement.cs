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
    ///     This is the outcome of a three dimensional geodetic calculation.  It represents
    ///     the path a between two GlobalPositions for a specified reference ellipsoid.
    /// </summary>
    public readonly struct GeodeticMeasurement : IEquatable<GeodeticMeasurement>
    {
        /// <summary>
        ///     The calculator used to compute this measurement
        /// </summary>
        public GeodeticCalculator Calculator => AverageCurve.Calculator;

        /// <summary>
        ///     Get the average geodetic curve.  This is the geodetic curve as measured
        ///     at the average elevation between two points.
        /// </summary>
        public GeodeticCurve AverageCurve { get; }

        /// <summary>
        ///     Get the ellipsoidal distance (in meters).  This is the length of the average geodetic
        ///     curve.  For actual point-to-point distance, use PointToPointDistance property.
        /// </summary>
        public double EllipsoidalDistance => AverageCurve.EllipsoidalDistance;

        /// <summary>
        ///     Get the azimuth.  This is angle from north from start to end.
        /// </summary>
        public Angle Azimuth => AverageCurve.Azimuth;

        /// <summary>
        ///     Get the reverse azimuth.  This is angle from north from end to start.
        /// </summary>
        public Angle ReverseAzimuth => AverageCurve.ReverseAzimuth;

        /// <summary>
        ///     Get the elevation change, in meters, going from the starting to the ending point.
        /// </summary>
        public double ElevationChange { get; }

        /// <summary>
        ///     Get the distance travelled, in meters, going from one point to the next.
        /// </summary>
        public double PointToPointDistance { get; }

        /// <summary>
        ///     Creates a new instance of GeodeticMeasurement.
        /// </summary>
        /// <param name="averageCurve">the geodetic curve as measured at the average elevation between two points</param>
        /// <param name="elevationChange">the change in elevation, in meters, going from the starting point to the ending point</param>
        internal GeodeticMeasurement(
            GeodeticCurve averageCurve,
            double elevationChange)
        {
            var ellDist = averageCurve.EllipsoidalDistance;

            AverageCurve = averageCurve;
            ElevationChange = elevationChange;
            PointToPointDistance = Math.Sqrt(ellDist*ellDist + ElevationChange*ElevationChange);
        }

        /// <summary>
        ///     Get the GeodeticMeasurement as a culture invariant string
        /// </summary>
        /// <returns>The measurement as culture invariant string</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(AverageCurve);
            builder.Append(";elev12=");
            builder.Append(ElevationChange.ToString(NumberFormatInfo.InvariantInfo));
            builder.Append(";p2p=");
            builder.Append(PointToPointDistance.ToString(NumberFormatInfo.InvariantInfo));

            return builder.ToString();
        }

        /// <summary>
        ///     Test wether another measurement is the same
        /// </summary>
        /// <param name="other">Another measurement</param>
        /// <returns>True if both are the same</returns>
        public bool Equals(GeodeticMeasurement other)
        {
            return ElevationChange.IsApproximatelyEqual(other.ElevationChange) &&
                   AverageCurve.Equals(other.AverageCurve);
        }
    }
}