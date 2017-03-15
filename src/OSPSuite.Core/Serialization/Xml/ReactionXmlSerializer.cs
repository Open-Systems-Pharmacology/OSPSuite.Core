using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ReactionXmlSerializer : ProcessXmlSerializer<Reaction>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         MapEnumerable(x => x.Educts,x => x.AddEduct);
         MapEnumerable(x => x.Products,x => x.AddProduct);
         MapEnumerable(x => x.ModifierNames, x => x.AddModifier);
      }
   }

   public class ReactionPartnerXmlSerializer : OSPSuiteXmlSerializer<ReactionPartner>
   {
      public override void PerformMapping()
      {
         Map(x => x.StoichiometricCoefficient);
         MapReference(x => x.Partner);
      }
   }
}