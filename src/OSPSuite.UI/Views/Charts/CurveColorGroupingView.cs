using System.Collections.Generic;
using System.Linq;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.UI.Views.Charts
{
   public partial class CurveColorGroupingView : BaseModalView, ICurveColorGroupingView
   {
      private ICurveColorGroupingPresenter _presenter;

      public CurveColorGroupingView()
      {
         InitializeComponent();
         colorGroupingDescriptionLabelControl.Text = Captions.Chart.ColorGrouping.ColorGroupingDialogDescription.FormatForLabel();
         this.Text = Captions.Chart.ColorGrouping.ColorGroupingByMetaData;
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
         return metaDataCheckedListBoxControl.CheckedIndices.Select(index => metaDataCheckedListBoxControl.GetItemText(index)).ToList();
      }
   }
}