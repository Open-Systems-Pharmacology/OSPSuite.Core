using System;
using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Container.Conventions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core
{
   /// <summary>
   ///    Registers the components with their default interface according to the following convention
   ///    <c>MyObject</c> will be registered in the component only it implements the interface <c>IMyObject</c>
   ///    In that case, <c>MyObject</c> will be registered with all interfaces <c>IMyObject</c>
   /// </summary>
   public class OSPSuiteRegistrationConvention : IRegistrationConvention
   {
      private readonly bool _registerConcreteType;

      public OSPSuiteRegistrationConvention() : this(false)
      {
      }

      public OSPSuiteRegistrationConvention(bool registerConcreteType)
      {
         _registerConcreteType = registerConcreteType;
      }

      public virtual void Process(Type concreteType, IContainer container, LifeStyle lifeStyle)
      {
         Register(concreteType, container, lifeStyle);
      }

      /// <summary>
      ///    Returns <c>true</c> if the <paramref name="concreteType" /> was registered with at least one interface in the
      ///    <paramref name="container" />
      ///    using this convention otherwise <c>false</c>
      /// </summary>
      protected virtual bool Register(Type concreteType, IContainer container, LifeStyle lifeStyle)
      {
         var interfaceName = "I" + concreteType.Name;

         var serviceTypes = concreteType.GetInterfaces().Where(t => t.Name == interfaceName).ToList();

         if (_registerConcreteType)
            serviceTypes.Add(concreteType);

         if (!serviceTypes.Any())
            return false;

         if (concreteType.IsAnImplementationOf<IStartable>())
            serviceTypes.Add(typeof(IStartable));

         container.Register(serviceTypes, concreteType, lifeStyle);

         return true;
      }
   }
}