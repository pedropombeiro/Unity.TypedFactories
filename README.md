Unity.TypedFactories
====================

This project provides automatic Automatic Factory functionality similar to [Castle.Windsor Typed Factories](http://docs.castleproject.org/Default.aspx?Page=Typed-Factory-Facility&NS=Windsor&AspxAutoDetectCookieSupport=1), for the [Unity](http://unity.codeplex.com/) IoC container.

Usage
-----

The library takes as an input an interface to be used as the factory interface.

It generates a proxy object upon resolution of this factory interface, with an implementation for each method of the interface that returns a reference object.

The factory interface is configured for a given concrete type, and the factory method's arguments are matched by name to the created object's constructor arguments.

	unityContainer
		.RegisterTypedFactory<IFooFactory>()
		.ForConcreteType<Foo>();	// It is assumed that Foo implements the interface returned by the method declared in the IFooFactory. A NotImplementedException will be thrown upon invocation of the factory method otherwise.

	public interface IFooFactory
	{
		#region Public Methods and Operators

		IFoo Create(uint id);

		#endregion
	}

In case an argument from the constructor is not provided by either the factory method or the IoC container, then a ConstructorArgumentsMismatchException exception will be thrown, detailing the missing arguments.