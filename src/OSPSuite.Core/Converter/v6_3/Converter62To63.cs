using System.Linq;
using System.Xml.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml.Extensions;

namespace OSPSuite.Core.Converter.v6_3
{
   public class Converter62To63 : IObjectConverter,
      IVisitor<ParameterIdentification>

   {
      private bool _converted;

      public bool IsSatisfiedBy(int version)
      {
         //no converter from 6.1 to 6.2
         return version == PKMLVersion.V6_1_1 || version == PKMLVersion.V6_2_1;
      }

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         _converted = false;
         this.Visit(objectToUpdate);
         return (PKMLVersion.V6_3_1, _converted);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         _converted = false;
         element.DescendantsAndSelfNamed("JacobianMatrix").Each(convertJacobianElement);
         return (PKMLVersion.V6_3_1, _converted);
      }

      private void convertJacobianElement(XElement jacobianXElement)
      {
         var parameterPaths = jacobianXElement.Element("ParameterPaths");
         if (parameterPaths == null) return;
         parameterPaths.Name = "ParameterNames";
         _converted = true;
      }

      public void Visit(ParameterIdentification parameterIdentification)
      {
         parameterIdentification.Results.Select(x => x.BestResult).Each(convertOptimizationRunResults);
         _converted = true;
      }

      private void convertOptimizationRunResults(OptimizationRunResult optimizationRunResult)
      {
         var simulationResults = optimizationRunResult.SimulationResults.ToList();
         //This will effectively update the column origin
         optimizationRunResult.SimulationResults = simulationResults;
      }
   }
}