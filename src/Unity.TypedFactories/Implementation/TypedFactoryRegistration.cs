// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypedFactoryRegistration.cs" company="Developer In The Flow">
//   © 2012 Pedro Pombeiro
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Unity.TypedFactories.Implementation
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Implements the fluent interface for registering typed factories.
    /// </summary>
    /// <typeparam name="TFactory">
    /// The interface of the factory.
    /// </typeparam>
    internal class TypedFactoryRegistration<TFactory> : TypedFactoryRegistrationBase
        where TFactory : class
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedFactoryRegistration{TFactory}"/> class.
        /// </summary>
        /// <param name="container">
        /// The target Unity container on which to perform the registrations.
        /// </param>
        public TypedFactoryRegistration(IUnityContainer container)
            : base(container)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Defines the concrete type which the factory will create.
        /// </summary>
        /// <typeparam name="TTo">
        /// The concrete type which the factory will instantiate.
        /// </typeparam>
        public override void ForConcreteType<TTo>()
        {
            this.Container.RegisterType<TFactory>(
                new InjectionFactory(
                    container => ProxyGenerator.CreateInterfaceProxyWithoutTarget<TFactory>(new FactoryInterceptor<TTo>(container))));
        }

        #endregion
    }
}