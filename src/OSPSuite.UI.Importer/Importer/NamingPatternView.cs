using System;
using System.Collections.Generic;
using DevExpress.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenter;
using OSPSuite.Presentation.View;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Importer
{
   public partial class NamingPatternView : BaseUserControl, INamingPatternView
   {
      private INamingPatternPresenter _presenter;

      public NamingPatternView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(INamingPatternPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         cbPatternComboBox.TextChanged += (o, e) => OnEvent(patternComboBoxOnTextChanged, e);
      }

      private void patternComboBoxOnTextChanged(EventArgs e)
      {
         _presenter.Pattern = cbPatternComboBox.Text;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         cbPatternComboBox.SuperTip = new SuperToolTip().WithText(Captions.Importer.ToolTips.NamingPattern);
         layoutItemNamingPattern.Text = Captions.Importer.NamingPattern.FormatForLabel();

         lblNamingPatternDescription.AsDescription();
      }

      public void SetNamingPatternDescriptiveText()
      {
         lblNamingPatternDescription.Text = _presenter.NamingPatternDescription.FormatForDescription();
      }

      public void UpdateNamingPatterns(List<string> patterns)
      {
         cbPatternComboBox.Properties.Items.Clear();
         cbPatternComboBox.Properties.Items.AddRange(patterns);
         if(cbPatternComboBox.Properties.Items.Count > 0)
            cbPatternComboBox.SelectedIndex = 0;
      }
   }
}