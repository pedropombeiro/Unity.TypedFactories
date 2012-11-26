// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="FactoryInterceptor.cs" company="Liebherr International AG">
// //   © 2012 Liebherr. All rights reserved.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

namespace Unity.TypedFactories.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Castle.DynamicProxy;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Defines an <see cref="IInterceptor"/> implementation which implements the factory methods, by passing the method arguments by name into <see cref="TConcrete"/>'s constructor.
    /// </summary>
    /// <typeparam name="TConcrete">The concrete class which will be constructed by the factory.</typeparam>
    public class FactoryInterceptor<TConcrete> : IInterceptor
    {
        #region Constructors and Destructors

        public FactoryInterceptor(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Container = container;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The injected <see cref="IUnityContainer"/> instance which will be used to resolve the <see cref="TConcrete"/> type.
        /// </summary>
        private IUnityContainer Container { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method == null || !invocation.Method.ReturnType.IsInterface)
            {
                throw new NotImplementedException();
            }

            var returnType = invocation.Method.ReturnType;

            if (!returnType.IsAssignableFrom(typeof(TConcrete)))
            {
                throw new InvalidCastException(string.Format("The concrete type {0} does not implement the factory return type {1}", typeof(TConcrete).FullName, returnType.FullName));
            }

            invocation.ReturnValue = invocation.Arguments.Any()
                                         ? this.Container.Resolve(typeof(TConcrete), GetResolverOverridesFor(invocation).ToArray())
                                         : this.Container.Resolve(typeof(TConcrete));
        }

        #endregion

        #region Methods

        private static IEnumerable<ResolverOverride> GetResolverOverridesFor(IInvocation invocation)
        {
            var arguments = invocation.Arguments;
            var parameterInfos = invocation.Method.GetParameters();

            for (var parameterIndex = 0; parameterIndex < arguments.Length; ++parameterIndex)
            {
                var parameterInfo = parameterInfos.ElementAt(parameterIndex);
                var parameterValue = arguments[parameterIndex];

                yield return new ParameterOverride(parameterInfo.Name, parameterValue);
            }
        }

        #endregion
    }
}