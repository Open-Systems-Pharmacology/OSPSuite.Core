using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.SensitivityAnalyses
{
   partial class SensitivityAnalysisParametersView
   {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
         _gridViewBinder.Dispose();
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.tablePanel = new DevExpress.Utils.Layout.TablePanel();
         this.labelSetRange = new DevExpress.XtraEditors.LabelControl();
         this.labelSetNMax = new DevExpress.XtraEditors.LabelControl();
         this.btnRemoveAll = new DevExpress.XtraEditors.SimpleButton();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new OSPSuite.UI.Controls.UxGridView();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemParameters = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemRemoveAll = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.multiSetValueControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemTablePanel = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.lblNumberOfParameters = new DevExpress.XtraEditors.LabelControl();
         this.layoutItemNumberOfParameters = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         this.tablePanel.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRemoveAll)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.multiSetValueControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNumberOfParameters)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.lblNumberOfParameters);
         this.layoutControl.Controls.Add(this.tablePanel);
         this.layoutControl.Controls.Add(this.btnRemoveAll);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(772, 200, 701, 527);
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(1584, 701);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // tablePanel
         // 
         this.tablePanel.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 17.32F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 42.68F)});
         this.tablePanel.Controls.Add(this.labelSetRange);
         this.tablePanel.Controls.Add(this.labelSetNMax);
         this.tablePanel.Location = new System.Drawing.Point(14, 35);
         this.tablePanel.Name = "tablePanel";
         this.tablePanel.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
         this.tablePanel.Size = new System.Drawing.Size(766, 67);
         this.tablePanel.TabIndex = 9;
         // 
         // labelSetRange
         // 
         this.tablePanel.SetColumn(this.labelSetRange, 0);
         this.labelSetRange.Location = new System.Drawing.Point(3, 32);
         this.labelSetRange.Name = "labelSetRange";
         this.tablePanel.SetRow(this.labelSetRange, 1);
         this.labelSetRange.Size = new System.Drawing.Size(69, 13);
         this.labelSetRange.TabIndex = 1;
         this.labelSetRange.Text = "labelSetRange";
         // 
         // labelSetNMax
         // 
         this.tablePanel.SetColumn(this.labelSetNMax, 0);
         this.labelSetNMax.Location = new System.Drawing.Point(3, 6);
         this.labelSetNMax.Name = "labelSetNMax";
         this.tablePanel.SetRow(this.labelSetNMax, 0);
         this.labelSetNMax.Size = new System.Drawing.Size(65, 13);
         this.labelSetNMax.TabIndex = 0;
         this.labelSetNMax.Text = "labelSetNMax";
         // 
         // btnRemoveAll
         // 
         this.btnRemoveAll.Location = new System.Drawing.Point(565, 118);
         this.btnRemoveAll.Name = "btnRemoveAll";
         this.btnRemoveAll.Size = new System.Drawing.Size(1017, 22);
         this.btnRemoveAll.StyleController = this.layoutControl;
         this.btnRemoveAll.TabIndex = 5;
         this.btnRemoveAll.Text = "btnRemoveAll";
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(121, 144);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(1461, 555);
         this.gridControl.TabIndex = 4;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.GridControl = this.gridControl;
         this.gridView.MultiSelect = false;
         this.gridView.Name = "gridView";
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemParameters,
            this.layoutItemRemoveAll,
            this.emptySpaceItem1,
            this.multiSetValueControlGroup,
            this.emptySpaceItem2,
            this.layoutItemNumberOfParameters});
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(1584, 701);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemParameters
         // 
         this.layoutItemParameters.Control = this.gridControl;
         this.layoutItemParameters.Location = new System.Drawing.Point(0, 142);
         this.layoutItemParameters.Name = "layoutItemParameters";
         this.layoutItemParameters.Size = new System.Drawing.Size(1584, 559);
         this.layoutItemParameters.TextSize = new System.Drawing.Size(107, 13);
         // 
         // layoutItemRemoveAll
         // 
         this.layoutItemRemoveAll.Control = this.btnRemoveAll;
         this.layoutItemRemoveAll.Location = new System.Drawing.Point(563, 116);
         this.layoutItemRemoveAll.Name = "layoutItemRemoveAll";
         this.layoutItemRemoveAll.Size = new System.Drawing.Size(1021, 26);
         this.layoutItemRemoveAll.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemRemoveAll.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(794, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(790, 116);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // multiSetValueControlGroup
         // 
         this.multiSetValueControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemTablePanel});
         this.multiSetValueControlGroup.Location = new System.Drawing.Point(0, 0);
         this.multiSetValueControlGroup.Name = "multiSetValueControlGroup";
         this.multiSetValueControlGroup.Size = new System.Drawing.Size(794, 116);
         // 
         // layoutItemTablePanel
         // 
         this.layoutItemTablePanel.Control = this.tablePanel;
         this.layoutItemTablePanel.Location = new System.Drawing.Point(0, 0);
         this.layoutItemTablePanel.Name = "layoutItemTablePanel";
         this.layoutItemTablePanel.Size = new System.Drawing.Size(770, 71);
         this.layoutItemTablePanel.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemTablePanel.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(118, 116);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(445, 26);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // lblNumberOfParameters
         // 
         this.lblNumberOfParameters.Location = new System.Drawing.Point(2, 127);
         this.lblNumberOfParameters.Name = "lblNumberOfParameters";
         this.lblNumberOfParameters.Size = new System.Drawing.Size(114, 13);
         this.lblNumberOfParameters.StyleController = this.layoutControl;
         this.lblNumberOfParameters.TabIndex = 10;
         this.lblNumberOfParameters.Text = "lblNumberOfParameters";
         // 
         // layoutItemNumberOfParameters
         // 
         this.layoutItemNumberOfParameters.ContentVertAlignment = DevExpress.Utils.VertAlignment.Bottom;
         this.layoutItemNumberOfParameters.Control = this.lblNumberOfParameters;
         this.layoutItemNumberOfParameters.Location = new System.Drawing.Point(0, 116);
         this.layoutItemNumberOfParameters.Name = "layoutItemNumberOfParameters";
         this.layoutItemNumberOfParameters.Size = new System.Drawing.Size(118, 26);
         this.layoutItemNumberOfParameters.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemNumberOfParameters.TextVisible = false;
         // 
         // SensitivityAnalysisParametersView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "SensitivityAnalysisParametersView";
         this.Size = new System.Drawing.Size(1584, 701);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         this.tablePanel.ResumeLayout(false);
         this.tablePanel.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRemoveAll)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.multiSetValueControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNumberOfParameters)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private UxGridControl gridControl;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemParameters;
      private DevExpress.XtraEditors.SimpleButton btnRemoveAll;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRemoveAll;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlGroup multiSetValueControlGroup;
      private UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.Utils.Layout.TablePanel tablePanel;
      private DevExpress.XtraEditors.LabelControl labelSetRange;
      private DevExpress.XtraEditors.LabelControl labelSetNMax;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemTablePanel;
      private DevExpress.XtraEditors.LabelControl lblNumberOfParameters;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemNumberOfParameters;
   }
}
