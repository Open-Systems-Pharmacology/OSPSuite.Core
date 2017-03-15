using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public abstract class SubjectPresenter<TView, TPresenter, TSubject> : SingleStartPresenter<TView, TPresenter>, ISingleStartPresenter<TSubject>
      where TView : IView<TPresenter>, IMdiChildView
      where TPresenter : IPresenter
   {
      protected SubjectPresenter(TView view) : base(view)
      {
      }

      public abstract void Edit(TSubject objectToEdit);

      public override void Edit(object objectToEdit)
      {
         Edit(objectToEdit.DowncastTo<TSubject>());
      }
   }
}