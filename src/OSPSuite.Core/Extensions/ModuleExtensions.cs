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
         var buildingBlockTypeToAdd = buildingBlockToAdd.GetType();

         // Check if the type to be added is in the forbiddenTypes set
         if (isForbiddenType(buildingBlockTypeToAdd))
            return false;

         // Check if the type to be added or any of its base types are in the uniqueTypes set
         if (!isUniqueType(buildingBlockTypeToAdd))
            return true;

         return !buildingBlockTypeToAdd.IsAnImplementationOfAny(module.BuildingBlocks.Select(block => block.GetType()));
      }

      private static bool isForbiddenType(Type buildingBlockTypeToAdd)
      {
         var forbiddenTypes = new HashSet<Type>
         {
            typeof(ExpressionProfileBuildingBlock),
            typeof(IndividualBuildingBlock)
         };

         return buildingBlockTypeToAdd.IsAnImplementationOfAny(forbiddenTypes);
      }

      private static bool isUniqueType(Type buildingBlockTypeToAdd)
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

         return buildingBlockTypeToAdd.IsAnImplementationOfAny(uniqueTypes);
      }
   }
}