namespace Unity.TypedFactories.Implementation
{
    using Castle.DynamicProxy;

    using Microsoft.Practices.Unity;

    internal abstract class TypedFactoryRegistrationBase : ITypedFactoryRegistration
    {
        #region Static Fields

        private static ProxyGenerator proxyGenerator;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedFactoryRegistration{TFactory}"/> class.
        /// </summary>
        /// <param name="container">
        /// The target Unity container on which to perform the registrations.
        /// </param>
        public TypedFactoryRegistrationBase(IUnityContainer container)
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

        protected static ProxyGenerator ProxyGenerator
        {
            get { return proxyGenerator ?? (proxyGenerator = new ProxyGenerator()); }
        }

        #endregion

        #region Public Methods and Operators

        public abstract void ForConcreteType<TTo>();

        #endregion
    }
}