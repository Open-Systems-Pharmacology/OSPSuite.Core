using System.Collections.Generic;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public abstract class AbstractDisposableCommandCollectorPresenter<TView, TPresenter> : AbstractDisposablePresenter<TView, TPresenter>, ICommandCollectorPresenter where TView : IView<TPresenter> where TPresenter : IDisposablePresenter
   {
      public ICommandCollector CommandCollector { get; private set; }

      protected AbstractDisposableCommandCollectorPresenter(TView view) : base(view)
      {
      }

      public virtual void InitializeWith(ICommandCollector commandCollector)
      {
         CommandCollector = commandCollector;
         _subPresenterManager.InitializeWith(this);         
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         CommandCollector = null;
      }

      public virtual void AddCommand(ICommand command)
      {
         CommandCollector.AddCommand(command);
         OnStatusChanged();
      }

      public virtual IEnumerable<ICommand> All()
      {
         return CommandCollector.All();
      }
   }
}