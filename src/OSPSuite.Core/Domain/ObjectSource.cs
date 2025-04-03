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

      /// <summary>
      ///    Consolidated path of the object in the simulation (if entity)
      /// </summary>
      public string Path { get; }

      /// <summary>
      ///    Type of the source object. Might be useful for filtering
      /// </summary>
      public string SourceObjectType { get; }

      public string ModuleId { get; }
      public string BuildingBlockId { get; }

      /// <summary>
      ///    Id of the actual source of the object.
      /// </summary>
      public string SourceId { get; }

      public ObjectSource(string objectId, string path, string moduleId, string buildingBlockId, string sourceObjectType, string sourceId)
      {
         ObjectId = objectId;
         Path = path;
         ModuleId = moduleId;
         BuildingBlockId = buildingBlockId;
         SourceObjectType = sourceObjectType;
         SourceId = sourceId;
      }
   }

   public class ObjectSources : IReadOnlyCollection<ObjectSource>, IVisitable<IVisitor>
   {
      private readonly Cache<string, ObjectSource> _sources = new Cache<string, ObjectSource>(x => x.ObjectId, x => null);

      public void Add(ObjectSource objectSource)
      {
         _sources[objectSource.Path] = objectSource;
      }

      public ObjectSource SourceFor(IEntity entity) => SourceById(entity.Id);

      public ObjectSource SourceById(string objectId) => _sources[objectId];

      public ObjectSource SourceByPath(string consolidatedPath) => _sources.Find(x => string.Equals(x.Path, consolidatedPath));

      public IEnumerator<ObjectSource> GetEnumerator() => _sources.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

      public int Count => _sources.Count;

      public virtual void AcceptVisitor(IVisitor visitor)
      {
         visitor.Visit(this);
      }
   }
}