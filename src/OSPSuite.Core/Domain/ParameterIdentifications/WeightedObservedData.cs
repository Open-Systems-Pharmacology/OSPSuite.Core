using System;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class WeightedObservedData
   {
      public virtual DataRepository ObservedData { get; }

      public float[] Weights { get; }

      [Obsolete("For serialization")]
      public WeightedObservedData()
      {
      }

      public WeightedObservedData(DataRepository observedData)
      {
         ObservedData = observedData;
         Weights = new float[observedData.BaseGrid.Count].InitializeWith(Constants.DEFAULT_WEIGHT);
      }

      public static implicit operator DataRepository(WeightedObservedData weightedObservedData) => weightedObservedData?.ObservedData;

      public string Name => ObservedData?.Name;
      public int Count => Weights.Length;

      public virtual WeightedObservedData Clone()
      {
         var clone = new WeightedObservedData(ObservedData);
         clone.updateWeights(Weights);
         return clone;
      }

      private void updateWeights(float[] weights)
      {
         for (int i = 0; i < weights.Length; i++)
         {
            Weights[i] = weights[i];
         }
      }
   }
}