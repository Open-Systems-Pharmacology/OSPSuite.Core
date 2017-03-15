using OSPSuite.Core.Serialization.SimModel.DTO;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public class EventExportSerializer : SimModelSerializerBase<EventExport>
   {
      public override void PerformMapping()
      {
         Map(x => x.ConditionFormulaId);
         Map(x => x.Id);
         Map(x => x.EntityId);
         Map(x => x.OneTime);
         MapEnumerable(x => x.AssignmentList);
         ElementName = SimModelSchemaConstants.Event;
      }
   }

   public class AssignmentExportSerializer : SimModelSerializerBase<AssigmentExport>
   {
      public override void PerformMapping()
      {
         Map(x => x.ObjectId);
         Map(x => x.NewFormulaId);
         Map(x => x.UseAsValue);
         ElementName = SimModelSchemaConstants.Assigment;
      }
   }
}