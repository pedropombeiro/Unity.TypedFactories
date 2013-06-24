// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypedFactoryRegistration.cs" company="Developer In The Flow">
//   © 2012-2013 Pedro Pombeiro
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
        ///     The target Unity container on which to perform the registrations.
        /// </param>
        /// <param name="name">Name that will be used to request the type.</param>
        public TypedFactoryRegistration(
            IUnityContainer container, 
            string name = null)
            : base(container, name)
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
            var injectionFactory = new InjectionFactory(container => ProxyGenerator.CreateInterfaceProxyWithoutTarget<TFactory>(new FactoryInterceptor<TTo>(container)));

            if (this.Name != null)
            {
                this.Container.RegisterType<TFactory>(this.Name, injectionFactory);
            }
            else
            {
                this.Container.RegisterType<TFactory>(injectionFactory);
            }
        }

        #endregion
    }
}