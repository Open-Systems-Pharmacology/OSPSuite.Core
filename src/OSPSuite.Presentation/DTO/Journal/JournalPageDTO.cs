using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.DTO.Journal
{
   public class JournalPageDTO : ValidatableDTO, IViewItem
   {
      public JournalPage JournalPage { get; private set; }
      public bool Edited { get; set; }
      public int UniqueIndex { get; set; }
      public int LineCount { get; set; }

      public JournalPageDTO(JournalPage journalPage)
      {
         JournalPage = journalPage;
      }


      private Origin _origin;
      public Origin Origin
      {
         get { return _origin; }
         set
         {
            _origin = value;
            OnPropertyChanged(() => Origin);
         }
      }

      private string _title;

      public virtual string Title
      {
         get { return _title; }
         set
         {
            _title = value;
            OnPropertyChanged(() => Title);
         }
      }

      public virtual string Caption
      {
         get { return string.Format("{0} - {1}", UniqueIndex, Title); }
      }

      private DateTime _createdAt;

      public virtual DateTime CreatedAt
      {
         get { return _createdAt; }
         set
         {
            _createdAt = value;
            OnPropertyChanged(() => CreatedAt);
         }
      }

      private string _createdAtBy;

      public virtual string CreatedAtBy
      {
         get { return _createdAtBy; }
         set
         {
            _createdAtBy = value;
            OnPropertyChanged(() => CreatedAtBy);
         }
      }

      private string _updatedAtBy;

      public virtual string UpdatedAtBy
      {
         get { return _updatedAtBy; }
         set
         {
            _updatedAtBy = value;
            OnPropertyChanged(() => UpdatedAtBy);
         }
      }

      private string _updatedBy;

      public virtual string UpdatedBy
      {
         get { return _updatedBy; }
         set
         {
            _updatedBy = value;
            OnPropertyChanged(() => UpdatedBy);
         }
      }

      private DateTime _updatedAt;

      public virtual DateTime UpdatedAt
      {
         get { return _updatedAt; }
         set
         {
            _updatedAt = value;
            OnPropertyChanged(() => UpdatedAt);
         }
      }

      private string _description;

      public virtual string Description
      {
         get { return _description; }
         set
         {
            _description = value;
            OnPropertyChanged(() => Description);
         }
      }

      private string _createdBy;

      public virtual string CreatedBy
      {
         get { return _createdBy; }
         set
         {
            _createdBy = value;
            OnPropertyChanged(() => CreatedBy);
         }
      }

      private IEnumerable<IBusinessRule> getTagRules()
      {
         yield return CreateRule.For<string>().Property(s => s.Length).WithRule((s, length) =>
            !string.IsNullOrEmpty(s) && !string.Equals(Separator, s)
            ).WithError(string.Empty);
      }

      private IEnumerable<string> _tags;

      public IEnumerable<string> Tags
      {
         get { return _tags; }
         set
         {
            _tags = value;
            //we should not raise the Tags event has it seems to creates issue with the tag binding
            OnPropertyChanged(() => TagsDisplay);
         }
      }

      public string TagsDisplay
      {
         get { return Tags.ToString(Separator); }
      }

      public IReadOnlyList<RelatedItem> RelatedItems
      {
         get { return JournalPage.RelatedItems; }
      }

      public static string Separator
      {
         get { return ","; }
      }

      public byte[] Data
      {
         get
         {
            // Return empty byte array if page is not loaded. Sometimes Devexpress tries to access the content when binding
            return !JournalPage.IsLoaded ? new byte[]{} :  JournalPage.Content.Data;
         }
         set { JournalPage.Content.Data = value; }
      }

      public bool ValidateTag(string tag)
      {
         return getTagRules().All(rule => rule.IsSatisfiedBy(tag));
      }
   }
}