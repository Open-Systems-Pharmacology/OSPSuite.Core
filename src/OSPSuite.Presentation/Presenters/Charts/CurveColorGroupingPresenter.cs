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
      public IReadOnlyList<string> SelectedMetaData { get; }

      public CurveColorGroupingEventArgs(IReadOnlyList<string> selectedMetaData)
      {
         SelectedMetaData = selectedMetaData;
      }
   }

   public interface ICurveColorGroupingPresenter : IPresenter<ICurveColorGroupingView>, IDisposablePresenter
   {
      void SetMetadata(IReadOnlyList<string> metaDataCategories);

      event EventHandler<CurveColorGroupingEventArgs> ApplySelectedColorGrouping;

      void ApplyColorGroupingButtonClicked(IReadOnlyList<string> selectedMetaData);
   }

   public class CurveColorGroupingPresenter : AbstractDisposablePresenter<ICurveColorGroupingView, ICurveColorGroupingPresenter>, ICurveColorGroupingPresenter
   {
      public event EventHandler<CurveColorGroupingEventArgs> ApplySelectedColorGrouping = delegate { };
      void ICurveColorGroupingPresenter.ApplyColorGroupingButtonClicked(IReadOnlyList<string> selectedMetaData)
      {
         ApplySelectedColorGrouping.Invoke(this, new CurveColorGroupingEventArgs(selectedMetaData));
      }

      public CurveColorGroupingPresenter(ICurveColorGroupingView view) : base(view)
      {
      }

      public void SetMetadata(IReadOnlyList<string> metaDataCategories)
      {
         _view.SetMetadata(metaDataCategories);
      }
   }
}
