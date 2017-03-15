using OSPSuite.Core.Journal;
using OSPSuite.Infrastructure.Serialization;

namespace OSPSuite.Infrastructure.Journal
{
   public class JournalDatabase : Database<JournalDatabase>
   {
      public Table<JournalPage> JournalPages { get; private set; }
      public Table<Content> Contents { get; private set; }
      public Table<RelatedItem> RelatedItems { get; private set; }
      public Table<Tag> Tags { get; private set; }
      public Table<JournalPageTag> JournalPageTags { get; private set; }
      public Table<JournalDiagram> Diagrams { get; private set; }

      public JournalDatabase()
      {
         JournalPages = new Table<JournalPage>(this, "JOURNAL_PAGES");
         Contents = new Table<Content>(this, "CONTENTS");
         RelatedItems = new Table<RelatedItem>(this, "RELATED_ITEMS");
         Tags = new Table<Tag>(this, "TAGS");
         JournalPageTags = new Table<JournalPageTag>(this, "JOURNAL_PAGE_TAGS");
         Diagrams = new Table<JournalDiagram>(this, "JOURNAL_DIAGRAMS");
      }

      public void Create()
      {
         DoInTransaction(() => Execute(databaseSchema));
      }

      private string databaseSchema => @"
               CREATE TABLE JOURNAL_PAGES (
                     Id TEXT not null PRIMARY KEY,
                     UniqueIndex INT not null CONSTRAINT [JournalPageUniqueIndexConstraint] UNIQUE ON CONFLICT ROLLBACK,
                     OriginId INT not null,
                     Title TEXT not null,
                     Description TEXT,
                     CreatedBy TEXT not null,
                     CreatedAt DATETIME not null,
                     UpdatedBy TEXT not null,
                     UpdatedAt DATETIME not null,
                     FullText TEXT,
                     ContentId TEXT,
                     ParentId TEXT,
                     constraint fk_Parent_JournalPage foreign key (ParentId) references JOURNAL_PAGES(Id) ON DELETE SET NULL,
                     constraint fk_JournalPage_Content foreign key (ContentId) references CONTENTS(Id)
               );

               CREATE TABLE CONTENTS (
                     Id TEXT not null PRIMARY KEY,
                     Data IMAGE
               );
         
               CREATE TABLE TAGS (
                     Id TEXT not null PRIMARY KEY
               );

               CREATE TABLE JOURNAL_PAGE_TAGS (
                     JournalPageId TEXT not null,
                     TagId TEXT not null,
                     PRIMARY KEY (JournalPageId, TagId),
                     constraint fk_Tagging_JournalPage foreign key (JournalPageId) references JOURNAL_PAGES(Id) ON DELETE CASCADE ON UPDATE CASCADE,
                     constraint fk_Tagging_Tag foreign key (TagId) references TAGS(Id) ON DELETE CASCADE ON UPDATE CASCADE
               );

               CREATE TABLE RELATED_ITEMS (
                     Id TEXT not null PRIMARY KEY,
                     JournalPageId TEXT not null,
                     ItemType TEXT not null,
                     Discriminator TEXT not null,
                     Name TEXT not null,
                     CreatedAt DATETIME not null,
                     Description TEXT,
                     IconName TEXT,
                     OriginId INT not null,
                     Version TEXT not null,
                     FullPath TEXT,
                     ContentId TEXT,
                     constraint fk_RelatedItem_JournalPage foreign key (JournalPageId) references JOURNAL_PAGES(Id) ON DELETE CASCADE ON UPDATE CASCADE,
                     constraint fk_RelatedItem_Content foreign key (ContentId) references CONTENTS(Id)
               );

               CREATE TABLE JOURNAL_DIAGRAMS (
                     Id TEXT not null PRIMARY KEY,
                     Name TEXT not null,
                     Data IMAGE
               );

            ";
   }
}