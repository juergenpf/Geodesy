/* See License.md in the solution root for license information.
 * File: ExtendDouble.cs
*/

using System;

namespace Geodesy.Extensions
{
    /// <summary>
    ///     Extend the double class with methods to do numerically more proper
    ///     comparisons of two double numbers.
    /// </summary>
    public static class ExtendDouble
    {
        /// <summary>
        ///     The default precision to use when comparing two doubles
        /// </summary>
        public const double DefaultPrecision = 0.000000000001;

        /// <summary>
        ///     Test whether or not a double is equal to another double in the limits of a given precision
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The second number</param>
        /// <param name="delta">The precision to use</param>
        /// <returns>True if they are approx. equal, false otherwise</returns>
        public static bool IsApproximatelyEqual(this double a, double b,
            double delta = DefaultPrecision)
        {
            if (double.IsNaN(a)) return double.IsNaN(b);
            if (double.IsInfinity(a)) return double.IsInfinity(b);
            if (a.Equals(b)) return true;
            var scale = 1.0;
            if (!(a.Equals(0.0) || b.Equals(0.0)))
                scale = Math.Max(Math.Abs(a), Math.Abs(b));
            return Math.Abs(a - b) <= scale*delta;
        }

        /// <summary>
        ///     Test wether or not a double is smaller than another one,
        ///     as long as they are not approximately equal.
        /// </summary>
        /// <param name="value1">The first number</param>
        /// <param name="value2">The second number</param>
        /// <param name="delta">The precision to use</param>
        /// <returns>True if they are not approx. equal and the first number is less than the second.</returns>
        public static bool IsSmaller(this double value1, double value2, double delta = DefaultPrecision)
        {
            if (value1.IsApproximatelyEqual(value2, delta))
                return false;
            return value1 < value2;
        }

        /// <summary>
        ///     Test wether a double is zero
        /// </summary>
        /// <param name="val">The value to test</param>
        /// <returns>True, if the number is zero</returns>
        public static bool IsZero(this double val)
        {
            return (Math.Sign(val) == 0);
        }

        /// <summary>
        ///     Test wether a double is negative
        /// </summary>
        /// <param name="val">The value to test</param>
        /// <returns>True, if the number is negative</returns>
        public static bool IsNegative(this double val)
        {
            return (Math.Sign(val) == -1);
        }

        /// <summary>
        ///     Test wether a double is positive
        /// </summary>
        /// <param name="val">The value to test</param>
        /// <returns>True, if the number is positive</returns>
        public static bool IsPositive(this double val)
        {
            return (Math.Sign(val) == 1);
        }
    }
}