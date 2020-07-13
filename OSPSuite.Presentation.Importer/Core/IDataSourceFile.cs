using OSPSuite.Infrastructure.Import.Services;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   /// <summary>
   /// Single file containing the data, e.g. excel file or csv file
   /// </summary>
   public interface IDataSourceFile
   {
      Dictionary<string, IDataSheet> DataSheets { get; set; }
	   string Path { get; set; }
   }

   public abstract class DataSourceFile : IDataSourceFile
   {
      protected readonly IImportLogger logger; //not sure this is the correct logger implementetion

      public DataSourceFile(string path, IImportLogger logger)
      {
         Path = path;
         this.logger = logger;
         DataSheets = LoadFromFile(path);
      }
      
      public Dictionary<string, IDataSheet> DataSheets 
      { 
         get; 
         set; 
      }
      
      public string Path 
      { 
         get; 
         set; 
      }

      protected abstract Dictionary<string, IDataSheet> LoadFromFile(string path);
   }
}
