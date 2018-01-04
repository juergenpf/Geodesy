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
    ///     Encapsulation of an Angle.  Angles are constructed and serialized in
    ///     degrees for human convenience, but a conversion to radians is provided
    ///     for mathematical calculations.
    ///     Angle comparisons are performed in absolute terms - no "wrapping" occurs.
    ///     In other words, 360 degress != 0 degrees.
    /// </summary>
    public struct Angle : IComparable<Angle>, IEquatable<Angle>
    {
        // precision to use in comparision operations
        private const double Precision = 0.00000000001;

        /// <summary>Degrees/Radians conversion constant.</summary>
        private const double PiOver180 = Math.PI/180.0;

        /// <summary>Zero Angle</summary>
        public static readonly Angle Zero = new Angle(0.0);

        /// <summary>180 degree Angle</summary>
        public static readonly Angle Angle180 = new Angle(180.0);

        /// <summary>
        ///     Get/set angle measured in degrees.
        /// </summary>
        public double Degrees { get; set; }


        /// <summary>
        ///     Construct a new Angle from a degree measurement.
        /// </summary>
        /// <param name="degrees">angle measurement</param>
        public Angle(double degrees)
        {
            Degrees = degrees;
        }

        /// <summary>
        ///     Construct a new Angle from degrees and minutes.
        /// </summary>
        /// <param name="degrees">degree portion of angle measurement</param>
        /// <param name="minutes">minutes portion of angle measurement (0 &lt;= minutes &lt; 60)</param>
        /// <exception cref="ArgumentOutOfRangeException">Raised if the minutes are not in range</exception>
        public Angle(int degrees, double minutes)
        {
            ValidateMinutesOrSeconds(minutes);
            Degrees = minutes/60.0;
            Degrees = (degrees < 0) ? (degrees - Degrees) : (degrees + Degrees);
        }

        /// <summary>
        ///     Construct a new Angle from degrees, minutes, and seconds.
        /// </summary>
        /// <param name="degrees">degree portion of angle measurement</param>
        /// <param name="minutes">minutes portion of angle measurement (0 &lt;= minutes &lt; 60)</param>
        /// <param name="seconds">seconds portion of angle measurement (0 &lt;= seconds &lt; 60)</param>
        /// <exception cref="ArgumentOutOfRangeException">Raised if the minutes or seconds are not in range</exception>
        public Angle(int degrees, int minutes, double seconds)
        {
            ValidateMinutesOrSeconds(minutes);
            ValidateMinutesOrSeconds(seconds);
            Degrees = (seconds/3600.0) + (minutes/60.0);
            Degrees = (degrees < 0) ? (degrees - Degrees) : (degrees + Degrees);
        }

        /// <summary>
        ///     Get/set angle measured in radians.
        /// </summary>
        public double Radians
        {
            get { return Degrees*PiOver180; }
            set { Degrees = value/PiOver180; }
        }

        /// <summary>
        ///     Compare this angle to another angle.
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
        ///     Convert degrees to radians
        /// </summary>
        /// <param name="deg">Degrees</param>
        /// <returns>Radians</returns>
        public static double DegToRad(double deg)
        {
            return deg*PiOver180;
        }

        /// <summary>
        ///     Convert radians to degrees
        /// </summary>
        /// <param name="rad">Radians</param>
        /// <returns>Degrees</returns>
        public static double RadToDeg(double rad)
        {
            return rad/PiOver180;
        }

        private static void ValidateMinutesOrSeconds(double timeval)
        {
            if (timeval < 0.0 || timeval >= 60.0)
                throw new ArgumentOutOfRangeException(Properties.Resource.MINSEC_0_TO_60);
        }

        /// <summary>
        ///     Get the absolute value of the angle (in degrees).
        /// </summary>
        public Angle Abs()
        {
            return new Angle(Math.Abs(Degrees));
        }

        /// <summary>
        ///     Calculate a hash code for the angle.
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return Degrees.GetHashCode();
        }

        /// <summary>
        ///     Compare this Angle to another Angle for equality.  Angle comparisons
        ///     are performed in absolute terms - no "wrapping" occurs.  In other
        ///     words, 360 degress != 0 degrees.
        /// </summary>
        /// <param name="other">other Angle to compare to</param>
        /// <returns>'true' if angles are equal</returns>
        public bool Equals(Angle other)
        {
            return Degrees.IsApproximatelyEqual(other.Degrees, Precision);
        }

        /// <summary>
        ///     Compare this Angle to another Angle for equality.  Angle comparisons
        ///     are performed in absolute terms - no "wrapping" occurs.  In other
        ///     words, 360 degress != 0 degrees.
        /// </summary>
        /// <param name="obj">object to compare to</param>
        /// <returns>'true' if angles are equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Angle)) return false;
            return ((IEquatable<Angle>) this).Equals((Angle)obj);
        }

        /// <summary>
        ///     Get coordinates as a string. This string is always culture invariant.
        /// </summary>
        /// <returns>The angle as a culture invariant string</returns>
        public override string ToString()
        {
            return Degrees.ToString(NumberFormatInfo.InvariantInfo);
        }


        #region Operators

        /// <summary>
        ///     Add two angles
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>The sum of the angles</returns>
        public static Angle operator +(Angle lhs, Angle rhs)
        {
            return new Angle(lhs.Degrees + rhs.Degrees);
        }

        /// <summary>
        ///     Subtract two angles
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>The difference of the angles</returns>
        public static Angle operator -(Angle lhs, Angle rhs)
        {
            return new Angle(lhs.Degrees - rhs.Degrees);
        }

        /// <summary>
        ///     Multiply an angle with a number
        /// </summary>
        /// <param name="lhs">The left operand number</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>The angle multiplied by the number</returns>
        public static Angle operator *(double lhs, Angle rhs)
        {
            return new Angle(lhs*rhs.Degrees);
        }

        /// <summary>
        ///     Multiply an angle with a number
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand number</param>
        /// <returns>The angle multiplied by the number</returns>
        public static Angle operator *(Angle lhs, double rhs)
        {
            return new Angle(lhs.Degrees*rhs);
        }

        /// <summary>
        ///     Test whether an angle is greater than another one
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>True if the left operand angle is greater than the right operand angle</returns>
        public static bool operator >(Angle lhs, Angle rhs)
        {
            return rhs.Degrees.IsSmaller(lhs.Degrees, Precision);
        }

        /// <summary>
        ///     Test whether an angle is greater than or equal another one
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>True if the left operand angle is greater than or equal the right operand angle</returns>
        public static bool operator >=(Angle lhs, Angle rhs)
        {
            return rhs.Degrees.IsApproximatelyEqual(lhs.Degrees, Precision) ||
                   rhs.Degrees.IsSmaller(lhs.Degrees, Precision);
        }

        /// <summary>
        ///     Test whether an angle is less than another one
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>True if the left operand angle is less than the right operand angle</returns>
        public static bool operator <(Angle lhs, Angle rhs)
        {
            return lhs.Degrees.IsSmaller(rhs.Degrees, Precision);
        }

        /// <summary>
        ///     Test whether an angle is less than or equal another one
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>True if the left operand angle is less than or equal the right operand angle</returns>
        public static bool operator <=(Angle lhs, Angle rhs)
        {
            return lhs.Degrees.IsApproximatelyEqual(rhs.Degrees, Precision) ||
                   lhs.Degrees.IsSmaller(rhs.Degrees, Precision);
        }

        /// <summary>
        ///     Test whether an angle is equal to another one
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>True if the left operand angle is equal to the right operand angle</returns>
        public static bool operator ==(Angle lhs, Angle rhs)
        {
            return lhs.Degrees.IsApproximatelyEqual(rhs.Degrees, Precision);
        }

        /// <summary>
        ///     Test whether an angle is not equal to another one
        /// </summary>
        /// <param name="lhs">The left operand angle</param>
        /// <param name="rhs">The right operand angle</param>
        /// <returns>True if the left operand angle is not equal to the right operand angle</returns>
        public static bool operator !=(Angle lhs, Angle rhs)
        {
            return !lhs.Degrees.IsApproximatelyEqual(rhs.Degrees, Precision);
        }

        /// <summary>
        ///     Negate the value of an angle
        /// </summary>
        /// <param name="unitary">The angle</param>
        /// <returns>The negative value of the angle</returns>
        public static Angle operator -(Angle unitary)
        {
            return new Angle(-unitary.Degrees);
        }

        /// <summary>
        ///     Imlplicity cast a double as an Angle measured in degrees.
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