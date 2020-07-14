using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   /// <summary>
   /// Data from a single experiment
   /// </summary>
   public interface IDataSet
   {
      IList<Dictionary<IColumn, IList<double>>> Data { get; set; }
   }

   public class DataSet : IDataSet
   {
      public IList<Dictionary<IColumn, IList<double>>> Data { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
   }
}