using System.Collections.Generic;
using System.Data;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_MatrixArrayToDataTableMapper : ContextSpecification<MatrixToDataTableMapper>
   {
      protected override void Context()
      {
         sut = new MatrixToDataTableMapper();
      }
   }

   public class When_mapping_a_array_of_double_to_a_data_table : concern_for_MatrixArrayToDataTableMapper
   {
      private IReadOnlyList<string> _captions;
      private Matrix _matrix;
      private DataTable _dataTable;

      protected override void Context()
      {
         base.Context();
         _captions = new List<string> { "parameter 1", "parameter 2" };
         _matrix = new Matrix(_captions, _captions);
         _matrix.SetRow(0, new [] {11d, 12d });
         _matrix.SetRow(1, new [] {21d, 22d });
      }

      protected override void Because()
      {
         _dataTable = sut.MapFrom(_matrix);
      }

      [Observation]
      public void the_data_table_should_have_the_correct_number_of_rows_and_columns()
      {
         _dataTable.Rows.Count.ShouldBeEqualTo(2);
         _dataTable.Columns.Count.ShouldBeEqualTo(3);
      }

      [Observation]
      public void the_column_headers_should_contain_the_captions()
      {
         _dataTable.Columns[0].Caption.ShouldBeEqualTo("Parameter");
         _dataTable.Columns[1].Caption.ShouldBeEqualTo("parameter 1");
         _dataTable.Columns[2].Caption.ShouldBeEqualTo("parameter 2");
      }

      [Observation]
      public void the_first_column_should_contain_the_captions()
      {
         _dataTable.Rows[0][0].ToString().ShouldBeEqualTo("parameter 1");
         _dataTable.Rows[1][0].ToString().ShouldBeEqualTo("parameter 2");
      }
   }
}
