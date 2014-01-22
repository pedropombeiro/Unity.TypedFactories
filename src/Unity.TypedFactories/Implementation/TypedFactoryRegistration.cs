// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypedFactoryRegistration.cs" company="Developer In The Flow">
//   © 2012-2014 Pedro Pombeiro
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Unity.TypedFactories.Implementation
{
    using System;

    using Castle.DynamicProxy;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Implements the fluent interface for registering typed factories.
    /// </summary>
    internal class TypedFactoryRegistration : ITypedFactoryRegistration
    {
        #region Static Fields

        /// <summary>
        /// The Castle proxy generator.
        /// </summary>
        private static ProxyGenerator proxyGenerator;

        #endregion

        #region Fields

        /// <summary>
        ///     The factory interface.
        /// </summary>
        private readonly Type factoryContractType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedFactoryRegistration"/> class.
        /// </summary>
        /// <param name="container">
        ///     The target Unity container on which to perform the registrations.
        /// </param>
        /// <param name="factoryContractType">
        ///     The factory interface.
        /// </param>
        /// <param name="name">
        ///     Name that will be used to request the type.
        /// </param>
        public TypedFactoryRegistration(
            IUnityContainer container,
            Type factoryContractType,
            string name = null)
        {
            this.factoryContractType = factoryContractType;
            this.Container = container;
            this.Name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the target Unity container on which to perform the registrations.
        /// </summary>
        public IUnityContainer Container { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Castle proxy generator. A new instance will be created upon the first access, and reused afterwards.
        /// </summary>
        private static ProxyGenerator ProxyGenerator
        {
            get
            {
                return proxyGenerator ?? (proxyGenerator = new ProxyGenerator());
            }
        }

        /// <summary>
        /// Gets the name that will be used to request the type.
        /// </summary>
        private string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Defines the concrete type which the factory will create.
        /// </summary>
        /// <param name="toType">
        /// The concrete type which the factory will instantiate.
        /// </param>
        public void ForConcreteType(Type toType)
        {
            var injectionFactory = new InjectionFactory(container => ProxyGenerator.CreateInterfaceProxyWithoutTarget(this.factoryContractType, new FactoryInterceptor(container, toType, this.Name)));

            if (this.Name != null)
            {
                this.Container.RegisterType(null, this.factoryContractType, this.Name, injectionFactory);
            }
            else
            {
                this.Container.RegisterType(null, this.factoryContractType, injectionFactory);
            }
        }

        #endregion
    }
}