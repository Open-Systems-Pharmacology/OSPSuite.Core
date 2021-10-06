using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IMetaDataParameterEditorPresenter : IDisposablePresenter
   {
      string Input { get; }
   }

   public class MetaDataParameterEditorPresenter : AbstractDisposablePresenter<IMetaDataParameterEditorView, IMetaDataParameterEditorPresenter>, IMetaDataParameterEditorPresenter
   {
      public string Input => View.Input;

      public MetaDataParameterEditorPresenter(
         IMetaDataParameterEditorView view
      ) : base(view)
      {
      }
   }
}
