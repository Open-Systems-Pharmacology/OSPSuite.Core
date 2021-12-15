using System.Drawing;
using OSPSuite.Assets;
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
         styleComboBoxEdit.Properties.Items.Insert(0, Captions.Chart.MultiCurveOptions.CurrentValue);
         styleComboBoxEdit.SelectedIndex = 0;
         symbolComboBoxEdit.FillComboBoxEditorWith(EnumHelper.AllValuesFor<Symbols>());
         symbolComboBoxEdit.Properties.Items.Insert(0, Captions.Chart.MultiCurveOptions.CurrentValue);
         symbolComboBoxEdit.SelectedIndex = 0;
         visibleComboBoxEdit.FillComboBoxEditorWith(EnumHelper.AllValuesFor<YesNoValues>());
         visibleComboBoxEdit.Properties.Items.Insert(0, Captions.Chart.MultiCurveOptions.CurrentValue);
         visibleComboBoxEdit.SelectedIndex = 0;
         inLegendComboBoxEdit.FillComboBoxEditorWith(EnumHelper.AllValuesFor<YesNoValues>());
         inLegendComboBoxEdit.Properties.Items.Insert(0, Captions.Chart.MultiCurveOptions.CurrentValue);
         inLegendComboBoxEdit.SelectedIndex = 0;
      }

      public void AttachPresenter(ICurveMultiItemEditorPresenter presenter)
      {
         _presenter = presenter;
      }

      public SelectedCurveValues GetSelectedValues()
      {
         Color? selectedColor;
         if (colorPickEdit1.Color.IsEmpty)
            selectedColor = null;
         else
            selectedColor = colorPickEdit1.Color;

         return new SelectedCurveValues()
         {
            Color = selectedColor,
            Style = styleComboBoxEdit.SelectedItem.ToString(),
            Symbol = symbolComboBoxEdit.SelectedItem.ToString(),
            Visible = visibleComboBoxEdit.SelectedItem.ToString(),
            VisibleInLegend = inLegendComboBoxEdit.SelectedItem.ToString()
      };
      }
   }
}