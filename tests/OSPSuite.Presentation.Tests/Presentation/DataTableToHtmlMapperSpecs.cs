using System.Data;
using System.Globalization;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Mappers;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_DataTableToHtmlMapper : ContextSpecification<DataTableToHtmlMapper>
   {
      protected override void Context()
      {
         sut = new DataTableToHtmlMapper();
      }

      protected DataTable GetDataTable()
      {
         var dataTable = new DataTable("tableName");

         dataTable.Columns.Add(new DataColumn("Name"));
         dataTable.Columns.Add(new DataColumn("Data"));

         var row = dataTable.NewRow();
         row[0] = "Name 1";
         row[1] = 1.1;
         dataTable.Rows.Add(row);

         row = dataTable.NewRow();
         row[0] = "Name 2";
         row[1] = 2.2;
         dataTable.Rows.Add(row);

         return dataTable;
      }
   }

   public class When_mapping_a_data_table_to_html : concern_for_DataTableToHtmlMapper
   {
      private string _htmlFragment;

      protected override void Because()
      {
         _htmlFragment = sut.MapFrom(GetDataTable(), true);
      }

      [Observation]
      public void html_fragment_must_contain_html_snippets()
      {

         _htmlFragment.Contains("<th>Name</th>").ShouldBeTrue();
         _htmlFragment.Contains("<th>Data</th>").ShouldBeTrue();
         _htmlFragment.Contains("<td>Name 1</td>").ShouldBeTrue();
         _htmlFragment.Contains("<td>Name 2</td>").ShouldBeTrue();
         _htmlFragment.Contains("<td>1" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "1</td>").ShouldBeTrue();
         _htmlFragment.Contains("<td>2" + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "2</td>").ShouldBeTrue();
      }
   }
}
