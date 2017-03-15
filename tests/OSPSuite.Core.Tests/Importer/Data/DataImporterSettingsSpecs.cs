using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Importer;

namespace OSPSuite.Importer.Data
{
   public abstract class concern_for_DataImporterSettings : ContextSpecification<DataImporterSettings>
   {
      protected override void Context()
      {
         base.Context();
         sut = new DataImporterSettings();
      }
   }

   public class when_clearing_naming_conventions : concern_for_DataImporterSettings
   {
      private IEnumerable<string> _result;

      protected override void Context()
      {
         base.Context();
         sut.AddNamingPatternMetaData("Species", "Organ");
         sut.AddNamingPatternMetaData("Species", "Organ", "Compartment");
      }

      protected override void Because()
      {
         sut.ClearMetaDataNamingPatterns();
         _result = sut.NamingConventions;
      }

      [Observation]
      public void resulting_convention_list_should_be_empty()
      {
         _result.Count().ShouldBeEqualTo(0);
      }
   }

   public class when_adding_multiple_naming_conventions : concern_for_DataImporterSettings
   {
      private IEnumerable<string> _result;

      protected override void Context()
      {
         base.Context();
         sut.AddNamingPatternMetaData("Species", "Organ");
         sut.AddNamingPatternMetaData("Species", "Organ", "Compartment");
      }

      protected override void Because()
      {
         _result = sut.NamingConventions;
      }

      [Observation]
      public void default_intended_convention_is_first_in_enumeration()
      {
         _result.First().Equals("{Species}.{Organ}").ShouldBeTrue();
      }

      [Observation]
      public void total_conventions_should_be_all_added_patterns()
      {
         _result.Count().ShouldBeEqualTo(2);
      }
   }

   public class when_getting_default_naming_convention : concern_for_DataImporterSettings
   {
      private IEnumerable<string> _result;

      protected override void Context()
      {
         base.Context();
         sut.Token = "~{0}~";
         sut.Delimiter = "-";
         sut.AddNamingPatternMetaData("Species", "Organ");
      }

      protected override void Because()
      {
         _result = sut.NamingConventions;
      }

      [Observation]
      public void token_and_delimiter_should_be_used_in_pattern()
      {
         _result.ShouldContain("~Species~-~Organ~");
      }

      [Observation]
      public void all_metadata_should_be_used_in_order()
      {
         var defaultNamingConvention = _result.First();

         defaultNamingConvention.IndexOf("Species", System.StringComparison.Ordinal).ShouldBeSmallerThan(defaultNamingConvention.IndexOf("Organ", System.StringComparison.Ordinal));
      }
   }
}
