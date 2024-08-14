using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ModuleXmlSerializer : ObjectBaseXmlSerializer<Module>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         MapEnumerable(x => x.BuildingBlocks, x => x.Add);
         Map(x => x.PKSimVersion);
         Map(x => x.ModuleImportVersion);
         Map(x => x.MergeBehavior);
         Map(x => x.IsPKSimModule);
      }
   }
}