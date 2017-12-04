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
         this.panelSetRange = new UxPanelControl();
         this.panelSetNMax = new UxPanelControl();
         this.btnRemoveAll = new DevExpress.XtraEditors.SimpleButton();
         this.gridControl = new UxGridControl();
         this.gridView = new UxGridView();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemParameters = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemRemoveAll = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.multiSetValueControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemSetNMax = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemSetRange = new DevExpress.XtraLayout.LayoutControlItem();
         this.lblNumberOfParameters = new DevExpress.XtraEditors.LabelControl();
         this.layoutItemNumberOfParameters = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelSetRange)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelSetNMax)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRemoveAll)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.multiSetValueControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSetNMax)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSetRange)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNumberOfParameters)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.lblNumberOfParameters);
         this.layoutControl.Controls.Add(this.panelSetRange);
         this.layoutControl.Controls.Add(this.panelSetNMax);
         this.layoutControl.Controls.Add(this.btnRemoveAll);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(772, 200, 701, 527);
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(1023, 414);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // panelSetRange
         // 
         this.panelSetRange.Location = new System.Drawing.Point(151, 49);
         this.panelSetRange.Name = "panelSetRange";
         this.panelSetRange.Size = new System.Drawing.Size(1, 38);
         this.panelSetRange.TabIndex = 7;
         // 
         // panelSetNMax
         // 
         this.panelSetNMax.Location = new System.Drawing.Point(151, 32);
         this.panelSetNMax.Name = "panelSetNMax";
         this.panelSetNMax.Size = new System.Drawing.Size(1, 13);
         this.panelSetNMax.TabIndex = 6;
         // 
         // btnRemoveAll
         // 
         this.btnRemoveAll.Location = new System.Drawing.Point(178, 94);
         this.btnRemoveAll.Name = "btnRemoveAll";
         this.btnRemoveAll.Size = new System.Drawing.Size(843, 22);
         this.btnRemoveAll.StyleController = this.layoutControl;
         this.btnRemoveAll.TabIndex = 5;
         this.btnRemoveAll.Text = "btnRemoveAll";
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(139, 120);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(882, 292);
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
            this.layoutItemNumberOfParameters,
            this.emptySpaceItem});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(1023, 414);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemParameters
         // 
         this.layoutItemParameters.Control = this.gridControl;
         this.layoutItemParameters.Location = new System.Drawing.Point(0, 118);
         this.layoutItemParameters.Name = "layoutItemParameters";
         this.layoutItemParameters.Size = new System.Drawing.Size(1023, 296);
         this.layoutItemParameters.TextSize = new System.Drawing.Size(134, 13);
         // 
         // layoutItemRemoveAll
         // 
         this.layoutItemRemoveAll.Control = this.btnRemoveAll;
         this.layoutItemRemoveAll.Location = new System.Drawing.Point(176, 92);
         this.layoutItemRemoveAll.Name = "layoutItemRemoveAll";
         this.layoutItemRemoveAll.Size = new System.Drawing.Size(847, 26);
         this.layoutItemRemoveAll.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemRemoveAll.TextVisible = false;
         // 
         // emptySpaceItem
         // 
         this.emptySpaceItem.AllowHotTrack = false;
         this.emptySpaceItem.Location = new System.Drawing.Point(166, 0);
         this.emptySpaceItem.Name = "emptySpaceItem";
         this.emptySpaceItem.Size = new System.Drawing.Size(10, 101);
         this.emptySpaceItem.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(176, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(847, 92);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // multiSetValueControlGroup
         // 
         this.multiSetValueControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemSetNMax,
            this.layoutControlItemSetRange});
         this.multiSetValueControlGroup.Location = new System.Drawing.Point(0, 0);
         this.multiSetValueControlGroup.Name = "multiSetValueControlGroup";
         this.multiSetValueControlGroup.Size = new System.Drawing.Size(166, 101);
         // 
         // layoutControlItemSetNMax
         // 
         this.layoutControlItemSetNMax.Control = this.panelSetNMax;
         this.layoutControlItemSetNMax.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemSetNMax.Name = "layoutControlItemSetNMax";
         this.layoutControlItemSetNMax.Size = new System.Drawing.Size(142, 17);
         this.layoutControlItemSetNMax.TextSize = new System.Drawing.Size(134, 13);
         // 
         // layoutControlItemSetRange
         // 
         this.layoutControlItemSetRange.Control = this.panelSetRange;
         this.layoutControlItemSetRange.Location = new System.Drawing.Point(0, 17);
         this.layoutControlItemSetRange.Name = "layoutControlItemSetRange";
         this.layoutControlItemSetRange.Size = new System.Drawing.Size(142, 42);
         this.layoutControlItemSetRange.TextSize = new System.Drawing.Size(134, 13);
         // 
         // lblNumberOfParameters
         // 
         this.lblNumberOfParameters.Location = new System.Drawing.Point(2, 103);
         this.lblNumberOfParameters.Name = "lblNumberOfParameters";
         this.lblNumberOfParameters.Size = new System.Drawing.Size(114, 13);
         this.lblNumberOfParameters.StyleController = this.layoutControl;
         this.lblNumberOfParameters.TabIndex = 8;
         this.lblNumberOfParameters.Text = "lblNumberOfParameters";
         // 
         // layoutItemNumberOfParameters
         // 
         this.layoutItemNumberOfParameters.Control = this.lblNumberOfParameters;
         this.layoutItemNumberOfParameters.Location = new System.Drawing.Point(0, 101);
         this.layoutItemNumberOfParameters.Name = "layoutItemNumberOfParameters";
         this.layoutItemNumberOfParameters.Size = new System.Drawing.Size(176, 17);
         this.layoutItemNumberOfParameters.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemNumberOfParameters.TextVisible = false;
         // 
         // SensitivityAnalysisParametersView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "SensitivityAnalysisParametersView";
         this.Size = new System.Drawing.Size(1023, 414);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelSetRange)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelSetNMax)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRemoveAll)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.multiSetValueControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSetNMax)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSetRange)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNumberOfParameters)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private UxGridControl gridControl;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemParameters;
      private DevExpress.XtraEditors.SimpleButton btnRemoveAll;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRemoveAll;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem;
      private Controls.UxPanelControl panelSetRange;
      private Controls.UxPanelControl panelSetNMax;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSetRange;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSetNMax;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlGroup multiSetValueControlGroup;
      private DevExpress.XtraEditors.LabelControl lblNumberOfParameters;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemNumberOfParameters;
   }
}
