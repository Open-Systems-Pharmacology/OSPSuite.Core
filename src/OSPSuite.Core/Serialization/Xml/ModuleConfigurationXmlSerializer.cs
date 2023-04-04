using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ModuleConfigurationXmlSerializer: OSPSuiteXmlSerializer<ModuleConfiguration>
   {
      public override void PerformMapping()
      {
         //is this right? Or is this a real instance of a module?
         Map(x => x.Module);
         MapReference(x => x.SelectedMoleculeStartValues);
         MapReference(x => x.SelectedParameterStartValues);
      }
   }
}