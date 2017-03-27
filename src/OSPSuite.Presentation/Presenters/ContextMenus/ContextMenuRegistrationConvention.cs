using System;
using System.Linq;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Container.Conventions;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class ContextMenuRegistrationConvention : IRegistrationConvention
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