using OSPSuite.Assets;
using OSPSuite.Utility.Collections;

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
      public static ExpressionType TransportProtein = new ExpressionType(ExpressionTypesId.Transport, IconNames.Transporter, Captions.Transporter);
      public static ExpressionType MetabolizingEnzyme = new ExpressionType(ExpressionTypesId.Enzyme, IconNames.Enzyme, Captions.MetabolizingEnzyme);
      public static ExpressionType ProteinBindingPartner = new ExpressionType(ExpressionTypesId.BindingPartner, IconNames.Protein, Captions.Protein);

      private static readonly ICache<ExpressionTypesId, ExpressionType> _typesCache = new Cache<ExpressionTypesId, ExpressionType>(x => x.Id)
      {
         TransportProtein,
         MetabolizingEnzyme,
         ProteinBindingPartner
      };

      public static ExpressionType ById(ExpressionTypesId expressionTypeId)
      {
         return _typesCache[expressionTypeId];
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

      public override string ToString()
      {
         return DisplayName;
      }
   }
}