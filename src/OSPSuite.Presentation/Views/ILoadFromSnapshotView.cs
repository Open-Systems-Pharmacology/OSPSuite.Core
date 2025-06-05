using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface ILoadFromSnapshotView : IModalView<ILoadFromSnapshotPresenter>
   {
      void AddLogView(IView view);
      void BindTo(LoadFromSnapshotDTO loadFromSnapshotDTO);
      void EnableButtons(bool cancelEnabled, bool okEnabled = false, bool startEnabled = false);
   }
}