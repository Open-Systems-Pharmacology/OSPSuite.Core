using OSPSuite.Infrastructure.Import.Services;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   /// <summary>
   /// Single file containing the data, e.g. excel file or csv file
   /// </summary>
   public interface IDataSourceFile
   {
      Dictionary<string, IDataSheet> DataSheets { get; }
	   string Path { get; set; }

      IDataFormat Format { get; set; }

      IList<IDataFormat> AvailableFormats { get; set; }
   }

   public abstract class DataSourceFile : IDataSourceFile
   {
      protected readonly IImportLogger _logger; //not sure this is the correct logger implementetion - could be we need to write our own

      public IDataFormat Format { get; set; }

      public IList<IDataFormat> AvailableFormats { get; set; }

      protected DataSourceFile(IImportLogger logger)
      {
         _logger = logger;
      }
      
      public Dictionary<string, IDataSheet> DataSheets 
      { 
         get;
         protected set; 
      }


      private string _path;
      public string Path 
      {
         get  => _path; 
         set { _path = value; DataSheets = LoadFromFile(value); }
      }

      protected abstract Dictionary<string, IDataSheet> LoadFromFile(string path);
   }
}
