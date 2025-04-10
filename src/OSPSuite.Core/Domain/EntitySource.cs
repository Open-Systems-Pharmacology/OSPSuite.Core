using System;
using System.Collections;
using System.Collections.Generic;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Tracks the origin of specific entities in the simulation (such as parameters, initial conditions, parameter values
   ///    and formulas).The origin building block will be the one that last modified the entity (e.g a parameter might be
   ///    created by multiple building blocks. This will be the one that created the entity). Each source is unique by path.
   ///    It will store the id of the building block (if defined)
   /// </summary>
   public class EntitySource
   {
      /// <summary>
      ///    Consolidated path of the entity in the simulation
      /// </summary>
      public string EntityPath { get; internal set; }

      public string BuildingBlockId { get; }

      /// <summary>
      ///    Id of the actual source of the object.
      /// </summary>
      public string SourceId { get; }

      /// <summary>
      ///    Type of the source object. Might be useful for filtering
      /// </summary>
      public string SourceType { get; }

      /// <summary>
      ///    Actual reference to the source of the entity. This will not be serialized and is just used to retrieve during
      ///    simulation construction
      /// </summary>
      internal IEntity Source { get; }

      [Obsolete("For serialization")]
      public EntitySource()
      {
      }

      internal EntitySource(EntitySource originalSource) : this(originalSource.BuildingBlockId, originalSource.SourceType, originalSource.SourceId, originalSource.Source)
      {
         EntityPath = originalSource.EntityPath;
      }

      internal EntitySource(string buildingBlockId, string sourceType, string sourceId, IEntity source)
      {
         BuildingBlockId = buildingBlockId;
         SourceType = sourceType;
         SourceId = sourceId;
         Source = source;
      }

      /// <summary>
      ///    Returns a clone of the object without the reference to the source
      /// </summary>
      /// <returns></returns>
      public EntitySource Clone()
      {
         return new EntitySource(BuildingBlockId, SourceType, SourceId, null)
         {
            EntityPath = EntityPath
         };
      }
   }

   public class EntitySources : IReadOnlyCollection<EntitySource>, IVisitable<IVisitor>
   {
      private readonly Cache<string, EntitySource> _sources = new Cache<string, EntitySource>(x => x.EntityPath, x => null);

      public void Add(EntitySource entitySource)
      {
         if (entitySource.EntityPath == null)
            return;

         _sources[entitySource.EntityPath] = entitySource;
      }

      public EntitySource SourceFor(IEntity entity) => SourceByPath(entity.Id);

      public EntitySource SourceByPath(string path) => _sources[path];

      public IEnumerator<EntitySource> GetEnumerator() => _sources.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

      public int Count => _sources.Count;

      public virtual void AcceptVisitor(IVisitor visitor)
      {
         visitor.Visit(this);
      }

      public EntitySources Clone()
      {
         var clone = new EntitySources();
         _sources.Each(x => clone.Add(x.Clone()));
         return clone;
      }
   }
}