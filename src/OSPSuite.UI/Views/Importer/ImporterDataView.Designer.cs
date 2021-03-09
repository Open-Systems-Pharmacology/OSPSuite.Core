using OSPSuite.Assets;
using System.Windows.Forms;

namespace OSPSuite.UI.Views.Importer
{
   partial class ImporterDataView
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
         this.useForImportCheckEdit = new DevExpress.XtraEditors.CheckEdit();
         this.btnImport = new DevExpress.XtraEditors.SimpleButton();
         this.btnImportAll = new DevExpress.XtraEditors.SimpleButton();
         this.importerTabControl = new DevExpress.XtraTab.XtraTabControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.importerLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemImportAll = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemImportCurrent = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.dataViewingGridControl = new DevExpress.XtraGrid.GridControl();
         this.dataViewingGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.useForImportCheckEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerTabControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportAll)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportCurrent)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridView)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this.useForImportCheckEdit);
         this.rootLayoutControl.Controls.Add(this.btnImport);
         this.rootLayoutControl.Controls.Add(this.btnImportAll);
         this.rootLayoutControl.Controls.Add(this.importerTabControl);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(3197, 116, 650, 400);
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(3122, 2166);
         this.rootLayoutControl.TabIndex = 0;
         this.rootLayoutControl.Text = "rootLayoutControl";
         // 
         // useForImportCheckEdit
         // 
         this.useForImportCheckEdit.Location = new System.Drawing.Point(4, 2108);
         this.useForImportCheckEdit.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
         this.useForImportCheckEdit.Name = "useForImportCheckEdit";
         this.useForImportCheckEdit.Properties.Caption = "Use the filters for importing the data";
         this.useForImportCheckEdit.Size = new System.Drawing.Size(1025, 47);
         this.useForImportCheckEdit.StyleController = this.rootLayoutControl;
         this.useForImportCheckEdit.TabIndex = 11;
         // 
         // btnImport
         // 
         this.btnImport.Location = new System.Drawing.Point(2723, 2108);
         this.btnImport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.btnImport.Name = "btnImport";
         this.btnImport.Size = new System.Drawing.Size(221, 54);
         this.btnImport.StyleController = this.rootLayoutControl;
         this.btnImport.TabIndex = 10;
         this.btnImport.Text = "btnImport";
         // 
         // btnImportAll
         // 
         this.btnImportAll.Location = new System.Drawing.Point(2952, 2108);
         this.btnImportAll.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.btnImportAll.Name = "btnImportAll";
         this.btnImportAll.Size = new System.Drawing.Size(166, 54);
         this.btnImportAll.StyleController = this.rootLayoutControl;
         this.btnImportAll.TabIndex = 9;
         this.btnImportAll.Text = "btnImportAll";
         // 
         // importerTabControl
         // 
         this.importerTabControl.Location = new System.Drawing.Point(4, 4);
         this.importerTabControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
         this.importerTabControl.Name = "importerTabControl";
         this.importerTabControl.Size = new System.Drawing.Size(3114, 2096);
         this.importerTabControl.TabIndex = 5;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.importerLayoutControlItem,
            this.layoutItemImportAll,
            this.layoutItemImportCurrent,
            this.emptySpaceItem1,
            this.layoutControlItem1});
         this.Root.Name = "Root";
         this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.Root.Size = new System.Drawing.Size(3122, 2166);
         this.Root.TextVisible = false;
         // 
         // importerLayoutControlItem
         // 
         this.importerLayoutControlItem.Control = this.importerTabControl;
         this.importerLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.importerLayoutControlItem.Name = "importerLayoutControlItem";
         this.importerLayoutControlItem.Size = new System.Drawing.Size(3122, 2104);
         this.importerLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.importerLayoutControlItem.TextVisible = false;
         // 
         // layoutItemImportAll
         // 
         this.layoutItemImportAll.Control = this.btnImportAll;
         this.layoutItemImportAll.Location = new System.Drawing.Point(2948, 2104);
         this.layoutItemImportAll.Name = "layoutItemImportAll";
         this.layoutItemImportAll.Size = new System.Drawing.Size(174, 62);
         this.layoutItemImportAll.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemImportAll.TextVisible = false;
         // 
         // layoutItemImportCurrent
         // 
         this.layoutItemImportCurrent.Control = this.btnImport;
         this.layoutItemImportCurrent.Location = new System.Drawing.Point(2719, 2104);
         this.layoutItemImportCurrent.Name = "layoutItemImportCurrent";
         this.layoutItemImportCurrent.Size = new System.Drawing.Size(229, 62);
         this.layoutItemImportCurrent.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemImportCurrent.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(1033, 2104);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(1686, 62);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.useForImportCheckEdit;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 2104);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(1033, 62);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // dataViewingGridControl
         // 
         this.dataViewingGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.dataViewingGridControl.Location = new System.Drawing.Point(222, 713);
         this.dataViewingGridControl.MainView = this.dataViewingGridView;
         this.dataViewingGridControl.Name = "dataViewingGridControl";
         this.dataViewingGridControl.Size = new System.Drawing.Size(400, 200);
         this.dataViewingGridControl.TabIndex = 0;
         this.dataViewingGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dataViewingGridView});
         // 
         // dataViewingGridView
         // 
         this.dataViewingGridView.GridControl = this.dataViewingGridControl;
         this.dataViewingGridView.Name = "dataViewingGridView";
         // 
         // ImporterDataView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.rootLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(19, 21, 19, 21);
         this.Name = "ImporterDataView";
         this.Size = new System.Drawing.Size(3122, 2166);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.useForImportCheckEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerTabControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportAll)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportCurrent)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridView)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.SimpleButton btnImport;
      private DevExpress.XtraEditors.SimpleButton btnImportAll;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemImportAll;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemImportCurrent;
      private DevExpress.XtraGrid.GridControl dataViewingGridControl;
      private DevExpress.XtraGrid.Views.Grid.GridView dataViewingGridView;
      private DevExpress.XtraTab.XtraTabControl importerTabControl;
      private DevExpress.XtraLayout.LayoutControlItem importerLayoutControlItem;
      private DevExpress.XtraEditors.CheckEdit useForImportCheckEdit;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
   }
}
