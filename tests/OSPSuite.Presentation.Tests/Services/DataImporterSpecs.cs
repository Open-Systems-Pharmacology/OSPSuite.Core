using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Import;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Exceptions;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Presentation.Services
{
   public abstract class concern_for_DataImporter : ContextForIntegration<IDataImporter>
   {
      protected IDialogCreator _dialogCreator;
      protected IImporter _importer;
      protected IApplicationController _applicationController;
      protected IDimensionFactory _dimensionFactory;
      protected ImporterConfiguration _importerConfiguration;
      protected ImporterConfiguration _importerConfigurationMW;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected DataImporterSettings _dataImporterSettings;
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected IDimension _molarConcentrationDimension;
      protected IDimension _lengthDimension;
      protected IDimension _massConcentrationDimension;
      protected IDimension _timeConcentrationDimension;
      protected IDimension _fractionConcentrationDimension;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _dimensionFactory = IoC.Container.Resolve<IDimensionFactory>();
         _molarConcentrationDimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         _lengthDimension = _dimensionFactory.Dimension(Constants.Dimension.LENGTH);
         _massConcentrationDimension = _dimensionFactory.Dimension(Constants.Dimension.MASS_CONCENTRATION);
         _timeConcentrationDimension = _dimensionFactory.Dimension(Constants.Dimension.TIME);
         _fractionConcentrationDimension = _dimensionFactory.Dimension(Constants.Dimension.FRACTION);
      }

      protected override void Context()
      {
         base.Context();
         _dialogCreator = A.Fake<IDialogCreator>();
         _importer = IoC.Container.Resolve<IImporter>();
         _applicationController = A.Fake<ApplicationController>();

         sut = new DataImporter(_dialogCreator, _importer, _applicationController, _dimensionFactory);

         _importerConfiguration = new ImporterConfiguration
            { FileName = "IntegrationSample1.xlsx", NamingConventions = "{Source}.{Sheet}.{Organ}.{Molecule}" };
         _importerConfiguration.AddToLoadedSheets("Sheet1");
         _importerConfigurationMW = new ImporterConfiguration
            { FileName = "IntegrationSample1.xlsx", NamingConventions = "{Source}.{Sheet}.{Organ}.{Molecule}" };
         _importerConfigurationMW.AddToLoadedSheets("Sheet1");
         _metaDataCategories = (IReadOnlyList<MetaDataCategory>)sut.DefaultMetaDataCategoriesForObservedData();
         _dataImporterSettings = new DataImporterSettings();
         _dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation = "Molecule";
         _dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation = "Molecular Weight";
         _dataImporterSettings.IgnoreSheetNamesAtImport = true;
         _dataImporterSettings.CheckMolWeightAgainstMolecule = false;
         _columnInfos = getDefaultColumnInfos();

         _metaDataCategories.First(md => md.Name == _dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation).ListOfValues
            .Add("TestInputMolecule", "233");
      }

      private List<DataFormatParameter> createBaseParameters(string moleculeColumnName, UnitDescription timeUnitDescription)
      {
         var parameterList = new List<DataFormatParameter>
         {
            new MappingDataFormatParameter("time  [h]",
               new Column() { Name = "Time", Dimension = _dimensionFactory.Dimension("Time"), Unit = timeUnitDescription }),
            new MappingDataFormatParameter("SD [mg/l]",
               new Column()
               {
                  Name = "Error", ErrorStdDev = "Arithmetic Standard Deviation", Dimension = _massConcentrationDimension,
                  Unit = new UnitDescription("mg/l")
               }),
            new MetaDataFormatParameter("VenousBlood", "Organ", false),
            new MetaDataFormatParameter(moleculeColumnName, "Molecule", false)
         };
         return parameterList;
      }

      protected List<DataFormatParameter> createTestParameters(string moleculeColumnName, UnitDescription timeUnitDescription, UnitDescription concentrationUnitDescription, IDimension _timeConcentrationDimension)
      {
         var parameterList = new List<DataFormatParameter>
         {
            new MappingDataFormatParameter("time  [h]",
               new Column() { Name = "Time", Dimension = _dimensionFactory.Dimension("Time"), Unit = timeUnitDescription }),
            new MappingDataFormatParameter("conc  [mg/l]",
               new Column()
               {
                  Name = "Concentration", Dimension = _timeConcentrationDimension, Unit = concentrationUnitDescription
               }),
            new MappingDataFormatParameter("SD [mg/l]",
               new Column()
               {
                  Name = "Error", ErrorStdDev = "Arithmetic Standard Deviation", Dimension = _massConcentrationDimension,
                  Unit = new UnitDescription("mg/l")
               }),
            new MetaDataFormatParameter("VenousBlood", "Organ", false),
            new MetaDataFormatParameter(moleculeColumnName, "Molecule", false)
         };
         return parameterList;
      }

      protected List<DataFormatParameter> createParametersForUnitsFromColumn()
      {
         var parameterList = new List<DataFormatParameter>
         {
            new MappingDataFormatParameter("Time",
               new Column() { Name = "Time",
               Unit = new UnitDescription("h", "Time_unit")}),
            new MappingDataFormatParameter("Measurement",
               new Column()
               {
                  Name = "Concentration",
                  Unit = new UnitDescription("µm", "Measurement_unit")
               })
         };
         return parameterList;
      }

      protected List<DataFormatParameter> createTestParametersWithLLOQ(string moleculeColumnName, UnitDescription timeUnitDescription,
         UnitDescription concentrationUnitDescription, IDimension _timeConcentrationDimension)
      {
         var parameterList = createBaseParameters("TestInputMolecule", new UnitDescription("h"));

         parameterList.Add(new MappingDataFormatParameter("conc  [mg/l]",
            new Column()
            {
               Name = "Concentration",
               Dimension = _timeConcentrationDimension,
               Unit = concentrationUnitDescription,
               LloqColumn = "LLOQ"
            }));

         return parameterList;
      }

      protected List<DataFormatParameter> createTestParametersWithGroupByColumn(string moleculeColumnName, UnitDescription timeUnitDescription,
         UnitDescription concentrationUnitDescription, IDimension _timeConcentrationDimension)
      {
         var parameterList = createBaseParameters("TestInputMolecule", new UnitDescription("h"));

         parameterList.Add(new GroupByDataFormatParameter("GroupBy"));

         parameterList.Add(new MetaDataFormatParameter("Gender", "Gender", true));

         return parameterList;
      }


      protected string getFileFullName(string fileName) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName);

      private IReadOnlyList<ColumnInfo> getDefaultColumnInfos()
      {
         var columns = new List<ColumnInfo>();
         var timeColumn = createTimeColumn();

         columns.Add(timeColumn);

         var concentrationInfo = createConcentrationColumn(timeColumn);

         columns.Add(concentrationInfo);

         var errorInfo = createErrorColumn(timeColumn, concentrationInfo);

         columns.Add(errorInfo);

         return columns;
      }

      private ColumnInfo createTimeColumn()
      {
         var timeColumn = new ColumnInfo
         {
            DefaultDimension = _dimensionFactory.Dimension("Time"),
            Name = "Time",
            DisplayName = "Time",
            IsMandatory = true,
         };

         timeColumn.SupportedDimensions.Add(_timeConcentrationDimension);
         return timeColumn;
      }

      private ColumnInfo createConcentrationColumn(ColumnInfo timeColumn)
      {
         var concentrationInfo = new ColumnInfo
         {
            DefaultDimension = _molarConcentrationDimension,
            Name = "Concentration",
            DisplayName = "Concentration",
            IsMandatory = true,
            BaseGridName = timeColumn.Name
         };

         concentrationInfo.SupportedDimensions.Add(_lengthDimension);
         concentrationInfo.SupportedDimensions.Add(_molarConcentrationDimension);
         concentrationInfo.SupportedDimensions.Add(_massConcentrationDimension);
         concentrationInfo.SupportedDimensions.Add(_fractionConcentrationDimension);
         return concentrationInfo;
      }

      private ColumnInfo createErrorColumn(ColumnInfo timeColumn, ColumnInfo concentrationInfo)
      {
         var errorInfo = new ColumnInfo
         {
            DefaultDimension = _molarConcentrationDimension,
            Name = "Error",
            DisplayName = "Error",
            IsMandatory = false,
            BaseGridName = timeColumn.Name,
            RelatedColumnOf = concentrationInfo.Name
         };

         errorInfo.SupportedDimensions.Add(_molarConcentrationDimension);
         errorInfo.SupportedDimensions.Add(_massConcentrationDimension);
         errorInfo.SupportedDimensions.Add(_dimensionFactory.NoDimension);
         return errorInfo;
      }
   }

   public class When_importing_data_from_correct_configuration : concern_for_DataImporter
   {
      protected override void Because()
      {
         var parameterList = createTestParameters("TestInputMolecule", new UnitDescription("h"), new UnitDescription("mg/l"),
            _massConcentrationDimension);
         var parameterListMolecularWeight = new List<DataFormatParameter>(parameterList);
         parameterListMolecularWeight.Add(new MetaDataFormatParameter("Molecular Weight", "Molecular Weight", true));
         _importerConfiguration.CloneParametersFrom(parameterList);
         _importerConfigurationMW.CloneParametersFrom(parameterListMolecularWeight);
      }

      [Observation]
      public void should_import_simple_data_from_excel()
      {
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName(
               "IntegrationSample1.xlsx")).Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_import_simple_data_from_csv()
      {
         var csvSeparatorSelector = IoC.Resolve<ICsvSeparatorSelector>();
         A.CallTo(() => csvSeparatorSelector.GetCsvSeparator(A<string>.Ignored)).Returns(new CSVSeparators { ColumnSeparator = ';', DecimalSeparator = ',' });

         var importFromConfiguration = sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName(
               "IntegrationSample1.csv"));


         importFromConfiguration.Count.ShouldBeEqualTo(1);
         var column = importFromConfiguration.First().Columns.First(x => x.Name.Equals("Concentration"));
         column.ConvertToDisplayValues(column.Values).ShouldContain(0.2168224f, 0.4386293f);
      }

      [Observation]
      public void should_return_empty_on_invalid_file_name()
      {
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName(
               "")).Count.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_return_empty_on_non_existent_file_name()
      {
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName(
               "sample_non_existent.xlsx")).Count.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_correctly_notify_and_return_empty_on_invalid_file_type()
      {
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName(
               "simple.pkml")).Count.ShouldBeEqualTo(0);
         A.CallTo(() => _dialogCreator.MessageBoxError(Error.UnsupportedFileType)).MustHaveHappened();
      }

      [Observation]
      public void should_correctly_notify_and_return_empty_on_invalid_file_format()
      {
         var invalidFileName = getFileFullName("invalid.xlsx");
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings, invalidFileName).Count
            .ShouldBeEqualTo(0);
         A.CallTo(() => _dialogCreator.MessageBoxError(Error.UnsupportedFileFormat(invalidFileName))).MustHaveHappened();
      }


      [Observation]
      public void should_identify_empty_file_as_having_invalid_file_format()
      {
         var invalidFileName = getFileFullName("emptyFile.xlsx");
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings, invalidFileName).Count
            .ShouldBeEqualTo(0);
         A.CallTo(() => _dialogCreator.MessageBoxError(Error.InvalidObservedDataFile(Error.ImporterEmptyFile))).MustHaveHappened();
      }

      //so we should simply pass to the metadataCategories the value of the MW and check against it.
      //then check that the exception gets thrown when we have set teh dataImporterSettings (let's make this default)
      //and it does not get thrown when we are doing for MoBi. For MoBi also check that we get the correct MW (the one coming from the
      //excel, not the Molecule)
      [Observation]
      public void should_convert_MW_correctly_excel_not_checking()
      {
         var result =
            sut.ImportFromConfiguration(_importerConfigurationMW, _metaDataCategories, _columnInfos, _dataImporterSettings,
               getFileFullName(
                  "IntegrationSample1.xlsx"));
         result[0].AllButBaseGridAsArray[0].DataInfo.MolWeight.ShouldBeEqualTo(2.08E-07);
      }

      [Observation]
      public void should_not_allow_import_when_checking_MW_against_molecule()
      {
         _dataImporterSettings.CheckMolWeightAgainstMolecule = true;

         sut.ImportFromConfiguration(_importerConfigurationMW, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName(
               "IntegrationSample1.xlsx"));
         A.CallTo(() => _dialogCreator.MessageBoxError(Error.InconsistentMoleculeAndMolWeightException)).MustHaveHappened();
      }

      [Observation]
      public void should_correctly_get_MW_from_molecule()
      {
         _dataImporterSettings.CheckMolWeightAgainstMolecule = true;

         var result =
            sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
               getFileFullName(
                  "IntegrationSample1.xlsx"));
         result[0].AllButBaseGridAsArray[0].DataInfo.MolWeight.ShouldBeEqualTo(2.3300000000000001E-07d);
      }

      [Observation]
      public void should_convert_MW_correctly_csv()
      {
         var result =
            sut.ImportFromConfiguration(_importerConfigurationMW, _metaDataCategories, _columnInfos, _dataImporterSettings,
               getFileFullName(
                  "IntegrationSample1.csv"));
         result[0].AllButBaseGridAsArray[0].DataInfo.MolWeight.ShouldBeEqualTo(2.08E-07);
      }

      [Observation]
      public void should_filter_out_empty_column()
      {
         var result =
            sut.ImportFromConfiguration(_importerConfigurationMW, _metaDataCategories, _columnInfos, _dataImporterSettings,
               getFileFullName(
                  "IntegrationSampleMissingColumn.xlsx"));
         result[0].AllButBaseGridAsArray[0].InternalValues[3].ToDouble().ShouldBeEqualTo(0);
      }
   }

   public class When_importing_data_with_missing_columns : concern_for_DataImporter
   {
      protected override void Because()
      {
         var parameterList = createTestParameters(null, new UnitDescription("h"), new UnitDescription("mg/l"), _massConcentrationDimension);
         _importerConfiguration.CloneParametersFrom(parameterList);
      }

      [Observation]
      public void should_correctly_notify_the_user_on_missing_mapping()
      {
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName("IntegrationSampleMissingMapping.xlsx"));
         A.CallTo(() => _dialogCreator.MessageBoxError(
               "The mapped column(s) \n \n 'SD [mg/l]' \n \n is missing at least from the sheet \n \n 'Sheet1' \n \n that you are trying to load."))
            .MustHaveHappened();
      }

      [Observation]
      public void should_not_notify_on_preset_mapping()
      {
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName("IntegrationSampleMissingMapping.xlsx"));
         A.CallTo(() => _dialogCreator.MessageBoxError("The mapped column 'VenousBlood' is missing from at least one of the sheets being loaded."))
            .MustNotHaveHappened();
      }
   }

   public class When_importing_data_with_missing_unit_columns : concern_for_DataImporter
   {
      protected override void Because()
      {
         var parameterList = createTestParameters(null, new UnitDescription("h", "timeUnitColumn"), new UnitDescription("mg/l"),
            _massConcentrationDimension);
         _importerConfiguration.CloneParametersFrom(parameterList);
      }

      [Observation]
      public void should_set_unit_of_missing_column_to_undefined()
      {
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName("IntegrationSampleMissingMappingUnit.xlsx"));
         A.CallTo(() => _dialogCreator.MessageBoxError(
               "The mapped column(s) \n \n 'timeUnitColumn' \n \n is missing at least from the sheet \n \n 'Sheet1' \n \n that you are trying to load."))
            .MustHaveHappened();
      }
   }

   public class When_importing_with_inconsistent_units_in_column : concern_for_DataImporter
   {
      protected override void Because()
      {
         var parameterList =
            createTestParameters("TestInputMolecule", new UnitDescription("h"), new UnitDescription("s"), _timeConcentrationDimension);
         _importerConfiguration.CloneParametersFrom(parameterList);
      }

      [Observation]
      public void should_throw_exception()
      {
         The.Action(() =>
            sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
               getFileFullName(
                  "IntegrationSample1.xlsx"))).ShouldThrowAn<OSPSuiteException>();
      }
   }

   public class When_importing_empty_metadata_columns : concern_for_DataImporter
   {
      protected override void Because()
      {
         var parameterList = createTestParameters("TestInputMolecule", new UnitDescription("h"), new UnitDescription("mg/l"),
            _massConcentrationDimension);
         parameterList.Add(new MetaDataFormatParameter("Dose", "Dose", true));
         _importerConfiguration.CloneParametersFrom(parameterList);
      }

      [Observation]
      public void should_not_import_empty_metadata()
      {
         var result = sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName(
               "IntegrationSample1.xlsx"));
         result.First().ExtendedProperties.Contains("Dose").ShouldBeFalse();
      }
   }

   public class When_importing_with_non_existent_excel_columns : concern_for_DataImporter
   {
      protected override void Because()
      {
         var parameterList =createTestParametersWithLLOQ("TestInputMolecule", new UnitDescription("h"), new UnitDescription("mg/l"),
            _massConcentrationDimension);
         _importerConfiguration.CloneParametersFrom(parameterList);
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName(
               "IntegrationSample1.xlsx"));
      }

      [Observation]
      public void should_throw_exception()
      {
         A.CallTo(() => _dialogCreator.MessageBoxError("The mapped column(s) \n \n 'LLOQ' \n \n is missing at least from the sheet \n \n 'Sheet1' \n \n that you are trying to load.")).MustHaveHappened();
      }
   }

   public class When_importing_excel_with_goupBy_defined_before_mapping : concern_for_DataImporter
   {
      protected override void Because()
      {
         var parameterList = createTestParametersWithGroupByColumn("TestInputMolecule", new UnitDescription("h"), new UnitDescription("mg/l"),
            _massConcentrationDimension);
         _importerConfiguration.CloneParametersFrom(parameterList);
      }

      [Observation]
      public void should_import_values_correctly()
      {
         var result = sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName(
               "IntegrationSampleWithGroupBy.xlsx"));
         result.First().ExtendedProperties["Gender"].ValueAsObject.ToString().ShouldBeEqualTo("F");
      }
   }

   public class When_trying_to_import_file_with_duplicate_headers : concern_for_DataImporter
   {
      protected override void Because()
      {
         var parameterList = createTestParameters("TestInputMolecule", new UnitDescription("h"), new UnitDescription("mg/l"),
            _massConcentrationDimension);
         var parameterListMolecularWeight = new List<DataFormatParameter>(parameterList);
         _importerConfiguration.CloneParametersFrom(parameterList);
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings, getFileFullName("IntegrationSampleDuplicateHeader.xlsx"));
      }

      [Observation]
      public void should_import_simple_data_from_excel()
      {
         A.CallTo(() => _dialogCreator.MessageBoxError(A<string>.That.Contains("In sheet Sheet1 the headers"))).MustHaveHappened();
         A.CallTo(() => _dialogCreator.MessageBoxError(A<string>.That.Contains("Dose"))).MustHaveHappened();
      }
   }

   public class When_importing_data_with_unit_from_column_data : concern_for_DataImporter
   {
      private ImporterConfiguration _importerConfigurationUnitsFromColumn;
      protected override void Because()
      {
         _importerConfigurationUnitsFromColumn = new ImporterConfiguration
            { FileName = "IntegrationSampleUnitFromColumn.xlsx", NamingConventions = "{Source}.{Sheet}.{Organ}.{Molecule}" };
         var parameterList = createParametersForUnitsFromColumn();
         _importerConfigurationUnitsFromColumn.CloneParametersFrom(parameterList);
      }

      [Observation]
      public void should_correctly_assign_the_dimension()
      {
         var result = sut.ImportFromConfiguration(_importerConfigurationUnitsFromColumn, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName("IntegrationSampleUnitFromColumn.xlsx")); 
         result.First().AllButBaseGridAsArray.First().Dimension.ShouldBeEqualTo(_lengthDimension);
      }
   }
}