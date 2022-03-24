using System.Collections.Generic;
using System.Linq;
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
         applyColorGroupingButton.Click += (o, e) => OnEvent(onApplyColorGroupingButtonClicked);
      }

      public void AttachPresenter(ICurveColorGroupingPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetMetadata(IReadOnlyList<string> metaDataCategories)
      {
         metaDataCheckedListBoxControl.Items.Clear();
         foreach (var metaDataCategory in metaDataCategories)
         {
            metaDataCheckedListBoxControl.Items.Add(metaDataCategory);
         }
      }

      private IReadOnlyList<string> getSelectedItems()
      {
         return metaDataCheckedListBoxControl.CheckedIndices.Select(index => metaDataCheckedListBoxControl.GetItemText(index)).ToList();
      }

      private void onApplyColorGroupingButtonClicked()
      {
         _presenter.ApplyColorGroupingButtonClicked(getSelectedItems());
      }
   }
}