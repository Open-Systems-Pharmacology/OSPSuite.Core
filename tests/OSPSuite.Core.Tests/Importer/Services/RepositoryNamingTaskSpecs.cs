using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;
using OSPSuite.Presentation.Services.Importer;

namespace OSPSuite.Importer.Services
{
   public abstract class concern_for_RepositoryNamingTask : ContextSpecification<RepositoryNamingTask>
   {
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected IEnumerable<string> _result;
      protected DataImporterSettings _dataImporterSettings;

      protected override void Context()
      {
         sut = new RepositoryNamingTask(new MetaDataCategoryToNamingPatternMapper());
      }

      protected override void Because()
      {
         _result = sut.CreateNamingPatternsBasedOn(_metaDataCategories, _dataImporterSettings);
      }

      [Observation]
      public void should_contain_fallback()
      {
         _result.ShouldContain("{File}.{Sheet}");
      }
   }

   public class when_naming_from_meta_data : concern_for_RepositoryNamingTask
   {
      protected override void Context()
      {
         base.Context();
         _metaDataCategories = new List<MetaDataCategory>
         {
            new MetaDataCategory {Name = "Species", IsMandatory = true},
            new MetaDataCategory {Name = "StudyId", IsMandatory = false}
         };
         _dataImporterSettings = new DataImporterSettings();
      }

      [Observation]
      public void should_contain_mandatory()
      {
         _result.ShouldContain("{Species}");
      }

      [Observation]
      public void should_contain_all()
      {
         _result.ShouldContain("{Species}.{StudyId}");
      }
   }

   public class when_naming_from_specific_conventions : concern_for_RepositoryNamingTask
   {
      protected override void Context()
      {
         base.Context();
         _metaDataCategories = new List<MetaDataCategory>();
         _dataImporterSettings = new DataImporterSettings();
         _dataImporterSettings.AddNamingPatternMetaData("File");
      }

      [Observation]
      public void should_contain_a_pattern_from_the_specified_convention()
      {
         _result.ShouldContain("{File}");
      }
   }


   public class when_naming_from_empty_metadata : concern_for_RepositoryNamingTask
   {
      protected override void Context()
      {
         base.Context();
         _metaDataCategories = new List<MetaDataCategory>();
         _dataImporterSettings = new DataImporterSettings();
      }

      [Observation]
      public void should_have_no_empty_templates()
      {
         _result.Any(string.IsNullOrEmpty).ShouldBeFalse();
      }

      [Observation]
      public void should_only_contain_the_default_result()
      {
         _result.ShouldOnlyContain("{File}.{Sheet}");
      }
   }
}
