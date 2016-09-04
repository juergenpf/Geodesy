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
    ///     This is the outcome of a three dimensional geodetic calculation.  It represents
    ///     the path a between two GlobalPositions for a specified reference ellipsoid.
    /// </summary>
    public struct GeodeticMeasurement
    {
        /// <summary>The average geodetic curve.</summary>
        private readonly GeodeticCurve _mCurve;

        /// <summary>The elevation change, in meters, going from the starting to the ending point.</summary>
        private readonly double _mElevationChange;

        /// <summary>The distance travelled, in meters, going from one point to the next.</summary>
        private readonly double _mP2P;

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

            _mCurve = averageCurve;
            _mElevationChange = elevationChange;
            _mP2P = Math.Sqrt(ellDist*ellDist + _mElevationChange*_mElevationChange);
        }

        /// <summary>
        ///     The calculator used to compute this measurement
        /// </summary>
        public GeodeticCalculator Calculator
        {
            get { return _mCurve.Calculator; }
        }

        /// <summary>
        ///     Get the average geodetic curve.  This is the geodetic curve as measured
        ///     at the average elevation between two points.
        /// </summary>
        public GeodeticCurve AverageCurve
        {
            get { return _mCurve; }
        }

        /// <summary>
        ///     Get the ellipsoidal distance (in meters).  This is the length of the average geodetic
        ///     curve.  For actual point-to-point distance, use PointToPointDistance property.
        /// </summary>
        public double EllipsoidalDistance
        {
            get { return _mCurve.EllipsoidalDistance; }
        }

        /// <summary>
        ///     Get the azimuth.  This is angle from north from start to end.
        /// </summary>
        public Angle Azimuth
        {
            get { return _mCurve.Azimuth; }
        }

        /// <summary>
        ///     Get the reverse azimuth.  This is angle from north from end to start.
        /// </summary>
        public Angle ReverseAzimuth
        {
            get { return _mCurve.ReverseAzimuth; }
        }

        /// <summary>
        ///     Get the elevation change, in meters, going from the starting to the ending point.
        /// </summary>
        public double ElevationChange
        {
            get { return _mElevationChange; }
        }

        /// <summary>
        ///     Get the distance travelled, in meters, going from one point to the next.
        /// </summary>
        public double PointToPointDistance
        {
            get { return _mP2P; }
        }

        /// <summary>
        ///     Get the GeodeticMeasurement as a culture invariant string
        /// </summary>
        /// <returns>The measurement as culture invariant string</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(_mCurve);
            builder.Append(";elev12=");
            builder.Append(_mElevationChange.ToString(NumberFormatInfo.InvariantInfo));
            builder.Append(";p2p=");
            builder.Append(_mP2P.ToString(NumberFormatInfo.InvariantInfo));

            return builder.ToString();
        }
    }
}