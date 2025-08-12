using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataSourceFileReaders;
using OSPSuite.Infrastructure.Import.Core.Exceptions;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Collections;
using IContainer = OSPSuite.Utility.Container.IContainer;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Presentation.Importer.Presenters
{
   internal class ImporterPresenterForTest : ImporterPresenter
   {
      public bool OnResetMappingBasedOnCurrentSheetInvoked { get; set; }

      public ImporterPresenterForTest(IImporterView view,
         IDataSetToDataRepositoryMapper dataRepositoryMapper,
         IImporter importer,
         INanPresenter nanPresenter,
         IImporterDataPresenter importerDataPresenter,
         IImportPreviewPresenter previewPresenter,
         IColumnMappingPresenter columnMappingPresenter,
         ISourceFilePresenter sourceFilePresenter,
         IDialogCreator dialogCreator,
         IPKMLPersistor pkmlPersistor,
         IDataSource dataSource, 
         IDataSourceToDimensionSelectionDTOMapper dataSourceToDimensionSelectionDTOMapper,
         IDimensionMappingPresenter dimensionMappingPresenter) :  base(view, 
         dataRepositoryMapper, 
         importer, 
         nanPresenter, 
         importerDataPresenter, 
         previewPresenter, 
         columnMappingPresenter,
         sourceFilePresenter, 
         dialogCreator, 
         pkmlPersistor, 
         dimensionMappingPresenter, 
         dataSourceToDimensionSelectionDTOMapper)
      {
         _dataSource = dataSource;
      }

      protected override bool ConfirmDroppingOfLoadedSheets()
      {
         return false;
      }
   }

   public abstract class concern_for_ImporterPresenter : ContextSpecification<ImporterPresenter>
   {
      protected ImportTriggeredEventArgs _eventArgs;
      protected List<MetaDataCategory> _metaDataCategories;
      protected DataImporterSettings _dataImporterSettings;
      protected IImporter _importer;
      protected IImporterView _importerView;
      protected INanPresenter _nanPresenter;
      protected IDataSetToDataRepositoryMapper _mapper;
      protected IImporterDataPresenter _importerDataPresenter;
      protected IImportPreviewPresenter _importPreviewPresenter;
      protected IDataSource _dataSource;
      protected IColumnMappingPresenter _columnMappingPresenter;
      protected ISourceFilePresenter _sourceFilePresenter;
      protected IDialogCreator _dialogCreator;
      protected IOSPSuiteXmlSerializerRepository _ospSuiteXmlSerializerRepository;
      protected IContainer _container;
      protected IDataSourceFile _dataSourceFile;
      protected ImporterConfiguration _importerConfiguration;
      protected IPKMLPersistor _pkmlPeristor;
      protected IDimensionFactory _dimensionFactory;
      protected IDataSourceToDimensionSelectionDTOMapper _dataSourceToDimensionSelectionDTOMapper;
      protected IDimensionMappingPresenter _dimensionMappingPresenter;

      protected override void Context()
      {
         _dimensionFactory = new DimensionFactoryForIntegrationTests();
         _dimensionMappingPresenter = A.Fake<IDimensionMappingPresenter>();
         _dataSourceToDimensionSelectionDTOMapper = A.Fake<IDataSourceToDimensionSelectionDTOMapper>();
         _dataImporterSettings = new DataImporterSettings();
         base.Context();
         _mapper = A.Fake<IDataSetToDataRepositoryMapper>();
         var cache = new Cache<string, IDataSet>();
         var dataSet = new DataSet();
         dataSet.AddData(new List<ParsedDataSet>()
         {
            new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(),
               new Dictionary<ExtendedColumn, IList<SimulationPoint>>())
         });
         _dataSource = A.Fake<IDataSource>();
         A.CallTo(() => _dataSource.DataSets).Returns(cache);
         cache.Add("sheet1", dataSet);
         var dataRepository = new DataRepository { Name = "name" };
         dataRepository.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = "Molecule", Value = "Molecule1" });
         dataRepository.ExtendedProperties.Add(new ExtendedProperty<string>() { Name = "Mol weight", Value = 22.0.ToString() });

         var dataColumn = new BaseGrid("Time", A.Fake<IDimension>());
         var dataInfo = new DataInfo(ColumnOrigins.Undefined);
         dataColumn.DataInfo = dataInfo;
         dataRepository.Add(dataColumn);

         var moleculeDataColumn = new DataColumn("Measurement", A.Fake<IDimension>(), dataColumn);
         dataColumn.DataInfo = dataInfo;
         dataRepository.Add(moleculeDataColumn);
         A.CallTo(() => _mapper.ConvertImportDataSet(A<ImportedDataSet>.Ignored)).Returns(new DataSetToDataRepositoryMappingResult(dataRepository));

         var moleculeMetaDataCategory = createMetaDataCategory<string>("Molecule", isMandatory: true);
         moleculeMetaDataCategory.IsListOfValuesFixed = true;
         moleculeMetaDataCategory.DefaultValue = "Molecule1";
         moleculeMetaDataCategory.ListOfValues.Add("Molecule1", 6.0.ToString());
         moleculeMetaDataCategory.ShouldListOfValuesBeIncluded = true;
         moleculeMetaDataCategory.SelectDefaultValue = true;

         _metaDataCategories = new List<MetaDataCategory>()
         {
            moleculeMetaDataCategory,
            createMetaDataCategory<string>("Mol weight", isMandatory: false)
         };
         var dataFormat = A.Fake<IDataFormat>();
         A.CallTo(() => dataFormat.Parameters).Returns(new List<DataFormatParameter>());
         _dataSourceFile = A.Fake<IDataSourceFile>();
         A.CallTo(() => _dataSourceFile.Format).Returns(dataFormat);
         _importerDataPresenter = A.Fake<IImporterDataPresenter>();
         A.CallTo(() => _importerDataPresenter.SetDataSource(A<string>.Ignored)).Returns(_dataSourceFile);
         _importerView = A.Fake<IImporterView>();
         _importer = A.Fake<IImporter>();
         _nanPresenter = A.Fake<INanPresenter>();
         _importPreviewPresenter = A.Fake<IImportPreviewPresenter>();
         _columnMappingPresenter = A.Fake<IColumnMappingPresenter>();
         _sourceFilePresenter = A.Fake<ISourceFilePresenter>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _pkmlPeristor = A.Fake<IPKMLPersistor>();

         sut = new ImporterPresenterForTest(
            _importerView,
            _mapper,
            _importer,
            _nanPresenter,
            _importerDataPresenter,
            _importPreviewPresenter,
            _columnMappingPresenter,
            _sourceFilePresenter,
            _dialogCreator,
            _pkmlPeristor,
            _dataSource, _dataSourceToDimensionSelectionDTOMapper, _dimensionMappingPresenter);
         _importerConfiguration = A.Fake<ImporterConfiguration>();
         sut.LoadConfiguration(_importerConfiguration, "");
         sut.SetSettings(_metaDataCategories, new ColumnInfoCache(), _dataImporterSettings);
      }

      protected static MetaDataCategory createMetaDataCategory<T>(string descriptiveName, bool isMandatory = false, bool isListOfValuesFixed = false,
         Action<MetaDataCategory> fixedValuesRetriever = null)
      {
         var category = new MetaDataCategory
         {
            Name = descriptiveName,
            DisplayName = descriptiveName,
            Description = descriptiveName,
            MetaDataType = typeof(T),
            IsMandatory = isMandatory,
            IsListOfValuesFixed = isListOfValuesFixed
         };

         fixedValuesRetriever?.Invoke(category);

         return category;
      }
   }

   public class When_setting_settings : concern_for_ImporterPresenter
   {
      protected ColumnInfoCache _columnInfos = new ColumnInfoCache();

      protected override void Because()
      {
         sut.SetSettings(_metaDataCategories, _columnInfos, _dataImporterSettings);
      }

      [Observation]
      public void sets_column_mapping_presenter_settings()
      {
         A.CallTo(() => _columnMappingPresenter.SetSettings(_metaDataCategories, _columnInfos)).MustHaveHappened();
      }

      [Observation]
      public void sets_importer_data_presenter_settings()
      {
         A.CallTo(() => _importerDataPresenter.SetSettings(_metaDataCategories, _columnInfos)).MustHaveHappened();
      }
   }

   public class When_setting_data_source : concern_for_ImporterPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _importerDataPresenter.SetDataSource(A<string>.Ignored)).Returns(A.Fake<IDataSourceFile>());
      }

      protected override void Because()
      {
         sut.SetDataSource("path");
      }

      [Observation]
      public void sets_column_mapping_presenter_settings()
      {
         A.CallTo(() => _dialogCreator.MessageBoxError(A<string>.Ignored)).MustNotHaveHappened();
      }
   }

   public class When_setting_data_source_with_empty_rows : concern_for_ImporterPresenter
   {
      protected Cache<string, DataSheet> _sheets;

      protected override void Context()
      {
         base.Context();
         _sheets = new Cache<string, DataSheet>();
         _sheets.Add("Sheet1", A.Fake<DataSheet>());

         _dataSourceFile = new ExcelDataSourceFile(A.Fake<IImportLogger>(), new HeavyWorkManagerForSpecs());
         _dataSourceFile.Format = A.Fake<IDataFormat>();
         _dataSourceFile.Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "IntegrationSampleUnitFromColumn.xlsx");
         A.CallTo(() => _importerDataPresenter.SetDataSource(A<string>.Ignored)).Returns(_dataSourceFile);
         _importerDataPresenter.OnImportSheets += Raise.With(new ImportSheetsEventArgs()
            { Filter = "", DataSourceFile = _dataSourceFile, SheetNames = _sheets.Keys.ToList() });
      }

      [Observation]
      public void should_not_throw_an_ImporterParsingException()
      {
         sut.LoadConfiguration(_importerConfiguration, "");
      }
   }

   public class When_import_data : concern_for_ImporterPresenter
   {
      protected bool triggered = false;

      protected override void Context()
      {
         base.Context();
         sut.UpdateAndGetConfiguration().Id = null;
         sut.OnTriggerImport += (o, a) => triggered = true;
      }

      protected override void Because()
      {
         sut.ImportData(A.Fake<object>(), A.Fake<EventArgs>());
      }

      [Observation]
      public void generates_ids()
      {
         sut.UpdateAndGetConfiguration().Id.ShouldNotBeNull();
      }

      [Observation]
      public void triggers_import()
      {
         triggered.ShouldBeTrue();
      }
   }

   public class When_format_changed : concern_for_ImporterPresenter
   {
      protected FormatChangedEventArgs _args;

      protected override void Context()
      {
         base.Context();
         var dataFormat = A.Fake<IDataFormat>();
         A.CallTo(() => dataFormat.Parameters).Returns(Enumerable.Empty<DataFormatParameter>().ToList());
         _args = new FormatChangedEventArgs() { Format = dataFormat };
      }

      protected override void Because()
      {
         _importerDataPresenter.OnFormatChanged += Raise.With(_args);
      }

      [Observation]
      public void invokes_column_mapping_presenter()
      {
         A.CallTo(() => _columnMappingPresenter.SetDataFormat(_args.Format)).MustHaveHappened();
      }
   }

   public class When_tab_changed : concern_for_ImporterPresenter
   {
      protected TabChangedEventArgs _args;

      protected override void Context()
      {
         base.Context();
         _args = new TabChangedEventArgs() { TabSheet = new DataSheet() };
      }

      protected override void Because()
      {
         _importerDataPresenter.OnTabChanged += Raise.With(_args);
      }

      [Observation]
      public void invokes_column_mapping_presenter()
      {
         A.CallTo(() => _columnMappingPresenter.SetRawData(_args.TabSheet)).MustHaveHappened();
      }
   }

   public class When_mapping_completed_with_loaded_data : concern_for_ImporterPresenter
   {
      protected Cache<string, DataSheet> _sheets;
      private ColumnInfo _columnInfo;

      protected override void Context()
      {
         base.Context();
         _sheets = new Cache<string, DataSheet> { { "sheet1", A.Fake<DataSheet>() } };
         _columnInfo = new ColumnInfo();
         var dto = new DimensionSelectionDTO("sheet", new[] { string.Empty }, _columnInfo, new List<IDimension> { DomainHelperForSpecs.ConcentrationDimensionForSpecs(), DomainHelperForSpecs.ConcentrationMassDimensionForSpecs() });

         A.CallTo(() => _dataSourceToDimensionSelectionDTOMapper.MapFrom(A<IDataSource>._, A<IReadOnlyList<string>>._)).Returns(new List<DimensionSelectionDTO> { dto });
         A.CallTo(() => _dimensionMappingPresenter.EditUnitToDimensionMap(A<IReadOnlyList<DimensionSelectionDTO>>._)).Invokes(x => dto.SelectedDimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs());
         _importerDataPresenter.OnImportSheets += Raise.With(new ImportSheetsEventArgs
            { Filter = "", DataSourceFile = _dataSourceFile, SheetNames = _sheets.Keys.ToList() });
         _columnMappingPresenter.OnMappingCompleted += Raise.With(new EventArgs());
      }

      [Observation]
      public void the_column_dimension_should_be_set_to_the_selected_dimension_of_the_dto()
      {
         _columnInfo.MappedDimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimensionForSpecs());
      }

      [Observation]
      public void the_dimension_mapping_presenter_is_used_for_ambiguous_mappings()
      {
         A.CallTo(() => _dimensionMappingPresenter.EditUnitToDimensionMap(A<IReadOnlyList<DimensionSelectionDTO>>._)).MustHaveHappened();
      }

      [Observation]
      public void the_dimension_mapping_task_is_used_to_auto_map_the_dimensions()
      {
         A.CallTo(() => _dataSourceToDimensionSelectionDTOMapper.MapFrom(A<IDataSource>._, A<IReadOnlyList<string>>._)).MustHaveHappened();
      }

      [Observation]
      public void invokes_column_mapping_presenter()
      {
         A.CallTo(() => _importerDataPresenter.OnCompletedMapping()).MustHaveHappened();
      }

      [Observation]
      public void shows_confirmation_view()
      {
         A.CallTo(() => _importerView.EnablePreviewView()).MustHaveHappened();
      }
   }

   public class When_mapping_completed_without_loaded_data : concern_for_ImporterPresenter
   {
      protected override void Because()
      {
         _columnMappingPresenter.OnMappingCompleted += Raise.With(new EventArgs());
      }

      [Observation]
      public void does_not_show_confirmation_view()
      {
         A.CallTo(() => _importerView.EnablePreviewView()).MustNotHaveHappened();
      }
   }

   public class When_mapping_completed_with_loaded_data_with_wrong_mapping : concern_for_ImporterPresenter
   {
      protected Cache<string, DataSheet> _sheets;

      protected override void Because()
      {
         var errors = new ParseErrors();
         errors.Add(new DataSet(), new List<ParseErrorDescription>() { new MismatchingArrayLengthsParseErrorDescription() });
         A.CallTo(() => _dataSource.AddSheets(A<DataSheetCollection>._, A<ColumnInfoCache>.Ignored, A<string>.Ignored)).Returns(errors);
         _sheets = new Cache<string, DataSheet>();
         _sheets.Add("sheet1", A.Fake<DataSheet>());
         _importerDataPresenter.OnImportSheets += Raise.With(new ImportSheetsEventArgs()
            { Filter = "", DataSourceFile = _dataSourceFile, SheetNames = _sheets.Keys.ToList() });
         _columnMappingPresenter.OnMappingCompleted += Raise.With(new EventArgs());
      }

      [Observation]
      public void does_not_show_confirmation_view()
      {
         A.CallTo(() => _importerView.EnablePreviewView()).MustNotHaveHappened();
      }
   }

   public class When_mapping_is_missing : concern_for_ImporterPresenter
   {
      protected override void Because()
      {
         _columnMappingPresenter.OnMissingMapping += Raise.With(new MissingMappingEventArgs());
      }

      [Observation]
      public void invokes_importer_data_presenter()
      {
         A.CallTo(() => _importerDataPresenter.OnMissingMapping()).MustHaveHappened();
      }

      [Observation]
      public void hides_confirmation_view()
      {
         A.CallTo(() => _importerView.DisableConfirmationView()).MustHaveHappened();
      }
   }

   public class When_setting_empty_wrong_source_file : concern_for_ImporterPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _importerDataPresenter.SetDataSource(A<string>.Ignored)).Returns(null);
      }

      [Observation]
      public void returns_false()
      {
         var path = "path";
         sut.SetSourceFile(path).ShouldBeFalse();
         A.CallTo(() => _sourceFilePresenter.SetFilePath(path)).MustNotHaveHappened();
      }
   }

   public class When_setting_empty_source_file : concern_for_ImporterPresenter
   {
      protected string path = "path";

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _importerDataPresenter.SetDataSource(A<string>.Ignored)).Returns(A.Fake<IDataSourceFile>());
      }

      protected override void Because()
      {
         sut.SetSourceFile(path);
      }

      [Observation]
      public void must_set_file_path_on_source_file_presenter()
      {
         A.CallTo(() => _sourceFilePresenter.SetFilePath(path)).MustHaveHappened();
      }

      [Observation]
      public void must_validate_mapping()
      {
         A.CallTo(() => _columnMappingPresenter.ValidateMapping()).MustHaveHappened();
      }
   }

   public class When_loading_configuration : concern_for_ImporterPresenter
   {
      private Cache<string, DataSheet> _sheets;

      protected override void Context()
      {
         base.Context();
         _sheets = new Cache<string, DataSheet>();
         _sheets.Add("sheet1", A.Fake<DataSheet>());
         var dataFormat = A.Fake<IDataFormat>();
         A.CallTo(() => dataFormat.Parameters).Returns(new List<DataFormatParameter>()
         {
            new MetaDataFormatParameter(null, "id1", false),
            new MetaDataFormatParameter("value", "id2", false)
         });
         A.CallTo(() => _dataSourceFile.Format).Returns(dataFormat);
      }

      protected override void Because()
      {
         _importerDataPresenter.OnImportSheets += Raise.With(new ImportSheetsEventArgs()
            { Filter = "", DataSourceFile = _dataSourceFile, SheetNames = _sheets.Keys.ToList() });
      }

      [Observation]
      public void must_filter_empty_mappings()
      {
         //id1 should be filtered from the mappings since it has null in the columnName
         A.CallTo(() => _dataSource.SetMappings
            (
               A<string>.Ignored,
               A<IReadOnlyList<MetaDataMappingConverter>>.That.Matches(c => c.All(m => m.Id != "id1")))
         ).MustHaveHappened();
      }
   }

   public class When_loading_configuration_from_button : concern_for_ImporterPresenter
   {
      protected override void Because()
      {
         (sut as ImporterPresenterForTest).OnResetMappingBasedOnCurrentSheetInvoked = false;
         sut.LoadConfigurationWithoutImporting();
      }
   }
}