using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface ICSVSeparatorSelectorView : IModalView<ICSVSeparatorSelectorPresenter>
   {
      void SetPreview(string previewText);
      void SetInstructions(string fileName);
   }
}