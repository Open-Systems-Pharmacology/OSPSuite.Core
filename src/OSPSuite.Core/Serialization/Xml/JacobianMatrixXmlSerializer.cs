using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class JacobianMatrixXmlSerializer : OSPSuiteXmlSerializer<JacobianMatrix>
   {
      public override void PerformMapping()
      {
         Map(x => x.ParameterNames);
         MapEnumerable(x => x.AllPartialDerivatives, x => x.AddPartialDerivatives);
         MapEnumerable(x => x.Rows, x => x.AddRow);
      }
   }
}