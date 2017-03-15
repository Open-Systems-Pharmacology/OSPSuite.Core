using System;

namespace OSPSuite.Core.Domain
{
   public interface IObjectBaseFactory
   {
      T Create<T>() where T : IObjectBase;
      T Create<T>(string id) where T : IObjectBase;

      T CreateObjectBaseFrom<T>(Type objectType);
      T CreateObjectBaseFrom<T>(Type objectType, string id);
      T CreateObjectBaseFrom<T>(T sourceObject);
   }
}