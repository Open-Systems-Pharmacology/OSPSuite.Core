using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface ICsvSeparatorSelectorView : IModalView<ICsvSeparatorSelectorPresenter>
   {
      void SetFileName(string fileName);
   }
}
