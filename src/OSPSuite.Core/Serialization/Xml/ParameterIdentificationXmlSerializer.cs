using System.Xml.Linq;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.Xml.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ParameterIdentificationXmlSerializer : ObjectBaseXmlSerializer<ParameterIdentification>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Configuration);
         Map(x => x.OutputMappings);
         MapEnumerable(x => x.AllIdentificationParameters, x => x.AddIdentificationParameter);
         MapEnumerable(x => x.Results, x => x.AddResult);
         MapEnumerable(x => x.Analyses, x => x.AddAnalysis);
      }

      protected override XElement TypedSerialize(ParameterIdentification parameterIdentification, SerializationContext context)
      {
         var element = base.TypedSerialize(parameterIdentification, context);
         element.Add(SerializerRepository.CreateSimulationReferenceListElement(parameterIdentification));

         return element;
      }

      protected override void TypedDeserialize(ParameterIdentification parameterIdentification, XElement parameterIdentificationElement, SerializationContext context)
      {
         base.TypedDeserialize(parameterIdentification, parameterIdentificationElement, context);

         var lazyLoadTask = IoC.Resolve<ILazyLoadTask>();

         parameterIdentificationElement.AddReferencedSimulations(parameterIdentification, context.IdRepository, lazyLoadTask);
      }
   }
}