
using OSPSuite.Presentation.Importer.Core;
using System.Collections;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Services
{
   public interface IImporter
   {
      IDataSourceFile LoadFile(string path);
      IDataSource ImportFromFile(IDataSourceFile file);
      IList<IFormat> AvailableFormats(IDataTable table);
   }

   public class Importer : IImporter
   {
      public IList<IFormat> AvailableFormats(IDataTable table)
      {
         throw new System.NotImplementedException();
      }

      public IDataSource ImportFromFile(IDataSourceFile file)
      {
         throw new System.NotImplementedException();
      }

      public IDataSourceFile LoadFile(string path)
      {
         //Maybe also list supported file formats
         throw new System.NotImplementedException();
      }
   }
}
