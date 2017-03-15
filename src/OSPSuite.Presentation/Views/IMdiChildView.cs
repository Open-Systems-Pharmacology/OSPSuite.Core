using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IMdiChildView : IContainerView
   {
      void Display();

      /// <summary>
      /// This function should be call whenever the mdi child view should be closed programmatically
      /// </summary>
      void CloseView();

      /// <summary>
      /// Save any values being edited in the model
      /// </summary>
      void SaveChanges();

      ISingleStartPresenter Presenter { get; }
   }



   public interface IMdiChildView<TPresenter> : IView<TPresenter>, IMdiChildView where TPresenter : ISingleStartPresenter
   {
   }
}