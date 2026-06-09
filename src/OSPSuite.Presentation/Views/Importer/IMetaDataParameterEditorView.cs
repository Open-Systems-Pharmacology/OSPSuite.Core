using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IMetaDataParameterEditorView : IView<IMetaDataParameterEditorPresenter>
   {
      string Input { get; }
   }
}
