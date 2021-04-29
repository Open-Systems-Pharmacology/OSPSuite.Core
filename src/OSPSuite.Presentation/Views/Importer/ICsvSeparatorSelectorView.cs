using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface ICsvSeparatorSelectorView : IView<ICsvSeparatorSelectorPresenter>
   {
      void SetFileName(string fileName);
   }
}
