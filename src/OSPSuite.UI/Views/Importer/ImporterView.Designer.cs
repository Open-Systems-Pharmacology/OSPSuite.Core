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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.rootLayoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.sourceFilePanelControl = new DevExpress.XtraEditors.PanelControl();
         this.nanPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.previewXtraTabControl = new DevExpress.XtraTab.XtraTabControl();
         this.sourceTabPage = new DevExpress.XtraTab.XtraTabPage();
         this.confirmationTabPage = new DevExpress.XtraTab.XtraTabPage();
         this.columnMappingPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.columnMappingLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.previewLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.nanLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.sourceFileLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewXtraTabControl)).BeginInit();
         this.previewXtraTabControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this.sourceFilePanelControl);
         this.rootLayoutControl.Controls.Add(this.nanPanelControl);
         this.rootLayoutControl.Controls.Add(this.previewXtraTabControl);
         this.rootLayoutControl.Controls.Add(this.columnMappingPanelControl);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Margin = new System.Windows.Forms.Padding(1);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(795, 540);
         this.rootLayoutControl.TabIndex = 0;
         // 
         // sourceFilePanelControl
         // 
         this.sourceFilePanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.sourceFilePanelControl.Location = new System.Drawing.Point(372, 28);
         this.sourceFilePanelControl.Margin = new System.Windows.Forms.Padding(1);
         this.sourceFilePanelControl.Name = "sourceFilePanelControl";
         this.sourceFilePanelControl.Size = new System.Drawing.Size(411, 14);
         this.sourceFilePanelControl.TabIndex = 8;
         // 
         // nanPanelControl
         // 
         this.nanPanelControl.Location = new System.Drawing.Point(12, 432);
         this.nanPanelControl.Name = "nanPanelControl";
         this.nanPanelControl.Size = new System.Drawing.Size(346, 96);
         this.nanPanelControl.TabIndex = 7;
         // 
         // previewXtraTabControl
         // 
         this.previewXtraTabControl.Location = new System.Drawing.Point(372, 72);
         this.previewXtraTabControl.Name = "previewXtraTabControl";
         this.previewXtraTabControl.SelectedTabPage = this.sourceTabPage;
         this.previewXtraTabControl.Size = new System.Drawing.Size(411, 456);
         this.previewXtraTabControl.TabIndex = 0;
         this.previewXtraTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.sourceTabPage,
            this.confirmationTabPage});
         // 
         // sourceTabPage
         // 
         this.sourceTabPage.Name = "sourceTabPage";
         this.sourceTabPage.Size = new System.Drawing.Size(409, 431);
         this.sourceTabPage.Text = "Source";
         // 
         // confirmationTabPage
         // 
         this.confirmationTabPage.Enabled = false;
         this.confirmationTabPage.Name = "confirmationTabPage";
         this.confirmationTabPage.Size = new System.Drawing.Size(409, 431);
         this.confirmationTabPage.Text = "Confirmation";
         // 
         // columnMappingPanelControl
         // 
         this.columnMappingPanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.columnMappingPanelControl.Location = new System.Drawing.Point(12, 28);
         this.columnMappingPanelControl.Margin = new System.Windows.Forms.Padding(1);
         this.columnMappingPanelControl.Name = "columnMappingPanelControl";
         this.columnMappingPanelControl.Size = new System.Drawing.Size(346, 400);
         this.columnMappingPanelControl.TabIndex = 6;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.columnMappingLayoutControlItem,
            this.previewLayoutControlItem,
            this.nanLayoutControlItem,
            this.emptySpaceItem2,
            this.splitterItem1,
            this.sourceFileLayoutControlItem});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(795, 540);
         this.Root.TextVisible = false;
         // 
         // columnMappingLayoutControlItem
         // 
         this.columnMappingLayoutControlItem.Control = this.columnMappingPanelControl;
         this.columnMappingLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.columnMappingLayoutControlItem.Name = "columnMappingLayoutControlItem";
         this.columnMappingLayoutControlItem.Size = new System.Drawing.Size(350, 420);
         this.columnMappingLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.columnMappingLayoutControlItem.TextSize = new System.Drawing.Size(163, 13);
         // 
         // previewLayoutControlItem
         // 
         this.previewLayoutControlItem.Control = this.previewXtraTabControl;
         this.previewLayoutControlItem.Location = new System.Drawing.Point(360, 44);
         this.previewLayoutControlItem.Name = "previewLayoutControlItem";
         this.previewLayoutControlItem.Size = new System.Drawing.Size(415, 476);
         this.previewLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.previewLayoutControlItem.TextSize = new System.Drawing.Size(163, 13);
         // 
         // nanLayoutControlItem
         // 
         this.nanLayoutControlItem.Control = this.nanPanelControl;
         this.nanLayoutControlItem.Location = new System.Drawing.Point(0, 420);
         this.nanLayoutControlItem.Name = "nanLayoutControlItem";
         this.nanLayoutControlItem.Size = new System.Drawing.Size(350, 100);
         this.nanLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.nanLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(360, 34);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(415, 10);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(350, 0);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.ShowSplitGlyph = DevExpress.Utils.DefaultBoolean.True;
         this.splitterItem1.Size = new System.Drawing.Size(10, 520);
         // 
         // sourceFileLayoutControlItem
         // 
         this.sourceFileLayoutControlItem.Control = this.sourceFilePanelControl;
         this.sourceFileLayoutControlItem.Location = new System.Drawing.Point(360, 0);
         this.sourceFileLayoutControlItem.Name = "sourceFileLayoutControlItem";
         this.sourceFileLayoutControlItem.Size = new System.Drawing.Size(415, 34);
         this.sourceFileLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.sourceFileLayoutControlItem.TextSize = new System.Drawing.Size(163, 13);
         // 
         // ImporterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.rootLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(1);
         this.Name = "ImporterView";
         this.Size = new System.Drawing.Size(795, 540);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewXtraTabControl)).EndInit();
         this.previewXtraTabControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayoutControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.PanelControl columnMappingPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem columnMappingLayoutControlItem;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraTab.XtraTabControl previewXtraTabControl;
      private DevExpress.XtraTab.XtraTabPage sourceTabPage;
      private DevExpress.XtraTab.XtraTabPage confirmationTabPage;
      private DevExpress.XtraLayout.LayoutControlItem previewLayoutControlItem;
      private DevExpress.XtraEditors.PanelControl nanPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem nanLayoutControlItem;
      private DevExpress.XtraEditors.PanelControl sourceFilePanelControl;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraLayout.LayoutControlItem sourceFileLayoutControlItem;
   }
}