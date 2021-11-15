using System.Collections.Generic;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface ICurveColorGroupingPresenter : IPresenter<ICurveColorGroupingView>, IDisposablePresenter
   {
      void SetMetadata(IEnumerable<string> metaDataCategories);
      void Show();
      bool Canceled();

      IEnumerable<string> GetSelectedItems();
   }

   public class CurveColorGroupingPresenter : AbstractDisposablePresenter<ICurveColorGroupingView, ICurveColorGroupingPresenter>, ICurveColorGroupingPresenter
   {
      public CurveColorGroupingPresenter(ICurveColorGroupingView view) : base(view)
      {
      }
      public void Show()
      {
         _view.Display();
      }

      public bool Canceled()
      {
         return _view.Canceled;
      }

      public IEnumerable<string> GetSelectedItems()
      {
         return _view.GetSelectedItems();
      }

      public void SetMetadata(IEnumerable<string> metaDataCategories)
      {
         _view.SetMetadata(metaDataCategories);
      }
   }
}
