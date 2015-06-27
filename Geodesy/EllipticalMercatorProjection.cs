/* See License.md in the solution root for license information.
 * File: EllipticalMercatorProjection.cs
*/

using System;

namespace Geodesy
{
    /// <summary>
    ///     This is the Mercator projection assuming the earth is a perfect elipsoid.
    ///     It's more exact, but the math is slightly more complex and thus slower.
    /// </summary>
    public class EllipticalMercatorProjection : GlobalMercatorProjection
    {
        /// <summary>
        ///     Instantiate an elliptical Mercator projection with WGS84 as Ellipsoid
        /// </summary>
        public EllipticalMercatorProjection() : this(Ellipsoid.WGS84)
        {
        }

        /// <summary>
        ///     Instantiate an elliptical Mercator projection with this reference Ellipsoid
        /// </summary>
        /// <param name="referenceGlobe">The reference Ellipsoid for the projection</param>
        public EllipticalMercatorProjection(Ellipsoid referenceGlobe)
            : base(referenceGlobe)
        {
        }

        /*
         * The implementation is based on the recommendations of the OpenStreetMap project-
         * See: http://wiki.openstreetmap.org/wiki/Mercator
         * For more details see also: http://en.wikipedia.org/wiki/Mercator_projection
         * */

        /// <summary>
        ///     Convert the longitude (n degrees) to an Y-coordinate (in meters) on a Mercator map
        /// </summary>
        /// <param name="latitude">The longitude in degrees</param>
        /// <returns>The Y coordinate in meters</returns>
        public override double LatitudeToY(Angle latitude)
        {
            var phi = NormalizeLatitude(latitude).Radians;
            var sinphi = Math.Sin(phi);
            var con = ReferenceGlobe.Eccentricity*sinphi;
            con = Math.Pow(((1.0 - con)/(1.0 + con)), 0.5*ReferenceGlobe.Eccentricity);
            var ts = Math.Tan(0.5*(0.5*Math.PI - phi))/con;
            return 0.0 - ReferenceGlobe.SemiMajorAxis*Math.Log(ts);
        }

        /// <summary>
        ///     Convert the Y coordinate (in meters) on a Mercator map back into the longitude
        /// </summary>
        /// <param name="y">The Y coordinate (in meters) on a Mercator map</param>
        /// <returns>The longitude (in degrees)</returns>
        public override double YToLatitude(double y)
        {
            var ts = Math.Exp(-y/ReferenceGlobe.SemiMajorAxis);
            var phi = 0.5*Math.PI - 2.0*Math.Atan(ts);
            var dphi = 1.0;
            var i = 0;
            while ((Math.Abs(dphi) > 0.000000001) && (i < 15))
            {
                var con = ReferenceGlobe.Eccentricity*Math.Sin(phi);
                dphi = 0.5*Math.PI -
                       2.0*Math.Atan(ts*Math.Pow((1.0 - con)/(1.0 + con), 0.5*ReferenceGlobe.Eccentricity)) - phi;
                phi += dphi;
                i++;
            }
            var d = Angle.RadToDeg(phi);
            return NormalizeLatitude(d).Degrees;
        }
    }
}