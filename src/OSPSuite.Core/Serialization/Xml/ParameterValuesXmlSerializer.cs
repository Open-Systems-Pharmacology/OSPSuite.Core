using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ParameterValuesXmlSerializer : OSPSuiteXmlSerializer<ParameterValues>
   {
      public override void PerformMapping()
      {
         Map(x => x.ParameterPath);
         Map(x => x.Values);

         //Percentiles will be saved in deserialized 
      }

      protected override void TypedDeserialize(ParameterValues parameterValues, XElement element, SerializationContext serializationContext)
      {
         base.TypedDeserialize(parameterValues, element, serializationContext);
         var percentilesElements = element.Element(Constants.Serialization.PERCENTILES);
         if (element.Element(Constants.Serialization.PERCENTILES) != null)
         {
            var percentileSerializer = SerializerRepository.SerializerFor<List<double>>();
            parameterValues.Percentiles = percentileSerializer.Deserialize<List<double>>(percentilesElements, serializationContext);
         }
         else
         {
            parameterValues.Percentiles = new double[parameterValues.Count].InitializeWith(Constants.DEFAULT_PERCENTILE).ToList();
         }
      }

      protected override XElement TypedSerialize(ParameterValues parameterValues, SerializationContext serializationContext)
      {
         var element = base.TypedSerialize(parameterValues, serializationContext);
         var allPercentiles = parameterValues.Percentiles.Distinct().ToList();
         if (allPercentiles.Count == 1 && allPercentiles[0] == Constants.DEFAULT_PERCENTILE)
            return element;

         var doubleSerializer = SerializerRepository.SerializerFor(parameterValues.Percentiles);
         element.Add(doubleSerializer.Serialize(parameterValues.Percentiles, serializationContext));

         return element;
      }
   }
}