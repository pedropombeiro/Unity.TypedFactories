// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityTypedFactoryTests.cs" company="Pedro Pombeiro">
//   © 2012 Pedro Pombeiro
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Unity.TypedFactories.Tests
// ReSharper disable InconsistentNaming
{
    using System;

    using JetBrains.Annotations;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    [TestFixture]
    public class UnityTypedFactoryTests
    {
        #region Interfaces

        public interface ISomeInstance
        {
        }

        public interface ISomeInstanceFactory
        {
            #region Public Methods and Operators

            ISomeInstance Create();

            #endregion
        }

        public interface ISomeService
        {
        }

        /// <summary>
        /// The Test interface for one parameter.
        /// </summary>
        public interface ITest1
        {
            #region Public Properties

            string TestProperty1 { get; }

            #endregion
        }

        public interface ITest1Factory
        {
            #region Public Methods and Operators

            ITest1 Create(string textParam);

            #endregion
        }

        /// <summary>
        /// The Test interface for three parameters.
        /// </summary>
        public interface ITest2
        {
            #region Public Properties

            ISomeService InjectedService { get; }

            string TestProperty1 { get; }

            ISomeInstance TestProperty2 { get; }

            string TestProperty3 { get; }

            #endregion
        }

        public interface ITest2Factory
        {
            #region Public Methods and Operators

            ITest2 Create(string testProperty1, ISomeInstance someInstance, string testProperty3);

            #endregion
        }

        #endregion

        #region Public Methods and Operators

        [Test]
        public void given_instantiated_Sut_when_Create_is_called_with_one_parameter_then_TestProperty1_on_resulting_TestClass_matches_specified_value()
        {
            // Arrange
            var unityContainer = new UnityContainer();

            unityContainer
                .RegisterTypedFactory<ITest1Factory>()
                .ForConcreteType<TestClass1>();

            const string TestValue = "TestValue";

            // Act
            var factory = unityContainer.Resolve<ITest1Factory>();
            var testClass = factory.Create(TestValue);

            // Assert
            Assert.AreEqual(TestValue, testClass.TestProperty1);
        }

        [Test]
        [ExpectedException(typeof(ConstructorArgumentsMismatchException))]
        public void given_instantiated_Sut_when_Create_is_called_with_one_parameter_with_non_matching_parameter_name_then_ConstructorArgumentsMismatchException_is_thrown()
        {
            // Arrange
            var unityContainer = new UnityContainer();

            unityContainer
                .RegisterTypedFactory<ITest2Factory>()
                .ForConcreteType<TestClass2NonMatchingName>();

            const string TestValue = "TestValue";
            ISomeInstance someInstance = new SomeInstance();

            // Act
            var factory = unityContainer.Resolve<ITest2Factory>();
            factory.Create(TestValue, someInstance, string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void given_instantiated_Sut_when_Create_is_called_with_two_parameters_and_parameter_1_is_null_then_ArgumentNullException_is_thrown()
        {
            // Arrange
            using (var unityContainer = new UnityContainer())
            {
                unityContainer
                    .RegisterTypedFactory<ITest2Factory>()
                    .ForConcreteType<TestClass2>();

                const string TestValue = null;
                ISomeInstance someInstance = new SomeInstance();

                unityContainer.RegisterType<ISomeService, SomeService>(new ContainerControlledLifetimeManager());

                // Act
                var factory = unityContainer.Resolve<ITest2Factory>();
                factory.Create(TestValue, someInstance, string.Empty);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void given_instantiated_Sut_when_Create_is_called_with_two_parameters_and_parameter_2_is_null_then_ArgumentNullException_is_thrown()
        {
            // Arrange
            using (var unityContainer = new UnityContainer())
            {
                unityContainer
                    .RegisterTypedFactory<ITest2Factory>()
                    .ForConcreteType<TestClass2>();

                const string TestValue = "TestValue";
                ISomeInstance someInstance = null;

                unityContainer.RegisterType<ISomeService, SomeService>(new ContainerControlledLifetimeManager());

                // Act
                var factory = unityContainer.Resolve<ITest2Factory>();
                factory.Create(string.Empty, someInstance, TestValue);
            }
        }

        [Test]
        public void given_instantiated_Sut_when_Create_is_called_with_two_parameters_then_TestProperty1_2_and_3_on_resulting_TestClass_matches_specified_values()
        {
            // Arrange
            using (var unityContainer = new UnityContainer())
            {
                unityContainer
                    .RegisterTypedFactory<ITest2Factory>()
                    .ForConcreteType<TestClass2>();

                const string TestValue = "TestValue";
                ISomeInstance someInstance = new SomeInstance();

                unityContainer.RegisterType<ISomeService, SomeService>(new ContainerControlledLifetimeManager());

                // Act
                var factory = unityContainer.Resolve<ITest2Factory>();
                var testClass = factory.Create(TestValue, someInstance, string.Empty);

                // Assert
                Assert.AreEqual(TestValue, testClass.TestProperty1);
                Assert.AreEqual(someInstance, testClass.TestProperty2);
                Assert.AreEqual(unityContainer.Resolve<ISomeService>(), testClass.InjectedService);
            }
        }

        [Test]
        public void given_instantiated_Sut_when_Create_is_called_without_parameters_then_valid_instance_is_returned()
        {
            // Arrange
            var unityContainer = new UnityContainer();

            unityContainer
                .RegisterTypedFactory<ISomeInstanceFactory>()
                .ForConcreteType<SomeInstance>();

            // Act
            var factory = unityContainer.Resolve<ISomeInstanceFactory>();
            var testClass = factory.Create();

            // Assert
            Assert.IsInstanceOf<SomeInstance>(testClass);
        }

        #endregion

        [UsedImplicitly]
        private class SomeInstance : ISomeInstance
        {
        }

        [UsedImplicitly]
        private class SomeInstanceFactory : ISomeInstanceFactory
        {
            #region Public Methods and Operators

            public ISomeInstance Create()
            {
                throw new System.NotImplementedException();
            }

            #endregion
        }

        [UsedImplicitly]
        private class SomeService : ISomeService
        {
        }

        [UsedImplicitly]
        private class TestClass1 : ITest1
        {
            #region Constructors and Destructors

            public TestClass1(string textParam)
            {
                this.TestProperty1 = textParam;
            }

            #endregion

            #region Public Properties

            public string TestProperty1 { get; private set; }

            #endregion
        }

        [UsedImplicitly]
        private class TestClass2 : ITest2
        {
            #region Constructors and Destructors

            public TestClass2(ISomeInstance someInstance, string testProperty1, ISomeService someService, string testProperty3)
            {
                this.InjectedService = someService;
                this.TestProperty1 = testProperty1;
                this.TestProperty2 = someInstance;
                this.TestProperty3 = testProperty3;
            }

            #endregion

            #region Public Properties

            public ISomeService InjectedService { get; private set; }

            public string TestProperty1 { get; private set; }

            public ISomeInstance TestProperty2 { get; private set; }

            public string TestProperty3 { get; private set; }

            #endregion
        }

        [UsedImplicitly]
        private class TestClass2NonMatchingName : ITest2
        {
            #region Constructors and Destructors

            public TestClass2NonMatchingName(ISomeInstance someInstance, string nonMatchingTestProperty1, ISomeService someService, string nonMatchingTestProperty3)
            {
                this.InjectedService = someService;
                this.TestProperty1 = nonMatchingTestProperty1;
                this.TestProperty2 = someInstance;
                this.TestProperty3 = nonMatchingTestProperty3;
            }

            #endregion

            #region Public Properties

            public ISomeService InjectedService { get; private set; }

            public string TestProperty1 { get; private set; }

            public ISomeInstance TestProperty2 { get; private set; }

            public string TestProperty3 { get; private set; }

            #endregion
        }
    }
}