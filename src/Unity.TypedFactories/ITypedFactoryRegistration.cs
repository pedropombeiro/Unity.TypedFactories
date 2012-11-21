// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITypedFactoryRegistration.cs" company="Pedro Pombeiro">
//   © 2012 Pedro Pombeiro
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Unity.TypedFactories
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Defines the contract for the fluent interface for registering typed factories.
    /// </summary>
    public interface ITypedFactoryRegistration
    {
        #region Public Properties

        /// <summary>
        /// Gets the target Unity container on which to perform the registrations.
        /// </summary>
        IUnityContainer Container { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Defines the concrete type which the factory will create.
        /// </summary>
        void ForConcreteType<TTo>();

        #endregion
    }
}