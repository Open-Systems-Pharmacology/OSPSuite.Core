using System;
using System.Collections.Generic;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters
{
   public abstract class AbstractCommandCollectorPresenter<TView, TPresenter> :
      AbstractPresenter<TView, TPresenter>, ICommandCollectorPresenter
      where TPresenter : IPresenter
      where TView : IView<TPresenter>
   {
      public ICommandCollector CommandCollector { get; private set; }

      protected AbstractCommandCollectorPresenter(TView view)
         : base(view)
      {
      }

      public virtual void InitializeWith(ICommandCollector commandRegister)
      {
         CommandCollector = commandRegister;
         _subPresenterManager.InitializeWith(this);
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         CommandCollector = null;
      }

      public virtual void AddCommand(Func<ICommand> commandFunc) => AddCommand(commandFunc());

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