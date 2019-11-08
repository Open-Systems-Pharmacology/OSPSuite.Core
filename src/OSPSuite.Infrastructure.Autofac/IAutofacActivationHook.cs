using Autofac.Core;

namespace OSPSuite.Infrastructure.Container.Autofac
{
   public interface IAutofacActivationHook
   {
      void OnActivated(IActivatedEventArgs<object> e);
   }
}