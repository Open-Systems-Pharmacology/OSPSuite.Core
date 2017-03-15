using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.UICommands
{
   public class GarbageCollectionCommand : IUICommand
   {
      public void Execute()
      {
         this.GCCollectAndCompact();
      }
   }
}