namespace OSPSuite.Core.Domain.Descriptors
{
   public class EntityDescriptor
   {
      public Tags Tags { get; set; }
      public Tag Container { get; set; }

      public EntityDescriptor()
      {
      }

      public EntityDescriptor(IEntity entity)
      {
         Tags = new Tags(entity.Tags){new Tag(entity.Name)};
         Container = new Tag(entity.ParentContainer?.Name);
      }
   }
}