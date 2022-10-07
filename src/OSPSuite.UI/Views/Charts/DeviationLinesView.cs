using OSPSuite.Assets;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   public partial class DeviationLinesView : BaseModalView, IDeviationLinesView
   {
      private IDeviationLinesPresenter _presenter;
      public DeviationLinesView()
      {
         InitializeComponent();
         foldValueInputControlItem.TextVisible = false;
         foldValueTextLabelControl.Text = Captions.Chart.DeviationLines.SpecifyFoldValue.FormatForLabel();
         deviationLineDescriptionLabelControl.AsDescription();
         deviationLineDescriptionLabelControl.Text = Captions.Chart.DeviationLines.DeviationLineDescription.FormatForDescription();
         //Regex: only numbers, with 2 points of decimal precision, and greater than 1
         foldValueTextEdit.Properties.Mask.EditMask = "[1-9](\\d+)?(\\R.\\d{0,2})?";
         foldValueTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         foldValueTextEdit.Select();
      }

      public void AttachPresenter(IDeviationLinesPresenter presenter)
      {
         _presenter = presenter;
      }

      public float GetFoldValue()
      {
         return foldValueTextEdit.EditValue.ConvertedTo<float>();
      }
   }
}