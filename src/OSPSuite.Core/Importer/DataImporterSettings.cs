using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Importer
{
   public class DataImporterSettings
   {
      public ApplicationIcon Icon { get; set; }
      public string Caption { get; set; }
      private readonly List<List<string>> _metaDataConventionsForNaming = new List<List<string>>();
      private const string TOKEN = "{{{0}}}";
      private const string DELIMITER = ".";
      private string _token;
      private string _delimiter;

      /// <summary>
      ///    Gets the default naming convention for the imported observed data
      /// </summary>
      public IEnumerable<string> NamingConventions
      {
         get
         {
            if (_metaDataConventionsForNaming.Count == 0)
               yield break;

            foreach (var convention in _metaDataConventionsForNaming)
            {
               var sb = new StringBuilder();
               convention.Each(metaData => sb.Append(string.Format(Token + Delimiter, metaData)));
               yield return sb.ToString().TrimEnd(Delimiter.ToCharArray());
            }
         }
      }

      /// <summary>
      ///    The token must include {0} to be useful. Ultimately, this is replaced by the naming algorithm.
      /// </summary>
      public string Token
      {
         get { return string.IsNullOrEmpty(_token) ? TOKEN : _token; }
         set { _token = value; }
      }

      /// <summary>
      ///    The delimiter is used between metadata values in the name
      /// </summary>
      public string Delimiter
      {
         get { return string.IsNullOrEmpty(_delimiter) ? DELIMITER : _delimiter; }
         set { _delimiter = value; }
      }

      /// <summary>
      ///    Add a metadata name that will be used to name the sheet once imported. The naming take place in the order that
      ///    strings are added
      /// </summary>
      /// <param name="metaDataNames">The list of the metadata names to be used when naming the import</param>
      public void AddNamingPatternMetaData(params string[] metaDataNames)
      {
         _metaDataConventionsForNaming.Add(metaDataNames.ToList());
      }

      public void ClearMetaDataNamingPatterns()
      {
         _metaDataConventionsForNaming.Clear();  
      }
   }
}