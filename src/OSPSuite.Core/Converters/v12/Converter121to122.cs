using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Converters.v12
{
   public class Converter121to122 : IObjectConverter,
      IVisitor<SolverSettings>
   {
      private bool _converted;

      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V12_1;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         _converted = false;
         performConversion(objectToUpdate);
         return (PKMLVersion.V12_2, _converted);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         return (PKMLVersion.V12_2, false);
      }

      private void performConversion(object objectToUpdate) => this.Visit(objectToUpdate);

      public void Visit(SolverSettings solverSettings)
      {
         //check that all required parameters are there

         _converted = true;
      }
   }
}