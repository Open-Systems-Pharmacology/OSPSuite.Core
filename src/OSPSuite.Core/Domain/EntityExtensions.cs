using System.Collections.Generic;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public static class EntityExtensions
   {
      public static T WithParentContainer<T>(this T entity, IContainer parent) where T : IEntity
      {
         parent?.Add(entity);
         return entity;
      }

      public static string EntityPath(this IEntity entity)
      {
         var list = new List<string> {entity.Name};
         var parent = entity.ParentContainer;
         while (parent != null)
         {
            list.Insert(0, parent.Name);
            parent = parent.ParentContainer;
         }

         return list.ToPathString();
      }
   }
}