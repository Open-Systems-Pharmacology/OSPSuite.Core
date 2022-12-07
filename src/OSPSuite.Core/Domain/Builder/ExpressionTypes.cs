using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain.Builder
{

   public enum ExpressionTypesId
   {
      Transport,
      Enzyme,
      BindingPartner
   }

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
      public static ExpressionType TransportProtein = new ExpressionType(TempAssets.IconNames.Transporter, TempAssets.Captions.Transporter, ExpressionTypesId.Transport);
      public static ExpressionType MetabolizingEnzyme = new ExpressionType(TempAssets.IconNames.Enzyme, TempAssets.Captions.Enzyme, ExpressionTypesId.Enzyme);
      public static ExpressionType ProteinBindingPartner = new ExpressionType(TempAssets.IconNames.Protein, TempAssets.Captions.Protein, ExpressionTypesId.BindingPartner);

      private static List<ExpressionType> _types = new List<ExpressionType>
      {
         TransportProtein,
         MetabolizingEnzyme,
         ProteinBindingPartner
      };

      public static ExpressionType ById(ExpressionTypesId expressionTypeId)
      {
         return _types.First(x => Equals(x.Id, expressionTypeId));
      }
   }

   public class ExpressionType
   {
      public ExpressionTypesId Id { get; }
      public string IconName { get; }
      public string DisplayName { get; }
      public ExpressionType(string iconName, string displayName, ExpressionTypesId expressionTypesId)
      {
         Id = expressionTypesId;
         IconName = iconName;
         DisplayName = displayName;
      }
   }
}
