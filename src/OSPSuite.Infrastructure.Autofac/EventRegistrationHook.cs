using System;
using Autofac;
using Autofac.Core;
using OSPSuite.Utility.Events;

namespace OSPSuite.Infrastructure.Container.Autofac
{
   public class EventRegistrationHook : IAutofacActivationHook
   {
      public void OnActivated(IActivatedEventArgs<object> e)
      {
         var instance = e.Instance;

         var listener = instance as IListener;
         if (listener == null) return;

         var eventPublisher = retrievePublisher(e.Context);
         eventPublisher?.AddListener(listener);
      }

      private IEventPublisher retrievePublisher(IComponentContext context)
      {
         try
         {
            return context.Resolve<IEventPublisher>();
         }
         catch (Exception)
         {
            return null;
         }
      }
   }
}