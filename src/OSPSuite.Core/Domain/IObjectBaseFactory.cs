using System;

namespace OSPSuite.Core.Domain
{
   public interface IObjectBaseFactory
   {
      T Create<T>() where T : class, IObjectBase;
      T Create<T>(string id) where T : class, IObjectBase;

      T CreateObjectBaseFrom<T>(Type objectType);
      T CreateObjectBaseFrom<T>(Type objectType, string id);
      T CreateObjectBaseFrom<T>(T sourceObject);
   }
}