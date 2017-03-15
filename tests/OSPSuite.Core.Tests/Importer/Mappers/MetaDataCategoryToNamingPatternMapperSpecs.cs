using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;

namespace OSPSuite.Importer.Mappers
{
   public abstract class concern_for_MetaDataCategoryToNamingPatternMapperSpecs : ContextSpecification<MetaDataCategoryToNamingPatternMapper>
   {
      protected List<MetaDataCategory> _metaDataCategories;
      protected override void Context()
      {
         base.Context();
         _metaDataCategories = new List<MetaDataCategory>
         {
            new MetaDataCategory {Name = "Species"},
            new MetaDataCategory {Name = "Organ"},
            new MetaDataCategory {Name = "StudyID", IsMandatory = false}
         };
         sut = new MetaDataCategoryToNamingPatternMapper();
      }
   }

   public class when_mapping_mandatory_metadata : concern_for_MetaDataCategoryToNamingPatternMapperSpecs
   {
      private string _result;
      


      protected override void Because()
      {

         _result = sut.RequiredMetaData(_metaDataCategories, "{{{0}}}", ".");
      }

      [Observation]
      public void all_meta_data_must_be_in_pattern()
      {
         _result.ShouldBeEqualTo("{Species}.{Organ}");
      }
   }

   public class when_mapping_all_metadata : concern_for_MetaDataCategoryToNamingPatternMapperSpecs
   {
      private string _result;

      protected override void Because()
      {
         _result = sut.AllMetaData(_metaDataCategories, "{{{0}}}", ".");
      }

      [Observation]
      public void all_meta_data_must_be_in_pattern()
      {
         _result.ShouldBeEqualTo("{Species}.{Organ}.{StudyID}");
      }
   }
}
