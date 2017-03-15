using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class RemoveLLOQMode
   {
      public string Name { get; }
      public string DisplayName { get; }
      public string Description { get; }

      public RemoveLLOQMode(string name, string displayName, string desription)
      {
         Name = name;
         DisplayName = displayName;
         Description = desription;
      }
   }

   public static class RemoveLLOQModes
   {
      private static readonly Cache<string, RemoveLLOQMode> _allUsages = new Cache<string, RemoveLLOQMode>(x => x.Name);

      public static RemoveLLOQMode Never = create(Constants.RemoveLLOQMode.NEVER, Captions.ParameterIdentification.RemoveLLOQModes.Never, Captions.ParameterIdentification.RemoveLLOQModes.NeverDescription);
      public static RemoveLLOQMode Always = create(Constants.RemoveLLOQMode.ALWAYS, Captions.ParameterIdentification.RemoveLLOQModes.Always, Captions.ParameterIdentification.RemoveLLOQModes.AlwaysDescription);
      public static RemoveLLOQMode NoTrailing = create(Constants.RemoveLLOQMode.NO_TRAILING, Captions.ParameterIdentification.RemoveLLOQModes.NoTrailing, Captions.ParameterIdentification.RemoveLLOQModes.NoTrailingDescription);

      public static IEnumerable<RemoveLLOQMode> AllUsages => _allUsages;

      private static RemoveLLOQMode create(string name, string displayName, string desription)
      {
         var usage = new RemoveLLOQMode(name, displayName, desription);
         _allUsages.Add(usage);
         return usage;
      }

      public static RemoveLLOQMode ByName(string name)
      {
         return _allUsages[name];
      }
   }
}