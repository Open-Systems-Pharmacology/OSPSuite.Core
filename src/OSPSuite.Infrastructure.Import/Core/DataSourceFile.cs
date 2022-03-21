using System.Collections.Generic;
using System.Linq;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Infrastructure.Import.Core
{
   /// <summary>
   /// Single file containing the data, e.g. excel file or csv file
   /// </summary>
   public interface IDataSourceFile
   {
      Cache<string, DataSheet> DataSheetsDeprecated { get; }
	   string Path { get; set; }
      IDataFormat Format { get; set; }
      IList<IDataFormat> AvailableFormats { get; set; }
      //Stores what sheet was used to calculate the format
      //so the presenter can actually select such a sheet
      //as active when initialized
      string FormatCalculatedFrom { get; set; }

   }

   public abstract class DataSourceFile : IDataSourceFile
   {
      protected readonly IImportLogger _logger; //not sure this is the correct logger implementetion - could be we need to write our own

      public IDataFormat Format { get; set; }

      private IList<IDataFormat> _availableFormats;
      public IList<IDataFormat> AvailableFormats 
      {
         get => _availableFormats; 
         set
         {
            _availableFormats = value;
            Format = value.FirstOrDefault();
         }
      }

      public string FormatCalculatedFrom { get; set; }

      protected DataSourceFile(IImportLogger logger)
      {
         _logger = logger;
      }
      
      public Cache<string, DataSheet> DataSheetsDeprecated 
      { 
         get;
         protected set; 
      }

      private string _path;
      public string Path 
      {
         get  => _path; 
         set { _path = value; DataSheetsDeprecated = LoadFromFile(value); }
      }

      protected abstract Cache<string, DataSheet> LoadFromFile(string path);
   }
}
