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
using Geodesy.Extensions;

namespace Geodesy
{
    /// <summary>
    /// Encapsulation of an Angle.  Angles are constructed and serialized in
    /// degrees for human convenience, but a conversion to radians is provided
    /// for mathematical calculations.
    /// 
    /// Angle comparisons are performed in absolute terms - no "wrapping" occurs.
    /// In other words, 360 degress != 0 degrees.
    /// </summary>
    public struct Angle : IComparable<Angle>
    {
        // precision to use in comparision operations
        private const double Precision = 0.00000000001;

        /// <summary>Degrees/Radians conversion constant.</summary>
        private const double PiOver180 = Math.PI/180.0;

        /// <summary>Angle value in degrees.</summary>
        private double _mDegrees;

        /// <summary>Zero Angle</summary>
        public static readonly Angle Zero = new Angle(0.0);

        /// <summary>180 degree Angle</summary>
        public static readonly Angle Angle180 = new Angle(180.0);

        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="deg">Degrees</param>
        /// <returns>Radians</returns>
        public static double DegToRad(double deg)
        {
            return deg*PiOver180;
        }

        /// <summary>
        /// Convert radians to degrees
        /// </summary>
        /// <param name="rad">Radians</param>
        /// <returns>Degrees</returns>
        public static double RadToDeg(double rad)
        {
            return rad/PiOver180;
        }

        /// <summary>
        /// Construct a new Angle from a degree measurement.
        /// </summary>
        /// <param name="degrees">angle measurement</param>
        public Angle(double degrees)
        {
            _mDegrees = degrees;
        }

        private static void ValidateMinutesOrSeconds(double timeval)
        {
            if (timeval < 0.0 || timeval >= 60.0)
                throw new ArgumentOutOfRangeException(Properties.Resources.MINSEC_0_TO_60);
        }

        /// <summary>
        /// Construct a new Angle from degrees and minutes.
        /// </summary>
        /// <param name="degrees">degree portion of angle measurement</param>
        /// <param name="minutes">minutes portion of angle measurement (0 &lt;= minutes &lt; 60)</param>
        public Angle(int degrees, double minutes)
        {
            ValidateMinutesOrSeconds(minutes);
            _mDegrees = minutes/60.0;
            _mDegrees = (degrees < 0) ? (degrees - _mDegrees) : (degrees + _mDegrees);
        }

        /// <summary>
        /// Construct a new Angle from degrees, minutes, and seconds.
        /// </summary>
        /// <param name="degrees">degree portion of angle measurement</param>
        /// <param name="minutes">minutes portion of angle measurement (0 &lt;= minutes &lt; 60)</param>
        /// <param name="seconds">seconds portion of angle measurement (0 &lt;= seconds &lt; 60)</param>
        public Angle(int degrees, int minutes, double seconds)
        {
            ValidateMinutesOrSeconds(minutes);
            ValidateMinutesOrSeconds(seconds);
            _mDegrees = (seconds/3600.0) + (minutes/60.0);
            _mDegrees = (degrees < 0) ? (degrees - _mDegrees) : (degrees + _mDegrees);
        }

        /// <summary>
        /// Get/set angle measured in degrees.
        /// </summary>
        public double Degrees
        {
            get { return _mDegrees; }
            set { _mDegrees = value; }
        }

        /// <summary>
        /// Get/set angle measured in radians.
        /// </summary>
        public double Radians
        {
            get { return _mDegrees*PiOver180; }
            set { _mDegrees = value/PiOver180; }
        }

        /// <summary>
        /// Get the absolute value of the angle (in degrees).
        /// </summary>
        public Angle Abs()
        {
            return new Angle(Math.Abs(_mDegrees));
        }

        /// <summary>
        /// Compare this angle to another angle.
        /// </summary>
        /// <param name="other">other angle to compare to.</param>
        /// <returns>result according to IComparable contract/></returns>
        public int CompareTo(Angle other)
        {
            if (this == other)
                return 0;
            if (this < other)
                return -1;
            return 1;
        }

        /// <summary>
        /// Calculate a hash code for the angle.
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return _mDegrees.GetHashCode();
        }

