using System.Xml.Linq;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Converters.v10
{
   public class Converter90To100 : IObjectConverter,
      IVisitor<ParameterIdentification>
   {
      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V9_0;
      private bool _converted;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         _converted = false;
         this.Visit(objectToUpdate);
         return (PKMLVersion.V10_0, _converted);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         return (PKMLVersion.V10_0, false);
      }

      public void Visit(ParameterIdentification parameterIdentification)
      {
         _converted = true;
      }
   }
}