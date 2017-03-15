using System;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Container.Conventions;

namespace OSPSuite.Presentation.UICommands
{
   /// <summary>
   ///    Registers the components as itself.
   ///    <c>MyObject</c> will be registered as <c>MyObject</c>
   /// </summary>
   public class ConcreteTypeRegistrationConvention : IRegistrationConvention
   {
      public void Process(Type concreteType, IContainer container, LifeStyle lifeStyle)
      {
         container.Register(concreteType, concreteType, lifeStyle);
      }
   }
}