namespace OSPSuite.Core.Commands
{
   /// <summary>
   ///    Command use after serialization
   /// </summary>
   public class ReadOnlyCommand : OSPSuiteCommand<IOSPSuiteExecutionContext>
   {
      public ReadOnlyCommand()
      {
         Loaded = false;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         /*nothing to do*/
      }

      protected override void ClearReferences()
      {
         /*nothing to do*/
      }
   }
}