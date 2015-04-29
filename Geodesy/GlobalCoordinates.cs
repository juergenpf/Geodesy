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
using Geodesy.Extensions;

namespace Geodesy
{
    /// <summary>
    /// Encapsulation of latitude and longitude coordinates on a globe.  Negative
    /// latitude is southern hemisphere.  Negative longitude is western hemisphere.
    /// 
    /// Any angle may be specified for longtiude and latitude, but all angles will
    /// be canonicalized such that:
    /// 
    ///      -90 &lt;= latitude &lt;= +90
    ///     -180 &lt; longitude &lt;= +180
    /// </summary>
    public struct GlobalCoordinates : IComparable<GlobalCoordinates>
    {
        /// <summary>Latitude.  Negative latitude is southern hemisphere.</summary>
        private Angle _mLatitude;

        /// <summary>Longitude.  Negative longitude is western hemisphere.</summary>
        private Angle _mLongitude;

        /// <summary>
        /// Canonicalize the current latitude and longitude values such that:
        /// 
        ///      -90 &lt;= latitude &lt;= +90
        ///     -180 &lt; longitude &lt;= +180
        /// </summary>
        private void Canonicalize()
        {
            var latitude = _mLatitude.Degrees;
            var longitude = _mLongitude.Degrees;

            latitude = (latitude + 180) % 360;
            if (latitude.IsNegative()) latitude += 360;
            latitude -= 180;

            if (latitude > 90)
            {
                latitude = 180 - latitude;
                longitude += 180;
            }
            else if (latitude < -90)
            {
                latitude = -180 - latitude;
                longitude += 180;
            }

            longitude = ((longitude + 180) % 360);
            if (longitude < 0) longitude += 360;
            longitude -= 180;

            _mLatitude = new Angle(latitude);
            _mLongitude = new Angle(longitude);
        }

        /// <summary>
        /// Construct a new GlobalCoordinates.  Angles will be canonicalized.
        /// </summary>
        /// <param name="latitude">latitude</param>
        /// <param name="longitude">longitude</param>
        public GlobalCoordinates(Angle latitude, Angle longitude)
        {
            _mLatitude = latitude;
            _mLongitude = longitude;
            Canonicalize();
        }

        /// <summary>
        /// Get/set latitude.  The latitude value will be canonicalized (which might
        /// result in a change to the longitude). Negative latitude is southern hemisphere.
        /// </summary>
        public Angle Latitude
        {
            get { return _mLatitude; }
            set
            {
                _mLatitude = value;
                Canonicalize();
            }
        }

        /// <summary>
        /// Get/set longitude.  The longitude value will be canonicalized. Negative
        /// longitude is western hemisphere.
        /// </summary>
        public Angle Longitude
        {
            get { return _mLongitude; }
            set
            {
                _mLongitude = value;
                Canonicalize();
            }
        }

        /// <summary>
        /// The coordinates of the Antipode of this point
        /// </summary>
        public GlobalCoordinates Antipode
        {
            get {
                return new GlobalCoordinates(
                    -_mLatitude.Degrees, 
                    -Math.Sign(_mLongitude.Degrees)*(180.0 - Math.Abs(_mLongitude.Degrees)));
            }
        }

        /// <summary>
        /// Compare these coordinates to another set of coordiates.  Western
        /// longitudes are less than eastern logitudes.  If longitudes are equal,
        /// then southern latitudes are less than northern latitudes.
        /// </summary>
        /// <param name="other">instance to compare to</param>
        /// <returns>-1, 0, or +1 as per IComparable contract</returns>
        public int CompareTo(GlobalCoordinates other)
        {
            int retval;

            if (_mLongitude < other._mLongitude) retval = -1;
            else if (_mLongitude > other._mLongitude) retval = +1;
            else if (_mLatitude < other._mLatitude) retval = -1;
            else if (_mLatitude > other._mLatitude) retval = +1;
            else retval = 0;

            return retval;
        }

