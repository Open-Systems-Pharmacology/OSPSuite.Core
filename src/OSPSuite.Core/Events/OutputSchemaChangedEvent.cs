using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Events
{
   public class OutputSchemaChangedEvent
   {
      public OutputSchema OutputSchema { get; }

      public OutputSchemaChangedEvent(OutputSchema outputSchema)
      {
         OutputSchema = outputSchema;
      }
   }
}
