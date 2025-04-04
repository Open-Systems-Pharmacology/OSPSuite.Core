using System;
using System.Collections;
using System.Collections.Generic;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Tracks the origin of specific entities in the simulation (such as parameters, initial conditions, parameter values
   ///    and formulas).
   ///    The origin module will be the one that last modified the entity (e.g a parameter might be created by multiple
   ///    module. This will be the one that created the entity)
   ///    Each source is unique by path. It will store the id of the module, potentially the id of the building block, the
   ///    type of the building block
   /// </summary>
   public class EntitySource
   {
      /// <summary>
      ///    Id of the entity in the simulation.
      /// </summary>
      public string EntityId { get; }

      public string BuildingBlockId { get; }

      /// <summary>
      ///    Actual reference to the source of the entity. This will not be serialized and is just used to retrieve during
      ///    simulation construction
      /// </summary>
      public IEntity Source { get; }

      /// <summary>
      ///    Id of the actual source of the object.
      /// </summary>
      public string SourceId { get; }

      /// <summary>
      ///    Type of the source object. Might be useful for filtering
      /// </summary>
      public string SourceType { get; }

      [Obsolete("For serialization")]
      public EntitySource()
      {
      }

      public EntitySource(string entityId, EntitySource originalSource) : this(entityId, originalSource.BuildingBlockId, originalSource.SourceType, originalSource.SourceId, originalSource.Source)
      {
      }

      public EntitySource(string entityId, string buildingBlockId, string sourceType, string sourceId, IEntity source)
      {
         EntityId = entityId;
         BuildingBlockId = buildingBlockId;
         SourceType = sourceType;
         SourceId = sourceId;
         Source = source;
      }
   }

   public class ObjectSources : IReadOnlyCollection<EntitySource>, IVisitable<IVisitor>
   {
      private readonly Cache<string, EntitySource> _sources = new Cache<string, EntitySource>(x => x.EntityId, x => null);

      public void Add(EntitySource entitySource)
      {
         _sources[entitySource.EntityId] = entitySource;
      }

      public EntitySource SourceFor(IEntity entity) => SourceById(entity.Id);

      public EntitySource SourceById(string entityId) => _sources[entityId];

      public IEnumerator<EntitySource> GetEnumerator() => _sources.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

      public int Count => _sources.Count;

      public virtual void AcceptVisitor(IVisitor visitor)
      {
         visitor.Visit(this);
      }
   }
}