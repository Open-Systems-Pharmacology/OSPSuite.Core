using System;
using System.Collections.Generic;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public class CurveColorGroupingEventArgs : EventArgs
   {
      /// <summary>
      ///    Selected observed data metaData according to which curves should be assigned the same color
      /// </summary>
      public IEnumerable<string> SelectedMetaData { get; }

      public CurveColorGroupingEventArgs(IEnumerable<string> selectedMetaData)
      {
         SelectedMetaData = selectedMetaData;
      }
   }

   public interface ICurveColorGroupingPresenter : IPresenter<ICurveColorGroupingView>, IDisposablePresenter
   {
      void SetMetadata(IEnumerable<string> metaDataCategories);

      event EventHandler<CurveColorGroupingEventArgs> ApplySelectedColorGrouping;

      void ApplyColorGroupingClicked(IEnumerable<string> selectedMetaData);
   }

   public class CurveColorGroupingPresenter : AbstractDisposablePresenter<ICurveColorGroupingView, ICurveColorGroupingPresenter>, ICurveColorGroupingPresenter
   {
      public event EventHandler<CurveColorGroupingEventArgs> ApplySelectedColorGrouping = delegate { };
      void ICurveColorGroupingPresenter.ApplyColorGroupingClicked(IEnumerable<string> selectedMetaData)
      {
         ApplySelectedColorGrouping.Invoke(this, new CurveColorGroupingEventArgs(selectedMetaData));
      }

      public CurveColorGroupingPresenter(ICurveColorGroupingView view) : base(view)
      {
      }

      public void SetMetadata(IEnumerable<string> metaDataCategories)
      {
         _view.SetMetadata(metaDataCategories);
      }
   }
}