        /// <summary>
        /// Compare this Angle to another Angle for equality.  Angle comparisons
        /// are performed in absolute terms - no "wrapping" occurs.  In other
        /// words, 360 degress != 0 degrees.
        /// </summary>
        /// <param name="obj">object to compare to</param>
        /// <returns>'true' if angles are equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Angle)) return false;

            Angle other = (Angle) obj;

            return _mDegrees.IsApproximatelyEqual(other._mDegrees,Precision);
        }

        /// <summary>
        /// Get coordinates as a string. This string is always culture invariant.
        /// </summary>
        /// <returns>The angle as a culture invariant string</returns>
        public override string ToString()
        {
            return _mDegrees.ToString(NumberFormatInfo.InvariantInfo);
        }

        #region Operators

        /// <summary>
        /// Add two angles
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>The sum of the angles</returns>
        public static Angle operator +(Angle lhs, Angle rhs)
        {
            return new Angle(lhs._mDegrees + rhs._mDegrees);
        }

        /// <summary>
        /// Subtract two angles
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>The difference of the angles</returns>
        public static Angle operator -(Angle lhs, Angle rhs)
        {
            return new Angle(lhs._mDegrees - rhs._mDegrees);
        }

        /// <summary>
        /// Multiply an angle with a number
        /// </summary>
        /// <param name="lhs">The left operand number</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>The angle multiplied by the number</returns>
        public static Angle operator *(double lhs, Angle rhs)
        {
            return new Angle(lhs * rhs._mDegrees);
        }

        /// <summary>
        /// Multiply an angle with a number
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand number</param>
        /// <returns>The angle multiplied by the number</returns>
        public static Angle operator *(Angle lhs, double rhs)
        {
            return new Angle(lhs._mDegrees * rhs);
        }

        /// <summary>
        /// Test whether an angle is greater than another one
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>True if the left operand angle is greater than the right operand angle</returns>
        public static bool operator >(Angle lhs, Angle rhs)
        {
            return rhs._mDegrees.IsSmaller(lhs._mDegrees,Precision);
        }

        /// <summary>
        /// Test whether an angle is greater than or equal another one
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>True if the left operand angle is greater than or equal the right operand angle</returns>
        public static bool operator >=(Angle lhs, Angle rhs)
        {
            return rhs._mDegrees.IsApproximatelyEqual(lhs._mDegrees,Precision) ||
                   rhs._mDegrees.IsSmaller(lhs._mDegrees,Precision);
        }

        /// <summary>
        /// Test whether an angle is less than another one
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>True if the left operand angle is less than the right operand angle</returns>
        public static bool operator <(Angle lhs, Angle rhs)
        {
            return lhs._mDegrees.IsSmaller(rhs._mDegrees, Precision);
        }

        /// <summary>
        /// Test whether an angle is less than or equal another one
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>True if the left operand angle is less than or equal the right operand angle</returns>
        public static bool operator <=(Angle lhs, Angle rhs)
        {
            return lhs._mDegrees.IsApproximatelyEqual(rhs._mDegrees,Precision) ||
                lhs._mDegrees.IsSmaller(rhs._mDegrees, Precision);
        }

        /// <summary>
        /// Test whether an angle is equal to another one
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>True if the left operand angle is equal to the right operand angle</returns>
        public static bool operator ==(Angle lhs, Angle rhs)
        {
            return lhs._mDegrees.IsApproximatelyEqual(rhs._mDegrees,Precision);
        }

        /// <summary>
        /// Test whether an angle is not equal to another one
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>True if the left operand angle is not equal to the right operand angle</returns>
        public static bool operator !=(Angle lhs, Angle rhs)
        {
            return !lhs._mDegrees.IsApproximatelyEqual(rhs._mDegrees,Precision);
        }

        /// <summary>
        /// Negate the value of an angle
        /// </summary>
        /// <param name="unitary">The angle</param>
        /// <returns>The negative value of the angle</returns>
        public static Angle operator -(Angle unitary)
        {
            return new Angle(-unitary.Degrees);
        }

        /// <summary>
        /// Imlplicity cast a double as an Angle measured in degrees.
        /// </summary>
        /// <param name="degrees">angle in degrees</param>
        /// <returns>double cast as an Angle</returns>
        public static implicit operator Angle(double degrees)
        {
            return new Angle(degrees);
        }
        #endregion
    }
}
