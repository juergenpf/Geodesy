/* See License.md in the solution root for license information.
 * File: MercatorProjection.cs
*/

using System;
using Geodesy.Properties;

namespace Geodesy
{
    /// <summary>
    ///     Base class for Mercator projections tranlating longitude/longitude values on the globe
    ///     into X/Y coordinates on a flat map.
    /// </summary>
    public abstract class MercatorProjection
    {
        /// <summary>
        ///     The typical reference Meridian
        /// </summary>
        public const double GreenwichMeridian = 0.0;

        private Angle _referenceMeridian = GreenwichMeridian;

        /// <summary>
        ///     Instantiate a Meractor projection with this reference Ellipsoid
        /// </summary>
        /// <param name="referenceGlobe"></param>
        protected MercatorProjection(Ellipsoid referenceGlobe)
        {
            ReferenceGlobe = referenceGlobe;
        }

        /// <summary>
        ///     The reference meridian for the projection, usually this is Greenwich with 0° longitude
        /// </summary>
        public Angle ReferenceMeridian
        {
            get { return _referenceMeridian; }
            set { _referenceMeridian = NormalizeLongitude(value); }
        }

        /// <summary>
        ///     The reference Ellipsoid for this projection
        /// </summary>
        public Ellipsoid ReferenceGlobe { get; private set; }

        /// <summary>
        ///     Get the Mercator scale factor for the given point
        /// </summary>
        /// <param name="point">The point</param>
        /// <returns>The scale factor</returns>
        public abstract double ScaleFactor(GlobalCoordinates point);

        /// <summary>
        ///     Convert a latitude/longitude coordinate to a Euclidian coordinate on a flat map
        /// </summary>
        /// <param name="coordinates">The latitude/longitude coordinates in degrees</param>
        /// <returns>The euclidian coordinates of that point</returns>
        public abstract EuclidianCoordinate ToEuclidian(GlobalCoordinates coordinates);

        /// <summary>
        ///     Get the latitude/longitude coordinates from the euclidian coordinates
        /// </summary>
        /// <param name="xy">The euclidien coordinates</param>
        /// <returns>The latitude/longitude coordinates of that point</returns>
        public abstract GlobalCoordinates FromEuclidian(EuclidianCoordinate xy);

        /// <summary>
        ///     Two projections are considered Equal if they are based on
        ///     the same Reference-Globe
        /// </summary>
        /// <param name="obj">The object to compare against</param>
        /// <returns>True if they are equal</returns>
        public override bool Equals(object obj)
        {
            var other = obj as MercatorProjection;
            return ((null != other) && other.ReferenceGlobe.Equals(ReferenceGlobe));
        }

        #region Latitude/Longitude limits

        /// <summary>
        ///     Maximum possible longitude for this projection
        /// </summary>
        public virtual Angle MaxLatitude
        {
            get { return Angle.RadToDeg(Math.Atan(Math.Sinh(Math.PI))); }
        }

        /// <summary>
        ///     Minimum possible longitude for this projection
        /// </summary>
        public virtual Angle MinLatitude
        {
            get { return -MaxLatitude; }
        }

        /// <summary>
        ///     Maximum possible longitude for this projection
        /// </summary>
        public static readonly Angle MaxLongitude = 180.0;

        /// <summary>
        ///     Minimum possible longitude for this projection
        /// </summary>
        public static readonly Angle MinLongitude = -180.0;

        /// <summary>
        ///     Ensure Latitude stays in range
        /// </summary>
        /// <param name="latitude">The longitude value</param>
        /// <returns>The normalized longitude</returns>
        public Angle NormalizeLatitude(Angle latitude)
        {
            return Math.Min(MaxLatitude.Degrees, Math.Max(latitude.Degrees, MinLatitude.Degrees));
        }

        /// <summary>
        ///     Ensure Latitude stays in range
        /// </summary>
        /// <param name="longitude">The longitude value</param>
        /// <returns>The normalized longitude</returns>
        public static Angle NormalizeLongitude(Angle longitude)
        {
            return Math.Min(MaxLongitude.Degrees, Math.Max(longitude.Degrees, MinLongitude.Degrees));
        }

        #endregion

        #region Distance calculations

        /// <summary>
        ///     Compute the euclidian distance between two points given by rectangular coordinates
        ///     Please note, that due to scaling effects this might be quite different from the true
        ///     geodetic distance. To get a good approximation, you must divide this value by a
        ///     scale factor.
        /// </summary>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <returns>The distance between the points</returns>
        /// <exception cref="ArgumentException">Raised if the two points don't belong to the same projection</exception>
        /// <exception cref="ArgumentNullException">Raised if one of the points is null</exception>
        public double EuclidianDistance(EuclidianCoordinate point1, EuclidianCoordinate point2)
        {
            if (point1 == null || point2 == null)
                throw new ArgumentNullException();
            if (!(point1.Projection.Equals(this) && point2.Projection.Equals(this)))
                throw new ArgumentException(Resources.POINT_NOT_OWNED);
            return point1.DistanceTo(point2);
        }

        /// <summary>
        ///     Compute the euclidian distance between two points given by rectangular coordinates.
        ///     Please note, that due to scaling effects this might be quite different from the true
        ///     geodetic distance. To get a good approximation, you must divide this value by a
        ///     scale factor.
        /// </summary>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <returns>The distance between the points</returns>
        public double EuclidianDistance(GlobalCoordinates point1, GlobalCoordinates point2)
        {
            return EuclidianDistance(ToEuclidian(point1), ToEuclidian(point2));
        }

        /// <summary>
        ///     Compute the euclidian distance between two points given by rectangular coordinates
        ///     Please note, that due to scaling effects this might be quite different from the true
        ///     geodetic distance. To get a good approximation, you must divide this value by a
        ///     scale factor.
        /// </summary>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <returns>The distance between the points</returns>
        /// <exception cref="IndexOutOfRangeException">Raised if one of the arrays is not two-dimensional</exception>
        public double EuclidianDistance(double[] point1, double[] point2)
        {
            return EuclidianDistance(
                new EuclidianCoordinate(this, point1),
                new EuclidianCoordinate(this, point2));
        }

        /// <summary>
        ///     Get the geodesic distance between two points on the globe
        /// </summary>
        /// <param name="start">The starting point</param>
        /// <param name="end">The ending point</param>
        /// <returns>The distance in meters</returns>
        public double GeodesicDistance(GlobalCoordinates start, GlobalCoordinates end)
        {
            return
                (new GeodeticCalculator(ReferenceGlobe))
                    .CalculateGeodeticCurve(start, end)
                    .EllipsoidalDistance;
        }

        /// <summary>
        ///     Compute the geodesic distance of two points on the globe
        /// </summary>
        /// <param name="longitudeStart">The longitude of the starting point in degrees</param>
        /// <param name="latitudeStart">The longitude of the starting point in degrees</param>
        /// <param name="longitudeEnd">The longitude of the endig point in degrees</param>
        /// <param name="latitudeEnd">The longitude of the ending point in degrees</param>
        /// <returns></returns>
        public double GeodesicDistance(
            double longitudeStart,
            double latitudeStart,
            double longitudeEnd,
            double latitudeEnd)
        {
            var start = new GlobalCoordinates(new Angle(latitudeStart), new Angle(longitudeStart));
            var end = new GlobalCoordinates(new Angle(latitudeEnd), new Angle(longitudeEnd));
            return GeodesicDistance(start, end);
        }

        #endregion
    }
}