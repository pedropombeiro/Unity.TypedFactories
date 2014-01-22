// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FactoryInterceptor.cs" company="Developer In The Flow">
//   © 2012-2014 Pedro Pombeiro
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Unity.TypedFactories.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Castle.DynamicProxy;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Defines an <see cref="IInterceptor"/> implementation which implements the factory methods, by passing the method arguments by name into a specified concrete type's constructor.
    /// </summary>
    public class FactoryInterceptor : IInterceptor
    {
        #region Fields

        /// <summary>
        /// The concrete class which will be constructed by the factory.
        /// </summary>
        private readonly Type concreteType;

        /// <summary>
        /// Name that will be used to request the type.
        /// </summary>
        private readonly string name;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FactoryInterceptor"/> class.
        /// </summary>
        /// <param name="container">
        ///     The Unity container.
        /// </param>
        /// <param name="concreteType">
        ///     The concrete class which will be constructed by the factory.
        /// </param>
        /// <param name="name">Name that will be used to request the type.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the injected <paramref name="container"/> is null.
        /// </exception>
        public FactoryInterceptor(IUnityContainer container,
                                  Type concreteType,
                                  string name)
        {
            this.concreteType = concreteType;
            this.name = name;
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Container = container;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the injected <see cref="IUnityContainer"/> instance which will be used to resolve the <see cref="concreteType"/> type.
        /// </summary>
        private IUnityContainer Container { get; set; }

        #endregion

        #region Explicit Interface Methods

        /// <inheritdoc />
        void IInterceptor.Intercept(IInvocation invocation)
        {
            if (invocation.Method == null || !invocation.Method.ReturnType.IsInterface)
            {
                throw new NotImplementedException();
            }

            var returnType = invocation.Method.ReturnType;
            var collectionType = typeof(IEnumerable<>).MakeGenericType(this.concreteType);
            var isCollection = collectionType.IsAssignableFrom(returnType);

            if (!returnType.IsAssignableFrom(this.concreteType) && !isCollection)
            {
                throw new InvalidCastException(string.Format("{2}: The concrete type {0} does not implement the factory method return type {1}", this.concreteType.FullName, returnType.FullName, invocation));
            }

            try
            {
                if (isCollection)
                {
                    invocation.ReturnValue =
                        (invocation.Arguments.Any()
                             ? this.Container.ResolveAll(this.concreteType, GetResolverOverridesFor(invocation).ToArray())
                             : this.Container.ResolveAll(this.concreteType))
                            .ToArray();
                }
                else
                {
                    if (this.name == null)
                    {
                        invocation.ReturnValue = invocation.Arguments.Any()
                                                     ? this.Container.Resolve(this.concreteType, GetResolverOverridesFor(invocation).ToArray())
                                                     : this.Container.Resolve(this.concreteType);
                    }
                    else
                    {
                        invocation.ReturnValue = invocation.Arguments.Any()
                                                     ? this.Container.Resolve(this.concreteType, this.name, GetResolverOverridesFor(invocation).ToArray())
                                                     : this.Container.Resolve(this.concreteType, this.name);
                    }
                }
            }
            catch (ResolutionFailedException resolutionFailedException)
            {
                var innerException = resolutionFailedException.InnerException;
                var invalidOperationException = innerException as InvalidOperationException;

// Check if the resolution failure was due to parameter name mismatches, and if so, report it to the user.
                if (invalidOperationException != null && innerException.Source == "Microsoft.Practices.Unity")
                {
                    var factoryParameterNames = invocation.Method.GetParameters().Select(x => x.Name).ToArray();
                    var nonExistingParamsPerConstructorDictionary = new Dictionary<ConstructorInfo, ParameterInfo[]>();

                    foreach (var constructorInfo in this.concreteType.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
                    {
                        var realConstructorNames = constructorInfo.GetParameters().Select(x => x.Name);

                        var nonExistingParamNames = factoryParameterNames.Except(realConstructorNames.Intersect(factoryParameterNames)).ToArray();
                        if (nonExistingParamNames.Any())
                        {
                            var nonMatchingParameterInfos = invocation.Method.GetParameters().Where(paramInfo => nonExistingParamNames.Contains(paramInfo.Name)).ToArray();

                            nonExistingParamsPerConstructorDictionary.Add(constructorInfo, nonMatchingParameterInfos);
                        }
                    }

                    if (nonExistingParamsPerConstructorDictionary.Any())
                    {
                        var selectedConstructorKvp = (from kvp in nonExistingParamsPerConstructorDictionary
                                                      orderby kvp.Value.Length
                                                      select kvp).FirstOrDefault();

                        var message = string.Format(
                                                    "Resolution failed.\nThere is a mismatch in parameter names between the typed factory interface {0} and {1}'s constructor.\nThe following parameter(s) seem to be missing in the constructor: {2}.",
                                                    invocation.Method.ReflectedType.Name,
                                                    resolutionFailedException.TypeRequested,
                                                    string.Join(", ", selectedConstructorKvp.Value.Select(paramInfo => paramInfo.Name)));

                        throw new ConstructorArgumentsMismatchException(
                            message,
                            invocation.Method.ReflectedType,
                            selectedConstructorKvp.Value,
                            resolutionFailedException);
                    }
                }
                else
                {
                    // If the constructor threw an exception, wrap it in a ObjectConstructionException
                    if (innerException != null)
                    {
                        throw innerException;
                    }
                }

                throw;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds a list consisting of one <see cref="ParameterOverride"/> instance for each argument of the method in <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">
        /// The invocation details.
        /// </param>
        /// <returns>
        /// The collection of <see cref="ParameterOverride"/>s.
        /// </returns>
        private static IEnumerable<ResolverOverride> GetResolverOverridesFor(IInvocation invocation)
        {
            var arguments = invocation.Arguments;
            var parameterInfos = invocation.Method.GetParameters();

            for (var parameterIndex = 0; parameterIndex < arguments.Length; ++parameterIndex)
            {
                var parameterInfo = parameterInfos.ElementAt(parameterIndex);
                var parameterValue = arguments[parameterIndex];

                yield return new ParameterOverride(parameterInfo.Name, new InjectionParameter(parameterInfo.ParameterType, parameterValue));
            }
        }

        #endregion
    }
}