using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ModuleXmlSerializer : ObjectBaseXmlSerializer<Module>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Molecule);
         Map(x => x.Reaction);
         Map(x => x.PassiveTransport);
         Map(x => x.SpatialStructure);
         Map(x => x.Observer);
         Map(x => x.EventGroup);
         MapEnumerable(x => x.MoleculeStartValuesCollection, x => x.AddMoleculeStartValueBlock);
         MapEnumerable(x => x.ParameterStartValuesCollection, x => x.AddParameterStartValueBlock);
         MapEnumerable(x => x.ExtendedProperties, x => x.ExtendedProperties.Add);
      }
   }
}