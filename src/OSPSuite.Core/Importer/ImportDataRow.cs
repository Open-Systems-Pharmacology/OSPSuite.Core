using System.Data;

namespace OSPSuite.Core.Importer
{
   public class ImportDataRow : DataRow
   {
      public ImportDataRow(DataRowBuilder builder) : base(builder)
      {
      }

      public new ImportDataTable Table
      {
         get { return (ImportDataTable) base.Table; }
      }
   }
}