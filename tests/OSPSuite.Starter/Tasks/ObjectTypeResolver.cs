using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Starter.Tasks
{
   public class ObjectTypeResolver : IObjectTypeResolver
   {
      public string TypeFor<T>(T objectRequiringType) where T : class
      {
         return objectRequiringType.GetType().Name;
      }

      public string TypeFor<T>()
      {
         return typeof (T).Name;
      }
   }
}