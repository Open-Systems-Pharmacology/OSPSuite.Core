using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface ITimeGridUpdater
   {
      void UpdateSimulationTimeGrid(IModelCoreSimulation modelCoreSimulation, RemoveLLOQMode removeLLOQMode, IEnumerable<DataRepository> observedDataUsedInMapping);
   }

   public class TimeGridUpdater : ITimeGridUpdater
   {
      private readonly ITimeGridRestrictor _timeGridRestrictor;

      public TimeGridUpdater(ITimeGridRestrictor timeGridRestrictor)
      {
         _timeGridRestrictor = timeGridRestrictor;
      }

      public void UpdateSimulationTimeGrid(IModelCoreSimulation modelCoreSimulation, RemoveLLOQMode removeLLOQMode, IEnumerable<DataRepository> observedDataUsedInMapping)
      {
         var times = timeGridFromObservedData(observedDataUsedInMapping, removeLLOQMode);
         modelCoreSimulation.BuildConfiguration.SimulationSettings.OutputSchema.AddTimePoints(convertToDoubleArray(times));
      }

      //We are not using the ToDoubleArray method here because of some precisions issue when converting value from float to double
      private IReadOnlyList<double> convertToDoubleArray(IReadOnlyList<float> floatArray)
      {
         var doubleArray = new double[floatArray.Count];
         for (int i = 0; i < floatArray.Count; i++)
         {
            doubleArray[i] = Convert.ToDouble(floatArray[i].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
         }
         return doubleArray;
      }

      private IReadOnlyList<float> timeGridFromObservedData(IEnumerable<DataRepository> observedDataUsedInMapping, RemoveLLOQMode removeLLOQMode)
      {
         var times = new List<float>();

         foreach (var observedData in observedDataUsedInMapping)
         {
            times.AddRange(_timeGridRestrictor.GetRelevantTimes(observedData, removeLLOQMode));
         }

         return times.ToList();
      }
   }
}