using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Represents the origin of an object
   /// </summary>
   public enum OriginId
   {
      PKSim = 1,
      MoBi = 2,
      R = 3,
      Matlab = 4,
      Other = 5,
   }

   public static class Origins
   {
      private static readonly ICache<OriginId, Origin> _allSources = new Cache<OriginId, Origin>(x => x.Id);

      public static Origin PKSim = create(OriginId.PKSim, IconNames.PKSIM, "PK-Sim");
      public static Origin MoBi = create(OriginId.MoBi, IconNames.MOBI, "MoBi");
      public static Origin Matlab = create(OriginId.Matlab, IconNames.MATLAB, "Matlab");
      public static Origin R = create(OriginId.R, IconNames.R, "R");
      public static Origin Other = create(OriginId.Other, IconNames.OTHER, "Other");

      public static IEnumerable<Origin> All => _allSources;

      private static Origin create(OriginId originId, string iconName, string displayName)
      {
         var source = new Origin(originId, iconName, displayName);
         _allSources.Add(source);
         return source;
      }

      public static Origin ById(OriginId originId)
      {
         return _allSources[originId];
      }
   }

   public class Origin
   {
      public OriginId Id { get; private set; }
      public string IconName { get; private set; }
      public string DisplayName { get; private set; }

      internal Origin(OriginId id, string iconName, string displayName)
      {
         Id = id;
         IconName = iconName;
         DisplayName = displayName;
      }

      public override string ToString()
      {
         return DisplayName;
      }
   }
}