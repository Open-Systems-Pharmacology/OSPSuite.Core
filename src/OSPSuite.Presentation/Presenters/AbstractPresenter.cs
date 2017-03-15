using System;
using System.Collections.Generic;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public abstract class AbstractPresenter<TView, TPresenter> : IPresenter<TView>
      where TView : IView<TPresenter>
      where TPresenter : IPresenter
   {
      public event EventHandler StatusChanged = delegate { };
      protected readonly ISubPresenterManager _subPresenterManager;

      protected readonly TView _view;

      protected AbstractPresenter(TView view)
      {
         _view = view;
         //attach base presenter
         _view.AttachPresenter(this);

         //attach base type presenter
         _view.AttachPresenter(this.DowncastTo<TPresenter>());
         _view.InitializeResources();
         _view.InitializeBinding();
         _subPresenterManager = new SubPresenterManager();
      }

      public TView View => _view;

      public virtual bool CanClose => !_view.HasError && _subPresenterManager.CanClose;

      protected virtual void OnStatusChanged(object sender, EventArgs e)
      {
         StatusChanged(this, EventArgs.Empty);
      }

      protected void OnStatusChanged()
      {
         OnStatusChanged(this, EventArgs.Empty);
      }

      public virtual void ViewChanged()
      {
         OnStatusChanged();
      }

      public IView BaseView => View;

      public virtual void Initialize()
      {
         _subPresenterManager.InitializeWith(this);
      }

      public virtual void ReleaseFrom(IEventPublisher eventPublisher)
      {
         eventPublisher.RemoveListener(this as IListener);
         View.Dispose();
         ClearSubPresenters(eventPublisher);
      }

      protected virtual void ClearSubPresenters(IEventPublisher eventPublisher)
      {
         _subPresenterManager.ReleaseFrom(eventPublisher);
      }

      /// <summary>
      ///    Add sub presenters that will be managed internally by a <see cref="SubPresenterManager" />.
      /// </summary>
      /// <param name="presenters">Enumeration of presenters to be added</param>
      protected void AddSubPresenters(params IPresenter[] presenters)
      {
         presenters.Each(_subPresenterManager.Add);
      }

      /// <summary>
      ///    Returns all presenters defined as sub presenters and managed by the underlying <see cref="SubPresenterManager" />
      /// </summary>
      protected virtual IEnumerable<IPresenter> AllSubPresenters => _subPresenterManager.All;
   }
}