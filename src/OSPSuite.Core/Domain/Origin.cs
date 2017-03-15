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

      public static Origin PKSim = create(OriginId.PKSim, ApplicationIcons.PKSim, "PK-Sim");
      public static Origin MoBi = create(OriginId.MoBi, ApplicationIcons.MoBi, "MoBi");
      public static Origin Matlab = create(OriginId.Matlab, ApplicationIcons.Matlab, "Matlab");
      public static Origin R = create(OriginId.R, ApplicationIcons.R, "R");
      public static Origin Other = create(OriginId.Other, ApplicationIcons.Other, "Other");

      public static IEnumerable<Origin> All
      {
         get { return _allSources; }
      }

      private static Origin create(OriginId originId, ApplicationIcon applicationIcon, string displayName)
      {
         var source = new Origin(originId, applicationIcon, displayName);
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
      public ApplicationIcon Icon { get; private set; }
      public string DisplayName { get; private set; }

      internal Origin(OriginId id, ApplicationIcon icon, string displayName)
      {
         Id = id;
         Icon = icon;
         DisplayName = displayName;
      }

      public override string ToString()
      {
         return DisplayName;
      }
   }
}