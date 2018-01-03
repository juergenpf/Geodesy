/* See License.md in the solution root for license information.
 * File: Constants.cs
*/
using Geodesy;

namespace UnitTestGeodesy
{
    /// <summary>
    /// Some constants used throughout the Unit Tests.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// That's where my home is.
        /// </summary>
        public static readonly GlobalCoordinates MyHome = 
            new GlobalCoordinates(49.8459444, 8.7993944);
         
        public static readonly GlobalCoordinates MyOffice =
            new GlobalCoordinates(50.2160806, 8.6152611);
    }
}