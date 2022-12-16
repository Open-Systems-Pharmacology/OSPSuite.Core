using System.Collections.Generic;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Assets.Captions.Importer;

namespace OSPSuite.UI.Views.Importer
{
   public partial class CSVSeparatorSelectorView : BaseModalView, ICSVSeparatorSelectorView
   {
      private ICsvSeparatorSelectorPresenter _presenter;
      private const char Comma = ',';
      private readonly List<char> _columnSeparatorList = new List<char> { Comma, '.', ' ', ';' };

      public CSVSeparatorSelectorView()
      {
         InitializeComponent();
         fillSeparatorComboBox();
         separatorComboBoxEdit.EditValueChanged += (s, a) => OnEvent(onSeparatorChanged);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         previewMemoEdit.Enabled = false;
         previewMemoEdit.Properties.LinesCount = 3;
         previewMemoEdit.Properties.AutoHeight = true;
         Caption = SeparatorSelection;
      }

      private void fillSeparatorComboBox()
      {
         _columnSeparatorList.Each(separator=> separatorComboBoxEdit.Properties.Items.Add(separator));
      }

      private void onSeparatorChanged()
      {
         _presenter.SelectedSeparator = char.Parse(separatorComboBoxEdit.SelectedItem.ToString());
      }

      public void AttachPresenter(ICsvSeparatorSelectorPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetPreview(string previewText)
      {
         previewMemoEdit.Text = previewText;
         separatorComboBoxEdit.SelectedIndex = 0;
      }

      public void SetInstructions(string fileName)
      {
         fileNameLabel.Text = fileName;
      }
   }
}