using OSPSuite.Core.Domain;
using OSPSuite.Serializer;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class QuantityAndContainerXmlSerializer<T> : QuantityXmlSerializer<T> where T : IQuantityAndContainer
   {
      public override void PerformMapping()
      {
         //it is necessary to load children first before processing to further deserialization 
         //for instance, percentile parameter is a child of the distributedParmaeter which needs to be available before setting the isfixedvalue
         MapEnumerable(x => x.Children, x => x.Add);
         base.PerformMapping();
         Map(x => x.Mode);
      }
   }

   public class MoleculeAmountXmlSerializer : QuantityAndContainerXmlSerializer<MoleculeAmount>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.ScaleDivisor);
      }
   }

   public abstract class DistributedParameterXmlSerializerBase<TDistributedParamter> : QuantityAndContainerXmlSerializer<TDistributedParamter> where TDistributedParamter : IDistributedParameter
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.BuildMode);
         Map(x => x.Info);
         Map(x => x.IsDefault);
         MapReference(x => x.RHSFormula);

         //no need to save origin, or default value for core parameter are those values are only used in PK-Sim
      }
   }

   public class DistributedParameterXmlSerializer : DistributedParameterXmlSerializerBase<DistributedParameter>
   {
   }
}