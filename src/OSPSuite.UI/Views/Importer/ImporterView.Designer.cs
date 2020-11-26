using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;
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
         this.sourceFilePanelControl = new DevExpress.XtraEditors.PanelControl();
         this.nanPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.previewXtraTabControl = new DevExpress.XtraTab.XtraTabControl();
         this.sourceTabPage = new DevExpress.XtraTab.XtraTabPage();
         this.confirmationTabPage = new DevExpress.XtraTab.XtraTabPage();
         this.columnMappingPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.columnMappingLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.previewLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.nanLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.sourceFileLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
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
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
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
         this.rootLayoutControl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(1988, 1371);
         this.rootLayoutControl.TabIndex = 0;
         // 
         // sourceFilePanelControl
         // 
         this.sourceFilePanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.sourceFilePanelControl.Location = new System.Drawing.Point(928, 49);
         this.sourceFilePanelControl.Name = "sourceFilePanelControl";
         this.sourceFilePanelControl.Size = new System.Drawing.Size(1047, 48);
         this.sourceFilePanelControl.TabIndex = 8;
         // 
         // nanPanelControl
         // 
         this.nanPanelControl.Location = new System.Drawing.Point(13, 1129);
         this.nanPanelControl.Margin = new System.Windows.Forms.Padding(8);
         this.nanPanelControl.Name = "nanPanelControl";
         this.nanPanelControl.Size = new System.Drawing.Size(886, 205);
         this.nanPanelControl.TabIndex = 7;
         // 
         // previewXtraTabControl
         // 
         this.previewXtraTabControl.Location = new System.Drawing.Point(928, 163);
         this.previewXtraTabControl.Margin = new System.Windows.Forms.Padding(8);
         this.previewXtraTabControl.Name = "previewXtraTabControl";
         this.previewXtraTabControl.SelectedTabPage = this.sourceTabPage;
         this.previewXtraTabControl.Size = new System.Drawing.Size(1047, 1195);
         this.previewXtraTabControl.TabIndex = 0;
         this.previewXtraTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.sourceTabPage,
            this.confirmationTabPage});
         // 
         // sourceTabPage
         // 
         this.sourceTabPage.Margin = new System.Windows.Forms.Padding(8);
         this.sourceTabPage.Name = "sourceTabPage";
         this.sourceTabPage.Size = new System.Drawing.Size(1043, 1133);
         this.sourceTabPage.Text = "Source";
         // 
         // confirmationTabPage
         // 
         this.confirmationTabPage.Enabled = false;
         this.confirmationTabPage.Margin = new System.Windows.Forms.Padding(8);
         this.confirmationTabPage.Name = "confirmationTabPage";
         this.confirmationTabPage.Size = new System.Drawing.Size(1043, 1133);
         this.confirmationTabPage.Text = "Confirmation";
         // 
         // columnMappingPanelControl
         // 
         this.columnMappingPanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.columnMappingPanelControl.Location = new System.Drawing.Point(13, 49);
         this.columnMappingPanelControl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
         this.columnMappingPanelControl.Name = "columnMappingPanelControl";
         this.columnMappingPanelControl.Size = new System.Drawing.Size(886, 1049);
         this.columnMappingPanelControl.TabIndex = 6;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.columnMappingLayoutControlItem,
            this.previewLayoutControlItem,
            this.emptySpaceItem1,
            this.nanLayoutControlItem,
            this.emptySpaceItem2,
            this.splitterItem1,
            this.sourceFileLayoutControlItem,
            this.emptySpaceItem3});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1988, 1371);
         this.Root.TextVisible = false;
         // 
         // columnMappingLayoutControlItem
         // 
         this.columnMappingLayoutControlItem.Control = this.columnMappingPanelControl;
         this.columnMappingLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.columnMappingLayoutControlItem.Name = "columnMappingLayoutControlItem";
         this.columnMappingLayoutControlItem.Size = new System.Drawing.Size(890, 1089);
         this.columnMappingLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.columnMappingLayoutControlItem.TextSize = new System.Drawing.Size(408, 33);
         // 
         // previewLayoutControlItem
         // 
         this.previewLayoutControlItem.Control = this.previewXtraTabControl;
         this.previewLayoutControlItem.Location = new System.Drawing.Point(915, 114);
         this.previewLayoutControlItem.Name = "previewLayoutControlItem";
         this.previewLayoutControlItem.Size = new System.Drawing.Size(1051, 1235);
         this.previewLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.previewLayoutControlItem.TextSize = new System.Drawing.Size(408, 33);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 1089);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(890, 27);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // nanLayoutControlItem
         // 
         this.nanLayoutControlItem.Control = this.nanPanelControl;
         this.nanLayoutControlItem.Location = new System.Drawing.Point(0, 1116);
         this.nanLayoutControlItem.Name = "nanLayoutControlItem";
         this.nanLayoutControlItem.Size = new System.Drawing.Size(890, 209);
         this.nanLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.nanLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(915, 88);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(1051, 26);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(890, 0);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(25, 1349);
         // 
         // sourceFileLayoutControlItem
         // 
         this.sourceFileLayoutControlItem.Control = this.sourceFilePanelControl;
         this.sourceFileLayoutControlItem.Location = new System.Drawing.Point(915, 0);
         this.sourceFileLayoutControlItem.Name = "sourceFileLayoutControlItem";
         this.sourceFileLayoutControlItem.Size = new System.Drawing.Size(1051, 88);
         this.sourceFileLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.sourceFileLayoutControlItem.TextSize = new System.Drawing.Size(408, 33);
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(0, 1325);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(890, 24);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ImporterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.rootLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
         this.Name = "ImporterView";
         this.Size = new System.Drawing.Size(1988, 1371);
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
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.PanelControl columnMappingPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem columnMappingLayoutControlItem;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraTab.XtraTabControl previewXtraTabControl;
      private DevExpress.XtraTab.XtraTabPage sourceTabPage;
      private DevExpress.XtraTab.XtraTabPage confirmationTabPage;
      private DevExpress.XtraLayout.LayoutControlItem previewLayoutControlItem;
      private DevExpress.XtraEditors.PanelControl nanPanelControl;
      private DevExpress.XtraLayout.LayoutControlItem nanLayoutControlItem;
      private DevExpress.XtraEditors.PanelControl sourceFilePanelControl;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraLayout.LayoutControlItem sourceFileLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
   }
}