using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   public enum ParameterGroupingModeIdForParameterAnalyzable
   {
      Simple,
      Advanced
   }

   /// <summary>
   ///    Parameter grouping modes for parameter identification and sensitivity analysis parameter selections.
   ///    For all others, see <see cref="ParameterGroupingModes" />
   /// </summary>
   public static class ParameterGroupingModesForParameterAnalyzable
   {
      private static readonly ICache<ParameterGroupingModeIdForParameterAnalyzable, ParameterGroupingModeForParameterAnalyzable> _allGroupModes = new Cache<ParameterGroupingModeIdForParameterAnalyzable, ParameterGroupingModeForParameterAnalyzable>(groupMode => groupMode.Id);

      public static ParameterGroupingModeForParameterAnalyzable Simple = create(ParameterGroupingModeIdForParameterAnalyzable.Simple, Captions.GroupingModeSimple);
      public static ParameterGroupingModeForParameterAnalyzable Advanced = create(ParameterGroupingModeIdForParameterAnalyzable.Advanced, Captions.GroupingModeAdvanced);

      private static ParameterGroupingModeForParameterAnalyzable create(ParameterGroupingModeIdForParameterAnalyzable id, string displayName)
      {
         var groupMode = new ParameterGroupingModeForParameterAnalyzable(id, displayName);
         _allGroupModes.Add(groupMode);
         return groupMode;
      }

      public static ParameterGroupingModeForParameterAnalyzable ById(ParameterGroupingModeIdForParameterAnalyzable id) => _allGroupModes[id];

      public static IReadOnlyList<ParameterGroupingModeForParameterAnalyzable> All() => _allGroupModes.ToList();
   }

   public class ParameterGroupingModeForParameterAnalyzable
   {
      public ParameterGroupingModeIdForParameterAnalyzable Id { get; }
      public string DisplayName { get; }

      public ParameterGroupingModeForParameterAnalyzable(ParameterGroupingModeIdForParameterAnalyzable id, string displayName)
      {
         Id = id;
         DisplayName = displayName;
      }

      public override string ToString() => DisplayName;
   }
}