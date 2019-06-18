using System;
using System.Linq;

namespace OSPSuite.Core.Domain
{
   [Flags]
   public enum PKSimBuildingBlockType
   {
      Simulation = 2 << 0,
      Compound = 2 << 1,
      Formulation = 2 << 2,
      Protocol = 2 << 3,
      Individual = 2 << 4,
      Population = 2 << 5,
      Event = 2 << 6,
      ObserverSet = 2 << 7,

      /// <summary>
      ///    All but simulation
      /// </summary>
      Template = Compound | Formulation | Protocol | Individual | Population | Event | ObserverSet,
      SimulationSubject = Individual | Population
   }

   public static class PKSimBuildingBlockTypeExtensions
   {
      public static bool Is(this PKSimBuildingBlockType buildingBlockType, PKSimBuildingBlockType typeToCompare)
      {
         return (buildingBlockType & typeToCompare) != 0;
      }

      public static bool IsOneOf(this PKSimBuildingBlockType buildingBlockType, params PKSimBuildingBlockType[] typesToCompare)
      {
         return typesToCompare.Any(b => buildingBlockType.Is(b));
      }
   }
}