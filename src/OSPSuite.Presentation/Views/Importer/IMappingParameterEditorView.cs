using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IMappingParameterEditorView : IView<IMappingParameterEditorPresenter>
   {
      void HideAll();
      void ShowUnits();
      void ShowLloq();
      void ShowErrorTypes();
      void FillUnitsView(IView view);
      void FillLloqView(IView view);
      void FillErrorView(IView view);
      void HideUnits();
   }
}