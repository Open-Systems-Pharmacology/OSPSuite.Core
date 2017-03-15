using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ObserverBuilderXmlSerializer<T> : EntityXmlSerializer<T> where T : class, IObserverBuilder
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Dimension);
         Map(x => x.ContainerCriteria);
         Map(x => x.MoleculeList);
         MapReference(x => x.Formula);
      }
   }

   public class ContainerObserverBuilderXmlSerializer : ObserverBuilderXmlSerializer<ContainerObserverBuilder>
   {
   }

   public class AmountObserverBuilderXmlSerializer : ObserverBuilderXmlSerializer<AmountObserverBuilder>
   {
   }
}