using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ModuleXmlSerializer : ObjectBaseXmlSerializer<Module>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         MapEnumerable(x => x.BuildingBlocks, x => x.Add);
         MapEnumerable(x => x.ExtendedProperties, x => x.ExtendedProperties.Add);
      }
   }
}