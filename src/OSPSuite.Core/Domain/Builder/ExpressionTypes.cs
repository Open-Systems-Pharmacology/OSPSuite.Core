using System;
using System.Collections.Generic;
using System.Text;

namespace OSPSuite.Core.Domain.Builder
{
   public static class TempAssets
   {
      public static class Captions
      {
         public static readonly string Species = "Species";
         public static readonly string Category = "Category";

         public static string Transporter = "Transporter";
         public static string Protein = "Protein";
         public static string Enzyme = "Enzyme";
      }

      public static class IconNames
      {
         public static string Transporter = "Transporter";
         public static string Protein = "Protein";
         public static string Enzyme = "Enzyme";
      }
   }

   public static class ExpressionTypes
   {
      public static ExpressionType TransportProtein = new ExpressionType(TempAssets.IconNames.Transporter, TempAssets.Captions.Transporter);
      public static ExpressionType MetabolizingEnzyme = new ExpressionType(TempAssets.IconNames.Enzyme, TempAssets.Captions.Enzyme);
      public static ExpressionType ProteinBindingPartner = new ExpressionType(TempAssets.IconNames.Protein, TempAssets.Captions.Protein);
   }

   public class ExpressionType
   {
      public string IconName { get; }
      public string DisplayName { get; }
      public ExpressionType(string iconName, string displayName)
      {
         IconName = iconName;
         DisplayName = displayName;
      }
   }
}
