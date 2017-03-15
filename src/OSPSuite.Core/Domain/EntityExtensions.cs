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
         string longName = entity.Name;
         var parent = entity.ParentContainer;
         while (parent != null)
         {
            longName = parent.Name + ObjectPath.PATH_DELIMITER + longName;
            parent = parent.ParentContainer;
         }
         return longName;
      }
   }
}