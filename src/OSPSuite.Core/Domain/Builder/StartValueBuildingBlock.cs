using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IStartValuesBuildingBlock<T> : IBuildingBlock<T> where T : class, IStartValue
   {
      /// <summary>
      ///    Returns the start value registered for the given path. If not found, null is returned
      /// </summary>
      /// <param name="objectPath"></param>
      /// <returns></returns>
      T this[ObjectPath objectPath] { get; set; }


      /// <summary>
      ///    Removes the start value with path <paramref name="objectPath" /> if available. Does nothing otherwise
      /// </summary>
      void Remove(ObjectPath objectPath);
   }

   public abstract class StartValueBuildingBlock<T> : PathAndValueEntityBuildingBlock<T>, IStartValuesBuildingBlock<T> where T : class, IStartValue
   {

      protected StartValueBuildingBlock()
      {
         _allValues = new Cache<ObjectPath, T>(x => x.Path, x => null);
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         _allValues.Each(msv => msv.AcceptVisitor(visitor));
      }
   }
}