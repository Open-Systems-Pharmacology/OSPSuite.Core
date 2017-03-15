using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Core
{
   public interface IApplicationController
   {
      /// <summary>
      ///    Start the presenter whose type was given as parameter.
      ///    the function Initialize will be called as well
      /// </summary>
      TPresenter Start<TPresenter>() where TPresenter : IPresenter;

      TPresenter Open<TPresenter, TSubject>(TSubject subject, ICommandCollector commandCollector) where TPresenter : ISingleStartPresenter<TSubject>;
      ISingleStartPresenter Open<TSubject>(TSubject subject, ICommandCollector commandCollector);

      void Close(object subject);
      void CloseAll();
      IEnumerable<ISingleStartPresenter> OpenedPresenters();

      bool HasPresenterOpenedFor<TSubject>(TSubject subject);

      void ReleasePresenter(IPresenter presenter);
      ISingleStartPresenter PresenterFor(object subject);
   }

   public class ApplicationController : IApplicationController
   {
      private readonly IContainer _container;
      private readonly IEventPublisher _eventPublisher;
      private readonly IList<ISingleStartPresenter> _allOpenedPresenters;

      public ApplicationController(IContainer container, IEventPublisher eventPublisher, IList<ISingleStartPresenter> singleStartPresenters)
      {
         _container = container;
         _eventPublisher = eventPublisher;
         _allOpenedPresenters = singleStartPresenters;
      }

      public ApplicationController(IContainer container, IEventPublisher eventPublisher)
         : this(container, eventPublisher, new List<ISingleStartPresenter>())
      {
      }

      public virtual TPresenter Start<TPresenter>() where TPresenter : IPresenter
      {
         var presenter = _container.Resolve<TPresenter>();
         presenter.Initialize();
         return presenter;
      }

      public virtual bool HasPresenterOpenedFor<TSubject>(TSubject subject)
      {
         return PresenterFor(subject) != null;
      }

      public virtual void ReleasePresenter(IPresenter presenter)
      {
         var listener = presenter as IListener;
         if (listener == null) return;
         _eventPublisher.RemoveListener(listener);
      }

      public virtual TPresenter Open<TPresenter, TSubject>(TSubject subject, ICommandCollector commandCollector) where TPresenter : ISingleStartPresenter<TSubject>
      {
         return Open(subject, commandCollector).DowncastTo<TPresenter>();
      }

      public virtual ISingleStartPresenter Open<TSubject>(TSubject subject, ICommandCollector commandCollector)
      {
         var presenterToOpen = PresenterFor(subject);
         if (presenterToOpen == null)
         {
            presenterToOpen = CreatePresenterForSubject(subject);
            presenterToOpen.Closing += removePresenter;
            presenterToOpen.InitializeWith(commandCollector);
            _allOpenedPresenters.Add(presenterToOpen);
         }
         return presenterToOpen;
      }

      protected virtual ISingleStartPresenter CreatePresenterForSubject<TSubject>(TSubject subject)
      {
         return Start<ISingleStartPresenter<TSubject>>();
      }

      public virtual void Close(object subject)
      {
         var presenterToClose = PresenterFor(subject);
         if (presenterToClose == null) return;
         presenterToClose.Close();
         //the line before might trigger a release as well (only if the view was visible)
         removePresenter(presenterToClose);
      }

      public virtual ISingleStartPresenter PresenterFor(object subject)
      {
         var presenterSubject = new PresenterSubject { Subject = subject };
         return _allOpenedPresenters.FirstOrDefault(presenterSubject.Matches);
      }

      public virtual void CloseAll()
      {
         _allOpenedPresenters.ToList().Each(presenter => presenter.Close());

         _allOpenedPresenters.Clear();
      }

      public virtual IEnumerable<ISingleStartPresenter> OpenedPresenters()
      {
         return _allOpenedPresenters;
      }

      private void removePresenter(object sender, EventArgs e)
      {
         removePresenter(sender.DowncastTo<ISingleStartPresenter>());
      }

      private void removePresenter(ISingleStartPresenter presenterToClose)
      {
         presenterToClose.Closing -= removePresenter;
         presenterToClose.ReleaseFrom(_eventPublisher);
         _allOpenedPresenters.Remove(presenterToClose);
         ReleasePresenter(presenterToClose);
      }
   }

}