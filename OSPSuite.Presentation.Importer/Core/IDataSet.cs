using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   /// <summary>
   /// Data from a single experiment
   /// </summary>
   public interface IDataSet
   {
      IList<Dictionary<Column, IList<ValueAndLloq>>> Data { get; set; }
   }

   public class DataSet : IDataSet
   {
      public IList<Dictionary<Column, IList<ValueAndLloq>>> Data { get; set; }
   }
}