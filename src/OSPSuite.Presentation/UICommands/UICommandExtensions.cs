using System;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Events;

namespace OSPSuite.Presentation.UICommands
{
   public static class UICommandExtensions
   {
      public static void ExecuteWithinExceptionHandler(this IUICommand uiCommand)
      {
         var eventPublisher = IoC.Resolve<IEventPublisher>();
         var changePropagator = IoC.Resolve<IChangePropagator>();
         uiCommand.ExecuteWithinExceptionHandler(eventPublisher, changePropagator);
      }

      public static void ExecuteWithinExceptionHandler(this IUICommand uiCommand, IEventPublisher eventPublisher, IChangePropagator changePropagator)
      {
         try
         {
            eventPublisher.PublishEvent(new HeavyWorkStartedEvent());
            changePropagator.SaveChanges();
            uiCommand.Execute();
         }
         catch (Exception e)
         {
            IoC.Resolve<IExceptionManager>().LogException(e);
         }
         finally
         {
            eventPublisher.PublishEvent(new HeavyWorkFinishedEvent());
         }
      }
   }
}