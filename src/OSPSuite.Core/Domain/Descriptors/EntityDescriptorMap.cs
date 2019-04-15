namespace OSPSuite.Core.Domain.Descriptors
{
   public class EntityDescriptorMap<T> where T : IEntity
   {
      public T Entity { get; }

      public EntityDescriptor Descriptor { get; }

      public EntityDescriptorMap(T entity)
      {
         Entity = entity;
         Descriptor = new EntityDescriptor(entity);
      }
   }
}