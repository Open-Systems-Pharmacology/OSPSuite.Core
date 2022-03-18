using OSPSuite.Assets;
using OSPSuite.Presentation.Views.Importer;
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
         this.resetMappingBasedOnCurrentSheetBtn = new OSPSuite.UI.Controls.UxSimpleButton();
         this.applyMappingBtn = new OSPSuite.UI.Controls.UxSimpleButton();
         this.saveMappingBtn = new OSPSuite.UI.Controls.UxSimpleButton();
         this.sourceFilePanelControl = new DevExpress.XtraEditors.PanelControl();
         this.nanPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.previewXtraTabControl = new DevExpress.XtraTab.XtraTabControl();
         this.columnMappingPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.columnMappingLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.previewLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.nanLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.sourceFileLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.saveMappingBtnLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.applyMappingLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.resetMappingBasedOnCurrentSheetLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewXtraTabControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.saveMappingBtnLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.applyMappingLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.resetMappingBasedOnCurrentSheetLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
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
         // resetMappingBasedOnCurrentSheetBtn
         // 
         this.resetMappingBasedOnCurrentSheetBtn.Location = new System.Drawing.Point(13, 1474);
         this.resetMappingBasedOnCurrentSheetBtn.Manager = null;
         this.resetMappingBasedOnCurrentSheetBtn.Name = "resetMappingBasedOnCurrentSheetBtn";
         this.resetMappingBasedOnCurrentSheetBtn.Shortcut = System.Windows.Forms.Keys.None;
         this.resetMappingBasedOnCurrentSheetBtn.Size = new System.Drawing.Size(1006, 54);
         this.resetMappingBasedOnCurrentSheetBtn.StyleController = this.rootLayoutControl;
         this.resetMappingBasedOnCurrentSheetBtn.TabIndex = 11;
         this.resetMappingBasedOnCurrentSheetBtn.Text = "Reset Mapping based on current sheet";
         // 
         // applyMappingBtn
         // 
         this.applyMappingBtn.Location = new System.Drawing.Point(499, 1542);
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
         this.sourceFilePanelControl.Location = new System.Drawing.Point(1048, 49);
         this.sourceFilePanelControl.Margin = new System.Windows.Forms.Padding(2);
         this.sourceFilePanelControl.Name = "sourceFilePanelControl";
         this.sourceFilePanelControl.Size = new System.Drawing.Size(1744, 60);
         this.sourceFilePanelControl.TabIndex = 8;
         // 
         // nanPanelControl
         // 
         this.nanPanelControl.Location = new System.Drawing.Point(13, 1281);
         this.nanPanelControl.Margin = new System.Windows.Forms.Padding(2);
         this.nanPanelControl.Name = "nanPanelControl";
         this.nanPanelControl.Size = new System.Drawing.Size(1006, 179);
         this.nanPanelControl.TabIndex = 7;
         // 
         // previewXtraTabControl
         // 
         this.previewXtraTabControl.Location = new System.Drawing.Point(1048, 149);
         this.previewXtraTabControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
         this.previewXtraTabControl.Name = "previewXtraTabControl";
         this.previewXtraTabControl.Size = new System.Drawing.Size(1744, 1447);
         this.previewXtraTabControl.TabIndex = 0;
         // 
         // columnMappingPanelControl
         // 
         this.columnMappingPanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.columnMappingPanelControl.Location = new System.Drawing.Point(13, 49);
         this.columnMappingPanelControl.Margin = new System.Windows.Forms.Padding(2);
         this.columnMappingPanelControl.Name = "columnMappingPanelControl";
         this.columnMappingPanelControl.Size = new System.Drawing.Size(1006, 1218);
         this.columnMappingPanelControl.TabIndex = 6;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.columnMappingLayoutControlItem,
            this.previewLayoutControlItem,
            this.nanLayoutControlItem,
            this.splitterItem1,
            this.sourceFileLayoutControlItem,
            this.saveMappingBtnLayoutControlItem,
            this.applyMappingLayoutControlItem,
            this.emptySpaceItem1,
            this.resetMappingBasedOnCurrentSheetLayoutControlItem,
            this.emptySpaceItem3,
            this.emptySpaceItem4,
            this.emptySpaceItem5});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(2805, 1609);
         this.Root.TextVisible = false;
         // 
         // columnMappingLayoutControlItem
         // 
         this.columnMappingLayoutControlItem.Control = this.columnMappingPanelControl;
         this.columnMappingLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.columnMappingLayoutControlItem.Name = "columnMappingLayoutControlItem";
         this.columnMappingLayoutControlItem.Size = new System.Drawing.Size(1010, 1258);
         this.columnMappingLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.columnMappingLayoutControlItem.TextSize = new System.Drawing.Size(408, 33);
         // 
         // previewLayoutControlItem
         // 
         this.previewLayoutControlItem.Control = this.previewXtraTabControl;
         this.previewLayoutControlItem.Location = new System.Drawing.Point(1035, 100);
         this.previewLayoutControlItem.Name = "previewLayoutControlItem";
         this.previewLayoutControlItem.Size = new System.Drawing.Size(1748, 1487);
         this.previewLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.previewLayoutControlItem.TextSize = new System.Drawing.Size(408, 33);
         // 
         // nanLayoutControlItem
         // 
         this.nanLayoutControlItem.Control = this.nanPanelControl;
         this.nanLayoutControlItem.Location = new System.Drawing.Point(0, 1268);
         this.nanLayoutControlItem.Name = "nanLayoutControlItem";
         this.nanLayoutControlItem.Size = new System.Drawing.Size(1010, 183);
         this.nanLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.nanLayoutControlItem.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(1010, 0);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.ShowSplitGlyph = DevExpress.Utils.DefaultBoolean.True;
         this.splitterItem1.Size = new System.Drawing.Size(25, 1587);
         // 
         // sourceFileLayoutControlItem
         // 
         this.sourceFileLayoutControlItem.Control = this.sourceFilePanelControl;
         this.sourceFileLayoutControlItem.Location = new System.Drawing.Point(1035, 0);
         this.sourceFileLayoutControlItem.Name = "sourceFileLayoutControlItem";
         this.sourceFileLayoutControlItem.Size = new System.Drawing.Size(1748, 100);
         this.sourceFileLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.sourceFileLayoutControlItem.TextSize = new System.Drawing.Size(408, 33);
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
         this.applyMappingLayoutControlItem.Location = new System.Drawing.Point(486, 1529);
         this.applyMappingLayoutControlItem.Name = "item0";
         this.applyMappingLayoutControlItem.Size = new System.Drawing.Size(524, 58);
         this.applyMappingLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.applyMappingLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(460, 1529);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(26, 58);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // resetMappingBasedOnCurrentSheetLayoutControlItem
         // 
         this.resetMappingBasedOnCurrentSheetLayoutControlItem.Control = this.resetMappingBasedOnCurrentSheetBtn;
         this.resetMappingBasedOnCurrentSheetLayoutControlItem.Location = new System.Drawing.Point(0, 1461);
         this.resetMappingBasedOnCurrentSheetLayoutControlItem.Name = "resetMappingBasedOnCurrentSheetLayoutControlItem";
         this.resetMappingBasedOnCurrentSheetLayoutControlItem.Size = new System.Drawing.Size(1010, 58);
         this.resetMappingBasedOnCurrentSheetLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.resetMappingBasedOnCurrentSheetLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(0, 1519);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(1010, 10);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem4
         // 
         this.emptySpaceItem4.AllowHotTrack = false;
         this.emptySpaceItem4.Location = new System.Drawing.Point(0, 1451);
         this.emptySpaceItem4.Name = "emptySpaceItem4";
         this.emptySpaceItem4.Size = new System.Drawing.Size(1010, 10);
         this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem5
         // 
         this.emptySpaceItem5.AllowHotTrack = false;
         this.emptySpaceItem5.Location = new System.Drawing.Point(0, 1258);
         this.emptySpaceItem5.Name = "emptySpaceItem5";
         this.emptySpaceItem5.Size = new System.Drawing.Size(1010, 10);
         this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
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
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.saveMappingBtnLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.applyMappingLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.resetMappingBasedOnCurrentSheetLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.PanelControl columnMappingPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem columnMappingLayoutControlItem;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraTab.XtraTabControl previewXtraTabControl;
      private DevExpress.XtraLayout.LayoutControlItem previewLayoutControlItem;
      private DevExpress.XtraEditors.PanelControl nanPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem nanLayoutControlItem;
      private DevExpress.XtraEditors.PanelControl sourceFilePanelControl;
      private DevExpress.XtraLayout.LayoutControlItem sourceFileLayoutControlItem;
      private UxSimpleButton saveMappingBtn;
      private DevExpress.XtraLayout.LayoutControlItem saveMappingBtnLayoutControlItem;
      private UxSimpleButton applyMappingBtn;
      private DevExpress.XtraLayout.LayoutControlItem applyMappingLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private UxSimpleButton resetMappingBasedOnCurrentSheetBtn;
      private DevExpress.XtraLayout.LayoutControlItem resetMappingBasedOnCurrentSheetLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;
   }
}