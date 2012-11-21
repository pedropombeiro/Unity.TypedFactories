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
                                         ? this.Container.Resolve(typeof(TConcrete), GetDependencyOverrides(invocation).ToArray())
                                         : this.Container.Resolve(typeof(TConcrete));
        }

        #endregion

        #region Methods

        private static IEnumerable<ResolverOverride> GetDependencyOverrides(IInvocation invocation)
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