using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Import;
using OSPSuite.Utility;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using System;
using System.IO;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_DataImporter : ContextForIntegration<IDataImporterTask>
   {
      protected override void Context()
      {
         base.Context();
         sut = Api.GetDataImporterTask();
      }

      protected string getFileFullName(string fileName) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName);
   }

   public class When_importing_data_from_r : concern_for_DataImporter
   {
      [Observation]
      public void should_throw_on_invalid_file_name()
      {
         The.Action(() => 
            sut.ImportXslxFromConfiguration(getFileFullName("importerConfiguration1.xml"), getFileFullName("sample_non_existent.xlsx"))
         ).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_on_invalid_file_format()
      {
         The.Action(() =>
            sut.ImportXslxFromConfiguration(getFileFullName("importerConfiguration1.xml"), getFileFullName("importerConfiguration1.xml"))
         ).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_import_simple_data()
      {
         sut.ImportXslxFromConfiguration(getFileFullName("importerConfiguration1.xml"), getFileFullName("sample1.xlsx")).Count.ShouldBeEqualTo(1);
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
         The.Action(() => sut.ImportXslxFromConfiguration(getFileFullName("importerConfiguration1.xml"), "")).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_read_configuration()
      {
         sut.GetConfiguration(getFileFullName("importerConfiguration1.xml")).ShouldNotBeNull();
      }

      [Observation]
      public void should_import_simple_data_with_configuration_object()
      {
         sut.ImportXslxFromConfiguration(sut.GetConfiguration(getFileFullName("importerConfiguration1.xml")), getFileFullName("sample1.xlsx")).Count.ShouldBeEqualTo(1);
      }


      [Observation]
      public void should_import_simple_data_from_csv_with_configuration_object()
      {
         sut.ImportCsvFromConfiguration(sut.GetConfiguration(getFileFullName("importerConfiguration1.csv.xml")), getFileFullName("sample1.csv"), ';').Count.ShouldBeEqualTo(1);
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
         sut.ImportXslxFromConfiguration(configuration, getFileFullName("sample1.xlsx")).ShouldNotBeNull();
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
   }
}
