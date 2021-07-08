using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_DataImporter : ContextForIntegration<IDataImporter>
   {
      protected ImporterConfiguration _importerConfiguration;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected DataImporterSettings _dataImporterSettings;
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      private IOSPSuiteXmlSerializerRepository _modelingXmlSerializerRepository;

      protected override void Context()
      {
         base.Context();
         sut = Api.GetDataImporter();
         _metaDataCategories = (IReadOnlyList<MetaDataCategory>)sut.DefaultMetaDataCategories();
         _dataImporterSettings = new DataImporterSettings();
         _dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation = "Molecule";
         _dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation = "Molecular Weight";
         _columnInfos = ((DataImporter)sut).DefaultPKSimImportConfiguration();
         _modelingXmlSerializerRepository = Api.GetOSPSuiteXmlSerializerRepository();
      }

      protected ImporterConfiguration getConfiguration(string fileName)
      {
         using (var serializationContext = SerializationTransaction.Create(Api.Container, Api.GetDimensionFactory()))
         {
            var serializer = _modelingXmlSerializerRepository.SerializerFor<ImporterConfiguration>();

            var xel = XElement.Load(getFileFulName(fileName));
            return serializer.Deserialize<ImporterConfiguration>(xel, serializationContext);
         }
      }

      protected string getFileFulName(string fileName) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName);
   }

   public class When_importing_data_from_r : concern_for_DataImporter
   {
      [Observation]
      public void should_import_simple_data()
      {
         var _importerConfiguration = getConfiguration("importerConfiguration1.xml");
         var dataRepositories = sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings, getFileFulName("sample1.xlsx"));

         dataRepositories.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_return_empty_on_invalid_file_name()
      {
         var _importerConfiguration = getConfiguration("importerConfiguration1.xml");
         var dataRepositories = sut.ImportFromConfiguration(_importerConfiguration, _metaDataCategories, _columnInfos, _dataImporterSettings, "");

         dataRepositories.Count.ShouldBeEqualTo(0);
      }
   }
}
