// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityTypedFactoryExtensions.cs" company="Developer In The Flow">
//   © 2012-2014 Pedro Pombeiro
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Unity.TypedFactories
{
    using System;

    using Microsoft.Practices.Unity;

    using Unity.TypedFactories.Implementation;

    /// <summary>
    /// Defines extension methods for providing custom typed factories based on a factory interface.
    /// </summary>
    public static class UnityTypedFactoryExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <param name="factoryContractType">
        /// The factory interface.
        /// </param>
        /// <param name="container">
        /// The Unity container.
        /// </param>
        /// <returns>
        /// The holder object which facilitates the fluent interface.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the <paramref name="factoryContractType"/> does not represent an interface type.
        /// </exception>
        public static ITypedFactoryRegistration RegisterTypedFactory(this IUnityContainer container,
                                                                     Type factoryContractType)
        {
            if (!factoryContractType.IsInterface)
            {
                throw new ArgumentException("The factory contract does not represent an interface!", "factoryContractType");
            }

            var typedFactoryRegistration = new TypedFactoryRegistration(container, factoryContractType);
            return typedFactoryRegistration;
        }

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <typeparam name="TFactory">
        /// The factory interface.
        /// </typeparam>
        /// <param name="container">
        /// The Unity container.
        /// </param>
        /// <returns>
        /// The holder object which facilitates the fluent interface.
        /// </returns>
        public static ITypedFactoryRegistration RegisterTypedFactory<TFactory>(this IUnityContainer container)
            where TFactory : class
        {
            if (!typeof(TFactory).IsInterface)
            {
                throw new ArgumentException("The factory contract does not represent an interface!");
            }

            var typedFactoryRegistration = new TypedFactoryRegistration<TFactory>(container);
            return typedFactoryRegistration;
        }

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <typeparam name="TFactory">
        /// The factory interface.
        /// </typeparam>
        /// <param name="container">
        /// The Unity container.
        /// </param>
        /// <param name="name">
        /// Name that will be used to request the type.
        /// </param>
        /// <returns>
        /// The holder object which facilitates the fluent interface.
        /// </returns>
        public static ITypedFactoryRegistration RegisterTypedFactory<TFactory>(this IUnityContainer container,
                                                                               string name)
            where TFactory : class
        {
            return container.RegisterTypedFactory(typeof(TFactory), name);
        }

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <param name="container">
        /// The Unity container.
        /// </param>
        /// <param name="factoryContractType">
        /// The factory interface.
        /// </param>
        /// <param name="name">
        /// Name that will be used to request the type.
        /// </param>
        /// <returns>
        /// The holder object which facilitates the fluent interface.
        /// </returns>
        public static ITypedFactoryRegistration RegisterTypedFactory(this IUnityContainer container,
                                                                     Type factoryContractType,
                                                                     string name)
        {
            var typedFactoryRegistration = new TypedFactoryRegistration(container, factoryContractType, name);
            return typedFactoryRegistration;
        }

        #endregion
    }
}