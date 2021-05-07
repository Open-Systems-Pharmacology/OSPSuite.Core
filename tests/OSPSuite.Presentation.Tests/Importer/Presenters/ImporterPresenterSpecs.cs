using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
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

   public abstract class ConcernForImporterPresenter : ContextSpecification<ImporterPresenter>
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
         A.CallTo(() => _mapper.ConvertImportDataSet(A<ImportedDataSet>.Ignored)).Returns(dataRepository);

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
         var dataSourceFile = A.Fake<IDataSourceFile>();
         A.CallTo(() => dataSourceFile.Format).Returns(dataFormat);
         _importerDataPresenter = A.Fake<IImporterDataPresenter>();
         A.CallTo(() => _importerDataPresenter.SetDataSource(A<string>.Ignored)).Returns(dataSourceFile);
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

   public class When_importing_data : ConcernForImporterPresenter
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

      //ToDo: move to ImporterSpecs
      /*
      [Observation]
      public void should_not_invoke_when_inconsistent_mol_weight()
      {
         dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation = "Molecule";
         dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation = "Mol weight";
         var importerPresenter = A.Fake<IImporterPresenter>();
         ImportTriggeredEventArgs result = null;
         sut.OnTriggerImport += (_, e) => result = e;
         sut.ImportData(this, null);

         //to check how an exception is being tested in unit tests
         //A.CallTo(Sut.ImportData()).Throws<>()

         Assert.IsNull(result);
      }
*/

      [Observation]
      public void sets_molWeight_from_molWeight()
      {
         _dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation = "Mol weight";
         var importerPresenter = A.Fake<IImporterPresenter>();
         ImportTriggeredEventArgs result = null;
         sut.OnTriggerImport += (_, e) => result = e;
         sut.ImportData(this, null);
         var molWeight = 22.0;
         Assert.IsTrue(result.DataRepositories.All(dr => dr.AllButBaseGrid().All(x => x.DataInfo.MolWeight == molWeight)));
      }
   }

}