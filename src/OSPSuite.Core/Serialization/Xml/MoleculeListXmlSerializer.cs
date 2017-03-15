using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class MoleculeListXmlSerializer : OSPSuiteXmlSerializer<MoleculeList>
   {
      public override void PerformMapping()
      {
         Map(x => x.ForAll);
         MapEnumerable(x => x.MoleculeNames, x => x.AddMoleculeName);
         MapEnumerable(x => x.MoleculeNamesToExclude, x => x.AddMoleculeNameToExclude);
      }
   }
}