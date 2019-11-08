using System;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core
{
   //TODO: Might move to Utility
   public interface IOSPSuiteContainer : IContainer
   {
      void RegisterOpenGeneric(Type serviceType, Type concreteType);
   }
}