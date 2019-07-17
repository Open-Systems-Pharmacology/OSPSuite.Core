using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Journal
{
   public abstract class JournalEntity : IWithId
   {
      public virtual string Id { get; set; }

      /// <summary>
      ///    Returns <c>true</c> if the object has not been persisted yet
      /// </summary>
      public bool IsTransient => string.IsNullOrEmpty(Id);
   }

   public abstract class ItemWithContent : JournalEntity, IWithDescription
   {
      private Content _content;
      private DateTime _createdAt;
      public virtual string Description { get; set; }
      public virtual string ContentId { get; set; }
      public virtual OriginId OriginId { get; set; }

      public virtual Origin Origin
      {
         get => Origins.ById(OriginId);
         set => OriginId = value.Id;
      }

      public virtual DateTime CreatedAt
      {
         get => _createdAt;
         set => _createdAt = value.ToUniversalTime();
      }

      protected ItemWithContent()
      {
         OriginId = OriginId.Other;
      }

      public virtual Content Content
      {
         get => _content;
         set
         {
            _content = value;
            if (_content != null)
               ContentId = _content.Id;
         }
      }

      public bool IsLoaded => Content != null;
   }

   public class Content : JournalEntity
   {
      public virtual byte[] Data { get; set; }
   }

   public class Tag : IWithId
   {
      public virtual string Id { get; set; }
   }

   public class JournalPageTag
   {
      public virtual string JournalPageId { get; set; }
      public virtual string TagId { get; set; }
   }

   public class RelatedItem : ItemWithContent, IWithName
   {
      public string Name { get; set; }
      public string Version { get; set; }
      public string ItemType { get; set; }
      public string IconName { get; set; }
      public string FullPath { get; set; }
      public string Discriminator { get; set; }

      public override string ToString()
      {
         return Display;
      }

      public string Display => IsFile ? 
         $"{ItemType}: {FullPath}" : 
         $"{ItemType}: {Name} from {Origin.DisplayName} version {Version}";

      public bool IsFile => string.Equals(ItemType, Constants.RELATIVE_ITEM_FILE_ITEM_TYPE);
   }

   public class JournalPage : ItemWithContent
   {
      private readonly HashSet<string> _tags;
      private readonly List<RelatedItem> _relatedItems;
      public virtual string Title { get; set; }
      public virtual int UniqueIndex { get; set; }

      private DateTime _updatedAt;

      public virtual string CreatedBy { get; set; }
      public virtual string UpdatedBy { get; set; }
      public virtual string ParentId { get; set; }
      public virtual string FullText { get; set; }

      public JournalPage()
      {
         _tags = new HashSet<string>();
         _relatedItems = new List<RelatedItem>();
      }

      public virtual IEnumerable<string> Tags => _tags;

      public virtual IReadOnlyList<RelatedItem> RelatedItems => _relatedItems;

      public virtual DateTime UpdatedAt
      {
         get => _updatedAt;
         set => _updatedAt = value.ToUniversalTime();
      }

      public void AddTag(string tag)
      {
         if (string.IsNullOrEmpty(tag))
            return;

         _tags.Add(tag);
      }

      public void AddRelatedItem(RelatedItem relatedItem)
      {
         _relatedItems.Add(relatedItem);
      }

      public void RemoveRelatedItem(RelatedItem relatedItem)
      {
         _relatedItems.Remove(relatedItem);
      }

      /// <summary>
      ///    Updates the Updated At and Updated By property with the current DateTime and UserName
      /// </summary>
      public void RunUpdate()
      {
         UpdatedAt = SystemTime.UtcNow();
         UpdatedBy = EnvironmentHelper.UserName();
      }

      public void UpdateTags(IEnumerable<string> tags)
      {
         if (ReferenceEquals(tags, _tags))
            return;

         _tags.Clear();
         tags.Each(AddTag);
      }

      public void AddTags(IEnumerable<Tag> tags)
      {
         UpdateTags(tags.Select(x => x.Id));
      }

      public void UpdateRelatedData(JournalPageData data)
      {
         AddTags(data.Tags);
         _relatedItems.Clear();
         data.RelatedItems.Each(AddRelatedItem);
      }
   }
}