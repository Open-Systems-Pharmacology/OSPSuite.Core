using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Charts
{
   public partial class CurveColorGroupingView : BaseUserControl, ICurveColorGroupingView
   {
      private ICurveColorGroupingPresenter _presenter;

      public CurveColorGroupingView()
      {
         InitializeComponent();
         colorGroupingDescriptionLabelControl.Text = Captions.Chart.ColorGrouping.ColorGroupingDialogDescription.FormatForLabel();
         applyColorGroupingButton.Text = Captions.Chart.ColorGrouping.ApplyColorGroupingButton;
         applyColorGroupingButton.Click += (s, a) => OnEvent(onApplyColorGroupingButtonClicked, s, a);
      }

      public void AttachPresenter(ICurveColorGroupingPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetMetadata(IEnumerable<string> metaDataCategories)
      {
         foreach (var metaDataCategory in metaDataCategories)
         {
            metaDataCheckedListBoxControl.Items.Add(metaDataCategory);
         }
      }

      public IEnumerable<string> GetSelectedItems()
      {
         throw new NotImplementedException();
      }

      private IEnumerable<string> getSelectedItems()
      {
         return metaDataCheckedListBoxControl.CheckedIndices.Select(index => metaDataCheckedListBoxControl.GetItemText(index)).ToList();
      }

      private void onApplyColorGroupingButtonClicked(object sender, EventArgs eventArgs)
      {
         _presenter.ApplyColorGroupingClicked(getSelectedItems());
      }
   }
}