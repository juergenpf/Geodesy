/* See License.md in the solution root for license information.
 * File: SphericalMercatorProjection.cs
*/
using System;

namespace Geodesy
{
    /// <summary>
    /// This is the Mercator projection assuming the earth is a perfect sphere.
    /// It's less exact, but the math is slightly easier and thus faster.
    /// </summary>
    public class SphericalMercatorProjection : GlobalMercatorProjection
    {
        /// <summary>
        /// Instantiate a spherical Mercator projection
        /// </summary>
        public SphericalMercatorProjection() : base(Ellipsoid.Sphere) { }

        /// <summary>
        /// Convert the Y coordinate (in meters) on a Mercator map back into the longitude
        /// </summary>
        /// <param name="y">The Y coordinate (in meters) on a Mercator map</param>
        /// <returns>The longitude (in degrees)</returns>
        public override double YToLatitude(double y)
        {
            var latitude = Angle.RadToDeg(2.0 * Math.Atan(Math.Exp(y / ReferenceGlobe.SemiMajorAxis)) - 0.5 * Math.PI);
            return NormalizeLatitude(latitude).Degrees;
        }

        /// <summary>
        /// Convert the longitude (n degrees) to an Y-coordinate (in meters) on a Mercator map
        /// </summary>
        /// <param name="latitude">The longitude in degrees</param>
        /// <returns>The Y coordinate in meters</returns>
        public override double LatitudeToY(Angle latitude)
        {
            return ReferenceGlobe.SemiMajorAxis * Math.Log(Math.Tan(Math.PI / 4.0 + latitude.Radians / 2.0));
        }
    }
}
