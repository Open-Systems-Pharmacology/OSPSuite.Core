using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Commands
{
   public class DataRowData
   {
      public DataRowData()
      {
         Data = new Cache<string, float>();
      }

      /// <summary>
      ///    This is the cache of the column key-value pairs that must be set by the add
      ///    Note that it does not include the basegrid column
      /// </summary>
      public Cache<string, float> Data { get; }

      /// <summary>
      ///    This is the value to be used for the basegrid column of the data repository
      /// </summary>
      public float BaseGridValue { set; get; }

      /// <summary>
      ///    This will fill this object with appropriate data from the
      ///    <paramref name="dataRepository">dataRepository</paramref>
      /// </summary>
      /// <param name="dataRowIndex">The row that should be used to fill the object</param>
      /// <param name="dataRepository">The repository that should be used to fill the object</param>
      public void FillFromRepository(int dataRowIndex, DataRepository dataRepository)
      {
         dataRepository.AllButBaseGrid().Each(column => Data.Add(column.Id, column[dataRowIndex]));
         var baseGridColumn = dataRepository.BaseGrid;

         BaseGridValue = baseGridColumn.Values[dataRowIndex];
      }
   }
}