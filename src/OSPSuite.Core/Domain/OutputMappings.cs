﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class OutputMappings : IUpdatable, IEnumerable<OutputMapping>
   {
      private readonly List<OutputMapping> _allOutputMappings = new List<OutputMapping>();

      public void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         var sourceOutputMappings = source as OutputMappings;
         if (sourceOutputMappings == null) return;
         _allOutputMappings.Clear();
         _allOutputMappings.AddRange(sourceOutputMappings.All.Select(x => x.Clone()));
      }

      public IReadOnlyList<OutputMapping> All => _allOutputMappings;

      public void Add(OutputMapping outputMapping)
      {
         _allOutputMappings.Add(outputMapping);
      }

      public void Remove(OutputMapping outputMapping)
      {
         _allOutputMappings.Remove(outputMapping);
      }

      public void Clear()
      {
         _allOutputMappings.Clear();
      }

      public void RemoveOutputsReferencing(ISimulation simulation)
      {
         _allOutputMappings.Where(x => outputBelongsToSimulation(simulation, x))
            .ToList().Each(x => _allOutputMappings.Remove(x));
      }

      public virtual void SwapSimulation(ISimulation oldSimulation, ISimulation newSimulation)
      {
         //Use ToList() here as the collection might be modified as we iterate
         _allOutputMappings.Where(x => x.UsesSimulation(oldSimulation)).ToList().Each(x => x.UpdateSimulation(newSimulation));
      }

      private bool outputBelongsToSimulation(ISimulation simulation, OutputMapping outputMapping)
      {
         return Equals(outputMapping.Simulation, simulation);
      }

      public virtual IEnumerable<OutputMapping> OutputMappingsUsingDataRepository(DataRepository dataRepository)
      {
         return All.Where(x => x.UsesObservedData(dataRepository));
      }

      /// <summary>
      ///    Returns all observed data mapped to the output with path <paramref name="fullOutputPath" />
      /// </summary>
      /// <param name="fullOutputPath">Full path of the output including simulation name</param>
      public virtual IEnumerable<DataRepository> AllDataRepositoryMappedTo(string fullOutputPath)
      {
         return All.Where(x => string.Equals(x.FullOutputPath, fullOutputPath)).Select(x => x.WeightedObservedData.ObservedData);
      }

      public virtual IEnumerable<DataRepository> AllDataRepositoryMappedFor(ISimulation simulation)
      {
         return AllOutputMappingsFor(simulation).Select(x => x.WeightedObservedData.ObservedData);
      }

      public virtual IEnumerable<OutputMapping> AllOutputMappingsFor(ISimulation simulation)
      {
         return All.Where(x => x.UsesSimulation(simulation));
      }

      public virtual IEnumerable<QuantitySelection> AllOutputsMappedFor(ISimulation simulation)
      {
         return AllOutputMappingsFor(simulation).Select(x => x.OutputSelection.QuantitySelection);
      }

      public bool UsesSimulation(ISimulation simulation)
      {
         return All.Any(x => outputBelongsToSimulation(simulation, x));
      }

      public bool Contains(OutputMapping outputMapping)
      {
         return _allOutputMappings.Contains(outputMapping);
      }

      public IEnumerator<OutputMapping> GetEnumerator() => _allOutputMappings.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }
}