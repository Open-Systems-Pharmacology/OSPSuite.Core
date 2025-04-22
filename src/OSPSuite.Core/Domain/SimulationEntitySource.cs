using System;
using System.Collections;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Tracks the origin of specific entities in the simulation (such as parameters, initial conditions, parameter values
   ///    and formulas).The origin building block will be the one that last modified the entity (e.g a parameter might be
   ///    created by multiple building blocks. This will be the one that created the entity). Each source is unique by path.
   /// </summary>
   public class SimulationEntitySource
   {
      /// <summary>
      ///    Consolidated path of the entity in the simulation
      /// </summary>
      public string SimulationEntityPath { get; internal set; }

      public string BuildingBlockName { get; }

      public string BuildingBlockType { get; }

      public string ModuleName { get; }

      public string SourcePath { get; }

      /// <summary>
      ///    Actual reference to the source of the entity. This will not be serialized and is just used to retrieve during
      ///    simulation construction
      /// </summary>
      internal IEntity Source { get; set; }

      [Obsolete("For serialization")]
      public SimulationEntitySource()
      {
      }

      internal SimulationEntitySource(SimulationEntitySource originalSource)
      {
         SimulationEntityPath = originalSource.SimulationEntityPath;
         BuildingBlockName = originalSource.BuildingBlockName;
         BuildingBlockType = originalSource.BuildingBlockType;
         ModuleName = originalSource.ModuleName;
         SourcePath = originalSource.SourcePath;
         Source = originalSource.Source;
      }

      internal SimulationEntitySource(IBuildingBlock buildingBlock, string sourcePath, IEntity source)
      {
         BuildingBlockName = buildingBlock.Name;
         BuildingBlockType = buildingBlock.GetType().Name;
         ModuleName = buildingBlock.Module?.Name ?? string.Empty;
         SourcePath = sourcePath;
         Source = source;
      }

      /// <summary>
      ///    Returns a clone of the object without the reference to the source
      /// </summary>
      /// <returns></returns>
      public SimulationEntitySource Clone() => new SimulationEntitySource(this) {Source = null};

      public override string ToString()
      {
         return $"{BuildingBlockName} ({BuildingBlockType}) - {ModuleName} - {SourcePath}";
      }
   }

   public class SimulationEntitySources : IReadOnlyCollection<SimulationEntitySource>, IVisitable<IVisitor>
   {
      private readonly Cache<string, SimulationEntitySource> _sources = new Cache<string, SimulationEntitySource>(x => x.SimulationEntityPath, x => null);

      public void Add(SimulationEntitySource simulationEntitySource)
      {
         if (simulationEntitySource.SimulationEntityPath == null)
            return;

         _sources[simulationEntitySource.SimulationEntityPath] = simulationEntitySource;
      }

      public SimulationEntitySource SourceByPath(string path) => _sources[path];

      public IEnumerator<SimulationEntitySource> GetEnumerator() => _sources.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

      public int Count => _sources.Count;

      public virtual void AcceptVisitor(IVisitor visitor)
      {
         visitor.Visit(this);
      }

      public SimulationEntitySources Clone()
      {
         var clone = new SimulationEntitySources();
         _sources.Each(x => clone.Add(x.Clone()));
         return clone;
      }

      public IReadOnlyCollection<SimulationEntitySource> All => _sources;
   }
}