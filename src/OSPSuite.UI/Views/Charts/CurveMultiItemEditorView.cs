using System.Drawing;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility;

namespace OSPSuite.UI.Views.Charts
{
   public partial class CurveMultiItemEditorView : BaseModalView, ICurveMultiItemEditorView
   {
      private ICurveMultiItemEditorPresenter _presenter;

      public CurveMultiItemEditorView()
      {
         InitializeComponent();

         styleComboBoxEdit.FillComboBoxEditorWith(EnumHelper.AllValuesFor<LineStyles>());
         styleComboBoxEdit.Properties.Items.Add("<Current value>");
         styleComboBoxEdit.SelectedItem = "<Current value>";
         symbolComboBoxEdit.FillComboBoxEditorWith(EnumHelper.AllValuesFor<Symbols>());
         symbolComboBoxEdit.Properties.Items.Add("<Current value>");
         symbolComboBoxEdit.SelectedItem = "<Current value>";
         visibleComboBoxEdit.FillComboBoxEditorWith(EnumHelper.AllValuesFor<BooleanComboBox>());
         visibleComboBoxEdit.Properties.Items.Add("<Current value>");
         visibleComboBoxEdit.SelectedItem = "<Current value>";
         inLegendComboBoxEdit.FillComboBoxEditorWith(EnumHelper.AllValuesFor<BooleanComboBox>());
         inLegendComboBoxEdit.Properties.Items.Add("<Current value>");
         inLegendComboBoxEdit.SelectedItem = "<Current value>";
      }

      public void AttachPresenter(ICurveMultiItemEditorPresenter presenter)
      {
         _presenter = presenter;
      }

      public SelectedCurveValues GetSelectedValues()
      {
         //should all these castings actually happen here????
         bool? selectedVisible = null;
         var selectedVisibleItem = (BooleanComboBox?)visibleComboBoxEdit.SelectedItem;
         if(selectedVisibleItem !=null )
            selectedVisible = selectedVisibleItem == BooleanComboBox.Yes;

         bool? selectedInLegend = null;
         var selectedInLegendItem = (BooleanComboBox?)inLegendComboBoxEdit.SelectedItem;
         if (selectedInLegendItem != null)
            selectedInLegend = selectedInLegendItem == BooleanComboBox.Yes;

         Color? selectedColor;
         if (colorPickEdit1.Color.IsEmpty)
            selectedColor = null;
         else
            selectedColor = colorPickEdit1.Color;


         return new SelectedCurveValues()
         {
            //ok, so one option would really be to check here once for null
            Color = selectedColor,
            Style = (LineStyles?)styleComboBoxEdit.SelectedItem,
            Symbol = (Symbols?)symbolComboBoxEdit.SelectedItem,
            Visible = selectedVisible,
            VisibleInLegend = selectedInLegend
         };
      }
   }
}