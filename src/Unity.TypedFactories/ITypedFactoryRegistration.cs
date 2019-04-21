// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITypedFactoryRegistration.cs" company="Developer In The Flow">
//   � 2012-2014 Pedro Pombeiro
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Unity.TypedFactories
{
    using System;

    using JetBrains.Annotations;

    using Unity;

    /// <summary>
    /// Defines the contract for the fluent interface for registering typed factories.
    /// </summary>
    public interface ITypedFactoryRegistration
    {
        #region Public Properties

        /// <summary>
        /// Gets the target Unity container on which to perform the registrations.
        /// </summary>
        [PublicAPI]
        IUnityContainer Container { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Defines the concrete type which the factory will create.
        /// </summary>
        /// <param name="toType">
        /// The concrete type which the factory will instantiate.
        /// </param>
        [PublicAPI]
        void ForConcreteType(Type toType);

        /// <summary>
        /// Defines the concrete type which the factory will create.
        /// </summary>
        /// <typeparam name="TTo">
        /// The concrete type which the factory will instantiate.
        /// </typeparam>
        void ForConcreteType<TTo>();

        #endregion
    }
}