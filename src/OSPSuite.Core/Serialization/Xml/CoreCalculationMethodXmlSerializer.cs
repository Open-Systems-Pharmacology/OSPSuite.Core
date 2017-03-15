using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Serialization.Xml
{
   public class CoreCalculationMethodXmlSerializer : BuildingBlockXmlSerializer<CoreCalculationMethod>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Category);
         Map(x => x.OutputFormulas);
         Map(x => x.HelpParameters);
      }
   }

   public class FormulaParameterDescriptorCacheXmlSerializer : XmlCacheSerializer<IFormula, ParameterDescriptor, SerializationContext>, IOSPSuiteXmlSerializer
   {
   }

   public class ParameterDescriptorCriteriaCacheXmlSerializer : XmlCacheSerializer<IParameter, DescriptorCriteria, SerializationContext>, IOSPSuiteXmlSerializer
   {
   }
}