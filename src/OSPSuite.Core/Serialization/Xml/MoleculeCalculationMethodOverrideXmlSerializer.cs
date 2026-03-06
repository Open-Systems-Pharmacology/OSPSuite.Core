using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml;

public class MoleculeCalculationMethodOverrideXmlSerializer : OSPSuiteXmlSerializer<MoleculeCalculationMethodOverride>
{
   public override void PerformMapping()
   {
      Map(x => x.MoleculeName);
      MapEnumerable(x => x.UsedCalculationMethods, x => x.AddUsedCalculationMethod);
   }
}