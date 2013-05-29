// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectConstructionException.cs" company="Developer In The Flow">
//   © 2012-2013 Pedro Pombeiro
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Unity.TypedFactories
{
    using System;

    /// <summary>
    /// Exception thrown when the constructor invoked by a factory throws an exception.
    /// </summary>
    public class ObjectConstructionException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConstructionException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public ObjectConstructionException(string message, 
                                           Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}