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
using Geodesy.Extensions;

namespace Geodesy
{
    /// <summary>
    ///     Encapsulation of an ellipsoid, and declaration of common reference ellipsoids.
    /// </summary>
    public readonly struct Ellipsoid : IEquatable<Ellipsoid>
    {
        /// <summary>Get semi major axis (meters).</summary>
        public double SemiMajorAxis { get; }

        /// <summary>Get flattening.</summary>
        public double Flattening { get; }

        /// <summary>
        ///     Construct a new Ellipsoid.  This is private to ensure the values are
        ///     consistent (flattening = 1.0 / inverseFlattening).  Use the methods
        ///     FromAAndInverseF() and FromAAndF() to create new instances.
        /// </summary>
        /// <param name="semiMajor"></param>
        /// <param name="flattening"></param>
        private Ellipsoid(double semiMajor, double flattening)
        {
            SemiMajorAxis = semiMajor;
            Flattening = flattening;
        }

        /// <summary>Get semi minor axis (meters).</summary>
        public double SemiMinorAxis => (1.0 - Flattening)*SemiMajorAxis;
        
        /// <summary>Get inverse flattening.</summary>
        public double InverseFlattening => 1.0/Flattening;

        /// <summary>
        ///     Get axis ratio
        /// </summary>
        public double Ratio => 1.0 - Flattening;

        /// <summary>
        ///     The eccentricity of the Ellipsoid
        /// </summary>
        public double Eccentricity => Math.Sqrt(1.0 - Math.Pow(Ratio, 2));

        /// <summary>
        ///     Build an Ellipsoid from the semi major axis measurement and the inverse flattening.
        /// </summary>
        /// <param name="semiMajor">Semi major axis (meters)</param>
        /// <param name="inverseFlattening">The inverse flattening</param>
        /// <returns>The Ellipsoid</returns>
        public static Ellipsoid FromAAndInverseF(double semiMajor, double inverseFlattening)
        {
            return new Ellipsoid(semiMajor, 1.0/inverseFlattening);
        }

        /// <summary>
        ///     Build an Ellipsoid from the semi major axis measurement and the flattening.
        /// </summary>
        /// <param name="semiMajor">Semi major axis (meters)</param>
        /// <param name="flattening">The flattening</param>
        /// <returns>The Ellipsoid</returns>
        public static Ellipsoid FromAAndF(double semiMajor, double flattening)
        {
            return new Ellipsoid(semiMajor, flattening);
        }

        /// <summary>
        ///     Test whether or not another ellipsoid is equal to the current one.
        /// </summary>
        /// <param name="other">The other ellipsoid</param>
        /// <returns>True, if the other object is an Ellipsoid with the same geometry</returns>
        public bool Equals(Ellipsoid other)
        {
            return this == other;
        }

        /// <summary>
        ///     Test whether or not another object is also an Ellipsoid, and
        ///     if yes, whether it's equal to the current one.
        /// </summary>
        /// <param name="obj">The other object</param>
        /// <returns>True, if the other object is an Ellipsoid with the same geometry</returns>
        public override bool Equals(object obj)
        {
            return obj is Ellipsoid ellipsoid && ((IEquatable<Ellipsoid>) this).Equals(ellipsoid);
        }

        /// <summary>
        ///     The hash code of the Ellipsoid
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            double[] xy = {SemiMajorAxis, SemiMajorAxis};
            return xy.GetHashCode();
        }


        /// <summary>
        ///     Test whether or not two Ellipsoids are the same
        /// </summary>
        /// <param name="lhs">The first Ellipsoid</param>
        /// <param name="rhs">The second Ellipsoid</param>
        /// <returns>True if both are equal (have the same geometry)</returns>
        public static bool operator ==(Ellipsoid lhs, Ellipsoid rhs)
        {
            return lhs.SemiMajorAxis.IsApproximatelyEqual(rhs.SemiMajorAxis) &&
                   lhs.Flattening.IsApproximatelyEqual(rhs.Flattening);
        }

        /// <summary>
        ///     Test whether or not two Ellipsoids are not the same
        /// </summary>
        /// <param name="lhs">The first Ellipsoid</param>
        /// <param name="rhs">The second Ellipsoid</param>
        /// <returns>True if both are not equal (have a different geometry)</returns>
        public static bool operator !=(Ellipsoid lhs, Ellipsoid rhs)
        {
            return !(lhs == rhs);
        }

        #region References Ellipsoids

        /// <summary>The WGS84 ellipsoid.</summary>
        public static readonly Ellipsoid WGS84 = FromAAndInverseF(6378137.0, 298.257223563);

        /// <summary>The GRS80 ellipsoid.</summary>
        public static readonly Ellipsoid GRS80 = FromAAndInverseF(6378137.0, 298.257222101);

        /// <summary>The GRS67 ellipsoid.</summary>
        public static readonly Ellipsoid GRS67 = FromAAndInverseF(6378160.0, 298.25);

        /// <summary>The ANS ellipsoid.</summary>
        public static readonly Ellipsoid ANS = FromAAndInverseF(6378160.0, 298.25);

        /// <summary>The WGS72 ellipsoid.</summary>
        public static readonly Ellipsoid WGS72 = FromAAndInverseF(6378135.0, 298.26);

        /// <summary>The Clarke1858 ellipsoid.</summary>
        public static readonly Ellipsoid Clarke1858 = FromAAndInverseF(6378293.645, 294.26);

        /// <summary>The Clarke1880 ellipsoid.</summary>
        public static readonly Ellipsoid Clarke1880 = FromAAndInverseF(6378249.145, 293.465);

        /// <summary>A spherical "ellipsoid".</summary>
        public static readonly Ellipsoid Sphere = FromAAndF(6371000, 0.0);

        #endregion
    }
}