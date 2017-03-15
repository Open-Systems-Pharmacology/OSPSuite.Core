using OSPSuite.Assets;

namespace OSPSuite.Presentation.Regions
{
   public class RegionName
   {
      public string Name { get; private set; }
      public string Caption { get; private set; }
      public ApplicationIcon Icon { get; private set; }

      public RegionName(string name, string caption, ApplicationIcon icon)
      {
         Icon = icon;
         Caption = caption;
         Name = name;
      }

      public static implicit operator string(RegionName regionName)
      {
         return regionName.Name;
      }
   }
}