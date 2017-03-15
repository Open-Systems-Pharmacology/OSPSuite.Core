using OSPSuite.Core.Commands;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public abstract class AbstractDisposableContainerPresenter<TView, TPresenter> : AbstractDisposableCommandCollectorPresenter<TView, TPresenter>
      where TPresenter : IDisposablePresenter where TView : IView<TPresenter>
   {
      protected readonly OSPSuiteMacroCommand<IOSPSuiteExecutionContext> _macroCommand;

      protected AbstractDisposableContainerPresenter(TView view) : base(view)
      {
         _macroCommand = new OSPSuiteMacroCommand<IOSPSuiteExecutionContext>();
      }

      public override void Initialize()
      {
         InitializeWith(_macroCommand);
      }
   }
}