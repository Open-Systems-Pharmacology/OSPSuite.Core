using OSPSuite.Utility.Extensions;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   partial class CategorialParameterIdentificationRunModeView
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
         _categoryScreenBinderCache.Each(binder => binder.Dispose());
         _categorialRunModeScreenBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.uxHintPanel = new OSPSuite.UI.Controls.UxHintPanel();
         this.chkAllTheSame = new OSPSuite.UI.Controls.UxCheckEdit();
         this.pivotGridControl = new OSPSuite.UI.Controls.UxPivotGrid();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemPivotGrid = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupCalculationMethodCategories = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemCheckAllTheSame = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItemHintPanel = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkAllTheSame.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPivotGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupCalculationMethodCategories)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCheckAllTheSame)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemHintPanel)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.uxHintPanel);
         this.layoutControl.Controls.Add(this.chkAllTheSame);
         this.layoutControl.Controls.Add(this.pivotGridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(371, 408, 962, 552);
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(514, 216);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // uxHintPanel
         // 
         this.uxHintPanel.Location = new System.Drawing.Point(0, 49);
         this.uxHintPanel.MaximumSize = new System.Drawing.Size(1000000, 40);
         this.uxHintPanel.MinimumSize = new System.Drawing.Size(200, 0);
         this.uxHintPanel.Name = "uxHintPanel";
         this.uxHintPanel.NoteText = "uxHintPanel";
         this.uxHintPanel.Size = new System.Drawing.Size(514, 40);
         this.uxHintPanel.TabIndex = 7;
         // 
         // chkAllTheSame
         // 
         this.chkAllTheSame.AllowClicksOutsideControlArea = false;
         this.chkAllTheSame.Location = new System.Drawing.Point(191, 14);
         this.chkAllTheSame.Name = "chkAllTheSame";
         this.chkAllTheSame.Properties.Caption = "chkAllTheSame";
         this.chkAllTheSame.Size = new System.Drawing.Size(301, 19);
         this.chkAllTheSame.StyleController = this.layoutControl;
         this.chkAllTheSame.TabIndex = 6;
         // 
         // pivotGridControl
         // 
         this.pivotGridControl.Location = new System.Drawing.Point(0, 93);
         this.pivotGridControl.Name = "pivotGridControl";
         this.pivotGridControl.Size = new System.Drawing.Size(514, 121);
         this.pivotGridControl.TabIndex = 5;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemPivotGrid,
            this.layoutGroupCalculationMethodCategories,
            this.layoutControlItemHintPanel});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(514, 216);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemPivotGrid
         // 
         this.layoutItemPivotGrid.Control = this.pivotGridControl;
         this.layoutItemPivotGrid.Location = new System.Drawing.Point(0, 91);
         this.layoutItemPivotGrid.Name = "layoutItemPivotGrid";
         this.layoutItemPivotGrid.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 2, 2);
         this.layoutItemPivotGrid.Size = new System.Drawing.Size(514, 125);
         this.layoutItemPivotGrid.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemPivotGrid.TextVisible = false;
         // 
         // layoutGroupCalculationMethodCategories
         // 
         this.layoutGroupCalculationMethodCategories.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemCheckAllTheSame,
            this.emptySpaceItem,
            this.emptySpaceItem1});
         this.layoutGroupCalculationMethodCategories.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupCalculationMethodCategories.Name = "layoutGroupCalculationMethodCategories";
         this.layoutGroupCalculationMethodCategories.Size = new System.Drawing.Size(514, 47);
         this.layoutGroupCalculationMethodCategories.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 2, 2);
         this.layoutGroupCalculationMethodCategories.TextVisible = false;
         // 
         // layoutItemCheckAllTheSame
         // 
         this.layoutItemCheckAllTheSame.Control = this.chkAllTheSame;
         this.layoutItemCheckAllTheSame.Location = new System.Drawing.Point(179, 0);
         this.layoutItemCheckAllTheSame.Name = "layoutItemCheckAllTheSame";
         this.layoutItemCheckAllTheSame.Size = new System.Drawing.Size(305, 23);
         this.layoutItemCheckAllTheSame.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemCheckAllTheSame.TextVisible = false;
         // 
         // emptySpaceItem
         // 
         this.emptySpaceItem.AllowHotTrack = false;
         this.emptySpaceItem.Location = new System.Drawing.Point(0, 0);
         this.emptySpaceItem.MaxSize = new System.Drawing.Size(179, 23);
         this.emptySpaceItem.MinSize = new System.Drawing.Size(179, 23);
         this.emptySpaceItem.Name = "emptySpaceItem";
         this.emptySpaceItem.Size = new System.Drawing.Size(179, 23);
         this.emptySpaceItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.emptySpaceItem.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(484, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(10, 23);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItemHintPanel
         // 
         this.layoutControlItemHintPanel.Control = this.uxHintPanel;
         this.layoutControlItemHintPanel.Location = new System.Drawing.Point(0, 47);
         this.layoutControlItemHintPanel.Name = "layoutControlItemHintPanel";
         this.layoutControlItemHintPanel.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 2, 2);
         this.layoutControlItemHintPanel.Size = new System.Drawing.Size(514, 44);
         this.layoutControlItemHintPanel.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemHintPanel.TextVisible = false;
         // 
         // CategorialParameterIdentificationRunModeView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "CategorialParameterIdentificationRunModeView";
         this.Size = new System.Drawing.Size(514, 216);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkAllTheSame.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPivotGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupCalculationMethodCategories)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCheckAllTheSame)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemHintPanel)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private UxPivotGrid pivotGridControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemPivotGrid;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupCalculationMethodCategories;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemCheckAllTheSame;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem;
      private UxHintPanel uxHintPanel;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemHintPanel;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private UxLayoutControl layoutControl;
      private UxCheckEdit chkAllTheSame;
   }
}
