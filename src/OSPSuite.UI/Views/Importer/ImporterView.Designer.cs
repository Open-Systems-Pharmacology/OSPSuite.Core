using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars.Navigation;
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
         this.nanPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.previewXtraTabControl = new DevExpress.XtraTab.XtraTabControl();
         this.sourceTabPage = new DevExpress.XtraTab.XtraTabPage();
         this.confirmationTabPage = new DevExpress.XtraTab.XtraTabPage();
         this.columnMappingPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.columnMappingLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.previewLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.nanLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nanPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewXtraTabControl)).BeginInit();
         this.previewXtraTabControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
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
         // nanPanelControl
         // 
         this.nanPanelControl.Location = new System.Drawing.Point(29, 734);
         this.nanPanelControl.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.nanPanelControl.Name = "nanPanelControl";
         this.nanPanelControl.Size = new System.Drawing.Size(869, 607);
         this.nanPanelControl.TabIndex = 7;
         // 
         // previewXtraTabControl
         // 
         this.previewXtraTabControl.Location = new System.Drawing.Point(933, 71);
         this.previewXtraTabControl.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.previewXtraTabControl.Name = "previewXtraTabControl";
         this.previewXtraTabControl.SelectedTabPage = this.sourceTabPage;
         this.previewXtraTabControl.Size = new System.Drawing.Size(1026, 1270);
         this.previewXtraTabControl.TabIndex = 0;
         this.previewXtraTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.sourceTabPage,
            this.confirmationTabPage});
         // 
         // sourceTabPage
         // 
         this.sourceTabPage.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.sourceTabPage.Name = "sourceTabPage";
         this.sourceTabPage.Size = new System.Drawing.Size(1022, 1208);
         this.sourceTabPage.Text = "Source";
         // 
         // confirmationTabPage
         // 
         this.confirmationTabPage.Enabled = false;
         this.confirmationTabPage.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
         this.confirmationTabPage.Name = "confirmationTabPage";
         this.confirmationTabPage.Size = new System.Drawing.Size(1022, 1208);
         this.confirmationTabPage.Text = "Confirmation";
         // 
         // columnMappingPanelControl
         // 
         this.columnMappingPanelControl.Location = new System.Drawing.Point(29, 71);
         this.columnMappingPanelControl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
         this.columnMappingPanelControl.Name = "columnMappingPanelControl";
         this.columnMappingPanelControl.Size = new System.Drawing.Size(869, 609);
         this.columnMappingPanelControl.TabIndex = 6;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.columnMappingLayoutControlItem,
            this.previewLayoutControlItem,
            this.splitterItem1,
            this.emptySpaceItem1,
            this.nanLayoutControlItem});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1988, 1371);
         this.Root.TextVisible = false;
         // 
         // columnMappingLayoutControlItem
         // 
         this.columnMappingLayoutControlItem.Control = this.columnMappingPanelControl;
         this.columnMappingLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.columnMappingLayoutControlItem.Name = "Mapping";
         this.columnMappingLayoutControlItem.Size = new System.Drawing.Size(879, 660);
         this.columnMappingLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.columnMappingLayoutControlItem.TextSize = new System.Drawing.Size(101, 33);
         // 
         // previewLayoutControlItem
         // 
         this.previewLayoutControlItem.Control = this.previewXtraTabControl;
         this.previewLayoutControlItem.Location = new System.Drawing.Point(904, 0);
         this.previewLayoutControlItem.Name = "Preview";
         this.previewLayoutControlItem.Size = new System.Drawing.Size(1036, 1321);
         this.previewLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.previewLayoutControlItem.TextSize = new System.Drawing.Size(101, 33);
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(879, 0);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(25, 1321);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 660);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(879, 44);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // nanLayoutControlItem
         // 
         this.nanLayoutControlItem.Control = this.nanPanelControl;
         this.nanLayoutControlItem.Location = new System.Drawing.Point(0, 704);
         this.nanLayoutControlItem.Name = "nanLayoutControlItem";
         this.nanLayoutControlItem.Size = new System.Drawing.Size(879, 617);
         this.nanLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.nanLayoutControlItem.TextVisible = false;
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
         ((System.ComponentModel.ISupportInitialize)(this.nanPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewXtraTabControl)).EndInit();
         this.previewXtraTabControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.columnMappingLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.previewLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanLayoutControlItem)).EndInit();
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
   }
}