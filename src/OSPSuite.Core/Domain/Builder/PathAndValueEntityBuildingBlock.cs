using System.Collections;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.Builder
{
   public abstract class PathAndValueEntityBuildingBlock<T> : BuildingBlock, ILookupBuildingBlock<T> where T : PathAndValueEntity
   {
      protected Cache<ObjectPath, T> _allValues = new Cache<ObjectPath, T>(x => x.Path, x => null);

      public T this[ObjectPath objectPath]
      {
         get => _allValues[objectPath];
         set => _allValues[objectPath] = value;
      }

      public T FindByPath(string objectPath) => _allValues[new ObjectPath(objectPath.ToPathArray())];

      public void Add(T pathAndValueEntity)
      {
         _allValues.Add(pathAndValueEntity);
         pathAndValueEntity.BuildingBlock = this;
      }

      public void Remove(T pathAndValueEntity)
      {
         if (pathAndValueEntity == null) return;
         Remove(pathAndValueEntity.Path);
      }

      public void Remove(ObjectPath objectPath)
      {
         _allValues.Remove(objectPath);
      }

      public IEnumerator<T> GetEnumerator()
      {
         return _allValues.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      T ILookupBuildingBlock<T>.ByPath(ObjectPath path)
      {
         return this[path];
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         _allValues.Each(x => x.AcceptVisitor(visitor));
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var pathAndValueEntityBuildingBlock = source as PathAndValueEntityBuildingBlock<T>;
         pathAndValueEntityBuildingBlock.Each(value => Add(cloneManager.Clone<T>(value)));
      }
   }
}