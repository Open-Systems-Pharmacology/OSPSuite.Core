using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   public enum ParameterGroupingModeId
   {
      Hierarchical,
      Simple,
      Advanced
   }

   public static class ParameterGroupingModes
   {
      private static readonly ICache<ParameterGroupingModeId, ParameterGroupingMode> _allGroupModes = new Cache<ParameterGroupingModeId, ParameterGroupingMode>(groupMode => groupMode.Id);

      public static ParameterGroupingMode Hierarchical = create(ParameterGroupingModeId.Hierarchical, Captions.GroupingModeHierarchical);
      public static ParameterGroupingMode Simple = create(ParameterGroupingModeId.Simple, Captions.GroupingModeSimple);
      public static ParameterGroupingMode Advanced = create(ParameterGroupingModeId.Advanced, Captions.GroupingModeAdvanced);

      private static ParameterGroupingMode create(ParameterGroupingModeId id, string displayName)
      {
         var groupMode = new ParameterGroupingMode(id, displayName);
         _allGroupModes.Add(groupMode);
         return groupMode;
      }

      public static ParameterGroupingMode ById(ParameterGroupingModeId id)
      {
         return _allGroupModes[id];
      }

      public static IEnumerable<ParameterGroupingMode> All()
      {
         return _allGroupModes;
      }
   }

   public class ParameterGroupingMode
   {
      public ParameterGroupingModeId Id { get; }
      public string DisplayName { get; }

      public ParameterGroupingMode(ParameterGroupingModeId id, string displayName)
      {
         Id = id;
         DisplayName = displayName;
      }

      public override string ToString()
      {
         return DisplayName;
      }
   }
}