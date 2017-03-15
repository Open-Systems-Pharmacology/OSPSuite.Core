using OSPSuite.Presentation.Presenters.ObservedData;

namespace OSPSuite.Presentation.Views.ObservedData
{
   public interface IEditMultipleDataRepositoriesMetaDataView : IModalView<IEditMultipleDataRepositoriesMetaDataPresenter>
   {
      /// <summary>
      /// Sets the internal editor view for this presenter (this presenter is only the modal dialog functionality)
      /// </summary>
      /// <param name="view">The view to be used inside the dialog</param>
      void SetDataEditor(IView view);
   }
}