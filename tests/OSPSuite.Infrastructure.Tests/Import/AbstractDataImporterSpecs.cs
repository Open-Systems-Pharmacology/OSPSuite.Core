using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Extensions;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_AbstractDataImporter : ContextSpecification<AbstractDataImporter>
   {
      protected override void Context()
      {
         sut = new DataImporterForSpecs(A.Fake<IImporter>(), A.Fake<IDimensionFactory>());
      }
   }

   public class When_retrieving_the_default_metadata_categories_for_observed_data : concern_for_AbstractDataImporter
   {
      private IReadOnlyList<MetaDataCategory> _categories;
      private MetaDataCategory _outputPathCategory;

      protected override void Because()
      {
         _categories = sut.DefaultMetaDataCategoriesForObservedData();
         _outputPathCategory = _categories.FirstOrDefault(x => x.Name == Constants.OUTPUT_PATH);
      }

      [Observation]
      public void should_offer_the_standard_categories_shared_by_all_applications()
      {
         var categoryNames = _categories.Select(x => x.Name).ToList();
         Constants.ObservedData.DEFAULT_CATEGORIES.Each(name => categoryNames.ShouldContain(name));
      }

      [Observation]
      public void should_offer_an_optional_output_path_category()
      {
         _outputPathCategory.ShouldNotBeNull();
         _outputPathCategory.IsMandatory.ShouldBeFalse();
         _outputPathCategory.MetaDataType.ShouldBeEqualTo(typeof(string));
         _outputPathCategory.IsListOfValuesFixed.ShouldBeFalse();
      }

      [Observation]
      public void should_describe_what_the_output_path_is_used_for()
      {
         string.IsNullOrEmpty(_outputPathCategory.Description).ShouldBeFalse();
         _outputPathCategory.Description.ShouldNotBeEqualTo(_outputPathCategory.Name);
      }

      [Observation]
      public void should_offer_the_output_path_category_only_once()
      {
         _categories.Count(x => x.Name == Constants.OUTPUT_PATH).ShouldBeEqualTo(1);
      }
   }

   internal class DataImporterForSpecs : AbstractDataImporter
   {
      public DataImporterForSpecs(IImporter importer, IDimensionFactory dimensionFactory) : base(importer, dimensionFactory)
      {
      }

      public override (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings, string dataFileName)
      {
         throw new NotSupportedException();
      }

      public override IReadOnlyList<DataRepository> ImportFromConfiguration(ImporterConfiguration configuration, IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings, string dataFileName)
      {
         throw new NotSupportedException();
      }

      public override bool AreFromSameMetaDataCombination(DataRepository sourceDataRepository, DataRepository targetDataRepository)
      {
         throw new NotSupportedException();
      }

      public override ReloadDataSets CalculateReloadDataSetsFromConfiguration(IReadOnlyList<DataRepository> dataSetsToImport, IReadOnlyList<DataRepository> existingDataSets)
      {
         throw new NotSupportedException();
      }
   }
}
