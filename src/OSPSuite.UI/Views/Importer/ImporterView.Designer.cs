using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Importer
{
   partial class ImporterView : BaseUserControl, IImporterView
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
         this._labelExtraErrors = new DevExpress.XtraEditors.LabelControl();
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
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.applyMappingLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItemExtraError = new DevExpress.XtraLayout.LayoutControlItem();
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
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.applyMappingLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExtraError)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this._labelExtraErrors);
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
         this.rootLayoutControl.Size = new System.Drawing.Size(1309, 780);
         this.rootLayoutControl.TabIndex = 0;
         // 
         // _labelExtraErrors
         // 
         this._labelExtraErrors.Appearance.ForeColor = System.Drawing.Color.Red;
         this._labelExtraErrors.Appearance.Options.UseForeColor = true;
         this._labelExtraErrors.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
         this._labelExtraErrors.Location = new System.Drawing.Point(12, 482);
         this._labelExtraErrors.Name = "_labelExtraErrors";
         this._labelExtraErrors.Size = new System.Drawing.Size(464, 1);
         this._labelExtraErrors.StyleController = this.rootLayoutControl;
         this._labelExtraErrors.TabIndex = 11;
         this._labelExtraErrors.Visible = false;
         // 
         // applyMappingBtn
         // 
         this.applyMappingBtn.Location = new System.Drawing.Point(187, 741);
         this.applyMappingBtn.Manager = null;
         this.applyMappingBtn.Margin = new System.Windows.Forms.Padding(1);
         this.applyMappingBtn.Name = "applyMappingBtn";
         this.applyMappingBtn.Shortcut = System.Windows.Forms.Keys.None;
         this.applyMappingBtn.Size = new System.Drawing.Size(157, 27);
         this.applyMappingBtn.StyleController = this.rootLayoutControl;
         this.applyMappingBtn.TabIndex = 10;
         this.applyMappingBtn.Text = "applyMappingBtn";
         // 
         // saveMappingBtn
         // 
         this.saveMappingBtn.Location = new System.Drawing.Point(12, 741);
         this.saveMappingBtn.Manager = null;
         this.saveMappingBtn.Name = "saveMappingBtn";
         this.saveMappingBtn.Shortcut = System.Windows.Forms.Keys.None;
         this.saveMappingBtn.Size = new System.Drawing.Size(156, 27);
         this.saveMappingBtn.StyleController = this.rootLayoutControl;
         this.saveMappingBtn.TabIndex = 9;
         this.saveMappingBtn.Text = "uxSimpleButton1";
         // 
         // sourceFilePanelControl
         // 
         this.sourceFilePanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.sourceFilePanelControl.Location = new System.Drawing.Point(492, 31);
         this.sourceFilePanelControl.Margin = new System.Windows.Forms.Padding(1);
         this.sourceFilePanelControl.Name = "sourceFilePanelControl";
         this.sourceFilePanelControl.Size = new System.Drawing.Size(805, 25);
         this.sourceFilePanelControl.TabIndex = 8;
         // 
         // nanPanelControl
         // 
         this.nanPanelControl.Location = new System.Drawing.Point(12, 487);
         this.nanPanelControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
         this.nanPanelControl.Name = "nanPanelControl";
         this.nanPanelControl.Size = new System.Drawing.Size(464, 250);
         this.nanPanelControl.TabIndex = 7;
         // 
         // previewXtraTabControl
         // 
         this.previewXtraTabControl.Location = new System.Drawing.Point(492, 79);
         this.previewXtraTabControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
         this.previewXtraTabControl.Name = "previewXtraTabControl";
         this.previewXtraTabControl.Size = new System.Drawing.Size(805, 689);
         this.previewXtraTabControl.TabIndex = 0;
         // 
         // columnMappingPanelControl
         // 
         this.columnMappingPanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.columnMappingPanelControl.Location = new System.Drawing.Point(12, 31);
         this.columnMappingPanelControl.Margin = new System.Windows.Forms.Padding(1);
         this.columnMappingPanelControl.Name = "columnMappingPanelControl";
         this.columnMappingPanelControl.Size = new System.Drawing.Size(464, 447);
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
            this.emptySpaceItem2,
            this.applyMappingLayoutControlItem,
            this.emptySpaceItem1,
            this.layoutControlItemExtraError});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1309, 780);
         this.Root.TextVisible = false;
         // 
         // columnMappingLayoutControlItem
         // 
         this.columnMappingLayoutControlItem.Control = this.columnMappingPanelControl;
         this.columnMappingLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.columnMappingLayoutControlItem.Name = "columnMappingLayoutControlItem";
         this.columnMappingLayoutControlItem.Size = new System.Drawing.Size(468, 470);
         this.columnMappingLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.columnMappingLayoutControlItem.TextSize = new System.Drawing.Size(193, 16);
         // 
         // previewLayoutControlItem
         // 
         this.previewLayoutControlItem.Control = this.previewXtraTabControl;
         this.previewLayoutControlItem.Location = new System.Drawing.Point(480, 48);
         this.previewLayoutControlItem.Name = "previewLayoutControlItem";
         this.previewLayoutControlItem.Size = new System.Drawing.Size(809, 712);
         this.previewLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.previewLayoutControlItem.TextSize = new System.Drawing.Size(193, 16);
         // 
         // nanLayoutControlItem
         // 
         this.nanLayoutControlItem.Control = this.nanPanelControl;
         this.nanLayoutControlItem.Location = new System.Drawing.Point(0, 475);
         this.nanLayoutControlItem.Name = "nanLayoutControlItem";
         this.nanLayoutControlItem.Size = new System.Drawing.Size(468, 254);
         this.nanLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.nanLayoutControlItem.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(468, 0);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.ShowSplitGlyph = DevExpress.Utils.DefaultBoolean.True;
         this.splitterItem1.Size = new System.Drawing.Size(12, 760);
         // 
         // sourceFileLayoutControlItem
         // 
         this.sourceFileLayoutControlItem.Control = this.sourceFilePanelControl;
         this.sourceFileLayoutControlItem.Location = new System.Drawing.Point(480, 0);
         this.sourceFileLayoutControlItem.Name = "sourceFileLayoutControlItem";
         this.sourceFileLayoutControlItem.Size = new System.Drawing.Size(809, 48);
         this.sourceFileLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.sourceFileLayoutControlItem.TextSize = new System.Drawing.Size(193, 16);
         // 
         // saveMappingBtnLayoutControlItem
         // 
         this.saveMappingBtnLayoutControlItem.Control = this.saveMappingBtn;
         this.saveMappingBtnLayoutControlItem.Location = new System.Drawing.Point(0, 729);
         this.saveMappingBtnLayoutControlItem.Name = "applyMappingLayoutControlItem";
         this.saveMappingBtnLayoutControlItem.Size = new System.Drawing.Size(160, 31);
         this.saveMappingBtnLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.saveMappingBtnLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(336, 729);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(132, 31);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // applyMappingLayoutControlItem
         // 
         this.applyMappingLayoutControlItem.Control = this.applyMappingBtn;
         this.applyMappingLayoutControlItem.Location = new System.Drawing.Point(175, 729);
         this.applyMappingLayoutControlItem.Name = "item0";
         this.applyMappingLayoutControlItem.Size = new System.Drawing.Size(161, 31);
         this.applyMappingLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.applyMappingLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(160, 729);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(15, 31);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItemExtraError
         // 
         this.layoutControlItemExtraError.Control = this._labelExtraErrors;
         this.layoutControlItemExtraError.Location = new System.Drawing.Point(0, 470);
         this.layoutControlItemExtraError.Name = "layoutControlItemExtraError";
         this.layoutControlItemExtraError.Size = new System.Drawing.Size(468, 5);
         this.layoutControlItemExtraError.Text = " ";
         this.layoutControlItemExtraError.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemExtraError.TextVisible = false;
         // 
         // ImporterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.rootLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(0);
         this.Name = "ImporterView";
         this.Size = new System.Drawing.Size(1309, 780);
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
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.applyMappingLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExtraError)).EndInit();
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
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private UxSimpleButton applyMappingBtn;
      private DevExpress.XtraLayout.LayoutControlItem applyMappingLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.LabelControl _labelExtraErrors;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemExtraError;
   }
}