using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Format;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Infrastructure.Export
{
   public abstract class concern_for_DataRepositoryExportTask : ContextSpecification<IDataRepositoryExportTask>
   {
      private IDimension _time;
      protected IDimension _mass;
      protected BaseGrid _baseGrid1;
      protected DataRepository _dataRepository;

      protected override void Context()
      {
         _time = new Dimension(new BaseDimensionRepresentation {TimeExponent = 1}, "Time", "s");
         _mass = new Dimension(new BaseDimensionRepresentation {MassExponent = 1}, "Mass", "kg");
         _mass.AddUnit("mg", 0.1, 0.0);
         _mass.DefaultUnit = _mass.Unit("mg");

         _baseGrid1 = new BaseGrid("BaseGrid1", _time) {Values = new float[] {1, 2, 3}};
         _dataRepository = new DataRepository("id").WithName("toto");
         sut = new DataRepositoryExportTask();;
      }
   }

   public class When_converting_a_data_repository_with_only_one_base_grid_to_a_datatable : concern_for_DataRepositoryExportTask
   {
      private DataColumn _col1;
      private DataColumn _col2;
      private DataTable _dataTable;
      private IEnumerable<DataTable> _results;

      protected override void Context()
      {
         base.Context();
         _col1 = new DataColumn("col1", _mass, _baseGrid1) {Values = new float[] {10, 20, 30}};
         _col2 = new DataColumn("col2", _mass, _baseGrid1) {Values = new float[] {100, 200, 300}};

         _dataRepository.Add(_col1);
         _dataRepository.Add(_col2);
      }

      protected override void Because()
      {
         _results = sut.ToDataTable(_dataRepository);
      }

      [Observation]
      public void should_return_a_data_table_containing_one_column_for_each_available_column_in_the_repository()
      {
         _results.Count().ShouldBeEqualTo(1);
         _dataTable = _results.ElementAt(0);
         _dataTable.Columns.Count.ShouldBeEqualTo(3);
         _dataTable.Columns[0].ColumnName.ShouldBeEqualTo(_baseGrid1.Name + " [s]");
         _dataTable.Columns[1].ColumnName.ShouldBeEqualTo(_col1.Name + " [mg]");
         _dataTable.Columns[2].ColumnName.ShouldBeEqualTo(_col2.Name + " [mg]");
      }

      [Observation]
      public void should_have_set_the_value_from_the_columns_into_the_datatable()
      {
         _dataTable = _results.ElementAt(0);
         _dataTable.Columns.Count.ShouldBeEqualTo(3);
         _dataTable.Rows.Count.ShouldBeEqualTo(3);

         _dataTable.Rows[0].ItemArray.ShouldOnlyContainInOrder(1, 100, 1000);
         _dataTable.Rows[1].ItemArray.ShouldOnlyContainInOrder(2, 200, 2000);
         _dataTable.Rows[2].ItemArray.ShouldOnlyContainInOrder(3, 300, 3000);
      }

      [Observation]
      public void should_have_set_the_name_of_the_data_repository_in_the_table()
      {
         _dataTable = _results.ElementAt(0);
         _dataTable.TableName.ShouldBeEqualTo(_dataRepository.Name);
      }
   }

   public class When_converting_a_data_repository_with_auxiliary_columns_to_datatable : concern_for_DataRepositoryExportTask
   {
      private DataColumn _col1;
      private DataColumn _col2;
      private DataTable _dataTable;
      private IEnumerable<DataTable> _results;

      protected override void Context()
      {
         base.Context();
         _col1 = new DataColumn("col1", _mass, _baseGrid1)
         {
            Values = new float[] {10, 20, 30},
            DataInfo = {Origin = ColumnOrigins.Observation}
         };
         _col2 = new DataColumn("col2", _mass, _baseGrid1)
         {
            Values = new float[] {100, 200, 300},
            DataInfo = {AuxiliaryType = AuxiliaryType.ArithmeticStdDev, Origin = ColumnOrigins.ObservationAuxiliary}
         };

         _col1.AddRelatedColumn(_col2);
         _dataRepository.Add(_col2);
         _dataRepository.Add(_col1);
      }

      protected override void Because()
      {
         _results = sut.ToDataTable(_dataRepository);
      }

      [Observation]
      public void should_add_the_auxiliary_columns_at_the_end()
      {
         _results.Count().ShouldBeEqualTo(1);
         _dataTable = _results.ElementAt(0);
         _dataTable.Columns.Count.ShouldBeEqualTo(3);
         _dataTable.Columns[0].ColumnName.ShouldBeEqualTo(_baseGrid1.Name + " [s]");
         _dataTable.Columns[1].ColumnName.ShouldBeEqualTo(_col1.Name + " [mg]");
         _dataTable.Columns[2].ColumnName.ShouldBeEqualTo(_col2.Name + " [mg]");
      }
   }

   public class When_converting_a_data_repository_with_only_one_base_grid_to_a_datatable_and_the_value_should_be_exported_in_base_unit : concern_for_DataRepositoryExportTask
   {
      private DataColumn _col1;
      private DataColumn _col2;
      private DataTable _dataTable;
      private IEnumerable<DataTable> _results;

      protected override void Context()
      {
         base.Context();
         _col1 = new DataColumn("col1", _mass, _baseGrid1) {Values = new float[] {10, 20, 30}};
         _col2 = new DataColumn("col2", _mass, _baseGrid1) {Values = new float[] {100, 200, 300}};

         _dataRepository.Add(_col1);
         _dataRepository.Add(_col2);
      }

      protected override void Because()
      {
         _results = sut.ToDataTable(_dataRepository, new DataColumnExportOptions {UseDisplayUnit = false});
      }

      [Observation]
      public void should_return_a_data_table_containing_one_column_for_each_available_column_in_the_repository_using_the_base_dimension()
      {
         _results.Count().ShouldBeEqualTo(1);
         _dataTable = _results.ElementAt(0);
         _dataTable.Columns.Count.ShouldBeEqualTo(3);
         _dataTable.Columns[0].ColumnName.ShouldBeEqualTo(_baseGrid1.Name + " [s]");
         _dataTable.Columns[1].ColumnName.ShouldBeEqualTo(_col1.Name + " [kg]");
         _dataTable.Columns[2].ColumnName.ShouldBeEqualTo(_col2.Name + " [kg]");
      }

      [Observation]
      public void should_have_set_the_value_from_the_columns_into_the_datatable_using_values_in_base_unit()
      {
         _dataTable = _results.ElementAt(0);
         _dataTable.Columns.Count.ShouldBeEqualTo(3);
         _dataTable.Rows.Count.ShouldBeEqualTo(3);

         _dataTable.Rows[0].ItemArray.ShouldOnlyContainInOrder(1, 10, 100);
         _dataTable.Rows[1].ItemArray.ShouldOnlyContainInOrder(2, 20, 200);
         _dataTable.Rows[2].ItemArray.ShouldOnlyContainInOrder(3, 30, 300);
      }

      [Observation]
      public void should_have_set_the_name_of_the_data_repository_in_the_table()
      {
         _dataTable = _results.ElementAt(0);
         _dataTable.TableName.ShouldBeEqualTo(_dataRepository.Name);
      }
   }

   public class When_converting_a_data_repository_and_output_should_be_formated : concern_for_DataRepositoryExportTask
   {
      private DataColumn _col1;
      private DataTable _dataTable;
      private IEnumerable<DataTable> _results;

      protected override void Context()
      {
         base.Context();
         NumericFormatterOptions.Instance.DecimalPlace = 2;
         _col1 = new DataColumn("col1", _mass, _baseGrid1) {Values = new float[] {0.11111f, 0.222222f, 0.33333333f}};
         _dataRepository.Add(_col1);
      }

      protected override void Because()
      {
         _results = sut.ToDataTable(_dataRepository, new DataColumnExportOptions {FormatOutput = true});
      }

      [Observation]
      public void should_have_set_the_value_from_the_columns_into_the_datatable()
      {
         _dataTable = _results.ElementAt(0);
         _dataTable.Columns.Count.ShouldBeEqualTo(2);
         _dataTable.Rows.Count.ShouldBeEqualTo(3);

         _dataTable.Rows[0].ItemArray.ShouldOnlyContainInOrder("1.00", "1.11");
         _dataTable.Rows[1].ItemArray.ShouldOnlyContainInOrder("2.00", "2.22");
         _dataTable.Rows[2].ItemArray.ShouldOnlyContainInOrder("3.00", "3.33");
      }
   }

   public class When_converting_a_data_repository_containing_two_column_having_the_same_display : concern_for_DataRepositoryExportTask
   {
      private DataColumn _col1;
      private DataColumn _col2;
      private DataTable _dataTable;
      private IEnumerable<DataTable> _results;

      protected override void Context()
      {
         base.Context();
         _col1 = new DataColumn("col2", _mass, _baseGrid1) {Values = new float[] {10, 20, 30}};
         _col2 = new DataColumn("col2", _mass, _baseGrid1) {Values = new float[] {100, 200, 300}};

         _dataRepository.Add(_col1);
         _dataRepository.Add(_col2);
      }

      protected override void Because()
      {
         _results = sut.ToDataTable(_dataRepository);
      }

      [Observation]
      public void should_simply_rename_the_display_to_ensure_unicity()
      {
         _dataTable = _results.ElementAt(0);
         _dataTable.Columns.Count.ShouldBeEqualTo(3);
      }
   }

   public class When_converting_a_data_repository_with_an_empty_name : concern_for_DataRepositoryExportTask
   {
      private DataColumn _col1;
      private DataTable _dataTable;
      private IEnumerable<DataTable> _results;

      protected override void Context()
      {
         base.Context();
         _col1 = new DataColumn("col2", _mass, _baseGrid1) {Values = new float[] {10, 20, 30}};
         _dataRepository.Name = string.Empty;
         _dataRepository.Add(_col1);
      }

      protected override void Because()
      {
         _results = sut.ToDataTable(_dataRepository);
      }

      [Observation]
      public void should_have_created_a_defautl_name_of_table()
      {
         _dataTable = _results.ElementAt(0);
         _dataTable.TableName.ShouldBeEqualTo("Table");
      }
   }

   public class When_converting_a_data_repository_with_an_name_to_long : concern_for_DataRepositoryExportTask
   {
      private DataColumn _col1;
      private DataTable _dataTable;
      private IEnumerable<DataTable> _results;

      protected override void Context()
      {
         base.Context();
         _col1 = new DataColumn("col2", _mass, _baseGrid1) {Values = new float[] {10, 20, 30}};
         _dataRepository.Name = "aaaaaaaaaaaaaaaaaaaaaaaaaaaabbbbbbbbbbbbbbb";
         _dataRepository.Add(_col1);
      }

      protected override void Because()
      {
         _results = sut.ToDataTable(_dataRepository);
      }

      [Observation]
      public void should_have_troncated_the_name_as_expected()
      {
         _dataTable = _results.ElementAt(0);
         _dataTable.TableName.ShouldBeEqualTo("aaaaaaaaaaaaaaaaaaaaaaaaaaaab");
      }
   }
}