using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface ITimeGridRestrictor
   {
      /// <summary>
      ///    Returns the indices that should be used in <paramref name="observedData" /> to compute e.g. the residuals based on
      ///    the <paramref name="removeLLOQMode" />.
      ///    The <paramref name="outputScaling" /> is used to remove possible 0 values when the output is using a
      ///    <see cref="Scalings.Log" /> scaling. Default is set to <see cref=" Scalings.Linear" />
      ///    so that no value is removed by default
      /// </summary>
      /// <returns></returns>
      IReadOnlyList<int> GetRelevantIndices(DataRepository observedData, RemoveLLOQMode removeLLOQMode, Scalings outputScaling = Scalings.Linear);

      IReadOnlyList<float> GetRelevantTimes(DataRepository observedData, RemoveLLOQMode removeLLOQMode);
   }

   public class TimeGridRestrictor : ITimeGridRestrictor
   {
      public IReadOnlyList<int> GetRelevantIndices(DataRepository observedData, RemoveLLOQMode removeLLOQMode, Scalings outputScaling = Scalings.Linear)
      {
         if (removeLLOQMode == RemoveLLOQModes.Never)
            return getRelevantIndicesAll(observedData, outputScaling);

         if (removeLLOQMode == RemoveLLOQModes.NoTrailing)
            return getRelevantIndicesNoTrailingLLOQ(observedData, outputScaling);

         if (removeLLOQMode == RemoveLLOQModes.Always)
            return getRelevantIndicesNoLLOQ(observedData, outputScaling);

         throw new ArgumentOutOfRangeException(nameof(removeLLOQMode));
      }

      public IReadOnlyList<float> GetRelevantTimes(DataRepository observedData, RemoveLLOQMode removeLLOQMode)
      {
         return GetRelevantIndices(observedData, removeLLOQMode).Select(i => observedData.BaseGrid[i]).ToList();
      }

      private static List<int> getRelevantIndicesAll(DataRepository observedData, Scalings outputScaling)
      {
         var dataColumn = observedData.FirstDataColumn();
         return getIndices(observedData, i => valueIsValid(outputScaling, dataColumn, i));
      }

      private static List<int> getRelevantIndicesNoLLOQ(DataRepository observedData, Scalings outputScaling)
      {
         var dataColumn = observedData.FirstDataColumn();
         return getIndices(observedData, i => !valueAtIndexIsBelowLLOQ(dataColumn, outputScaling, i));
      }

      private static List<int> getRelevantIndicesNoTrailingLLOQ(DataRepository observedData, Scalings outputScaling)
      {
         //precondition: BaseGrid values are in strictly monotonic order (see class documentation)
         var dataColumn = observedData.FirstDataColumn();

         // set n to number of obsData without trailing LLOQ values except for the first of those
         int n = observedData.BaseGrid.Count;
         for (int i = observedData.BaseGrid.Count - 1; i >= 0; i--)
         {
            if (valueAtIndexIsBelowLLOQ(dataColumn, outputScaling, i))
            {
               n = i + 1;
            }
            else
            {
               break;
            }
         }

         return getIndices(observedData, i => i < n && valueIsValid(outputScaling, dataColumn, i));
      }

      private static List<int> getIndices(DataRepository observedData, Func<int, bool> selector)
      {
         return Enumerable.Range(0, observedData.BaseGrid.Count).Where(selector).ToList();
      }

      private static bool valueAtIndexIsBelowLLOQ(DataColumn dataColumn, Scalings outputScaling, int i)
      {
         if (dataColumn.ColumnValueIsBelowLLOQ(i))
            return true;

         return !valueIsValid(outputScaling, dataColumn, i);
      }

      private static bool valueIsValid(Scalings scaling, DataColumn dataColumn, int i)
      {
         return valueIsValid(scaling, dataColumn[i]);
      }

      private static bool valueIsValid(Scalings scaling, float value)
      {
         return scaling == Scalings.Linear || value >= Constants.LOG_SAFE_EPSILON;
      }
   }
}