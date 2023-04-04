using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ModuleXmlSerializer : ObjectBaseXmlSerializer<Module>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Molecules);
         Map(x => x.Reactions);
         Map(x => x.PassiveTransports);
         Map(x => x.SpatialStructure);
         Map(x => x.Observers);
         Map(x => x.EventGroups);
         MapEnumerable(x => x.MoleculeStartValuesCollection, x => x.AddMoleculeStartValueBlock);
         MapEnumerable(x => x.ParameterStartValuesCollection, x => x.AddParameterStartValueBlock);
         MapEnumerable(x => x.ExtendedProperties, x => x.ExtendedProperties.Add);
      }
   }
}