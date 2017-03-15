using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Container.Castle
{
   public class CastleWindsorContainer : IContainer
   {
      private readonly IWindsorLifeStyleMapper _lifeStyleMapper;
      private bool _disposed;

      public CastleWindsorContainer() : this(new WindsorContainer())
      {
      }

      public CastleWindsorContainer(IWindsorContainer windsorContainer) : this(windsorContainer, new WindsorLifeStyleMapper())
      {
      }

      public CastleWindsorContainer(IWindsorContainer windsorContainer, IWindsorLifeStyleMapper lifeStyleMapper, bool enablePropertiesInjection = false)
      {
         WindsorContainer = windsorContainer;
         _lifeStyleMapper = lifeStyleMapper;
         WindsorContainer.Kernel.ReleasePolicy = new NoTrackingReleasePolicy();
         this.enablePropertiesInjection = enablePropertiesInjection;
      }

      public IWindsorContainer WindsorContainer { get; }

      public TInterface Resolve<TInterface>()
      {
         try
         {
            return WindsorContainer.Resolve<TInterface>();
         }

         catch (ComponentNotFoundException)
         {
            var allImplentations = WindsorContainer.ResolveAll<TInterface>();
            if (allImplentations != null && allImplentations.GetLength(0) > 0)
               return allImplentations[0];

            //not found rethrow
            throw;
         }
      }

      public TInterface Resolve<TInterface>(string key)
      {
         return WindsorContainer.Resolve<TInterface>(key);
      }

      public object Resolve(Type type)
      {
         return WindsorContainer.Resolve(type);
      }

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

      public void Register(IReadOnlyCollection<Type> serviceTypes, Type concreteType, LifeStyle lifeStyle)
      {
         var key = $"{serviceTypes.Select(x => x.FullName).ToString("-")}-{concreteType.FullName}";
         register(serviceTypes, concreteType, lifeStyle, key);
      }

      private void register(IReadOnlyCollection<Type> serviceTypes, Type concreteType, LifeStyle lifeStyle, string key)
      {
         WindsorContainer.Register(Component.For(serviceTypes)
            .ImplementedBy(concreteType)
            .LifeStyle.Is(_lifeStyleMapper.MapFrom(lifeStyle))
            .Named(key));
      }

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

      public void RegisterFactory<TFactory>() where TFactory : class
      {
         WindsorContainer.Register(Component.For<TFactory>().AsFactory());
      }

      private bool enablePropertiesInjection
      {
         set
         {
            //Enable by default
            if (value)
               return;

            //retrieve the property inspector 
            var propertiesInspector = WindsorContainer.Kernel?.ComponentModelBuilder.Contributors.FirstOrDefault(c => c.IsAnImplementationOf<PropertiesDependenciesModelInspector>());

            //does not exist and we want to remove contributor, nthg to do
            if (propertiesInspector == null)
               return;

            WindsorContainer.Kernel.ComponentModelBuilder.RemoveContributor(propertiesInspector);
         }
      }

      public IDisposable OptimizeDependencyResolution()
      {
         return WindsorContainer.Kernel.DowncastTo<DefaultKernel>().OptimizeDependencyResolution();
      }

      public void RegisterImplementationOf<TInterface>(TInterface component) where TInterface : class
      {
         RegisterImplementationOf(component, typeof(TInterface).FullName);
      }

      public void RegisterImplementationOf<TInterface>(TInterface component, string key) where TInterface : class
      {
         WindsorContainer.Register(Component.For<TInterface>().Instance(component).Named(key));
      }

      public IEnumerable<TInterface> ResolveAll<TInterface>()
      {
         return WindsorContainer.ResolveAll<TInterface>();
      }

      public void Dispose()
      {
         if (_disposed) return;
         _disposed = true;
         Cleanup();
         GC.SuppressFinalize(this);
      }

      protected virtual void Cleanup()
      {
         WindsorContainer.Dispose();
      }

      ~CastleWindsorContainer()
      {
         Cleanup();
      }

      public class NoTrackingReleasePolicy : IReleasePolicy
      {
         public void Dispose()
         {
         }

         public IReleasePolicy CreateSubPolicy()
         {
            return this;
         }

         public bool HasTrack(object instance)
         {
            return false;
         }

         public void Release(object instance)
         {
         }

         public void Track(object instance, Burden burden)
         {
         }
      }
   }
}