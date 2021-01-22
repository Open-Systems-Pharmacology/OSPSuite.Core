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
         //ColumnsComboBox.EditValueChanged += (s, e) => OnEvent(() => _presenter.SetLloqColumn(ColumnsComboBox.EditValue.ToString()));
         LloqToggleSwitch.IsOnChanged += onIsOnChanged;
      }

      private ILloqEditorPresenter _presenter;
      public void AttachPresenter(ILloqEditorPresenter presenter)
      {
         _presenter = presenter;
      }

      private void onIsOnChanged(object sender, EventArgs e)
      {
         if (LloqToggleSwitch.IsOn)
         {
            LloqDescriptionLabelLayoutControlItem.Visibility = LayoutVisibility.Never;
            LloqDescriptionLayoutControlItem.Visibility = LayoutVisibility.Always;
            //LloqDescriptionPanelControl.Visible = true;
            //ColumnsComboBoxLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            //onColumnComboBoxTextChanged();
         }
         else
         {
            LloqDescriptionLabelLayoutControlItem.Visibility = LayoutVisibility.Always;
            LloqDescriptionLayoutControlItem.Visibility = LayoutVisibility.Never;

            //LloqDescriptionLayoutControlItem.TextVisible = true;
            //LloqDescriptionPanelControl.Visible = false;

            //ColumnsComboBoxLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            //here we have to instead present the text
            //_columnLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //onUnitComboBoxTextChanged();
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
         LloqDescriptionPanelControl.FillWith(view);
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
         LloqDescriptionPanelControl.Dock = DockStyle.Fill;
         //LloqDescriptionLabelLayoutControlItem.Visibility = LayoutVisibility.Never; //actually this should depend on the file
      }
   }
}