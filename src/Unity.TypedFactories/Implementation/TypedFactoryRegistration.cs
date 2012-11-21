// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypedFactoryRegistration.cs" company="Pedro Pombeiro">
//   © 2012 Pedro Pombeiro
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Unity.TypedFactories.Implementation
{
    using Castle.DynamicProxy;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Implements the fluent interface for registering typed factories.
    /// </summary>
    /// <typeparam name="TFactory">
    /// The interface of the factory.
    /// </typeparam>
    internal class TypedFactoryRegistration<TFactory> : ITypedFactoryRegistration
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
        {
            this.Container = container;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the target Unity container on which to perform the registrations.
        /// </summary>
        public IUnityContainer Container { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Defines the concrete type which the factory will create.
        /// </summary>
        public void ForConcreteType<TTo>()
        {
            this.Container.RegisterInstance(new ProxyGenerator().CreateInterfaceProxyWithoutTarget<TFactory>(new FactoryInterceptor<TTo>(this.Container)));
        }

        #endregion
    }
}