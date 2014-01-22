namespace Unity.TypedFactories
{
    /// <summary>
    /// Defines useful extension methods for the <see cref="ITypedFactoryRegistration"/> type.
    /// </summary>
    public static class TypedFactoryRegistrationExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Defines the concrete type which the factory will create.
        /// </summary>
        /// <typeparam name="TTo">
        /// The concrete type which the factory will instantiate.
        /// </typeparam>
        public static void ForConcreteType<TTo>(this ITypedFactoryRegistration registration) where TTo : class
        {
            registration.ForConcreteType(typeof(TTo));
        }

        #endregion
    }
}