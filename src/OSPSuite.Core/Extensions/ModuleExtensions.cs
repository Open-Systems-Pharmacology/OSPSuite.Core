using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Extensions
{
   public static class ModuleNodeExtensions
   {
      /// <summary>
      ///    Checks if a building block can be added to a module.
      /// </summary>
      public static bool CanAdd<T>(this Module module, T buildingBlockToAdd) where T : IBuildingBlock
      {
         // Check if the type to be added is in the forbiddenTypes set
         if (isForbiddenType(buildingBlockToAdd))
            return false;

         // Check if the type to be added or any of its base types are in the uniqueTypes set
         if (!isUniqueType(buildingBlockToAdd))
            return true;

         return !buildingBlockToAdd.IsAnImplementationOfAny(module.BuildingBlocks.Select(block => block.GetType()));
      }

      private static bool isForbiddenType(IBuildingBlock buildingBlockToAdd)
      {
         var forbiddenTypes = new HashSet<Type>
         {
            typeof(ExpressionProfileBuildingBlock),
            typeof(IndividualBuildingBlock)
         };

         return buildingBlockToAdd.IsAnImplementationOfAny(forbiddenTypes);
      }

      private static bool isUniqueType(IBuildingBlock buildingBlockToAdd)
      {
         var uniqueTypes = new HashSet<Type>
         {
            typeof(MoleculeBuildingBlock),
            typeof(PassiveTransportBuildingBlock),
            typeof(EventGroupBuildingBlock),
            typeof(ReactionBuildingBlock),
            typeof(SpatialStructure),
            typeof(ObserverBuildingBlock)
         };

         return buildingBlockToAdd.IsAnImplementationOfAny(uniqueTypes);
      }
   }
}