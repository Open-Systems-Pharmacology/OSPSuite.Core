using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ProcessBuilderXmlSerializer<T> : ContainerXmlSerializer<T> where T : class, IProcessBuilder
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Dimension);
         Map(x => x.CreateProcessRateParameter);
         Map(x => x.ProcessRateParameterPersistable);
         MapReference(x => x.Formula);
      }
   }

   public class ReactionBuilderXmlSerializer : ProcessBuilderXmlSerializer<ReactionBuilder>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.ContainerCriteria);
         MapEnumerable(x => x.Educts, x => x.AddEduct);
         MapEnumerable(x => x.Products, x => x.AddProduct);
         MapEnumerable(x => x.ModifierNames, x => x.AddModifier);
      }
   }

   public class TranportBuilderXmlSerializer : ProcessBuilderXmlSerializer<TransportBuilder>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.SourceCriteria);
         Map(x => x.TargetCriteria);
         Map(x => x.TransportType);
         Map(x => x.MoleculeList);
      }
   }

   public class ReactionPartnerBuilderXmlSerializer : OSPSuiteXmlSerializer<ReactionPartnerBuilder>
   {
      public override void PerformMapping()
      {
         Map(x => x.MoleculeName);
         Map(x => x.StoichiometricCoefficient);
      }
   }
}