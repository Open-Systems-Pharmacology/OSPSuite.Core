using System.Windows.Forms;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Importer
{
   partial class ImporterView 
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
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.rootLayoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.clearMappingBtn = new OSPSuite.UI.Controls.UxSimpleButton();
         this.resetMappingBasedOnCurrentSheetBtn = new OSPSuite.UI.Controls.UxSimpleButton();
         this.applyMappingBtn = new OSPSuite.UI.Controls.UxSimpleButton();
         this.saveMappingBtn = new OSPSuite.UI.Controls.UxSimpleButton();
         this.sourceFilePanelControl = new DevExpress.XtraEditors.PanelControl();
         this.nanPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.previewXtraTabControl = new DevExpress.XtraTab.XtraTabControl();
         this.columnMappingPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.nanLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.saveMappingBtnLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.applyMappingLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.mappingLayoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.columnMappingLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.resetMappingBasedOnCurrentSheetLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.clearMappingLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.previewLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.sourceFileLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewXtraTabControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.saveMappingBtnLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.applyMappingLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.mappingLayoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.resetMappingBasedOnCurrentSheetLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.clearMappingLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this.clearMappingBtn);
         this.rootLayoutControl.Controls.Add(this.resetMappingBasedOnCurrentSheetBtn);
         this.rootLayoutControl.Controls.Add(this.applyMappingBtn);
         this.rootLayoutControl.Controls.Add(this.saveMappingBtn);
         this.rootLayoutControl.Controls.Add(this.sourceFilePanelControl);
         this.rootLayoutControl.Controls.Add(this.nanPanelControl);
         this.rootLayoutControl.Controls.Add(this.previewXtraTabControl);
         this.rootLayoutControl.Controls.Add(this.columnMappingPanelControl);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Margin = new System.Windows.Forms.Padding(0);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(3685, 241, 812, 500);
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(2805, 1609);
         this.rootLayoutControl.TabIndex = 0;
         // 
         // clearMappingBtn
         // 
         this.clearMappingBtn.Location = new System.Drawing.Point(562, 79);
         this.clearMappingBtn.Manager = null;
         this.clearMappingBtn.Name = "clearMappingBtn";
         this.clearMappingBtn.Shortcut = System.Windows.Forms.Keys.None;
         this.clearMappingBtn.Size = new System.Drawing.Size(525, 54);
         this.clearMappingBtn.StyleController = this.rootLayoutControl;
         this.clearMappingBtn.TabIndex = 12;
         this.clearMappingBtn.Text = "clearMappingBtn";
         // 
         // resetMappingBasedOnCurrentSheetBtn
         // 
         this.resetMappingBasedOnCurrentSheetBtn.Location = new System.Drawing.Point(36, 79);
         this.resetMappingBasedOnCurrentSheetBtn.Manager = null;
         this.resetMappingBasedOnCurrentSheetBtn.Name = "resetMappingBasedOnCurrentSheetBtn";
         this.resetMappingBasedOnCurrentSheetBtn.Shortcut = System.Windows.Forms.Keys.None;
         this.resetMappingBasedOnCurrentSheetBtn.Size = new System.Drawing.Size(522, 54);
         this.resetMappingBasedOnCurrentSheetBtn.StyleController = this.rootLayoutControl;
         this.resetMappingBasedOnCurrentSheetBtn.TabIndex = 11;
         this.resetMappingBasedOnCurrentSheetBtn.Text = "resetMappingBasedOnCurrentSheetBtn";
         // 
         // applyMappingBtn
         // 
         this.applyMappingBtn.Location = new System.Drawing.Point(473, 1542);
         this.applyMappingBtn.Manager = null;
         this.applyMappingBtn.Margin = new System.Windows.Forms.Padding(2);
         this.applyMappingBtn.Name = "applyMappingBtn";
         this.applyMappingBtn.Shortcut = System.Windows.Forms.Keys.None;
         this.applyMappingBtn.Size = new System.Drawing.Size(520, 54);
         this.applyMappingBtn.StyleController = this.rootLayoutControl;
         this.applyMappingBtn.TabIndex = 10;
         this.applyMappingBtn.Text = "applyMappingBtn";
         // 
         // saveMappingBtn
         // 
         this.saveMappingBtn.Location = new System.Drawing.Point(13, 1542);
         this.saveMappingBtn.Manager = null;
         this.saveMappingBtn.Margin = new System.Windows.Forms.Padding(6);
         this.saveMappingBtn.Name = "saveMappingBtn";
         this.saveMappingBtn.Shortcut = System.Windows.Forms.Keys.None;
         this.saveMappingBtn.Size = new System.Drawing.Size(456, 54);
         this.saveMappingBtn.StyleController = this.rootLayoutControl;
         this.saveMappingBtn.TabIndex = 9;
         this.saveMappingBtn.Text = "saveMappingBtn";
         // 
         // sourceFilePanelControl
         // 
         this.sourceFilePanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.sourceFilePanelControl.Location = new System.Drawing.Point(1146, 62);
         this.sourceFilePanelControl.Margin = new System.Windows.Forms.Padding(2);
         this.sourceFilePanelControl.Name = "sourceFilePanelControl";
         this.sourceFilePanelControl.Padding = new System.Windows.Forms.Padding(9, 0, 0, 3);
         this.sourceFilePanelControl.Size = new System.Drawing.Size(1633, 63);
         this.sourceFilePanelControl.TabIndex = 8;
         // 
         // nanPanelControl
         // 
         this.nanPanelControl.Location = new System.Drawing.Point(13, 1371);
         this.nanPanelControl.Margin = new System.Windows.Forms.Padding(2);
         this.nanPanelControl.Name = "nanPanelControl";
         this.nanPanelControl.Padding = new System.Windows.Forms.Padding(0, 0, 13, 0);
         this.nanPanelControl.Size = new System.Drawing.Size(1091, 138);
         this.nanPanelControl.TabIndex = 7;
         // 
         // previewXtraTabControl
         // 
         this.previewXtraTabControl.Location = new System.Drawing.Point(1133, 178);
         this.previewXtraTabControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
         this.previewXtraTabControl.Name = "previewXtraTabControl";
         this.previewXtraTabControl.Padding = new System.Windows.Forms.Padding(9, 9, 0, 0);
         this.previewXtraTabControl.Size = new System.Drawing.Size(1659, 1408);
         this.previewXtraTabControl.TabIndex = 0;
         // 
         // columnMappingPanelControl
         // 
         this.columnMappingPanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.columnMappingPanelControl.Location = new System.Drawing.Point(26, 137);
         this.columnMappingPanelControl.Margin = new System.Windows.Forms.Padding(2);
         this.columnMappingPanelControl.Name = "columnMappingPanelControl";
         this.columnMappingPanelControl.Size = new System.Drawing.Size(1061, 1207);
         this.columnMappingPanelControl.TabIndex = 6;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.nanLayoutControlItem,
            this.splitterItem1,
            this.saveMappingBtnLayoutControlItem,
            this.applyMappingLayoutControlItem,
            this.emptySpaceItem4,
            this.emptySpaceItem5,
            this.emptySpaceItem2,
            this.mappingLayoutControlGroup,
            this.emptySpaceItem1,
            this.previewLayoutControlItem,
            this.layoutControlGroup1});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(2805, 1609);
         this.Root.TextVisible = false;
         // 
         // nanLayoutControlItem
         // 
         this.nanLayoutControlItem.Control = this.nanPanelControl;
         this.nanLayoutControlItem.Location = new System.Drawing.Point(0, 1358);
         this.nanLayoutControlItem.Name = "nanLayoutControlItem";
         this.nanLayoutControlItem.Size = new System.Drawing.Size(1095, 142);
         this.nanLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.nanLayoutControlItem.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(1095, 0);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.ShowSplitGlyph = DevExpress.Utils.DefaultBoolean.True;
         this.splitterItem1.Size = new System.Drawing.Size(25, 1587);
         // 
         // saveMappingBtnLayoutControlItem
         // 
         this.saveMappingBtnLayoutControlItem.Control = this.saveMappingBtn;
         this.saveMappingBtnLayoutControlItem.Location = new System.Drawing.Point(0, 1529);
         this.saveMappingBtnLayoutControlItem.Name = "applyMappingLayoutControlItem";
         this.saveMappingBtnLayoutControlItem.Size = new System.Drawing.Size(460, 58);
         this.saveMappingBtnLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.saveMappingBtnLayoutControlItem.TextVisible = false;
         // 
         // applyMappingLayoutControlItem
         // 
         this.applyMappingLayoutControlItem.Control = this.applyMappingBtn;
         this.applyMappingLayoutControlItem.Location = new System.Drawing.Point(460, 1529);
         this.applyMappingLayoutControlItem.Name = "item0";
         this.applyMappingLayoutControlItem.Size = new System.Drawing.Size(524, 58);
         this.applyMappingLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.applyMappingLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem4
         // 
         this.emptySpaceItem4.AllowHotTrack = false;
         this.emptySpaceItem4.Location = new System.Drawing.Point(0, 1500);
         this.emptySpaceItem4.Name = "emptySpaceItem4";
         this.emptySpaceItem4.Size = new System.Drawing.Size(1095, 29);
         this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem5
         // 
         this.emptySpaceItem5.AllowHotTrack = false;
         this.emptySpaceItem5.Location = new System.Drawing.Point(0, 1348);
         this.emptySpaceItem5.Name = "emptySpaceItem5";
         this.emptySpaceItem5.Size = new System.Drawing.Size(1095, 10);
         this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(984, 1529);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(111, 58);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // mappingLayoutControlGroup
         // 
         this.mappingLayoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.columnMappingLayoutControlItem,
            this.resetMappingBasedOnCurrentSheetLayoutControlItem,
            this.clearMappingLayoutControlItem,
            this.emptySpaceItem3});
         this.mappingLayoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.mappingLayoutControlGroup.Name = "mappingLayoutControlGroup";
         this.mappingLayoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(9, 13, 9, 9);
         this.mappingLayoutControlGroup.Size = new System.Drawing.Size(1095, 1348);
         // 
         // columnMappingLayoutControlItem
         // 
         this.columnMappingLayoutControlItem.Control = this.columnMappingPanelControl;
         this.columnMappingLayoutControlItem.Location = new System.Drawing.Point(0, 58);
         this.columnMappingLayoutControlItem.Name = "columnMappingLayoutControlItem";
         this.columnMappingLayoutControlItem.Size = new System.Drawing.Size(1065, 1211);
         this.columnMappingLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.columnMappingLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.columnMappingLayoutControlItem.TextVisible = false;
         // 
         // resetMappingBasedOnCurrentSheetLayoutControlItem
         // 
         this.resetMappingBasedOnCurrentSheetLayoutControlItem.Control = this.resetMappingBasedOnCurrentSheetBtn;
         this.resetMappingBasedOnCurrentSheetLayoutControlItem.Location = new System.Drawing.Point(10, 0);
         this.resetMappingBasedOnCurrentSheetLayoutControlItem.Name = "resetMappingBasedOnCurrentSheetLayoutControlItem";
         this.resetMappingBasedOnCurrentSheetLayoutControlItem.Size = new System.Drawing.Size(526, 58);
         this.resetMappingBasedOnCurrentSheetLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.resetMappingBasedOnCurrentSheetLayoutControlItem.TextVisible = false;
         // 
         // clearMappingLayoutControlItem
         // 
         this.clearMappingLayoutControlItem.Control = this.clearMappingBtn;
         this.clearMappingLayoutControlItem.Location = new System.Drawing.Point(536, 0);
         this.clearMappingLayoutControlItem.Name = "clearMappingLayoutControlItem";
         this.clearMappingLayoutControlItem.Size = new System.Drawing.Size(529, 58);
         this.clearMappingLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.clearMappingLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(0, 0);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(10, 58);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(1120, 1577);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(1663, 10);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // previewLayoutControlItem
         // 
         this.previewLayoutControlItem.Control = this.previewXtraTabControl;
         this.previewLayoutControlItem.Location = new System.Drawing.Point(1120, 129);
         this.previewLayoutControlItem.Name = "previewLayoutControlItem";
         this.previewLayoutControlItem.Size = new System.Drawing.Size(1663, 1448);
         this.previewLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.previewLayoutControlItem.TextSize = new System.Drawing.Size(339, 33);
         // 
         // sourceFileLayoutControlItem
         // 
         this.sourceFileLayoutControlItem.Control = this.sourceFilePanelControl;
         this.sourceFileLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.sourceFileLayoutControlItem.Name = "sourceFileLayoutControlItem";
         this.sourceFileLayoutControlItem.Size = new System.Drawing.Size(1637, 103);
         this.sourceFileLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.sourceFileLayoutControlItem.TextSize = new System.Drawing.Size(339, 33);
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.sourceFileLayoutControlItem});
         this.layoutControlGroup1.Location = new System.Drawing.Point(1120, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(1663, 129);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // ImporterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.rootLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(0);
         this.Name = "ImporterView";
         this.Size = new System.Drawing.Size(2805, 1609);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewXtraTabControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.saveMappingBtnLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.applyMappingLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.mappingLayoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.resetMappingBasedOnCurrentSheetLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.clearMappingLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.PanelControl columnMappingPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem columnMappingLayoutControlItem;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraTab.XtraTabControl previewXtraTabControl;
      private DevExpress.XtraEditors.PanelControl nanPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem nanLayoutControlItem;
      private DevExpress.XtraEditors.PanelControl sourceFilePanelControl;
      private UxSimpleButton saveMappingBtn;
      private DevExpress.XtraLayout.LayoutControlItem saveMappingBtnLayoutControlItem;
      private UxSimpleButton applyMappingBtn;
      private DevExpress.XtraLayout.LayoutControlItem applyMappingLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraLayout.LayoutControlGroup mappingLayoutControlGroup;
      private UxSimpleButton clearMappingBtn;
      private UxSimpleButton resetMappingBasedOnCurrentSheetBtn;
      private DevExpress.XtraLayout.LayoutControlItem resetMappingBasedOnCurrentSheetLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem clearMappingLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlItem previewLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem sourceFileLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
   }
}