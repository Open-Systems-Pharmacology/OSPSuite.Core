using OSPSuite.Assets;
using OSPSuite.Presentation.Regions;

namespace OSPSuite.Starter.Presenters
{
   internal class RegionNames
   {
      public static RegionName Journal = createRegionName("Journal", Captions.Journal.JournalView, ApplicationIcons.Journal);
      public static RegionName Comparison = createRegionName("Comparison", "Comparison", ApplicationIcons.Comparison);
      public static RegionName Explorer = createRegionName("Explorer", "Explorer", ApplicationIcons.BuildingBlockExplorer);

      private static RegionName createRegionName(string name, string caption, ApplicationIcon icon)
      {
         return new RegionName(name, caption, icon);
      }

   }
}