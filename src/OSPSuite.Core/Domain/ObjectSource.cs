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
   public class ObjectSource
   {
      /// <summary>
      ///    Id of the object in the simulation.
      /// </summary>
      public string ObjectId { get; }

      public string ModuleId { get; }
      public string BuildingBlockId { get; }

      /// <summary>
      ///    Id of the actual source of the object.
      /// </summary>
      public string SourceId { get; }

      /// <summary>
      ///    Absolute path of the source object
      /// </summary>
      public string SourcePath { get; }

      /// <summary>
      ///    Type of the source object. Might be useful for filtering
      /// </summary>
      public string SourceType { get; }

      [Obsolete("For serialization")]
      public ObjectSource()
      {
      }

      public ObjectSource(string objectId, ObjectSource originalSource) : this(objectId, originalSource.ModuleId, originalSource.BuildingBlockId, originalSource.SourcePath, originalSource.SourceType, originalSource.SourceId)
      {
      }

      public ObjectSource(string objectId, string moduleId, string buildingBlockId, string sourcePath, string sourceType, string sourceId)
      {
         ObjectId = objectId;
         SourcePath = sourcePath;
         ModuleId = moduleId;
         BuildingBlockId = buildingBlockId;
         SourceType = sourceType;
         SourceId = sourceId;
      }
   }

   public class ObjectSources : IReadOnlyCollection<ObjectSource>, IVisitable<IVisitor>
   {
      private readonly Cache<string, ObjectSource> _sources = new Cache<string, ObjectSource>(x => x.ObjectId, x => null);

      public void Add(ObjectSource objectSource)
      {
         _sources[objectSource.ObjectId] = objectSource;
      }

      public ObjectSource SourceFor(IEntity entity) => SourceById(entity.Id);

      public ObjectSource SourceById(string objectId) => _sources[objectId];

      public ObjectSource SourceByPath(string consolidatedPath) => _sources.Find(x => string.Equals(x.SourcePath, consolidatedPath));

      public IEnumerator<ObjectSource> GetEnumerator() => _sources.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

      public int Count => _sources.Count;

      public virtual void AcceptVisitor(IVisitor visitor)
      {
         visitor.Visit(this);
      }
   }
}