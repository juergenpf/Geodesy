/* See License.md in the solution root for license information.
 * File: GlobalMesh.cs
*/
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Geodesy
{
    /// <summary>
    /// This class overlays the globe with a mesh of squares.
    /// The algorithm is based on subdividing the UTM grids into
    /// finer cell structures, so the coverage is for latitudes
    /// between 80° South and 84° North.
    /// </summary>
    public class GlobalMesh
    {
        const int MinimumMeshSize = 1;

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
        /// The total number of meshes used to cover an UTM Grid.
        /// </summary>
        public long Count
        {
            get { return _meshCount; }
        }

        /// <summary>
        /// The total number of meshes used to cover the globe.
        /// </summary>
        public long GlobalCount
        {
            get { return _meshCount*UtmGrid.NumberOfGrids; }
        }

        /// <summary>
        /// Instantiate the Mesh with the given nuber of meters as the size
        /// of the mesh squares. We do not support squares less than 1m.
        /// Please note that the actual mesh size used is a derived value
        /// that approximates the requested mesh size in order to provide
        /// better computational efficiency.
        /// </summary>
        /// <param name="squareSizeinMeters">The size of the squares in meter. The defauklt value is 1000m.</param>
        public GlobalMesh(int squareSizeinMeters=1000)
        {
            if (squareSizeinMeters <= MinimumMeshSize)
                throw new ArgumentOutOfRangeException(Properties.Resources.MESHSIZE_MIN_VIOLATION);

            _squareSize = squareSizeinMeters;
            var dblSquareSize = (double) squareSizeinMeters;
            _maxWidth = double.MinValue;
            _maxHeight = double.MinValue;

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

            var xModulus = (long)Math.Round((_maxWidth  + dblSquareSize - 1.0) / dblSquareSize, MidpointRounding.AwayFromZero);
            var yModulus = (long)Math.Round((_maxHeight + dblSquareSize - 1.0) / dblSquareSize, MidpointRounding.AwayFromZero);
            if (xModulus < 2 || yModulus < 2)
                throw new ArgumentOutOfRangeException(Properties.Resources.MESHSIZE_TOO_BIG);
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

        void ValidateMeshNumber(long meshNumber)
        {
            if (meshNumber < 0 || meshNumber >= GlobalCount)
                throw new ArgumentOutOfRangeException(Properties.Resources.INVALID_MESH_NUMBER);
        }

        /// <summary>
        /// Return the central coordinates of a Mesh given by its number.
        /// Please note that this center is on the UTM map, but at the borders
        /// of a grid this coordinate may actually overlap and belong to another
        /// UTM grid. So if you convert them to a Latitude/Longitude and then back
        /// to an UtmCoordinate, you may get different values.
        /// </summary>
        /// <param name="meshNumber">The number of the mesh</param>
        /// <returns>The UTM coordinates of the center of the square</returns>
        public UtmCoordinate CenterOf(long meshNumber)
        {
            ValidateMeshNumber(meshNumber);
            var ord = (int)(meshNumber/_meshCount);
            var local = meshNumber%(_meshCount);
            var theGrid = new UtmGrid(_utm,ord);
            var relX = (local/_modulus)*_squareSize + _squareSize/2;
            var relY = (local%_modulus)*_squareSize + _squareSize/2;
            return new UtmCoordinate(theGrid,theGrid.Origin.X+relX,theGrid.Origin.Y+relY);
        }

        /// <summary>
        /// Return the lower left corner coordinates of a Mesh given by its number.
        /// Please note that this point is on the UTM map, but at the borders
        /// of a grid this coordinate may actually overlap and belong to another
        /// UTM grid. So if you convert them to a Latitude/Longitude and then back
        /// to an UtmCoordinate, you may get different values.
        /// </summary>
        /// <param name="meshNumber"></param>
        /// <returns>The UTM coordinates of the lower left corner of the Mesh</returns>
        public UtmCoordinate LowerLeft(long meshNumber)
        {
            ValidateMeshNumber(meshNumber);
            var ord = (int)(meshNumber / _meshCount);
            var local = meshNumber % (_meshCount);
            var theGrid = new UtmGrid(_utm, ord);
            var relX = (local / _modulus) * _squareSize;
            var relY = (local % _modulus) * _squareSize;
            return new UtmCoordinate(theGrid, theGrid.Origin.X + relX, theGrid.Origin.Y + relY);
        }
    
        /// <summary>
        /// Return the lower right corner coordinates of a Mesh given by its number.
        /// Please note that this point is on the UTM map, but at the borders
        /// of a grid this coordinate may actually overlap and belong to another
        /// UTM grid. So if you convert them to a Latitude/Longitude and then back
        /// to an UtmCoordinate, you may get different values.
        /// </summary>
        /// <param name="meshNumber"></param>
        /// <returns>The UTM coordinates of the lower right corner of the Mesh</returns>
        public UtmCoordinate LowerRight(long meshNumber)
        {
            ValidateMeshNumber(meshNumber);
            var ord = (int)(meshNumber / _meshCount);
            var local = meshNumber % (_meshCount);
            var theGrid = new UtmGrid(_utm, ord);
            var relX = (local / _modulus) * _squareSize + _squareSize;
            var relY = (local % _modulus) * _squareSize;
            return new UtmCoordinate(theGrid, theGrid.Origin.X + relX, theGrid.Origin.Y + relY);
        }

        /// <summary>
        /// Return the upper left corner coordinates of a Mesh given by its number.
        /// Please note that this point is on the UTM map, but at the borders
        /// of a grid this coordinate may actually overlap and belong to another
        /// UTM grid. So if you convert them to a Latitude/Longitude and then back
        /// to an UtmCoordinate, you may get different values.
        /// </summary>
        /// <param name="meshNumber"></param>
        /// <returns>The UTM coordinates of the upper left corner of the Mesh</returns>
        public UtmCoordinate UpperLeft(long meshNumber)
        {
            ValidateMeshNumber(meshNumber);
            var ord = (int)(meshNumber / _meshCount);
            var local = meshNumber % (_meshCount);
            var theGrid = new UtmGrid(_utm, ord);
            var relX = (local / _modulus) * _squareSize;
            var relY = (local % _modulus) * _squareSize + _squareSize;
            return new UtmCoordinate(theGrid, theGrid.Origin.X + relX, theGrid.Origin.Y + relY);
        }

        /// <summary>
        /// Return the upper right corner coordinates of a Mesh given by its number.
        /// Please note that this point is on the UTM map, but at the borders
        /// of a grid this coordinate may actually overlap and belong to another
        /// UTM grid. So if you convert them to a Latitude/Longitude and then back
        /// to an UtmCoordinate, you may get different values.
        /// </summary>
        /// <param name="meshNumber"></param>
        /// <returns>The UTM coordinates of the upper right corner of the Mesh</returns>
        public UtmCoordinate UpperRight(long meshNumber)
        {
            ValidateMeshNumber(meshNumber);
            var ord = (int)(meshNumber / _meshCount);
            var local = meshNumber % (_meshCount);
            var theGrid = new UtmGrid(_utm, ord);
            var relX = (local / _modulus) * _squareSize + _squareSize;
            var relY = (local % _modulus) * _squareSize + _squareSize;
            return new UtmCoordinate(theGrid, theGrid.Origin.X + relX, theGrid.Origin.Y + relY);
        }

        private void GetDimension(long meshNumber, out long maxx, out long maxy)
        {
            ValidateMeshNumber(meshNumber);
            var ord = (int)(meshNumber / _meshCount);
            var grid = new UtmGrid(_utm,ord);
            var ll = MeshNumber(grid.LowerLeftCorner) % _meshCount;
            var llx = ll / _modulus;
            var lly = ll % _modulus;
            var lr = MeshNumber(grid.LowerRightCorner) % _meshCount;
            var lrx = lr / _modulus;
            var lry = lr % _modulus;
            var ul = MeshNumber(grid.UpperLeftCorner) % _meshCount;
            var ulx = ul / _modulus;
            var uly = ul % _modulus;
            var ur = MeshNumber(grid.UpperRightCorner) % _meshCount;
            var urx = ur / _modulus;
            var ury = ur % _modulus;
            maxx = Math.Max(lrx, urx);
            maxy = Math.Max(uly, ury);
        }

        /// <summary>
        /// Get the list of neighbor meshes in a specified "distance". Distance 1 means
        /// direct neighbors, 2 means neighbors that are 2 meshes away etc.
        /// </summary>
        /// <param name="meshNumber">The mesh number</param>
        /// <param name="distance">The distance (0-3 currently supported)</param>
        /// <returns>The list of mesh numbers of the neighbors</returns>
        public List<long> Neighborhood(long meshNumber, int distance)
        {
            const int maxDistance = 3;
            ValidateMeshNumber(meshNumber);
            if (distance < 0 || distance > maxDistance)
                throw new ArgumentOutOfRangeException(Properties.Resources.INVALID_DISTANCE);
            if (distance == 0)
            {
                return new List<long> {meshNumber};
            }
            else
            {
                long maxx, maxy;
                GetDimension(meshNumber,out maxx,out maxy);
                var ord = (int)(meshNumber / _meshCount);
                var local = meshNumber % (_meshCount);
                var relX = (local/_modulus);
                var relY = (local % _modulus);
                var result = new List<long>();
                for (var y = -distance; y <= distance; y++)
                {
                    for (var x = -distance; x <= distance; x++)
                    {
                        var add = false;
                        if (!(x == 0 && y == 0))
                        {
                            var nx = relX + x;
                            if (!(nx < 0 || nx > maxx))
                            {
                                var ny = relY + y;
                                if (!(ny < 0 || ny > maxy))
                                {
                                    if (Math.Abs(y) == distance)
                                        add = true;
                                    else
                                    {
                                        if (Math.Abs(x) == distance)
                                            add = true;
                                    }
                                    if (add)
                                        result.Add(ord*_meshCount + nx*_modulus + ny);
                                }
                            }
                        }
                    }
                }
                return result;
            }
        }
    }
}
