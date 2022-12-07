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

   public static class ExpressionTypes
   {
      public static ExpressionType TransportProtein = new ExpressionType(ExpressionTypesId.Transport, Assets.IconNames.Transporter, Assets.Captions.Transporter);
      public static ExpressionType MetabolizingEnzyme = new ExpressionType(ExpressionTypesId.Enzyme, Assets.IconNames.Enzyme, Assets.Captions.Enzyme);
      public static ExpressionType ProteinBindingPartner = new ExpressionType(ExpressionTypesId.BindingPartner, Assets.IconNames.Protein, Assets.Captions.Protein);

      private static readonly List<ExpressionType> _types = new List<ExpressionType>
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
      public ExpressionType(ExpressionTypesId expressionTypesId, string iconName, string displayName)
      {
         Id = expressionTypesId;
         IconName = iconName;
         DisplayName = displayName;
      }
   }
}
