namespace Unity.TypedFactories
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Exception thrown when the arguments supplied in a typed factory method do not fulfill the arguments required by a concrete class' constructors.
    /// </summary>
    public class ConstructorArgumentsMismatchException : Exception
    {
        #region Constructors and Destructors

        public ConstructorArgumentsMismatchException(string message, Type typedFactoryType, ParameterInfo[] nonMatchingParameters, Exception innerException)
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