using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   /// <summary>
   /// Single file containing the data, e.g. excel file or csv file
   /// </summary>
   public interface IDataSourceFile
   {
      Dictionary<string, IDataTable> DataTables { get; set; }
	   string Path { get; set; }
   }

   public abstract class DataSourceFile : IDataSourceFile
   {
      public DataSourceFile(string path)
      {
         Path = path;
         DataTables = LoadFromFile(path);
      }
      
      public Dictionary<string, IDataTable> DataTables 
      { 
         get; 
         set; 
      }
      
      public string Path 
      { 
         get; 
         set; 
      }

      protected abstract Dictionary<string, IDataTable> LoadFromFile(string path);
   }
}