        /// <summary>
        /// Test whether or not another coordinate is close to this coordinate
        /// within a defined precision.
        /// </summary>
        /// <param name="other">The coordinates of the ther place</param>
        /// <param name="precision">The precsion (optional, defaults to some small value)</param>
        /// <returns>True if the places are close to each other</returns>
        public bool IsApproximatelyEqual(
            GlobalCoordinates other, 
            double precision = Extensions.ExtendDouble.DefaultPrecision)
        {
            return _mLongitude.Degrees.IsApproximatelyEqual(other.Longitude.Degrees, precision)
                && _mLatitude.Degrees.IsApproximatelyEqual(other.Latitude.Degrees, precision);
        }

        /// <summary>
        /// Get a hash code for these coordinates.
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return ((int)(_mLongitude.GetHashCode() * (_mLatitude.GetHashCode() + 1021))) * 1000033;
        }

        /// <summary>
        /// Compare these coordinates to another object for equality.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if they are the same</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is GlobalCoordinates)) return false;

            var other = (GlobalCoordinates)obj;

            return (_mLongitude == other._mLongitude) && (_mLatitude == other._mLatitude);
        }

        /// <summary>
        /// Get coordinates as a culture invariant string.
        /// </summary>
        /// <returns>The coordinates as culture invariant string</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(_mLatitude.Abs().ToString());
            builder.Append((_mLatitude >= Angle.Zero) ? 'N' : 'S');
            builder.Append(';');
            builder.Append(_mLongitude.Abs().ToString());
            builder.Append((_mLongitude >= Angle.Zero) ? 'E' : 'W');
            builder.Append(';');

            return builder.ToString();
        }

        #region operators
        /// <summary>
        /// Test whether two GlobalCoordinates are the same
        /// </summary>
        /// <param name="lhs">The first coordinate</param>
        /// <param name="rhs">The second coordinate</param>
        /// <returns>True if they are the same</returns>
        public static bool operator ==(GlobalCoordinates lhs, GlobalCoordinates rhs)
        {
            return lhs.CompareTo(rhs) == 0;
        }

        /// <summary>
        /// Test whether two GlobalCoordinates are not the same
        /// </summary>
        /// <param name="lhs">The first coordinate</param>
        /// <param name="rhs">The second coordinate</param>
        /// <returns>True if they are not the same</returns>
        public static bool operator !=(GlobalCoordinates lhs, GlobalCoordinates rhs)
        {
            return lhs.CompareTo(rhs) != 0;
        }

        /// <summary>
        /// Test whether one GlobalCoordinates is greater than the other
        /// </summary>
        /// <param name="lhs">The first coordinate</param>
        /// <param name="rhs">The second coordinate</param>
        /// <returns>True if the first coordinate is greater</returns>
        public static bool operator >(GlobalCoordinates lhs, GlobalCoordinates rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        /// <summary>
        /// Test whether one GlobalCoordinates is greater or equal than the other
        /// </summary>
        /// <param name="lhs">The first coordinate</param>
        /// <param name="rhs">The second coordinate</param>
        /// <returns>True if the first coordinate is greater or equal</returns>
        public static bool operator >=(GlobalCoordinates lhs, GlobalCoordinates rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        /// <summary>
        /// Test whether one GlobalCoordinates is smaller than the other
        /// </summary>
        /// <param name="lhs">The first coordinate</param>
        /// <param name="rhs">The second coordinate</param>
        /// <returns>True if the first coordinate is smaller</returns>
        public static bool operator <(GlobalCoordinates lhs, GlobalCoordinates rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        /// <summary>
        /// Test whether one GlobalCoordinates is smaller or equal than the other
        /// </summary>
        /// <param name="lhs">The first coordinate</param>
        /// <param name="rhs">The second coordinate</param>
        /// <returns>True if the first coordinate is smaller or equal</returns>
        public static bool operator <=(GlobalCoordinates lhs, GlobalCoordinates rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }
        #endregion
    }
}
