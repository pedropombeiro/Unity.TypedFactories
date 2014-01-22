namespace Unity.TypedFactories.Implementation
{
    using System;
    using System.Linq;

    using Castle.DynamicProxy;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Defines an <see cref="IInterceptor"/> implementation which implements the factory methods, by passing the method arguments by name into <see cref="TConcrete"/>'s constructor.
    /// </summary>
    /// <typeparam name="TConcrete">The concrete class which will be constructed by the factory.</typeparam>
    public class GenericFactoryInterceptor<TConcrete> : FactoryInterceptor
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericFactoryInterceptor{TConcrete}"/> class.
        /// </summary>
        /// <param name="container">
        ///     The Unity container.
        /// </param>
        /// <param name="name">Name that will be used to request the type.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the injected <paramref name="container"/> is null.
        /// </exception>
        public GenericFactoryInterceptor(
            IUnityContainer container,
            string name)
            : base(container, typeof(TConcrete), name)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resolve an array of objects based on the context described in <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The invocation context.</param>
        protected override void ResolveArray(IInvocation invocation)
        {
            invocation.ReturnValue =
                (invocation.Arguments.Any()
                     ? this.Container.ResolveAll<TConcrete>(GetResolverOverridesFor(invocation).ToArray())
                     : this.Container.ResolveAll<TConcrete>())
                    .ToArray();
        }

        #endregion
    }
}