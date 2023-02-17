using System.Collections;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Builder
{
   public abstract class PathAndValueEntityBuildingBlock<T> : BuildingBlock, IBuildingBlock<T> where T: class, IWithPath, IObjectBase
   {
      protected ICache<ObjectPath, T> _allValues = new Cache<ObjectPath, T>(x => x.Path, x => null);

      public T this[ObjectPath objectPath]
      {
         get => _allValues[objectPath];
         set => _allValues[objectPath] = value;
      }


      public void Add(T startValue)
      {
         _allValues.Add(startValue);
      }

      public void Remove(T startValue)
      {
         if (startValue == null) return;
         _allValues.Remove(startValue.Path);
      }


      public void Remove(ObjectPath objectPath)
      {
         Remove(this[objectPath]);
      }

      public IEnumerator<T> GetEnumerator()
      {
         return _allValues.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var pathAndValueEntityBuildingBlock = source as PathAndValueEntityBuildingBlock<T>;
         pathAndValueEntityBuildingBlock.Each(value => Add(cloneManager.Clone<T>(value)));
      }
   }
}