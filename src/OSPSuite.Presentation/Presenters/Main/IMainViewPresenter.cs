using OSPSuite.Utility.Events;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Events;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters.Main
{
   public interface IMainViewPresenter :
      IChangePropagator,
      IPresenterWithContextMenu<IMdiChildView>,
      IListener<HeavyWorkFinishedEvent>,
      IListener<HeavyWorkStartedEvent>,
      IListener<RollBackStartedEvent>,
      IListener<RollBackFinishedEvent>
   {
      ISingleStartPresenter ActivePresenter { get; }
      void Activate(IMdiChildView view);

      void Run();

      void RemoveAlert();

      void OpenFile(string fileName);
   }
}  