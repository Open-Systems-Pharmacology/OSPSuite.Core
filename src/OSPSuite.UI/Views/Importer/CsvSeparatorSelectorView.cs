using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using static OSPSuite.Assets.Captions.Importer;

namespace OSPSuite.UI.Views.Importer
{
   public partial class CSVSeparatorSelectorView : BaseModalView, ICSVSeparatorSelectorView
   {
      private ICSVSeparatorSelectorPresenter _presenter;
      private const char Comma = ',';
      private const char Period = '.';
      private readonly List<char> _columnSeparatorList = new List<char> { Comma, ' ', ';', Period };
      private readonly List<char> _decimalSeparatorList = new List<char> { Period, Comma };

      public CSVSeparatorSelectorView()
      {
         InitializeComponent();
         fillSeparatorComboBox();
         columnSeparatorComboBoxEdit.EditValueChanged += (s, a) => OnEvent(onSeparatorChanged);
         decimalSeparatorComboBoxEdit.EditValueChanged += (s, a) => OnEvent(onSeparatorChanged);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         previewMemoEdit.Enabled = false;
         previewMemoEdit.Properties.LinesCount = _presenter.PreviewLineCount;
         previewMemoEdit.Properties.AutoHeight = true;
         Caption = SeparatorSelection;

         columnSeparatorLayoutControlItem.Text = ColumnSeparator.FormatForLabel();
         decimalSeparatorLayoutControlItem.Text = DecimalSeparator.FormatForLabel();
      }

      private void fillSeparatorComboBox()
      {
         columnSeparatorComboBoxEdit.Properties.Items.AddRange(_columnSeparatorList);
         decimalSeparatorComboBoxEdit.Properties.Items.AddRange(_decimalSeparatorList);
      }

      private void onSeparatorChanged()
      {
         _presenter.SetColumnSeparator(selectedCharacterFromComboBox(columnSeparatorComboBoxEdit));
         configureDecimalSeparatorSelection();

         _presenter.SetDecimalSeparator(selectedCharacterFromComboBox(decimalSeparatorComboBoxEdit));
      }

      private void configureDecimalSeparatorSelection()
      {
         decimalSeparatorLayoutControlItem.Visibility = LayoutVisibilityConvertor.FromBoolean(!columnSeparatorComboBoxEdit.SelectedItem.Equals(Comma));
         if (!decimalSeparatorComboBoxEdit.Visible)
            resetDecimalSeparator();
      }

      private void resetDecimalSeparator()
      {
         decimalSeparatorComboBoxEdit.SelectedIndex = _decimalSeparatorList.IndexOf(Period);
      }

      private static char selectedCharacterFromComboBox(ComboBoxEdit comboBoxEdit)
      {
         return char.Parse(comboBoxEdit.SelectedItem.ToString());
      }

      public void AttachPresenter(ICSVSeparatorSelectorPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetPreview(string previewText)
      {
         previewMemoEdit.Text = previewText;
         columnSeparatorComboBoxEdit.SelectedIndex = 0;
         resetDecimalSeparator();
      }

      public void SetInstructions(string fileName)
      {
         fileNameLabel.Text = fileName;
      }
   }
}