using System.Collections.Generic;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public static class EntityExtensions
   {
      public static T WithParentContainer<T>(this T entity, IContainer parent) where T : IEntity
      {
         parent?.Add(entity);
         return entity;
      }

      public static T Under<T>(this T entity, IContainer parent) where T : IEntity => WithParentContainer(entity, parent);

      public static string EntityPath(this IEntity entity)
      {
         return entityPathAsListFor(entity).ToPathString();
      }

      public static string ConsolidatedPath(this IEntity entity)
      {
         var path = entityPathAsListFor(entity);
         if (path.Count > 1)
            path.RemoveAt(0);
         return path.ToPathString();
      }

      private static List<string> entityPathAsListFor(IEntity entity)
      {
         var list = new List<string> {entity.Name};
         var parent = entity.ParentContainer;
         while (parent != null)
         {
            list.Insert(0, parent.Name);
            parent = parent.IsAnImplementationOf<IRootContainer>() ? null : parent.ParentContainer;
         }

         return list;
      }
   }
}