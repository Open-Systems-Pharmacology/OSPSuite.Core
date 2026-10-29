using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public class When_validating_the_observed_data_import_file_filter : StaticContextSpecification
   {
      private string[] _entries;
      private string _defaultPatterns;

      protected override void Context()
      {
         // Filter format is "Description|Patterns|Description|Patterns|..."
         _entries = Captions.Importer.ImportFileFilter.Split('|');
         _defaultPatterns = _entries[1];
      }

      [Observation]
      public void the_default_entry_should_accept_all_supported_file_types()
      {
         patternsOf(_defaultPatterns).ShouldOnlyContain(
            Constants.Filter.XLS_EXTENSION,
            Constants.Filter.XLSX_EXTENSION,
            Constants.Filter.CSV_EXTENSION,
            Constants.Filter.NONMEM_EXTENSION);
      }

      [Observation]
      public void the_default_entry_description_should_list_all_supported_file_types()
      {
         _entries[0].ShouldBeEqualTo("All Supported Files (*.xls, *.xlsx, *.csv, *.NMdat)");
      }

      [Observation]
      public void should_still_offer_an_excel_only_entry()
      {
         patternsOf(patternsForDescription("Excel Files (*.xls, *.xlsx)")).ShouldOnlyContain(Constants.Filter.XLS_EXTENSION, Constants.Filter.XLSX_EXTENSION);
      }

      [Observation]
      public void should_still_offer_a_csv_only_entry()
      {
         patternsOf(patternsForDescription("Comma Separated Value Files (*.csv)")).ShouldOnlyContain(Constants.Filter.CSV_EXTENSION);
      }

      [Observation]
      public void should_still_offer_a_nonmem_only_entry()
      {
         patternsOf(patternsForDescription("NonMem Files (*.NMdat)")).ShouldOnlyContain(Constants.Filter.NONMEM_EXTENSION);
      }

      [Observation]
      public void should_still_offer_an_all_files_entry()
      {
         patternsForDescription("All Files (*.*)").ShouldBeEqualTo("*.*");
      }

      private string patternsForDescription(string description)
      {
         var index = Array.IndexOf(_entries, description);
         index.ShouldBeGreaterThanOrEqualTo(0);
         return _entries[index + 1];
      }

      // "*.xls;*.NMdat" => [".xls", ".nmdat"]; file dialog filters are case-insensitive
      private static string[] patternsOf(string patterns) => patterns.Split(';').Select(x => x.TrimStart('*').ToLowerInvariant()).ToArray();
   }
}
