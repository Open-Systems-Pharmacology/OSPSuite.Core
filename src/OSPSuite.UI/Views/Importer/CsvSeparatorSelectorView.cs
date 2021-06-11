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

      public void SetFileName(string fileName)
      {
         separatorDescriptionLabelControl.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         separatorDescriptionLabelControl.Text = Captions.Importer.CsvSeparatorDescription(fileName);
         addRowsToText(fileName);
         separatorComboBoxEdit.SelectedIndex = 0;
      }

      private void addRowsToText(string fileName)
      {
         System.IO.StreamReader file = new System.IO.StreamReader(fileName);
         int counter = 0;
         string line;
         separatorDescriptionLabelControl.Text += ":\r\n";
         while ((line = file.ReadLine()) != null && counter < 3)
         {
            separatorDescriptionLabelControl.Text += "\r\n" + line;
            counter++;
         }
         separatorDescriptionLabelControl.Text += "\r\n...";
         file.Close();
      }
   }
}
