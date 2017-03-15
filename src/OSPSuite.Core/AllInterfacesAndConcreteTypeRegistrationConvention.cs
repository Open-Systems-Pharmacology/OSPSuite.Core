using System;
using System.Linq;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Container.Conventions;

namespace OSPSuite.Core
{
   /// <summary>
   ///    Registers a component with ALL implemented interfaces. The component will also be registered as is.
   /// </summary>
   public class AllInterfacesAndConcreteTypeRegistrationConvention : IRegistrationConvention
   {
      public void Process(Type concreteType, IContainer container, LifeStyle lifeStyle)
      {
         var allInterfaces = concreteType.GetInterfaces()
            .Where(x => x.IsInterface)
            .ToList();

         allInterfaces.Add(concreteType);
         container.Register(allInterfaces, concreteType, lifeStyle);
      }
   }
}