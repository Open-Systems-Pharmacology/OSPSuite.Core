using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class LLOQMode
   {
      public string Name { get; }
      public string DisplayName { get; }
      public string Description { get; }

      public LLOQMode(string name, string displayName, string description)
      {
         Name = name;
         DisplayName = displayName;
         Description = description;
      }
   }

   public static class LLOQModes
   {
      private static readonly Cache<string, LLOQMode> _allModes = new Cache<string, LLOQMode>(x => x.Name);

      public static LLOQMode OnlyObservedData = create(Constants.LLOQModes.ONLY_OBSERVED_DATA, Captions.ParameterIdentification.LLOQModes.OnlyObservedData, Captions.ParameterIdentification.LLOQModes.OnlyObservedDataDescription);
      public static LLOQMode SimulationOutputAsObservedDataLLOQ = create(Constants.LLOQModes.SIMULATION_OUTPUT_AS_OBSERVED_DATA_LLOQ, Captions.ParameterIdentification.LLOQModes.SimulationOutputAsObservedDataLLOQ, Captions.ParameterIdentification.LLOQModes.SimulationOutputAsObservedDataLLOQDescription);

      public static IEnumerable<LLOQMode> AllModes => _allModes;

      private static LLOQMode create(string name, string displayName, string description)
      {
         var mode = new LLOQMode(name, displayName, description);
         _allModes.Add(mode);
         return mode;
      }

      public static LLOQMode ByName(string name)
      {
         return _allModes[name];
      }
   }
}