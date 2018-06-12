using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO.Journal
{
   public class JournalPageDTO : ValidatableDTO, IViewItem
   {
      public JournalPage JournalPage { get; }
      public bool Edited { get; set; }
      public int UniqueIndex { get; set; }
      public int LineCount { get; set; }
      private Origin _origin;
      private string _title;
      private string _createdAtBy;
      private string _updatedAtBy;
      private string _updatedBy;
      private DateTime _updatedAt;
      private string _description;
      private string _createdBy;
      private DateTime _createdAt;

      public JournalPageDTO(JournalPage journalPage)
      {
         JournalPage = journalPage;
      }

      public virtual string Caption => $"{UniqueIndex} - {Title}";

      public Origin Origin
      {
         get => _origin;
         set => SetProperty(ref _origin, value);
      }

      public virtual string Title
      {
         get => _title;
         set => SetProperty(ref _title, value);
      }

      public virtual DateTime CreatedAt
      {
         get => _createdAt;
         set => SetProperty(ref _createdAt, value);
      }

      public virtual string CreatedAtBy
      {
         get => _createdAtBy;
         set => SetProperty(ref _createdAtBy, value);
      }

      public virtual string UpdatedAtBy
      {
         get => _updatedAtBy;
         set => SetProperty(ref _updatedAtBy, value);
      }

      public virtual string UpdatedBy
      {
         get => _updatedBy;
         set => SetProperty(ref _updatedBy, value);
      }

      public virtual DateTime UpdatedAt
      {
         get => _updatedAt;
         set => SetProperty(ref _updatedAt, value);
      }

      public virtual string Description
      {
         get => _description;
         set => SetProperty(ref _description, value);
      }

      public virtual string CreatedBy
      {
         get => _createdBy;
         set => SetProperty(ref _createdBy, value);
      }

      private static IBusinessRule tagNotEmpty { get; } = CreateRule.For<string>()
         .Property(s => s.Length)
         .WithRule((s, length) => !string.IsNullOrEmpty(s) && !string.Equals(Separator, s))
         .WithError(string.Empty);

      private IEnumerable<IBusinessRule> allTagRules { get; } = new[]
      {
         tagNotEmpty
      };

      private IEnumerable<string> _tags;

      public IEnumerable<string> Tags
      {
         get => _tags;
         set
         {
            _tags = value;
            //we should not raise the Tags event has it seems to creates issue with the tag binding
            OnPropertyChanged(() => TagsDisplay);
         }
      }

      public string TagsDisplay => Tags.ToString(Separator);

      public IReadOnlyList<RelatedItem> RelatedItems => JournalPage.RelatedItems;

      public static string Separator => ",";

      public byte[] Data
      {
         // Return empty byte array if page is not loaded. Sometimes Devexpress tries to access the content when binding
         get => !JournalPage.IsLoaded ? new byte[] { } : JournalPage.Content.Data;
         set => JournalPage.Content.Data = value;
      }

      public bool ValidateTag(string tag)
      {
         return allTagRules.All(rule => rule.IsSatisfiedBy(tag));
      }
   }
}