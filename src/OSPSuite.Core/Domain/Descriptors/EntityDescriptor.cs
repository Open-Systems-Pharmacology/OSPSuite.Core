using System;

namespace OSPSuite.Core.Domain.Descriptors
{
   public class EntityDescriptor
   {
      public Tags Tags { get; }

      //Ensure that we only compute the parent descriptor tags when required
      private readonly Lazy<EntityDescriptor> _lazyParentDescriptor;
      private Tags _parentContainerTags;

      public EntityDescriptor(IEntity entity) : this(entity, createDescriptor)
      {
      }

      internal EntityDescriptor(IEntity entity, Func<IEntity, EntityDescriptor> createDescriptor)
      {
         Tags = new Tags(entity.Tags) {new Tag(entity.Name)};
         _lazyParentDescriptor = new Lazy<EntityDescriptor>(() => createDescriptor(entity.ParentContainer));
      }

      private static EntityDescriptor createDescriptor(IEntity entity) => entity == null ? null : new EntityDescriptor(entity);

      public Tags ParentContainerTags
      {
         get
         {
            if (_parentContainerTags != null)
               return _parentContainerTags;

            _parentContainerTags = new Tags();

            var parentDescriptor = _lazyParentDescriptor.Value;
            if (parentDescriptor != null)
            {
               _parentContainerTags.Add(parentDescriptor.Tags);
               _parentContainerTags.Add(parentDescriptor.ParentContainerTags);
            }

            return _parentContainerTags;
         }
      }
   }
}