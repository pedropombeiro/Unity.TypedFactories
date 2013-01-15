// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstructorArgumentsMismatchException.cs" company="Developer In The Flow">
//   © 2012 Pedro Pombeiro
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Unity.TypedFactories
{
    using System;
    using System.Reflection;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Exception thrown when the arguments supplied in a typed factory method do not fulfill the arguments required by a concrete class' constructors.
    /// </summary>
    public class ConstructorArgumentsMismatchException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorArgumentsMismatchException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="typedFactoryType">
        /// The type of the factory interface.
        /// </param>
        /// <param name="nonMatchingParameters">
        /// The list of non-matching parameters.
        /// </param>
        /// <param name="innerException">
        /// The inner exception, documenting the resolution failure.
        /// </param>
        public ConstructorArgumentsMismatchException(string message, Type typedFactoryType, ParameterInfo[] nonMatchingParameters, ResolutionFailedException innerException)
            : base(message, innerException)
        {
            this.TypedFactoryType = typedFactoryType;
            this.NonMatchingParameters = nonMatchingParameters;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the list of parameters from the factory method which were not found in the concrete type's constructor.
        /// </summary>
        public ParameterInfo[] NonMatchingParameters { get; private set; }

        /// <summary>
        /// Gets the <see cref="Type"/> of the Typed Factory interface.
        /// </summary>
        public Type TypedFactoryType { get; private set; }

        #endregion
    }
}