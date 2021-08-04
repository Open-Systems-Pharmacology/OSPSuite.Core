using OSPSuite.BDDHelper;
using System;
using System.Collections.Generic;
using System.IO;
using OSPSuite.Assets;
using FakeItEasy;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Services;
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
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected DataImporterSettings _dataImporterSettings;
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected IDimension _molarConcentrationDimension;
      protected IDimension _massConcentrationDimension;
      protected IDimension _timeConcentrationDimension;
      protected IDimension _fractionConcentrationDimension;

      protected override void Context()
      {
         base.Context();
         _dialogCreator = A.Fake<IDialogCreator>();
         _importer = IoC.Container.Resolve<IImporter>();
         _applicationController = A.Fake<ApplicationController>();
         _dimensionFactory = IoC.Container.Resolve<IDimensionFactory>();

         sut = new DataImporter(_dialogCreator, _importer, _applicationController);

         _molarConcentrationDimension = _dimensionFactory.Dimension("Concentration (molar)");
         _massConcentrationDimension = _dimensionFactory.Dimension("Concentration (mass)");
         _timeConcentrationDimension = _dimensionFactory.Dimension("Time");
         _fractionConcentrationDimension = _dimensionFactory.Dimension("Fraction");
         _importerConfiguration = new ImporterConfiguration();
         _importerConfiguration.FileName = "IntegrationSample1.xlsx";
         _importerConfiguration.NamingConventions = "{Source}.{Sheet}.{Organ}.{Molecule}";

         _importerConfiguration.AddToLoadedSheets("Sheet1");
         _metaDataCategories = (IReadOnlyList<MetaDataCategory>)sut.DefaultMetaDataCategories();
         _dataImporterSettings = new DataImporterSettings();
         _dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation = "Molecule";
         _dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation = "Molecular Weight";
         _dataImporterSettings.IgnoreSheetNamesAtImport = true;
         _columnInfos = getDefaultColumnInfos();
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
         var parameterList = new List<DataFormatParameter>
         {
            new MappingDataFormatParameter("time  [h]",
               new Column() {Name = "Time", Dimension = _dimensionFactory.Dimension("Time"), Unit = new UnitDescription("h")}),
            new MappingDataFormatParameter("conc  [mg/l]",
               new Column()
               {
                  Name = "Concentration", Dimension = _massConcentrationDimension, Unit = new UnitDescription("mg/l")
               }),
            new MappingDataFormatParameter("SD [mg/l]",
               new Column()
               {
                  Name = "Error", ErrorStdDev = "Arithmetic Standard Deviation", Dimension = _massConcentrationDimension,
                  Unit = new UnitDescription("mg/l")
               }),
            new MetaDataFormatParameter("VenousBlood", "Organ", false),
            new MetaDataFormatParameter(null, "Molecule", false)
         };
         _importerConfiguration.CloneParametersFrom(parameterList);
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
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName(
               "IntegrationSample1.csv")).Count.ShouldBeEqualTo(1);
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
      public void should_throw_on_invalid_file_type()
      {
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName(
               "simple.pkml")).Count.ShouldBeEqualTo(0);
         A.CallTo(() => _dialogCreator.MessageBoxError(Error.UnsupportedFileType)).MustHaveHappened();
      }

      [Observation]
      public void should_return_empty_on_invalid_file_format()
      {
         sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName(
               "sample1.xlsx")).Count.ShouldBeEqualTo(0);
      }

      //currently not working
      /*
            [Observation]
            public void should_inform_on_missing_mapping()
            {
               sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
                  getFileFullName(
                     "IntegrationSampleMissingMapping.xlsx")).Count.ShouldBeEqualTo(0);
            }
      */
   }

   public class When_importing_data_from_incorrect_configuration : concern_for_DataImporter
   {
      protected override void Because()
      {
         var parameterList = new List<DataFormatParameter>
         {
            new MappingDataFormatParameter("time  [h]",
               new Column() {Name = "Time", Dimension = _dimensionFactory.Dimension("Time"), Unit = new UnitDescription("h")}),
            new MappingDataFormatParameter("conc  [mg/l]",
               new Column()
               {
                  Name = "Concentration", Dimension = _timeConcentrationDimension, Unit = new UnitDescription("s")
               }),
            new MappingDataFormatParameter("SD [mg/l]",
               new Column()
               {
                  Name = "Error", ErrorStdDev = "Arithmetic Standard Deviation", Dimension = _massConcentrationDimension,
                  Unit = new UnitDescription("mg/l")
               }),
            new MetaDataFormatParameter("VenousBlood", "Organ", false),
            new MetaDataFormatParameter(null, "Molecule", false)
         };
         _importerConfiguration.CloneParametersFrom(parameterList);
      }

      [Observation]
      public void should_throw_exception_when_trying_to_import()
      {
         The.Action(() =>
            sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings,
            getFileFullName(
               "IntegrationSample1.xlsx"))).ShouldThrowAn<OSPSuiteException>();
      }
   }

   public class When_importing_with_inconsistent_units_in_column : concern_for_DataImporter
   {
      protected override void Because()
      {
         var parameterList = new List<DataFormatParameter>
         {
            new MappingDataFormatParameter("time  [h]",
               new Column() {Name = "Time", Dimension = _dimensionFactory.Dimension("Time"), Unit = new UnitDescription("h")}),
            new MappingDataFormatParameter("conc  [mg/l]",
               new Column()
               {
                  Name = "Concentration", Dimension = _timeConcentrationDimension, Unit = new UnitDescription("s")
               }),
            new MappingDataFormatParameter("SD [mg/l]",
               new Column()
               {
                  Name = "Error", ErrorStdDev = "Arithmetic Standard Deviation", Dimension = _massConcentrationDimension,
                  Unit = new UnitDescription("mg/l")
               }),
            new MetaDataFormatParameter("VenousBlood", "Organ", false),
            new MetaDataFormatParameter(null, "Molecule", false)
         };
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

}

