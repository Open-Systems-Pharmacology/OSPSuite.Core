using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Journal
{
   public class Journal
   {
      private readonly List<JournalPage> _journalPages;
      private readonly List<JournalDiagram> _journalDiagrams;
      public virtual string FullPath { get; set; }

      /// <summary>
      ///    The <see cref="JournalPage" /> being edited. Null if no item is edited
      /// </summary>
      public virtual JournalPage Edited { get; set; }

      public virtual IReadOnlyList<JournalPage> JournalPages => _journalPages;

      public Journal()
      {
         _journalPages = new List<JournalPage>();
         _journalDiagrams = new List<JournalDiagram>();
      }

      public virtual void AddJournalPage(JournalPage journalPage)
      {
         AddJournalPages(new[] {journalPage});
      }

      public virtual void Remove(JournalPage journalPage)
      {
         if (Equals(Edited, journalPage))
            Edited = null;

         _journalPages.Remove(journalPage);
         ChildrenOf(journalPage).Each(x => x.ParentId = null);
      }

      public virtual void AddJournalPages(IEnumerable<JournalPage> journalPages)
      {
         _journalPages.AddRange(journalPages);
      }

      public virtual JournalPage JournalPageById(string id)
      {
         return _journalPages.FindById(id);
      }

      public virtual JournalPage JournalPageContaining(RelatedItem relatedItem)
      {
         return _journalPages.First(page => page.RelatedItems.Contains(relatedItem));
      }

      public virtual RelatedItem RelatedItemdById(string id)
      {
         return _journalPages.SelectMany(x => x.RelatedItems).FindById(id);
      }

      public virtual JournalPage LastCreatedJournalPage
      {
         get { return _journalPages.OrderByDescending(x => x.UniqueIndex).FirstOrDefault(); }
      }

      /// <summary>
      ///    Returns the parent <see cref="JournalPage" /> of <paramref name="journalPage" /> if defined or null otherwise
      /// </summary>
      public virtual JournalPage ParentOf(JournalPage journalPage)
      {
         if (string.IsNullOrEmpty(journalPage.ParentId))
            return null;

         return JournalPageById(journalPage.ParentId);
      }

      /// <summary>
      ///    Returns the children <see cref="JournalPage" /> of <paramref name="journalPage" />
      /// </summary>
      public virtual IReadOnlyList<JournalPage> ChildrenOf(JournalPage journalPage)
      {
         return _journalPages.Where(x => string.Equals(journalPage.Id, x.ParentId)).ToList();
      }

      public virtual void AddDiagrams(IEnumerable<JournalDiagram> diagrams)
      {
         diagrams.Each(AddDiagram);
      }

      public virtual void AddDiagram(JournalDiagram diagram)
      {
         _journalDiagrams.Add(diagram);
         diagram.Journal = this;
      }

      public JournalDiagram Diagram => _journalDiagrams.FindByName(Captions.Journal.DefaultDiagramName);

      public virtual IReadOnlyList<JournalDiagram> Diagrams => _journalDiagrams;
   }
}