using System;
using System.Linq;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Container.Conventions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core
{
   /// <summary>
   ///    Register a component with all implemented interfaces deriving from <typeparamref name="T"></typeparamref>.
   ///    If at least one implementation is found, the component will also be     registered as itself, using the concreteType
   ///    for resolution. On the other hand, if no implementation if found of <typeparamref name="T"></typeparamref>, the
   ///    registration will default to the default behavior of the <see cref="DefaultRegistrationConvention" /> only if the
   ///    flag <c>registerWithDefaultConvention</c> is set to <c>true</c> (default behavior)
   /// </summary>
   public class RegisterTypeConvention<T> : DefaultRegistrationConvention
   {
      private readonly bool _registerWithDefaultConvention;

      public RegisterTypeConvention() : this(true)
      {
      }

      public RegisterTypeConvention(bool registerWithDefaultConvention)
      {
         _registerWithDefaultConvention = registerWithDefaultConvention;
      }

      public override void Process(Type concreteType, IContainer container, LifeStyle lifeStyle)
      {
         var type = typeof(T);
         if (concreteType.IsAnImplementationOf(type))
         {
            var interfaces = concreteType.GetInterfaces().Where(t => t.IsAnImplementationOf(type)).ToList();
            interfaces.Add(concreteType);
            container.Register(interfaces, concreteType, lifeStyle);
         }
         else if (_registerWithDefaultConvention)
         {
            base.Process(concreteType, container, lifeStyle);
         }
      }
   }
}