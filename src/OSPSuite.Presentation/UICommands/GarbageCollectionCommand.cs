using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.UICommands
{
   public class GarbageCollectionCommand : IUICommand
   {
      public void Execute()
      {
         GarbageCollectionTask.ForceGC();
      }
   }
}