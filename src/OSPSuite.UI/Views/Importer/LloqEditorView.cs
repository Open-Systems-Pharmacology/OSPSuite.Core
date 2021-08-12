using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class LloqEditorView : BaseUserControl, ILloqEditorView
   {
      public LloqEditorView()
      {
         InitializeComponent();
         LloqToggleSwitch.IsOnChanged += (s,a) => OnEvent(onIsOnChanged);
         lloqToggleLayoutControlItem.Text = Captions.Importer.ImportLLOQFromColumn;
      }

      private ILloqEditorPresenter _presenter;
      public void AttachPresenter(ILloqEditorPresenter presenter)
      {
         _presenter = presenter;
      }

      private void onIsOnChanged()
      {
         if (LloqToggleSwitch.IsOn)
         {
            LloqDescriptionLabelLayoutControlItem.Visibility = LayoutVisibility.Never;
            LloqColumnLayoutControlItem.Visibility = LayoutVisibility.Always;
         }
         else
         {
            LloqDescriptionLabelLayoutControlItem.Visibility = LayoutVisibility.Always;
            LloqColumnLayoutControlItem.Visibility = LayoutVisibility.Never;
         }
      }
      public void FillComboBox(IEnumerable<string> columns, string defaultValue)
      {
         //ColumnsComboBox.Properties.Items.Clear();
         //ColumnsComboBox.Properties.Items.AddRange(columns.ToArray());
         //ColumnsComboBox.EditValue = defaultValue;
      }

      public void FillLloqSelector(IView view)
      {
         LloqColumnPanelControl.FillWith(view);
      }

      public void SetLloqToggle(bool lloqColumnsSelection)
      {
         LloqToggleSwitch.IsOn = lloqColumnsSelection;
      }

      public bool IsLloqToggleOn()
      {
         return LloqToggleSwitch.IsOn;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Text = Captions.Importer.LloqColumnEditorTitle;
         LloqDescriptionLabelControl.Text = Captions.Importer.LloqDescription;
         LloqColumnPanelControl.Dock = DockStyle.Fill;
         LloqColumnLayoutControlItem.Text = Captions.Importer.Column;
         //LloqDescriptionLabelLayoutControlItem.Visibility = LayoutVisibility.Never; //actually this should depend on the file
      }
   }
}