/* See License.md in the solution root for license information.
 * File: GlobalMesh.cs
*/
using System;

namespace Geodesy
{
    /// <summary>
    /// This class overlays the globe with a mesh of squares
    /// </summary>
    public class GlobalMesh
    {
        private readonly UtmProjection _utm = new UtmProjection();
        private readonly double _maxWidth, _maxHeight;
        private readonly int _squareSize;
        private readonly long _modulus;
        private readonly long _meshCount;

        /// <summary>
        /// The UTM Projection for the Globe we cover with the mesh.
        /// </summary>
        public UtmProjection Projection
        {
            get { return _utm;  }
        }

        /// <summary>
        /// The total number of meshes used to cover the Globe.
        /// </summary>
        public long Count
        {
            get { return _meshCount; }
        }

        /// <summary>
        /// Instantiate the Mesh with the given nuber of meters as the size
        /// of the mesh squares. We do not support squares less than 10m.
        /// </summary>
        /// <param name="squareSizeinMeters">The size of the squares in meter</param>
        public GlobalMesh(int squareSizeinMeters)
        {
            if (squareSizeinMeters <= 10)
                throw new ArgumentOutOfRangeException(Properties.Resources.MESHSIZE_MIN_10);

            _squareSize = squareSizeinMeters;
            var s = (double) squareSizeinMeters;

            for (var zone = 1; zone <= UtmGrid.NumberOfZones; zone++)
            {
                for (var band = 0; band < UtmGrid.NumberOfBands; band++)
                {
                    try
                    {
                        var theGrid = new UtmGrid(_utm, zone, band);
                        _maxWidth = Math.Max(_maxWidth, theGrid.MapWidth);
                        _maxHeight = Math.Max(_maxHeight, theGrid.MapHeight);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            var xModulus = (long)Math.Round((_maxWidth  + s - 1.0) / s, MidpointRounding.AwayFromZero);
            var yModulus = (long)Math.Round((_maxHeight + s - 1.0) / s, MidpointRounding.AwayFromZero);
            var m = Math.Max(xModulus, yModulus);
            var lnmod = (int)Math.Round(Math.Log(m)/Math.Log(2) + 0.5, MidpointRounding.AwayFromZero);
            while ((1 << lnmod) < m) lnmod++;
            _modulus = 1 << lnmod;
            _meshCount = _modulus*_modulus;
        }

        /// <summary>
        /// Get the globally unique Mesh number of a coordinate
        /// </summary>
        /// <param name="coord">The UTM coordinate to convert</param>
        /// <returns>The mesh number to which the coordinate belongs</returns>
        public long MeshNumber(UtmCoordinate coord)
        {
            var gridOrdinal = coord.Grid.Ordinal;
            var relX = (long)Math.Round(coord.X - coord.Grid.Origin.X,MidpointRounding.AwayFromZero)/_squareSize;
            var relY = (long)Math.Round(coord.Y - coord.Grid.Origin.Y,MidpointRounding.AwayFromZero)/_squareSize;
            var res = coord.Grid.Ordinal*_meshCount +  relX*_modulus + relY;
            return res;
        }

        /// <summary>
        /// Get the globally unique Mesh number of a location given by
        /// latitude and longitude.
        /// </summary>
        /// <param name="coord">The location to convert</param>
        /// <returns>The mesh number to which the location belongs</returns>
        public long MeshNumber(GlobalCoordinates coord)
        {
            return MeshNumber((UtmCoordinate)_utm.ToEuclidian(coord));
        }

        /// <summary>
        /// Get the globally unique Mesh number of a location given by
        /// latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude (in degrees)</param>
        /// <param name="longitude">The longitude (in degrees)</param>
        /// <returns>The mesh number to which the location belongs</returns>
        public long MeshNumber(Angle latitude, Angle longitude)
        {
            return MeshNumber(new GlobalCoordinates(latitude, longitude));
        }

        /// <summary>
        /// Return the central coordinates of a Mesh given by its number.
        /// Please note that this center is on the UTM map, but at the borders
        /// of a grid this coordinate may actually overlap and belong to another
        /// grid. So if you convert them to a Latitude/Longitude and then back to
        /// an UtmCoordinate, you may get different values.
        /// </summary>
        /// <param name="meshNumber">The number of the mesh</param>
        /// <returns>The UTM coordinates of the center of the square</returns>
        public UtmCoordinate CenterOf(long meshNumber)
        {
            var ord = (int)(meshNumber/_meshCount);
            var local = meshNumber%(_meshCount);
            var theGrid = new UtmGrid(_utm,ord);
            var relX = (local/_modulus)*_squareSize + _squareSize/2;
            var relY = (local%_modulus)*_squareSize + _squareSize/2;
            return new UtmCoordinate(theGrid,theGrid.Origin.X+relX,theGrid.Origin.Y+relY);
        }
    }
}
