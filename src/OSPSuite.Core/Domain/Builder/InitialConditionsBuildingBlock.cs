using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public interface ILookupBuildingBlock<T> : IBuildingBlock<T> where T : IBuilder
   {
      T ByPath(ObjectPath path);
   }

   public class InitialConditionsBuildingBlock : PathAndValueEntityBuildingBlock<InitialCondition>
   {
      public InitialConditionsBuildingBlock()
      {
         Icon = IconNames.INITIAL_CONDITIONS;
      }
   }
}