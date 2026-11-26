using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Import;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public abstract class concern_for_DataFormat_with_error_type_column : ContextSpecification<DataFormatHeadersWithUnits>
   {
      protected const string ERROR_TYPE_COLUMN = "Error type";
      protected DataSheet _dataSheet;
      protected ColumnInfoCache _columnInfos;
      protected Column _errorColumn;
      protected IReadOnlyList<ParsedDataSet> _result;

      protected override void Context()
      {
         _columnInfos = new ColumnInfoCache
         {
            new ColumnInfo { DisplayName = "Time", Name = "Time", IsMandatory = true },
            new ColumnInfo { DisplayName = "Concentration", Name = "Concentration", IsMandatory = true, BaseGridName = "Time" },
            new ColumnInfo { DisplayName = "Error", Name = "Error", IsMandatory = false, BaseGridName = "Time", RelatedColumnOf = "Concentration" }
         };

         _dataSheet = new DataSheet { SheetName = "Sheet1" };
         var headers = new[] { "Id", "Time", "Concentration", "Error", ERROR_TYPE_COLUMN };
         for (var i = 0; i < headers.Length; i++)
         {
            _dataSheet.AddColumn(headers[i], i);
         }

         addRow("A", "1", "10", "2", "GSD");
         addRow("A", "2", "20", "3", " gsd ");
         addRow("B", "1", "10", "1", "");
         addRow("B", "2", "20", "2", "");
         addRow("C", "1", "10", "1", "ASD");
         addRow("C", "2", "20", "2", "GSD");

         _errorColumn = new Column
         {
            Name = "Error",
            Unit = new UnitDescription("µmol/l"),
            Dimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs(),
            ErrorStdDev = Constants.STD_DEV_ARITHMETIC,
            ErrorStdDevColumn = ERROR_TYPE_COLUMN
         };

         var configuration = new ImporterConfiguration();
         configuration.AddParameter(new MappingDataFormatParameter("Time", new Column { Name = "Time", Unit = new UnitDescription("min"), Dimension = DomainHelperForSpecs.TimeDimensionForSpecs() }));
         configuration.AddParameter(new MappingDataFormatParameter("Concentration", new Column { Name = "Concentration", Unit = new UnitDescription("µmol/l"), Dimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs() }));
         configuration.AddParameter(new MappingDataFormatParameter("Error", _errorColumn));
         configuration.AddParameter(new GroupByDataFormatParameter("Id"));

         sut = new DataFormatHeadersWithUnits();
         sut.CopyParametersFromConfiguration(configuration);
      }

      private void addRow(params string[] values) => _dataSheet.AddRow(values);

      protected ExtendedColumn errorColumnOf(string id)
      {
         return _result.Single(x => x.Description.Single().Value == id).Data.Keys.Single(x => x.ColumnInfo.Name == "Error");
      }
   }

   public class When_parsing_data_sets_whose_error_type_comes_from_a_column : concern_for_DataFormat_with_error_type_column
   {
      protected override void Because()
      {
         _result = sut.Parse(_dataSheet, _columnInfos).ToList();
      }

      [Observation]
      public void should_not_split_data_sets_by_error_type()
      {
         _result.Count.ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_read_the_error_type_of_every_row_of_the_data_set()
      {
         errorColumnOf("A").ErrorStdDevValues.ShouldOnlyContainInOrder("GSD", "gsd");
         errorColumnOf("B").ErrorStdDevValues.ShouldOnlyContainInOrder("", "");
         errorColumnOf("C").ErrorStdDevValues.ShouldOnlyContainInOrder("ASD", "GSD");
      }

      [Observation]
      public void should_leave_the_configured_error_type_to_be_resolved_once_the_rows_to_ignore_are_removed()
      {
         var errorColumn = errorColumnOf("A");
         errorColumn.ErrorDeviation.ShouldBeEqualTo(Constants.STD_DEV_ARITHMETIC);
         errorColumn.Column.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimensionForSpecs());
      }

      [Observation]
      public void should_not_change_the_configured_error_column()
      {
         _errorColumn.ErrorStdDev.ShouldBeEqualTo(Constants.STD_DEV_ARITHMETIC);
         _errorColumn.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimensionForSpecs());
      }
   }

   public class When_parsing_a_sheet_without_the_error_type_column : concern_for_DataFormat_with_error_type_column
   {
      protected override void Context()
      {
         base.Context();
         _errorColumn.ErrorStdDevColumn = "Missing column";
      }

      [Observation]
      public void should_throw_a_missing_column_exception()
      {
         The.Action(() => sut.Parse(_dataSheet, _columnInfos).ToList()).ShouldThrowAn<MissingColumnException>();
      }
   }
}
