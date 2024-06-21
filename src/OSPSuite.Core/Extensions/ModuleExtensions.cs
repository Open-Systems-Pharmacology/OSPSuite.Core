using OSPSuite.Core.Domain.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Extensions
{
   public static class ModuleNodeExtensions
   {
      /// <summary>
      /// Checks if a building block can be added to a module.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="module"></param>
      /// <param name="buildingBlockToAdd"></param>
      /// <returns></returns>
      public static bool CanAdd<T>(this Module module, T buildingBlockToAdd) where T : IBuildingBlock
      {
         HashSet<Type> forbiddenTypes = new HashSet<Type>
            {
            typeof(ExpressionProfileBuildingBlock),
            typeof(IndividualBuildingBlock)
         };

         HashSet<Type> uniqueTypes = new HashSet<Type>
         {
            typeof(MoleculeBuildingBlock),
            typeof(PassiveTransportBuildingBlock),
            typeof(EventGroupBuildingBlock),
            typeof(ReactionBuildingBlock),
            typeof(SpatialStructure),
            typeof(ObserverBuildingBlock)
         };

         var buildingBlockTypeToAdd = buildingBlockToAdd.GetType();

         // Check if the type to be added is in the forbiddenTypes set
         bool isForbiddenType = forbiddenTypes.Any(forbiddenType => forbiddenType.IsAssignableFrom(buildingBlockTypeToAdd));
         if(isForbiddenType)
            return false;

         // Check if the type to be added or any of its base types are in the uniqueTypes set
         bool isSubtypeOfUniqueTypes = uniqueTypes.Any(uniqueType => uniqueType.IsAssignableFrom(buildingBlockTypeToAdd));

         if (!isSubtypeOfUniqueTypes)
            return true;

         var existingBuildingBlockTypes = module.BuildingBlocks.Select(block => block.GetType());

         return !existingBuildingBlockTypes.Any(existingType =>
            existingType.IsAssignableFrom(buildingBlockTypeToAdd) || buildingBlockTypeToAdd.IsAssignableFrom(existingType));
      }
   }
}
