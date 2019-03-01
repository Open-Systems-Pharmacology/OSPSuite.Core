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

      /// <summary>
      ///    Identifier used to uniquely identify an weighted observed data as the same observed data could be used more than
      ///    once
      /// </summary>
      public int? Id { get; set; }

      public int Count => Weights.Length;

      public string DisplayName
      {
         get
         {
            if (Id.HasValue)
               return $"{Name} - {Id}";

            return Name;
         }
      }

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