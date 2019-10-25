using OSPSuite.Core.Extensions;

namespace OSPSuite.Infrastructure.Serialization.Journal.Queries
{
   public class NextAvailableJournalPageIndex : IQuery<int>
   {
   }

   public class NextAvailableJournalPageIndexQuery : JournalDatabaseQuery<NextAvailableJournalPageIndex, int>
   {
      public NextAvailableJournalPageIndexQuery(IJournalSession journalSession) : base(journalSession)
      {
      }

      public override int Query(NextAvailableJournalPageIndex query)
      {
         var sql = $"SELECT MAX(UniqueIndex) as UniqueIndex FROM {Db.JournalPages.TableName}";
         var dataRow = Db.Connection.ExecuteQueryForSingleRow(sql);
         return dataRow.ValueAt<int>("UniqueIndex") + 1;
      }
   }
}