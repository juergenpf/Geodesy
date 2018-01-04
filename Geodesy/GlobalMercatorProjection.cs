/* See License.md in the solution root for license information.
 * File: GlobalMercatorProjection.cs
*/

using System;

namespace Geodesy
{
    /// <summary>
    ///     This is the base class of all Mercator projections that map the
    ///     globe as a whole.
    /// </summary>
    public abstract class GlobalMercatorProjection : MercatorProjection
    {
        /// <summary>
        ///     Instantiate a projection with the given reference Ellipsoid
        /// </summary>
        /// <param name="referenceGlobe">The reference Ellipsoid</param>
        protected GlobalMercatorProjection(Ellipsoid referenceGlobe)
            : base(referenceGlobe)
        {
        }

        /// <summary>
        ///     Get the Mercator scale factor for the given latitude
        /// </summary>
        /// <param name="latitude">The latitude</param>
        /// <returns>The scale factor</returns>
        public double ScaleFactor(double latitude)
        {
            var phi = Angle.DegToRad(latitude);
            var k = Math.Sqrt(1.0 - Math.Pow(Math.Sin(phi)*ReferenceGlobe.Eccentricity, 2.0))/Math.Cos(phi);
            return k;
        }

        /// <summary>
        ///     Get the Mercator scale factor for the given point
        /// </summary>
        /// <param name="point">The point</param>
        /// <returns>The scale factor</returns>
        public override double ScaleFactor(GlobalCoordinates point)
        {
            return ScaleFactor(point.Latitude.Degrees);
        }


        /// <summary>
        ///     Convert a latitude/longitude coordinate to a Euclidian coordinate on a flat map
        /// </summary>
        /// <param name="coordinates">The latitude/longitude coordinates in degrees</param>
        /// <returns>The euclidian coordinates of that point</returns>
        public override EuclidianCoordinate ToEuclidian(GlobalCoordinates coordinates)
        {
            return new EuclidianCoordinate(this)
            {
                X = LongitudeToX(coordinates.Longitude),
                Y = LatitudeToY(coordinates.Latitude)
            };
        }

        /// <summary>
        ///     Get the latitude/longitude coordinates from the euclidian coordinates
        /// </summary>
        /// <param name="xy">The euclidien coordinates</param>
        /// <returns>The latitude/longitude coordinates of that point</returns>
        public override GlobalCoordinates FromEuclidian(EuclidianCoordinate xy)
        {
            return new GlobalCoordinates(YToLatitude(xy.Y), XToLongitude(xy.X));
        }

        /// <summary>
        ///     Convert coordinates to an XY position on a Mercator map
        /// </summary>
        /// <param name="coordinates">The coordinates on the globe</param>
        /// <returns>An array of two doubles, the first is X, the second Y</returns>
        public double[] GlobalCoordinatesToXy(GlobalCoordinates coordinates)
        {
            var e = ToEuclidian(coordinates);
            return new[] {e.X, e.Y};
        }

        /// <summary>
        ///     Convert a XY position on a Mercator map into global coordinates
        /// </summary>
        /// <param name="x">The X position on the Mercator map</param>
        /// <param name="y">The Y position on the Mercator map</param>
        /// <returns>The global coordinates</returns>
        public GlobalCoordinates XyToGlobalCoordinates(double x, double y)
        {
            return FromEuclidian(new EuclidianCoordinate(this, x, y));
        }

        /// <summary>
        ///     Convert a XY position on a Mercator map into global coordinates
        /// </summary>
        /// <param name="xy">The xy position on the Mercator map</param>
        /// <returns>The global coordinates</returns>
        public GlobalCoordinates XyToGlobalCoordinates(double[] xy)
        {
            var e = new EuclidianCoordinate(this, xy);
            return FromEuclidian(e);
        }

        /// <summary>
        ///     Convert the longitude (n degrees) to an X-coordinate (in meters) on a Mercator map
        /// </summary>
        /// <param name="longitude">The longitude in degrees</param>
        /// <returns>The X coordinate in meters</returns>
        public double LongitudeToX(Angle longitude)
        {
            return ReferenceGlobe.SemiMajorAxis*(longitude - ReferenceMeridian).Radians;
        }

