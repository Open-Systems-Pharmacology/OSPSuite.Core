using System;
using Castle.Core;
using Castle.MicroKernel.Facilities;
using OSPSuite.Utility.Events;

namespace OSPSuite.Infrastructure.Container.Castle
{
    public class EventRegisterFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.ComponentCreated += onComponentCreated;
        }

        private void onComponentCreated(ComponentModel model, object instance)
        {
            var listener = instance as IListener;
            if (listener == null) return;

            var eventPublisher = retrievePublisher();
           eventPublisher?.AddListener(listener);
        }

        private IEventPublisher retrievePublisher()
        {
            try
            {
                return Kernel.Resolve<IEventPublisher>();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}