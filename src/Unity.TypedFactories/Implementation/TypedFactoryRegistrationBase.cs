// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypedFactoryRegistrationBase.cs" company="Developer In The Flow">
//   © 2012-2013 Pedro Pombeiro
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Unity.TypedFactories.Implementation
{
    using Castle.DynamicProxy;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Implements the <see cref="ProxyGenerator"/> property on the fluent interface implementation for registering typed factories.
    /// </summary>
    internal abstract class TypedFactoryRegistrationBase : ITypedFactoryRegistration
    {
        #region Static Fields

        /// <summary>
        /// The Castle proxy generator.
        /// </summary>
        private static ProxyGenerator proxyGenerator;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedFactoryRegistrationBase"/> class.
        /// </summary>
        /// <param name="container">
        /// The target Unity container on which to perform the registrations.
        /// </param>
        protected TypedFactoryRegistrationBase(IUnityContainer container)
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

        #region Properties

        /// <summary>
        /// Gets the Castle proxy generator. A new instance will be created upon the first access, and reused afterwards.
        /// </summary>
        protected static ProxyGenerator ProxyGenerator
        {
            get { return proxyGenerator ?? (proxyGenerator = new ProxyGenerator()); }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Defines the concrete type which the factory will create.
        /// </summary>
        /// <typeparam name="TTo">
        /// The concrete type which the factory will instantiate.
        /// </typeparam>
        public abstract void ForConcreteType<TTo>() where TTo : class;

        #endregion
    }
}