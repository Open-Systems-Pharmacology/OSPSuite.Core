using System;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Domain
{
   public interface IEntity : IObjectBase, IValidatable
   {
      IContainer ParentContainer { get; set; }
      Tags Tags { get; }
      void AddTag(Tag tag);
      IContainer RootContainer { get; }
      void RemoveTag(Tag tag);
      void RemoveTag(string tag);
      void AddTag(string tagValue);

      /// <summary>
      ///    Returns true if the current entity is a descendant from the given container otherwise false
      /// </summary>
      /// <remarks>This is not necessarily the direct parent. </remarks>
      bool IsDescendant(IContainer container);

      /// <summary>
      ///    Returns <c>true</c> if the current entity has an ancestor in its hierarchy named <paramref name="parentName" />
      ///    otherwise
      ///    <c>false</c>
      /// </summary>
      /// <remarks>This is not necessarily the direct parent. </remarks>
      bool HasAncestorNamed(string parentName);

      /// <summary>
      ///    Returns <c>true</c> of the current entity has an ancestor in its hierarchy fulfilling the
      ///    <paramref name="criteria" /> otherwise <c>false</c>
      /// </summary>
      /// <remarks>This is not necessarily the direct parent. </remarks>
      bool HasAncestorWith(Func<IContainer, bool> criteria);
   }

   public abstract class Entity : ObjectBase, IEntity
   {
      public IContainer ParentContainer { get; set; }

      protected Entity()
      {
         ParentContainer = null;
         Tags = new Tags();
         Rules = DefaultRules();
      }

      protected virtual IBusinessRuleSet DefaultRules()
      {
         return new BusinessRuleSet(EntityRules.All());
      }

      public Tags Tags { get; }

      public void AddTag(Tag tag) => Tags.Add(tag);

      public void RemoveTag(Tag tag) => Tags.Remove(tag);

      public void RemoveTag(string tag) => Tags.Remove(tag);

      public void AddTag(string tagValue) => AddTag(new Tag(tagValue));

      public bool HasAncestorNamed(string parentName) => HasAncestorWith(x => string.Equals(x.Name, parentName));

      public bool HasAncestorWith(Func<IContainer, bool> criteria)
      {
         if (ParentContainer == null) return false;
         if (criteria(ParentContainer))
            return true;

         return ParentContainer.HasAncestorWith(criteria);
      }

      public IContainer RootContainer
      {
         get
         {
            if (ParentContainer == null)
               return this as IContainer;

            if (this.IsAnImplementationOf<IRootContainer>())
               return this as IContainer;

            return ParentContainer.RootContainer;
         }
      }

      public virtual bool IsDescendant(IContainer container)
      {
         if (ParentContainer == null) return false;
         if (ReferenceEquals(ParentContainer, this)) return false;
         if (ReferenceEquals(ParentContainer, container)) return true;
         return ParentContainer.IsDescendant(container);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceEntity = source as IEntity;
         sourceEntity?.Tags.Each(t => AddTag(t.Value));
      }

      public virtual IBusinessRuleSet Rules { get; }
   }
}