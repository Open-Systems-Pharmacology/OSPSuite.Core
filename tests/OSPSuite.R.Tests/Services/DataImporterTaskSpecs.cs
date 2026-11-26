using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
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

      [Observation]
      public void should_throw_meaningful_exception_not_null_reference_exception()
      {
         // This sheet has a column configuration where the dimension cannot be determined, but an error column
         // where the dimension can be determined. It should error with an appropriate exception and message
         var fileFullName = getFileFullName("CompiledDataSet.xlsx");
         var configuration = sut.CreateConfigurationFor(fileFullName, "TestSheet_1_withMW");
         sut.SetAllLoadedSheet(configuration, "TestSheet_1_withMW");
         sut.IgnoreSheetNamesAtImport = false;
         The.Action(() => sut.ImportExcelFromConfiguration(configuration, fileFullName)).ShouldThrowAn<ImporterParsingException>();
      }
   }

   public class When_importing_data_where_the_unit_column_is_empty : concern_for_DataImporter
   {
      private ImporterParsingException _exception;

      protected override void Because()
      {
         try
         {
            sut.ImportExcelFromConfiguration(getFileFullName("importerConfiguration_unitsFromColumns.xml"), getFileFullName("MissingTimeUnit.xlsx"));
         }
         catch (ImporterParsingException e)
         {
            _exception = e;
         }
      }

      [Observation]
      public void should_throw_a_parsing_exception()
      {
         _exception.ShouldNotBeNull();
      }

      [Observation]
      public void should_report_that_the_empty_unit_is_not_supported_by_the_mapping()
      {
         _exception.Message.ShouldBeEqualTo(Error.ParseErrorMessage(Error.InvalidDimensionException("", "Time")));
      }
   }

   public class When_importing_data_where_the_unit_column_is_empty_for_some_rows : concern_for_DataImporter
   {
      private ImporterParsingException _exception;

      protected override void Because()
      {
         try
         {
            sut.ImportCsvFromConfiguration(getFileFullName("importerConfiguration_unitsFromColumns.xml"), getFileFullName("PartiallyMissingTimeUnit.csv"), ';');
         }
         catch (ImporterParsingException e)
         {
            _exception = e;
         }
      }

      [Observation]
      public void should_throw_a_parsing_exception()
      {
         _exception.ShouldNotBeNull();
      }

      [Observation]
      public void should_report_that_the_empty_unit_is_not_supported_by_the_mapping()
      {
         _exception.Message.ShouldBeEqualTo(Error.ParseErrorMessage(Error.InvalidDimensionException("", "Time")));
      }
   }

   public class When_importing_data_whose_error_type_comes_from_a_column : concern_for_DataImporter
   {
      private IReadOnlyList<DataRepository> _dataRepositories;

      protected override void Because()
      {
         sut.IgnoreSheetNamesAtImport = true;
         _dataRepositories = sut.ImportCsvFromConfiguration(getFileFullName("errorTypeFromColumnConfiguration.xml"), getFileFullName("errorTypeFromColumn.csv"), ';');
      }

      private DataColumn errorColumnOf(string id)
      {
         return _dataRepositories
            .Single(x => x.ExtendedProperties["Id"].ValueAsObject.ToString() == id)
            .AllButBaseGrid()
            .Single(x => x.DataInfo.AuxiliaryType != AuxiliaryType.Undefined);
      }

      private Column errorMappingOf(ImporterConfiguration configuration)
      {
         return configuration.Parameters.OfType<MappingDataFormatParameter>().Single(x => x.MappedColumn.Name == "Error").MappedColumn;
      }

      [Observation]
      public void should_create_one_data_set_per_group_and_not_per_error_type()
      {
         _dataRepositories.Count.ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_import_the_measurement_of_every_data_set()
      {
         foreach (var dataRepository in _dataRepositories)
         {
            var measurementColumn = dataRepository.AllButBaseGrid().Single(x => x.DataInfo.AuxiliaryType == AuxiliaryType.Undefined);
            measurementColumn.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MASS_CONCENTRATION);
         }
      }

      [Observation]
      public void should_import_a_dimensionless_geometric_error_as_written_for_a_geometric_data_set()
      {
         var errorColumn = errorColumnOf("A");
         errorColumn.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.GeometricStdDev);
         errorColumn.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.DIMENSIONLESS);
         errorColumn.Values.ToArray().ShouldBeEqualTo(new[] { 2f, 3f });
      }

      [Observation]
      public void should_import_an_arithmetic_error_for_a_data_set_without_error_type()
      {
         var errorColumn = errorColumnOf("B");
         errorColumn.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.ArithmeticStdDev);
         errorColumn.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MASS_CONCENTRATION);
      }

      [Observation]
      public void should_import_an_arithmetic_error_for_an_arithmetic_data_set()
      {
         errorColumnOf("C").DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.ArithmeticStdDev);
      }

      [Observation]
      public void should_keep_the_error_type_column_when_saving_and_loading_the_configuration()
      {
         var tempFileName = FileHelper.GenerateTemporaryFileName();
         sut.SaveConfiguration(sut.GetConfiguration(getFileFullName("errorTypeFromColumnConfiguration.xml")), tempFileName);
         var configurationCopy = sut.GetConfiguration(tempFileName);
         FileHelper.DeleteFile(tempFileName);

         errorMappingOf(configurationCopy).ErrorStdDevColumn.ShouldBeEqualTo("Error type");
      }

      [Observation]
      public void should_load_a_configuration_without_error_type_column()
      {
         errorMappingOf(sut.GetConfiguration(getFileFullName("importerConfiguration1.xml"))).ErrorStdDevColumn.ShouldBeNull();
      }
   }

   public class When_importing_data_whose_error_type_and_units_come_from_columns : concern_for_DataImporter
   {
      private IReadOnlyList<DataRepository> _dataRepositories;

      protected override void Because()
      {
         sut.IgnoreSheetNamesAtImport = true;
         _dataRepositories = sut.ImportCsvFromConfiguration(getFileFullName("errorTypeAndUnitFromColumnsConfiguration.xml"), getFileFullName("errorTypeAndUnitFromColumns.csv"), ';');
      }

      private DataRepository dataRepositoryOf(string id) => _dataRepositories.Single(x => x.ExtendedProperties["Id"].ValueAsObject.ToString() == id);

      private DataColumn errorColumnOf(string id) => dataRepositoryOf(id).AllButBaseGrid().Single(x => x.DataInfo.AuxiliaryType != AuxiliaryType.Undefined);

      private DataColumn measurementColumnOf(string id) => dataRepositoryOf(id).AllButBaseGrid().Single(x => x.DataInfo.AuxiliaryType == AuxiliaryType.Undefined);

      [Observation]
      public void should_import_a_geometric_error_as_written_whatever_the_unit_of_its_rows()
      {
         foreach (var id in new[] { "A", "B" })
         {
            var errorColumn = errorColumnOf(id);
            errorColumn.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.GeometricStdDev);
            errorColumn.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.DIMENSIONLESS);
            errorColumn.Values.ToArray().ShouldBeEqualTo(new[] { 2f, 3f });
         }
      }

      [Observation]
      public void should_convert_an_arithmetic_error_with_the_unit_of_its_rows_like_the_measurement()
      {
         var errorColumn = errorColumnOf("C");
         var measurementColumn = measurementColumnOf("C");
         errorColumn.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.ArithmeticStdDev);
         errorColumn.Dimension.ShouldBeEqualTo(measurementColumn.Dimension);
         for (var i = 0; i < measurementColumn.Values.Count; i++)
         {
            ((double)errorColumn.Values[i] / measurementColumn.Values[i]).ShouldBeEqualTo(0.1, 1e-5);
         }
      }
   }

   public class When_mapping_a_column_to_a_meta_data_category : concern_for_DataImporter
   {
      private MetaDataFormatParameter metaDataColumnFor(string metaDataId) => sut.GetAllMetaDataColumns(_configuration).SingleOrDefault(p => p.MetaDataId == metaDataId);

      [Observation]
      public void should_add_the_column_as_meta_data_of_the_category()
      {
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.SUBJECT_COUNT);
         metaDataColumnFor(Constants.ObservedData.SUBJECT_COUNT).ColumnName.ShouldBeEqualTo("N");
         sut.GetAllGroupingColumns(_configuration).ShouldOnlyContain("N");
      }

      [Observation]
      public void should_throw_if_the_category_is_not_a_meta_data_category_of_observed_data()
      {
         The.Action(() => sut.AddMetaDataColumn(_configuration, "N", "Subject cnt")).ShouldThrowAn<OSPSuiteException>();
         sut.GetAllMetaDataColumns(_configuration).ShouldBeEmpty();
      }

      [Observation]
      public void should_replace_a_grouping_column_of_the_same_column()
      {
         sut.AddGroupingColumn(_configuration, "N");
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.SUBJECT_COUNT);
         _configuration.Parameters.OfType<GroupByDataFormatParameter>().ShouldBeEmpty();
         sut.GetAllGroupingColumns(_configuration).ShouldOnlyContain("N");
      }

      [Observation]
      public void should_replace_the_mapping_of_the_same_column_to_another_category()
      {
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.STUDY_ID);
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.SUBJECT_COUNT);
         metaDataColumnFor(Constants.ObservedData.STUDY_ID).ShouldBeNull();
         metaDataColumnFor(Constants.ObservedData.SUBJECT_COUNT).ColumnName.ShouldBeEqualTo("N");
      }

      [Observation]
      public void should_replace_the_mapping_of_the_same_category_to_another_column()
      {
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.SUBJECT_COUNT);
         sut.AddMetaDataColumn(_configuration, "n subjects", Constants.ObservedData.SUBJECT_COUNT);
         sut.GetAllMetaDataColumns(_configuration).Length.ShouldBeEqualTo(1);
         metaDataColumnFor(Constants.ObservedData.SUBJECT_COUNT).ColumnName.ShouldBeEqualTo("n subjects");
      }

      [Observation]
      public void should_replace_a_fixed_value_of_the_same_category()
      {
         _configuration.AddParameter(new MetaDataFormatParameter("5", Constants.ObservedData.SUBJECT_COUNT, isColumn: false));
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.SUBJECT_COUNT);
         _configuration.Parameters.OfType<MetaDataFormatParameter>().ShouldOnlyContain(metaDataColumnFor(Constants.ObservedData.SUBJECT_COUNT));
      }

      [Observation]
      public void should_not_return_a_fixed_value_as_meta_data_column()
      {
         _configuration.AddParameter(new MetaDataFormatParameter("Human", Constants.ObservedData.SPECIES, isColumn: false));
         sut.GetAllMetaDataColumns(_configuration).ShouldBeEmpty();
      }

      [Observation]
      public void should_replace_the_naming_convention_key_of_a_replaced_grouping_column()
      {
         _configuration.NamingConventions = "{Source}.{Id}.{N}";
         sut.AddGroupingColumn(_configuration, "Id");
         sut.AddGroupingColumn(_configuration, "N");
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.SUBJECT_COUNT);
         _configuration.NamingConventions.ShouldBeEqualTo("{Source}.{Id}.{Subject count}");
      }

      [Observation]
      public void should_replace_the_naming_convention_key_of_the_same_column_mapped_to_another_category()
      {
         _configuration.NamingConventions = "{Source}.{Study Id}";
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.STUDY_ID);
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.SUBJECT_COUNT);
         _configuration.NamingConventions.ShouldBeEqualTo("{Source}.{Subject count}");
      }

      [Observation]
      public void should_remove_the_naming_convention_key_of_a_replaced_grouping_column_if_the_category_is_already_in_the_naming_convention()
      {
         _configuration.NamingConventions = "{Source}.{N}.{Subject count}";
         sut.AddMetaDataColumn(_configuration, "M", Constants.ObservedData.SUBJECT_COUNT);
         sut.AddGroupingColumn(_configuration, "N");
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.SUBJECT_COUNT);
         _configuration.NamingConventions.ShouldBeEqualTo("{Source}.{Subject count}");
      }

      [Observation]
      public void should_remove_the_separator_after_the_key_if_the_key_starts_the_naming_convention()
      {
         _configuration.NamingConventions = "{N}-{Subject count}";
         sut.AddGroupingColumn(_configuration, "N");
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.SUBJECT_COUNT);
         _configuration.NamingConventions.ShouldBeEqualTo("{Subject count}");
      }

      [Observation]
      public void should_not_set_a_naming_convention_if_none_is_defined()
      {
         sut.AddGroupingColumn(_configuration, "N");
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.SUBJECT_COUNT);
         _configuration.NamingConventions.ShouldBeNull();
      }

      [Observation]
      public void should_remove_the_mapping_of_the_category()
      {
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.SUBJECT_COUNT);
         sut.RemoveMetaDataColumn(_configuration, Constants.ObservedData.SUBJECT_COUNT);
         sut.GetAllMetaDataColumns(_configuration).ShouldBeEmpty();
      }

      [Observation]
      public void should_keep_a_fixed_value_of_the_category_when_removing()
      {
         var fixedValue = new MetaDataFormatParameter("Human", Constants.ObservedData.SPECIES, isColumn: false);
         _configuration.AddParameter(fixedValue);
         sut.RemoveMetaDataColumn(_configuration, Constants.ObservedData.SPECIES);
         _configuration.Parameters.ShouldContain(fixedValue);
      }

      [Observation]
      public void should_not_throw_when_removing_a_category_that_is_not_mapped()
      {
         sut.AddGroupingColumn(_configuration, Constants.ObservedData.SUBJECT_COUNT);
         sut.RemoveMetaDataColumn(_configuration, Constants.ObservedData.SUBJECT_COUNT);
         sut.GetAllGroupingColumns(_configuration).ShouldOnlyContain(Constants.ObservedData.SUBJECT_COUNT);
      }
   }

   public abstract class concern_for_importing_data_with_a_column_mapped_to_subject_count : concern_for_DataImporter
   {
      protected override void Context()
      {
         base.Context();
         sut.IgnoreSheetNamesAtImport = true;
         _configuration.NamingConventions = "{Source}.{Id}";
         sut.AddGroupingColumn(_configuration, "Id");
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.SUBJECT_COUNT);
      }
   }

   public class When_importing_data_with_a_column_mapped_to_subject_count : concern_for_importing_data_with_a_column_mapped_to_subject_count
   {
      private IReadOnlyList<DataRepository> _dataRepositories;

      protected override void Because()
      {
         _dataRepositories = sut.ImportCsvFromConfiguration(_configuration, getFileFullName("subjectCountFromColumn.csv"), ';');
      }

      private DataRepository dataRepositoryOf(string id) => _dataRepositories.Single(x => x.ExtendedPropertyValueFor("Id") == id);

      [Observation]
      public void should_store_the_value_of_the_column_as_subject_count_of_each_data_set()
      {
         dataRepositoryOf("A").SubjectCount().ShouldBeEqualTo(5);
         dataRepositoryOf("B").SubjectCount().ShouldBeEqualTo(12);
      }

      [Observation]
      public void should_not_store_the_column_name_as_meta_data()
      {
         _dataRepositories.Each(x => x.ExtendedProperties.Contains("N").ShouldBeFalse());
      }
   }

   public class When_importing_data_named_by_a_grouping_column_that_was_mapped_to_subject_count : concern_for_DataImporter
   {
      private IReadOnlyList<DataRepository> _dataRepositories;

      protected override void Context()
      {
         base.Context();
         sut.IgnoreSheetNamesAtImport = true;
         _configuration.NamingConventions = "{Source}.{Id}.{N}";
         sut.AddGroupingColumn(_configuration, "Id");
         sut.AddGroupingColumn(_configuration, "N");
         sut.AddMetaDataColumn(_configuration, "N", Constants.ObservedData.SUBJECT_COUNT);
      }

      protected override void Because()
      {
         _dataRepositories = sut.ImportCsvFromConfiguration(_configuration, getFileFullName("subjectCountFromColumn.csv"), ';');
      }

      [Observation]
      public void should_name_the_data_sets_with_the_value_of_the_column()
      {
         _dataRepositories.Select(x => x.Name).ShouldOnlyContain("subjectCountFromColumn.A.5", "subjectCountFromColumn.B.12");
      }
   }

   public class When_importing_data_with_an_invalid_value_in_a_column_mapped_to_subject_count : concern_for_importing_data_with_a_column_mapped_to_subject_count
   {
      [Observation]
      public void should_throw_a_parsing_exception()
      {
         The.Action(() => sut.ImportCsvFromConfiguration(_configuration, getFileFullName("subjectCountFromColumn_invalid.csv"), ';'))
            .ShouldThrowAn<ImporterParsingException>();
      }
   }

   public class When_importing_data_with_conflicting_error_types_in_one_data_set : concern_for_DataImporter
   {
      [Observation]
      public void should_throw_a_parsing_exception()
      {
         sut.IgnoreSheetNamesAtImport = true;
         The.Action(() => sut.ImportCsvFromConfiguration(getFileFullName("errorTypeFromColumnConfiguration.xml"), getFileFullName("errorTypeFromColumn_conflict.csv"), ';'))
            .ShouldThrowAn<ImporterParsingException>();
      }
   }

   public class When_importing_an_arithmetic_data_set_with_a_geometric_default_error_type_without_unit : concern_for_DataImporter
   {
      [Observation]
      public void should_throw_a_parsing_exception_because_the_arithmetic_error_has_no_unit()
      {
         sut.IgnoreSheetNamesAtImport = true;
         The.Action(() => sut.ImportCsvFromConfiguration(getFileFullName("errorTypeFromColumnGeometricDefaultConfiguration.xml"), getFileFullName("errorTypeFromColumn.csv"), ';'))
            .ShouldThrowAn<ImporterParsingException>();
      }
   }
}
