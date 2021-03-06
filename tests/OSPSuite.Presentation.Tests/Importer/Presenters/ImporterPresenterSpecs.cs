﻿using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Presentation.Importer.Presenters
{
   internal class ImporterPresenterForTest : ImporterPresenter
   {
      public ImporterPresenterForTest(
         IImporterView view,
         IDataSetToDataRepositoryMapper dataRepositoryMapper,
         IImporter importer,
         INanPresenter nanPresenter,
         IImporterDataPresenter importerDataPresenter,
         IImportConfirmationPresenter confirmationPresenter,
         IColumnMappingPresenter columnMappingPresenter,
         ISourceFilePresenter sourceFilePresenter,
         IDialogCreator dialogCreator,
         IDimensionFactory dimensionFactory,
         IOSPSuiteXmlSerializerRepository modelingXmlSerializerRepository,
         Utility.Container.IContainer container,
         IDataSource dataSource
      ) : base(view, dataRepositoryMapper, importer, nanPresenter, importerDataPresenter, confirmationPresenter, columnMappingPresenter, sourceFilePresenter, dialogCreator, dimensionFactory, modelingXmlSerializerRepository, container)
      {
         _dataSource = dataSource;
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
      protected IImportConfirmationPresenter _importConfirmationPresenter;
      protected IDataSource _dataSource;
      protected IColumnMappingPresenter _columnMappingPresenter;
      protected ISourceFilePresenter _sourceFilePresenter;
      protected IDialogCreator _dialogCreator;
      protected IDimensionFactory _dimensionFactory;
      protected IOSPSuiteXmlSerializerRepository _ospSuiteXmlSerializerRepository;
      protected Utility.Container.IContainer _container;
      protected IDataSourceFile _dataSourceFile;

      protected override void Context()
      {
         _dataImporterSettings = new DataImporterSettings();
         base.Context();
         _mapper = A.Fake<IDataSetToDataRepositoryMapper>();
         var cache = new Cache<string, IDataSet>();
         var dataSet = new DataSet();
         dataSet.AddData(new List<ParsedDataSet>()
         {
            new ParsedDataSet(new List<(string ColumnName, IList<string> ExistingValues)>(), A.Fake<IUnformattedData>(), new List<UnformattedRow>(), new Dictionary<ExtendedColumn, IList<SimulationPoint>>())
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
         _importConfirmationPresenter = A.Fake<IImportConfirmationPresenter>();
         _columnMappingPresenter = A.Fake<IColumnMappingPresenter>();
         _sourceFilePresenter = A.Fake<ISourceFilePresenter>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _ospSuiteXmlSerializerRepository = A.Fake<IOSPSuiteXmlSerializerRepository>();
         _container = A.Fake<Utility.Container.IContainer>();
         sut = new ImporterPresenterForTest(
            _importerView,
            _mapper,
            _importer,
            _nanPresenter,
            _importerDataPresenter,
            _importConfirmationPresenter,
            _columnMappingPresenter,
            _sourceFilePresenter,
            _dialogCreator,
            _dimensionFactory,
            _ospSuiteXmlSerializerRepository,
            _container,
            _dataSource);
         sut.LoadConfiguration(A.Fake<OSPSuite.Core.Import.ImporterConfiguration>(), "");
         sut.SetSettings(_metaDataCategories, new List<ColumnInfo>(), _dataImporterSettings);
      }

      protected static MetaDataCategory createMetaDataCategory<T>(string descriptiveName, bool isMandatory = false, bool isListOfValuesFixed = false, Action<MetaDataCategory> fixedValuesRetriever = null)
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

   public class When_importing_data : concern_for_ImporterPresenter
   {
      [Observation]
      public void sets_molWeight_from_molecule()
      {
         _dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation = "Molecule";
         ImportTriggeredEventArgs result = null;
         sut.OnTriggerImport += (_, e) => result = e;
         sut.ImportData(this, null);
         var molWeight = 6.0;
         Assert.IsTrue(result.DataRepositories.All(dr => dr.AllButBaseGrid().All(x => x.DataInfo.MolWeight == molWeight)));
      }

      [Observation]
      public void sets_molWeight_from_molWeight()
      {
         _dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation = "Mol weight";
         ImportTriggeredEventArgs result = null;
         sut.OnTriggerImport += (_, e) => result = e;
         sut.ImportData(this, null);
         var molWeight = 22.0;
         Assert.IsTrue(result.DataRepositories.All(dr => dr.AllButBaseGrid().All(x => x.DataInfo.MolWeight == molWeight)));
      }
   }

   public class When_setting_settings : concern_for_ImporterPresenter
   {
      protected IReadOnlyList<ColumnInfo> _columnInfos = A.Fake<IReadOnlyList<ColumnInfo>>();

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
         _args = new TabChangedEventArgs() { TabData = new UnformattedData() };
      }

      protected override void Because()
      {
         _importerDataPresenter.OnTabChanged += Raise.With(_args);
      }

      [Observation]
      public void invokes_column_mapping_presenter()
      {
         A.CallTo(() => _columnMappingPresenter.SetRawData(_args.TabData)).MustHaveHappened();
      }
   }

   public class When_mapping_completed_with_loaded_data : concern_for_ImporterPresenter
   {
      protected Cache<string, DataSheet> _sheets;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dataSource.ValidateDataSource(A<IReadOnlyList<ColumnInfo>>.Ignored, A<IDimensionFactory>.Ignored)).Returns(true);
         _sheets = new Cache<string, DataSheet>();
         _sheets.Add("sheet1", A.Fake<DataSheet>());
         _importerDataPresenter.OnImportSheets += Raise.With(new ImportSheetsEventArgs() { Filter = "", DataSourceFile = _dataSourceFile, Sheets = _sheets });
         _columnMappingPresenter.OnMappingCompleted += Raise.With(new EventArgs());
      }

      [Observation]
      public void invokes_column_mapping_presenter()
      {
         A.CallTo(() => _importerDataPresenter.onCompletedMapping()).MustHaveHappened();
      }

      [Observation]
      public void shows_confirmation_view()
      {
         A.CallTo(() => _importerView.EnableConfirmationView()).MustHaveHappened();
      }
   }

   public class When_mapping_completed_without_loaded_data : concern_for_ImporterPresenter
   {
      protected override void Because()
      {
         A.CallTo(() => _dataSource.ValidateDataSource(A<IReadOnlyList<ColumnInfo>>.Ignored, A<IDimensionFactory>.Ignored)).Returns(true);
         _columnMappingPresenter.OnMappingCompleted += Raise.With(new EventArgs());
      }

      [Observation]
      public void does_not_show_confirmation_view()
      {
         A.CallTo(() => _importerView.EnableConfirmationView()).MustNotHaveHappened();
      }
   }

   public class When_mapping_completed_with_loaded_data_with_wrong_mapping : concern_for_ImporterPresenter
   {
      protected Cache<string, DataSheet> _sheets;

      protected override void Because()
      {
         A.CallTo(() => _dataSource.ValidateDataSource(A<IReadOnlyList<ColumnInfo>>.Ignored, A<IDimensionFactory>.Ignored)).Returns(false);
         _sheets = new Cache<string, DataSheet>();
         _sheets.Add("sheet1", A.Fake<DataSheet>());
         _importerDataPresenter.OnImportSheets += Raise.With(new ImportSheetsEventArgs() { Filter = "", DataSourceFile = _dataSourceFile, Sheets = _sheets });
         _columnMappingPresenter.OnMappingCompleted += Raise.With(new EventArgs());
      }

      [Observation]
      public void does_not_show_confirmation_view()
      {
         A.CallTo(() => _importerView.EnableConfirmationView()).MustNotHaveHappened();
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
         A.CallTo(() => _importerDataPresenter.onMissingMapping()).MustHaveHappened();
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
}
