using OSPSuite.Core.Serialization.SimModel.DTO;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public class ObserverExportSerializer : QuantityExportSerializer<QuantityExport>
   {
      public ObserverExportSerializer(): base(SimModelSchemaConstants.FormulaId)
      {
      }

      public override void PerformMapping()
      {
         base.PerformMapping();
         ElementName = SimModelSchemaConstants.Observer;
      }
   }
}