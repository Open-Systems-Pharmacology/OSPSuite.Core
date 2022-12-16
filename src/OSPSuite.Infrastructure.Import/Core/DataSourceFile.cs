using System.Collections.Generic;
using System.Linq;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Infrastructure.Import.Core
{
   /// <summary>
   ///    Single file containing the data, e.g. excel file or csv file
   /// </summary>
   public interface IDataSourceFile
   {
      string Path { get; set; }
      IDataFormat Format { get; set; }

      IList<IDataFormat> AvailableFormats { get; set; }

      //Stores what sheet was used to calculate the format
      //so the presenter can actually select such a sheet
      //as active when initialized
      string FormatCalculatedFrom { get; set; }
      DataSheetCollection DataSheets { get; }
   }

   public abstract class DataSourceFile : IDataSourceFile
   {
      protected readonly IImportLogger _logger; //ToDo: not sure this is the correct logger implementation - could be we need to write our own

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
      public DataSheetCollection DataSheets { get; } = new DataSheetCollection();

      protected DataSourceFile(IImportLogger logger)
      {
         _logger = logger;
      }

      private string _path;

      public string Path
      {
         get => _path;
         set
         {
            _path = value;
            LoadFromFile(value);
         }
      }

      protected abstract void LoadFromFile(string path);
   }
}