using System.Data;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.R.Domain;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_AgingData : ContextSpecification<AgingData>
   {
      protected override void Context()
      {
         sut = new AgingData();
      }
   }

   public class When_converting_an_uninitialized_aging_data_to_data_table : concern_for_AgingData
   {
      [Observation]
      public void should_thrown_an_exception()
      {
         The.Action(() => sut.ToDataTable()).ShouldThrowAn<InvalidArgumentException>();
      }
   }

   public class When_converting_an_invalid_aging_data_to_data_table : concern_for_AgingData
   {
      protected override void Context()
      {
         base.Context();
         sut.IndividualIds = new[] {1, 2};
         sut.ParameterPaths = new[] {"Path1", "Path1"};
         sut.Times = new[] {1.0, 2.0};
         sut.Values = new[] {1.0, 2.0, 3.0};
      }

      [Observation]
      public void should_thrown_an_exception()
      {
         The.Action(() => sut.ToDataTable()).ShouldThrowAn<InvalidArgumentException>();
      }
   }

   public class When_converting_a_valid_aging_data_to_data_table : concern_for_AgingData
   {
      private DataTable _dataTable;

      protected override void Context()
      {
         base.Context();
         sut.IndividualIds = new[] {1, 2};
         sut.ParameterPaths = new[] {"Path1", "Path2"};
         sut.Times = new[] {1.0, 2.0};
         sut.Values = new[] {3.0, 4.0};
      }

      protected override void Because()
      {
         _dataTable = sut.ToDataTable();
      }

      [Observation]
      public void should_thrown_an_exception()
      {
         _dataTable.Columns.Count.ShouldBeEqualTo(4);
         _dataTable.Rows[0].ValueAt<int>(Constants.Population.INDIVIDUAL_ID_COLUMN).ShouldBeEqualTo(1);
         _dataTable.Rows[1].ValueAt<int>(Constants.Population.INDIVIDUAL_ID_COLUMN).ShouldBeEqualTo(2);

         _dataTable.Rows[0].ValueAt<string>(Constants.Population.PARAMETER_PATH_COLUMN).ShouldBeEqualTo("Path1");
         _dataTable.Rows[1].ValueAt<string>(Constants.Population.PARAMETER_PATH_COLUMN).ShouldBeEqualTo("Path2");

         _dataTable.Rows[0].ValueAt<double>(Constants.Population.TIME_COLUMN).ShouldBeEqualTo(1.0);
         _dataTable.Rows[1].ValueAt<double>(Constants.Population.TIME_COLUMN).ShouldBeEqualTo(2.0);

         _dataTable.Rows[0].ValueAt<double>(Constants.Population.VALUE_COLUMN).ShouldBeEqualTo(3.0);
         _dataTable.Rows[1].ValueAt<double>(Constants.Population.VALUE_COLUMN).ShouldBeEqualTo(4.0);
      }
   }
}