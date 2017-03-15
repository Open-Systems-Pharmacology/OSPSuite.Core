using System;

namespace OSPSuite.Core.Extensions
{
   public static class Do
   {
      public static Action Action(Action action)
      {
         return action;
      }
   }

   public static class ActionExtensions
   {
      public static void ThenFinally(this Action action, Action finallyAction)
      {
         try
         {
            action();
         }
         finally
         {
            finallyAction();
         }
      }
   }
}