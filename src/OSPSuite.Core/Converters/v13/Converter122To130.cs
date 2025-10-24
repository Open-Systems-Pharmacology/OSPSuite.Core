using OSPSuite.Core.Serialization;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Converters.v13
{
   public class Converter122To130 : IObjectConverter,
      IVisitor<SolverSettings>
   {
      private bool _converted;

      private readonly ISolverSettingsFactory _solverSettingsFactory;

      public Converter122To130(ISolverSettingsFactory solverSettingsFactory)
      {
         _solverSettingsFactory = solverSettingsFactory;
      }
      // 13.0 pkml is not compatible with 12.1, but you don't need an explicit conversion to move forward.
      // To satisfy the next converter, the object must pass through v13.0 conversion
      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V12_2;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         _converted = false;
         performConversion(objectToUpdate);
         return (PKMLVersion.V13_0, _converted);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         return (PKMLVersion.V13_0, false);
      }
      private void performConversion(object objectToUpdate) => this.Visit(objectToUpdate);

      public void Visit(SolverSettings solverSettings)
      {
         _solverSettingsFactory.AddAutoReduceToleranceParameter(solverSettings);
         _solverSettingsFactory.AddAutoReduceToleranceParameter(solverSettings);

         _converted = true;
      }
   }
}
