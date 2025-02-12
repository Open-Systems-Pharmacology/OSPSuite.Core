using System;
using System.IO;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Import;
using OSPSuite.Utility;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_DataImporter : ContextForIntegration<IDataImporterTask>
   {
      protected ImporterConfiguration _configuration;

      protected override void Context()
      {
         base.Context();
         sut = Api.GetDataImporterTask();
         _configuration = sut.CreateConfiguration();
      }

      protected string getFileFullName(string fileName) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName);
   }

   public class When_importing_data_from_r : concern_for_DataImporter
   {
      [Observation]
      public void should_throw_on_invalid_file_name()
      {
         The.Action(() =>
            sut.ImportExcelFromConfiguration(getFileFullName("importerConfiguration1.xml"), getFileFullName("sample_non_existent.xlsx"))
         ).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_on_invalid_file_format()
      {
         The.Action(() =>
            sut.ImportExcelFromConfiguration(getFileFullName("importerConfiguration1.xml"), getFileFullName("importerConfiguration1.xml"))
         ).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_import_simple_data()
      {
         sut.ImportExcelFromConfiguration(getFileFullName("importerConfiguration1.xml"), getFileFullName("sample1.xlsx")).Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_throw_on_invalid_file_type()
      {
         The.Action(() => sut.ImportExcelFromConfiguration(sut.CreateConfiguration(), getFileFullName("simple.pkml")).Count.ShouldBeEqualTo(0)).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_import_simple_data_from_csv()
      {
         sut.ImportCsvFromConfiguration(getFileFullName("importerConfiguration1.xml"), getFileFullName("sample1.csv"), ';').Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void use_sheet_names_when_importing_simple_data_from_csv()
      {
         sut.IgnoreSheetNamesAtImport = false;
         sut.ImportCsvFromConfiguration(getFileFullName("importerConfiguration1.xml"), getFileFullName("sample1.csv"), ';').Count.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_throw_on_empty_file_name()
      {
         The.Action(() => sut.ImportExcelFromConfiguration(getFileFullName("importerConfiguration1.xml"), "")).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_read_configuration()
      {
         sut.GetConfiguration(getFileFullName("importerConfiguration1.xml")).ShouldNotBeNull();
      }

      [Observation]
      public void should_import_simple_data_with_configuration_object()
      {
         sut.ImportExcelFromConfiguration(sut.GetConfiguration(getFileFullName("importerConfiguration1.xml")), getFileFullName("sample1.xlsx")).Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_import_simple_data_from_csv_with_configuration_object()
      {
         sut.ImportCsvFromConfiguration(sut.GetConfiguration(getFileFullName("importerConfiguration1.csv.xml")), getFileFullName("sample1.csv"), ';').Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_import_simple_data_from_xls_without_naming_pattern()
      {
         sut.ImportExcelFromConfiguration(sut.GetConfiguration(getFileFullName("dataImporterConfiguration_noSheets.xml")), getFileFullName("CompiledDataSet_oneSheet.xlsx")).ShouldNotBeNull();
      }

      [Observation]
      public void should_create_configuration()
      {
         sut.CreateConfiguration().ShouldNotBeNull();
      }

      [Observation]
      public void should_create_configuration_from_data()
      {
         var configuration = sut.CreateConfigurationFor(getFileFullName("sample1.xlsx"));
         configuration.Parameters.Count.ShouldBeEqualTo(3);
         configuration.Parameters[0].ColumnName.ShouldBeEqualTo("time  [h]");
         configuration.Parameters[1].ColumnName.ShouldBeEqualTo("conc  [mg/l]");
         configuration.Parameters[2].ColumnName.ShouldBeEqualTo("SD [mg/l]");
         (configuration.Parameters[0] as MappingDataFormatParameter).MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("h");
         (configuration.Parameters[1] as MappingDataFormatParameter).MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("mg/l");
         (configuration.Parameters[2] as MappingDataFormatParameter).MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("mg/l");
         sut.ImportExcelFromConfiguration(configuration, getFileFullName("sample1.xlsx")).ShouldNotBeNull();
      }

      [Observation]
      public void should_create_configuration_from_data_with_duplicated_column()
      {
         var configuration = sut.CreateConfigurationFor(getFileFullName("duplicatedDoseCol.xlsx"));
         configuration.Parameters.OfType<MetaDataFormatParameter>().Count(x => x.MetaDataId.Equals("Dose")).ShouldBeEqualTo(1);
         configuration.Parameters.Count(x => x.ColumnName.Equals("DOSE")).ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_create_configuration_from_data_and_sheet_name()
      {
         var configuration = sut.CreateConfigurationFor(getFileFullName("sample2.xlsx"));
         configuration.Parameters.Count.ShouldBeEqualTo(3);
         configuration.Parameters[0].ColumnName.ShouldBeEqualTo("time  [h]");
         configuration.Parameters[1].ColumnName.ShouldBeEqualTo("conc  [mg/l]");
         configuration.Parameters[2].ColumnName.ShouldBeEqualTo("SD [mg/l]");

         configuration = sut.CreateConfigurationFor(getFileFullName("sample2.xlsx"), "Sheet2");
         configuration.Parameters.Count.ShouldBeEqualTo(3);
         configuration.Parameters[0].ColumnName.ShouldBeEqualTo("time  [s]");
         configuration.Parameters[1].ColumnName.ShouldBeEqualTo("measurement  [mg/l]");
         configuration.Parameters[2].ColumnName.ShouldBeEqualTo("molecule");
      }

      [Observation]
      public void should_create_configuration_from_data_respecting_meta_data_columns()
      {
         var configuration = sut.CreateConfigurationFor(getFileFullName("011.xlsx"));
         configuration.Parameters.OfType<MappingDataFormatParameter>().Any(p => p.MappedColumn.Name == "Time" && p.ColumnName == "Time [min]").ShouldBeTrue();
         configuration.Parameters.OfType<MappingDataFormatParameter>().Any(p => p.MappedColumn.Name == "Measurement" && p.ColumnName == "Measurement [mg/ml]").ShouldBeTrue();
         configuration.Parameters.OfType<MetaDataFormatParameter>().Any(p => p.MetaDataId == "Organ" && p.ColumnName == "Organ").ShouldBeTrue();
         configuration.Parameters.OfType<MetaDataFormatParameter>().Any(p => p.MetaDataId == "Study Id" && p.ColumnName == "Study Id").ShouldBeTrue();
         configuration.Parameters.OfType<MetaDataFormatParameter>().Any(p => p.MetaDataId == "Subject Id" && p.ColumnName == "Subject Id").ShouldBeTrue();
      }

      [Observation]
      public void should_create_configuration_with_correct_units_from_data_1()
      {
         var configuration = sut.CreateConfigurationFor(getFileFullName("sample_header_units_test_1.xlsx"));
         configuration.Parameters[0].ColumnName.ShouldBeEqualTo("[min] Time [h]");
         configuration.Parameters[1].ColumnName.ShouldBeEqualTo("conc [mol/l] [mg/l]");
         configuration.Parameters[2].ColumnName.ShouldBeEqualTo("[mg/l]SD ");
         (configuration.Parameters[0] as MappingDataFormatParameter).MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("h");
         (configuration.Parameters[1] as MappingDataFormatParameter).MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("mg/l");
         (configuration.Parameters[2] as MappingDataFormatParameter).MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("?");
      }

      [Observation]
      public void should_create_configuration_with_correct_units_from_data_2()
      {
         var configuration = sut.CreateConfigurationFor(getFileFullName("sample_header_units_test_2.xlsx"));
         configuration.Parameters[0].ColumnName.ShouldBeEqualTo("[Time after dose] [h]");
         configuration.Parameters[1].ColumnName.ShouldBeEqualTo("conc mg/l");
         configuration.Parameters[2].ColumnName.ShouldBeEqualTo("SD [semester]");
         (configuration.Parameters[0] as MappingDataFormatParameter).MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("h");
         (configuration.Parameters[1] as MappingDataFormatParameter).MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("?");
         (configuration.Parameters[2] as MappingDataFormatParameter).MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("?");
      }

      [Observation]
      public void should_create_configuration_with_correct_dimension_from_data()
      {
         var configuration = sut.CreateConfigurationFor(getFileFullName("sample_header_dimensions_test.xlsx"));
         var dimension = (configuration.Parameters[1] as MappingDataFormatParameter).MappedColumn.Dimension;
         dimension.DisplayName.ShouldBeEqualTo("Time");
         dimension = (configuration.Parameters[2] as MappingDataFormatParameter).MappedColumn.Dimension;
         dimension.DisplayName.ShouldBeEqualTo("Concentration (mass)");
         dimension = (configuration.Parameters[0] as MappingDataFormatParameter).MappedColumn.Dimension;
         dimension.DisplayName.ShouldBeEqualTo("Concentration (mass)");
      }

      [Observation]
      public void should_save_configuration()
      {
         var tempFileName = FileHelper.GenerateTemporaryFileName();
         var configuration = sut.GetConfiguration(getFileFullName("importerConfiguration1.xml"));
         sut.SaveConfiguration(configuration, tempFileName);
         var configurationCopy = sut.GetConfiguration(tempFileName);
         FileHelper.DeleteFile(tempFileName);

         configurationCopy.FilterString.ShouldBeEqualTo(configuration.FilterString);
         configurationCopy.Id.ShouldBeEqualTo(configuration.Id);
         configurationCopy.LoadedSheets.ShouldBeEqualTo(configuration.LoadedSheets);
         configurationCopy.NamingConventions.ShouldBeEqualTo(configuration.NamingConventions);
         configurationCopy.NanSettings.ShouldBeEqualTo(configuration.NanSettings);
         configurationCopy.Parameters.Each((p, i) => p.EquivalentTo(configuration.Parameters[i]).ShouldBeTrue());
      }

      [Observation]
      public void should_get_and_set_time()
      {
         var time = sut.GetTime(_configuration);
         var columnName = new Guid().ToString();
         time.ColumnName = columnName;
         sut.GetTime(_configuration).ColumnName.ShouldBeEqualTo(columnName);
      }

      [Observation]
      public void should_get_and_set_concentration()
      {
         var concentration = sut.GetMeasurement(_configuration);
         var columnName = new Guid().ToString();
         concentration.ColumnName = columnName;
         sut.GetMeasurement(_configuration).ColumnName.ShouldBeEqualTo(columnName);
      }

      [Observation]
      public void should_get_and_set_error()
      {
         sut.AddError(_configuration);
         var error = sut.GetError(_configuration);
         var columnName = new Guid().ToString();
         error.ColumnName = columnName;
         sut.GetError(_configuration).ColumnName.ShouldBeEqualTo(columnName);
      }

      [Observation]
      public void should_add_and_remove_error()
      {
         sut.GetError(_configuration).ShouldBeNull();
         sut.AddError(_configuration);
         sut.GetError(_configuration).ShouldNotBeNull();
         sut.RemoveError(_configuration);
         sut.GetError(_configuration).ShouldBeNull();
      }

      [Observation]
      public void should_get_add_and_remove_all_grouping_columns()
      {
         sut.GetAllGroupingColumns(_configuration).ShouldBeEmpty();
         var groupingColumn = "column1";
         sut.AddGroupingColumn(_configuration, groupingColumn);
         sut.GetAllGroupingColumns(_configuration).ShouldContain(groupingColumn);
         sut.RemoveGroupingColumn(_configuration, groupingColumn);
         sut.GetAllGroupingColumns(_configuration).ShouldBeEmpty();
      }

      [Observation]
      public void should_not_duplicate_grouping_columns()
      {
         sut.GetAllGroupingColumns(_configuration).ShouldBeEmpty();
         var groupingColumn = "column1";
         sut.AddGroupingColumn(_configuration, groupingColumn);
         sut.GetAllGroupingColumns(_configuration).Count().ShouldBeEqualTo(1);
         sut.AddGroupingColumn(_configuration, groupingColumn);
         sut.GetAllGroupingColumns(_configuration).Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_not_throw_when_removing_non_existing_grouping_columns()
      {
         sut.GetAllGroupingColumns(_configuration).ShouldBeEmpty();
         var groupingColumn = "column1";
         sut.RemoveGroupingColumn(_configuration, groupingColumn);
         sut.GetAllGroupingColumns(_configuration).Count().ShouldBeEqualTo(0);
         sut.AddGroupingColumn(_configuration, groupingColumn);
         sut.GetAllGroupingColumns(_configuration).Count().ShouldBeEqualTo(1);
         sut.RemoveGroupingColumn(_configuration, groupingColumn);
         sut.GetAllGroupingColumns(_configuration).Count().ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_get_add_and_remove_all_loaded_sheets()
      {
         sut.GetAllLoadedSheets(_configuration).ShouldBeEmpty();
         var sheet = "sheet1";
         sut.SetAllLoadedSheet(_configuration, new[] { sheet });
         sut.GetAllLoadedSheets(_configuration).ShouldContain(sheet);
         _configuration.ClearLoadedSheets();
         sut.GetAllLoadedSheets(_configuration).ShouldBeEmpty();
      }

      [Observation]
      public void should_get_add_and_remove_all_loaded_sheets_from_single_string()
      {
         sut.GetAllLoadedSheets(_configuration).ShouldBeEmpty();
         var sheet = "sheet1";
         sut.SetAllLoadedSheet(_configuration, sheet);
         sut.GetAllLoadedSheets(_configuration).ShouldContain(sheet);
         _configuration.ClearLoadedSheets();
         sut.GetAllLoadedSheets(_configuration).ShouldBeEmpty();
      }

      [Observation]
      public void should_consider_columns_with_string_values_and_skip_invalid_sheets()
      {
         var configuration = sut.CreateConfigurationFor(getFileFullName("BookStrings.xlsx"));
         configuration.Parameters.Any(x => (x as MappingDataFormatParameter).ColumnName == "Time [h]").ShouldBeTrue();
         configuration.Parameters.Any(x => (x as MappingDataFormatParameter).ColumnName == "Measurement [mg/l]").ShouldBeTrue();
         configuration.Parameters.Any(x => (x as MappingDataFormatParameter).ColumnName == "Error [mg/l]").ShouldBeTrue();
      }
   }
}