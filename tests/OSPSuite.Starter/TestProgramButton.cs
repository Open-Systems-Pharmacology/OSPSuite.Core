using System;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Starter
{
   public class TestProgramButton : MenuBarButton
   {
      public TestProgramButton(string caption, Action action=null)
      {
         Caption = caption;
         if(action!=null)
            Command =new ExecuteActionUICommand(action);
      }

      public override void Click()
      {
         Command.Execute();
      }
   }
}