        /// <summary>
        ///     Convert the X coordinate (in meters) on a Mercator map back into the longitude
        /// </summary>
        /// <param name="x">The X coordinate (in meters) on a Mercator map</param>
        /// <returns>The longitude (in degrees)</returns>
        public double XToLongitude(double x)
        {
            var longitude = ReferenceMeridian + Angle.RadToDeg(x/ReferenceGlobe.SemiMajorAxis);
            return NormalizeLongitude(longitude).Degrees;
        }

        /// <summary>
        ///     Convert the longitude (n degrees) to an Y-coordinate (in meters) on a Mercator map
        /// </summary>
        /// <param name="latitude">The longitude in degrees</param>
        /// <returns>The Y coordinate in meters</returns>
        public abstract double LatitudeToY(Angle latitude);

        /// <summary>
        ///     Convert the Y coordinate (in meters) on a Mercator map back into the longitude
        /// </summary>
        /// <param name="y">The Y coordinate (in meters) on a Mercator map</param>
        /// <returns>The longitude (in degrees)</returns>
        public abstract double YToLatitude(double y);

        /// <summary>
        ///     Compute a loxodromic path from start to end witha given number of points
        /// </summary>
        /// <param name="start">starting coordinates</param>
        /// <param name="end">ending coordinates</param>
        /// <param name="mercatorRhumbDistance">The distance of the two points on a Rhumb line on the Mercator projection</param>
        /// <param name="bearing">The constant course for the path</param>
        /// <param name="numberOfPoints">Number of points on the path (including start and end)</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the number of points is less than 2</exception>
        /// <returns>An array of points describing the loxodromic path from start to end</returns>
        public GlobalCoordinates[] CalculatePath(
            GlobalCoordinates start,
            GlobalCoordinates end,
            out double mercatorRhumbDistance,
            out Angle bearing,
            int numberOfPoints = 10)
        {
            mercatorRhumbDistance = 0;
            bearing = 0;
            if (numberOfPoints < 2)
                throw new ArgumentOutOfRangeException(Message.GEODETIC_PATH_MIN_2);
            if (start == end || numberOfPoints == 2)
                return new[] {start, end};

            var cStart = ToEuclidian(start);
            var cEnd = ToEuclidian(end);
            var dist = EuclidianDistance(cStart, cEnd);
            var step = dist/(numberOfPoints - 1);
            var dx = (cEnd.X - cStart.X)/dist;
            var dy = (cEnd.Y - cStart.Y)/dist;
            bearing = 0;
            bearing.Radians = Math.Atan2(dx, dy);
            if (bearing < 0) bearing += 360;
            if (bearing == 90 || bearing == 270)
            {
                mercatorRhumbDistance = dist*ScaleFactor(start.Latitude.Degrees);
            }
            else
            {
                /*
                 * This is based on a paper published by Miljenko Petrović
                 * See: http://hrcak.srce.hr/file/24998
                 * */
                var e2 = ReferenceGlobe.Eccentricity*ReferenceGlobe.Eccentricity;
                mercatorRhumbDistance = ReferenceGlobe.SemiMajorAxis/Math.Cos(bearing.Radians)*
                                        (((1 - e2/4.0)*(end.Latitude - start.Latitude).Radians)
                                         -
                                         e2*(Math.Sin(2*end.Latitude.Radians)
                                             - Math.Sin(2*start.Latitude.Radians))*3.0/8.0);
            }
            var result = new GlobalCoordinates[numberOfPoints];
            result[0] = start;
            result[numberOfPoints - 1] = end;
            for (var i = 1; i < numberOfPoints - 1; i++)
            {
                var point = new EuclidianCoordinate(this, cStart.X + i*dx*step, cStart.Y + i*dy*step);
                result[i] = FromEuclidian(point);
            }
            return result;
        }
    }
}