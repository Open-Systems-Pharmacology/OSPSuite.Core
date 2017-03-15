using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   public class ValuePointExportSerializer : SimModelSerializerBase<ValuePoint>
   {
      
      public override void PerformMapping()
      {
         ElementName = SimModelSchemaConstants.ValuePoint;
         Map(x => x.X);
         Map(x => x.Y);
         Map(x => x.RestartSolver);
      }
   }
}