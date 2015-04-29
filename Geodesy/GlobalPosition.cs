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
using System.Text;
using System.Globalization;
using Geodesy.Extensions;

namespace Geodesy
{
    /// <summary>
    /// Encapsulates a three dimensional location on a globe (GlobalCoordinates combined with
    /// an elevation in meters above a reference ellipsoid).
    /// </summary>
    public struct GlobalPosition : IComparable<GlobalPosition>
    {
        private const double Precision = 0.000000000001;

        /// <summary>Global coordinates.</summary>
        private GlobalCoordinates _mCoordinates;

        /// <summary>Elevation, in meters, above the surface of the ellipsoid.</summary>
        private double _mElevation;

        /// <summary>
        /// Creates a new instance of GlobalPosition.
        /// </summary>
        /// <param name="coords">coordinates on the reference ellipsoid.</param>
        /// <param name="elevation">elevation, in meters, above the reference ellipsoid.</param>
        public GlobalPosition(GlobalCoordinates coords, double elevation)
        {
            _mCoordinates = coords;
            _mElevation = elevation;
        }

        /// <summary>
        /// Creates a new instance of GlobalPosition for a position on the surface of
        /// the reference ellipsoid.
        /// </summary>
        /// <param name="coords"></param>
        public GlobalPosition(GlobalCoordinates coords)
            : this(coords, 0.0)
        { }

        /// <summary>Get/set global coordinates.</summary>
        public GlobalCoordinates Coordinates
        {
            get { return _mCoordinates; }
            set { _mCoordinates = value; }
        }

        /// <summary>Get/set latitude.</summary>
        public Angle Latitude
        {
            get { return _mCoordinates.Latitude; }
            set { _mCoordinates.Latitude = value; }
        }

        /// <summary>Get/set longitude.</summary>
        public Angle Longitude
        {
            get { return _mCoordinates.Longitude; }
            set { _mCoordinates.Longitude = value; }
        }

        /// <summary>
        /// Get/set elevation, in meters, above the surface of the reference ellipsoid.
        /// </summary>
        public double Elevation
        {
            get { return _mElevation; }
            set { _mElevation = value; }
        }

        /// <summary>
        /// Compare this position to another.  Western longitudes are less than
        /// eastern logitudes.  If longitudes are equal, then southern latitudes are
        /// less than northern latitudes.  If coordinates are equal, lower elevations
        /// are less than higher elevations
        /// </summary>
        /// <param name="other">instance to compare to</param>
        /// <returns>-1, 0, or +1 as per IComparable contract</returns>
        public int CompareTo(GlobalPosition other)
        {
            var retval = _mCoordinates.CompareTo(other._mCoordinates);

            if (retval != 0)
                return retval;

            if (_mElevation.IsApproximatelyEqual(other._mElevation, Precision)) retval = 0;
            else if (_mElevation.IsSmaller(other._mElevation, Precision)) retval = -1;
            else retval = 1;

            return retval;
        }

        /// <summary>
        /// Calculate a hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hash = _mCoordinates.GetHashCode();

            if (_mElevation != 0) hash *= (int)_mElevation;

            return hash;
        }

        /// <summary>
        /// Compare this position to another object for equality.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is GlobalPosition)) return false;

            var other = (GlobalPosition)obj;

            return (_mElevation.IsApproximatelyEqual(other._mElevation,Precision)) 
                && (_mCoordinates.Equals(other._mCoordinates));
        }


        /// <summary>
        /// Get position as a culture invariant string.
        /// </summary>
        /// <returns>The position as culture invariant string</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(_mCoordinates.ToString());
            builder.Append(_mElevation.ToString(NumberFormatInfo.InvariantInfo));
            builder.Append("m");

            return builder.ToString();
        }

        #region Operators
        /// <summary>
        /// Test two GlobalPositions for equality
        /// </summary>
        /// <param name="lhs">The first position</param>
        /// <param name="rhs">The second position</param>
        /// <returns>True if they are the same</returns>
        public static bool operator ==(GlobalPosition lhs, GlobalPosition rhs)
        {
            return (lhs.CompareTo(rhs) == 0);
        }

        /// <summary>
        /// Test two GlobalPositions for equality
        /// </summary>
        /// <param name="lhs">The first position</param>
        /// <param name="rhs">The second position</param>
        /// <returns>True if they are the same</returns>
        public static bool operator !=(GlobalPosition lhs, GlobalPosition rhs)
        {
            return (lhs.CompareTo(rhs) != 0);
        }

        /// <summary>
        /// Test whether a GlobalPosition is smaller than another one
        /// </summary>
        /// <param name="lhs">The first position</param>
        /// <param name="rhs">The second position</param>
        /// <returns>True if the first position is smaller than the second</returns>
        public static bool operator <(GlobalPosition lhs, GlobalPosition rhs)
        {
            return (lhs.CompareTo(rhs) < 0);
        }

        /// <summary>
        /// Test whether a GlobalPosition is smaller or equal than another one
        /// </summary>
        /// <param name="lhs">The first position</param>
        /// <param name="rhs">The second position</param>
        /// <returns>True if the first position is smaller than or equal to the second</returns>
        public static bool operator <=(GlobalPosition lhs, GlobalPosition rhs)
        {
            return (lhs.CompareTo(rhs) <= 0);
        }

        /// <summary>
        /// Test whether a GlobalPosition is greater than another one
        /// </summary>
        /// <param name="lhs">The first position</param>
        /// <param name="rhs">The second position</param>
        /// <returns>True if the first position is greater than the second</returns>
        public static bool operator >(GlobalPosition lhs, GlobalPosition rhs)
        {
            return (lhs.CompareTo(rhs) > 0);
        }

        /// <summary>
        /// Test whether a GlobalPosition is greater or equal than another one
        /// </summary>
        /// <param name="lhs">The first position</param>
        /// <param name="rhs">The second position</param>
        /// <returns>True if the first position is greater than or equal to the second</returns>
        public static bool operator >=(GlobalPosition lhs, GlobalPosition rhs)
        {
            return (lhs.CompareTo(rhs) >= 0);
        }
        #endregion
    }
}
