using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using OSPSuite.Core;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using IContainer = Autofac.IContainer;
using IoCContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Infrastructure.Container.Autofac
{
   public class AutofacContainer : IOSPSuiteContainer
   {
      public AutofacContainer() : this(new ContainerBuilder())
      {
      }

      public AutofacContainer(ContainerBuilder containerBuilder)
      {
         AutofacBuilder = containerBuilder;
      }

      public ContainerBuilder AutofacBuilder { get; }

      public IContainer Container { get; private set; }

      public void Build()
      {
         Container = AutofacBuilder.Build();
      }

      public void Dispose()
      {
         throw new NotImplementedException();
      }

      public TInterface Resolve<TInterface>() => Container.Resolve<TInterface>();

      public TInterface Resolve<TInterface>(string key) => Container.ResolveKeyed<TInterface>(key);

      public object Resolve(Type type) => Container.Resolve(type);

      public TInterface Resolve<TInterface>(Type type) where TInterface : class
      {
         return Resolve(type) as TInterface;
      }

      public void Register<TInterface, TImplementation>()
      {
         Register<TInterface, TImplementation>(LifeStyle.Transient);
      }

      public void Register<TInterface, TImplementation>(string key)
      {
         Register<TInterface, TImplementation>(LifeStyle.Transient, key);
      }

      public void Register<TInterface, TImplementation>(LifeStyle lifeStyle, string key)
      {
         register(new[] {typeof(TInterface)}, typeof(TImplementation), lifeStyle, key);
      }

      public void Register(Type serviceType, Type concreteType)
      {
         if (serviceType.IsGenericTypeDefinition && concreteType.IsGenericTypeDefinition)
            RegisterOpenGeneric(serviceType, concreteType);
         else
            Register(serviceType, concreteType, LifeStyle.Transient);
      }

      public void Register(Type serviceType, Type concreteType, LifeStyle lifeStyle)
      {
         Register(new[] {serviceType}, concreteType, lifeStyle);
      }

      public void Register(Type serviceType, Type concreteType, LifeStyle lifeStyle, string key)
      {
         register(new[] {serviceType}, concreteType, lifeStyle, key);
      }

      public IEnumerable<TInterface> ResolveAll<TInterface>() => Container.Resolve<IEnumerable<TInterface>>();

      public void Register<TService1, TConcreteType>(LifeStyle lifeStyle)
      {
         Register(new[] {typeof(TService1)}, typeof(TConcreteType), lifeStyle);
      }

      public void Register<TService1, TService2, TConcreteType>(LifeStyle lifeStyle)
      {
         Register(new[] {typeof(TService1), typeof(TService2)}, typeof(TConcreteType), lifeStyle);
      }

      public void Register<TService1, TService2, TService3, TConcreteType>(LifeStyle lifeStyle)
      {
         Register(new[] {typeof(TService1), typeof(TService2), typeof(TService3)}, typeof(TConcreteType), lifeStyle);
      }

      public void Register<TService1, TService2, TService3, TService4, TConcreteType>(LifeStyle lifeStyle)
      {
         Register(new[] {typeof(TService1), typeof(TService2), typeof(TService3), typeof(TService4)}, typeof(TConcreteType), lifeStyle);
      }

      public void RegisterImplementationOf<TInterface>(TInterface component) where TInterface : class
      {
         AutofacBuilder.RegisterInstance(component).ExternallyOwned();
      }

      public void Register(IReadOnlyCollection<Type> serviceTypes, Type concreteType, LifeStyle lifeStyle)
      {
         register(serviceTypes, concreteType, lifeStyle);
      }

      public void RegisterImplementationOf<TInterface>(TInterface component, string key) where TInterface : class
      {
         AutofacBuilder.RegisterInstance(component).Keyed<TInterface>(key).ExternallyOwned();
      }

      public void RegisterOpenGeneric(Type serviceType, Type concreteType)
      {
         AutofacBuilder.RegisterGeneric(concreteType).As(serviceType).ExternallyOwned();
      }

      private void register(IReadOnlyCollection<Type> serviceTypes, Type concreteType, LifeStyle lifeStyle, string key = null)
      {
         var builder = AutofacBuilder
            .RegisterType(concreteType)
            .As(serviceTypes.ToArray())
            .ExternallyOwned();

         if (!string.IsNullOrEmpty(key))
            builder.Keyed(key, concreteType);

         if (lifeStyle == LifeStyle.Singleton)
            builder.SingleInstance();
      }

      public void RegisterFactory<TFactory>() where TFactory : class
      {
      }

      public IDisposable OptimizeDependencyResolution()
      {
         throw new NotImplementedException();
      }
   }
}