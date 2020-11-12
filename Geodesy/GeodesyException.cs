/* See License.md in the solution root for license information.
 * File: GeodesyException.cs
*/

using System;

namespace Geodesy
{
    /// <summary>
    ///     The exception being used for internal errors in the Geodesy library
    /// </summary>
    [Serializable]
    public class GeodesyException : Exception
    {
        private GeodesyException()
        { }

        /// <summary>
        ///     New GeodesyException with a specified message
        /// </summary>
        /// <param name="message">The message for this exception</param>
        public GeodesyException(string message) : base(message)
        { }

        /// <summary>
        ///     New GeodesyException with a specified message and causing inner exception
        /// </summary>
        /// <param name="message">The message for this exception</param>
        /// <param name="innerException">The inner exception causing this Geodesy exception</param>
        public GeodesyException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}