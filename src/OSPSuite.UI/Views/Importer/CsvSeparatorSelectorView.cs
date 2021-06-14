using System;
using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.UI.Views.Importer
{
   public partial class CsvSeparatorSelectorView : BaseModalView, ICsvSeparatorSelectorView
   {
      private ICsvSeparatorSelectorPresenter _presenter;
      public CsvSeparatorSelectorView()
      {
         InitializeComponent();
         fillSeparatorComboBox();
         separatorComboBoxEdit.EditValueChanged += onSeparatorChanged;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         separatorDescriptionLabelControl.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
      }

      private void fillSeparatorComboBox()
      {
         var separatorList = new List<char>() {',', '.', ' ', ';'};

         foreach (var separator in separatorList)
         {
            separatorComboBoxEdit.Properties.Items.Add(separator);
         }
      }

      private void onSeparatorChanged(object sender, EventArgs e)
      {
         _presenter.SelectedSeparator = char.Parse(separatorComboBoxEdit.SelectedItem.ToString());
      }

      public void AttachPresenter(ICsvSeparatorSelectorPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetDescription(string description)
      {
         separatorDescriptionLabelControl.Text = description;
         separatorComboBoxEdit.SelectedIndex = 0;
      }
   }
}
