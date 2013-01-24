// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityTypedFactoryExtensions.cs" company="Developer In The Flow">
//   © 2012-2013 Pedro Pombeiro
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Unity.TypedFactories
{
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
            var typedFactoryRegistration = new TypedFactoryRegistration<TFactory>(container);
            return typedFactoryRegistration;
        }

        #endregion
    }
}