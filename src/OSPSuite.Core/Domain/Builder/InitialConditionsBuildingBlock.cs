using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IBuildingBlockWithInitialConditions : IBuildingBlock
   {
      IReadOnlyCollection<InitialCondition> InitialConditions { get; }
      void RemoveInitialCondition(InitialCondition initialCondition);
      void AddInitialCondition(InitialCondition initialCondition);
   }

   public class InitialConditionsBuildingBlock : PathAndValueEntityBuildingBlock<InitialCondition>, IBuildingBlockWithInitialConditions
   {
      public InitialConditionsBuildingBlock()
      {
         Icon = IconNames.INITIAL_CONDITIONS;
      }

      public IReadOnlyCollection<InitialCondition> InitialConditions => this.ToList();
      
      public void RemoveInitialCondition(InitialCondition initialCondition)
      {
         Remove(initialCondition);
      }

      public void AddInitialCondition(InitialCondition initialCondition)
      {
         Add(initialCondition);
      }
   }
}