using OSPSuite.Assets;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   public partial class DeviationLinesView : BaseModalView, IDeviationLinesView
   {
      private IDeviationLinesPresenter _presenter;
      public DeviationLinesView()
      {
         InitializeComponent();
         layoutControlItem1.TextVisible = false;
         labelControl1.Text = Captions.Chart.DeviationLines.SpecifyFoldValue.FormatForLabel();
         foldValueTextEdit.Properties.Mask.EditMask = "\\d+(\\R.\\d{0,2})?";
         foldValueTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;

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