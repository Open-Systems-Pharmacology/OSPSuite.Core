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
         this.btnImport = new DevExpress.XtraEditors.SimpleButton();
         this.btnImportAll = new DevExpress.XtraEditors.SimpleButton();
         this.importerTabControl = new DevExpress.XtraTab.XtraTabControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.importerLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemImportAll = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemImportCurrent = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.dataViewingGridControl = new DevExpress.XtraGrid.GridControl();
         this.dataViewingGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.importerTabControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportAll)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportCurrent)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataViewingGridView)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this.btnImport);
         this.rootLayoutControl.Controls.Add(this.btnImportAll);
         this.rootLayoutControl.Controls.Add(this.importerTabControl);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(1249, 853);
         this.rootLayoutControl.TabIndex = 0;
         this.rootLayoutControl.Text = "rootLayoutControl";
         // 
         // btnImport
         // 
         this.btnImport.Location = new System.Drawing.Point(1037, 829);
         this.btnImport.Margin = new System.Windows.Forms.Padding(1);
         this.btnImport.Name = "btnImport";
         this.btnImport.Size = new System.Drawing.Size(120, 22);
         this.btnImport.StyleController = this.rootLayoutControl;
         this.btnImport.TabIndex = 10;
         this.btnImport.Text = "btnImport";
         // 
         // btnImportAll
         // 
         this.btnImportAll.Location = new System.Drawing.Point(1161, 829);
         this.btnImportAll.Margin = new System.Windows.Forms.Padding(1);
         this.btnImportAll.Name = "btnImportAll";
         this.btnImportAll.Size = new System.Drawing.Size(86, 22);
         this.btnImportAll.StyleController = this.rootLayoutControl;
         this.btnImportAll.TabIndex = 9;
         this.btnImportAll.Text = "btnImportAll";
         // 
         // importerTabControl
         // 
         this.importerTabControl.Location = new System.Drawing.Point(2, 2);
         this.importerTabControl.Name = "importerTabControl";
         this.importerTabControl.Size = new System.Drawing.Size(1245, 823);
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
            this.emptySpaceItem2});
         this.Root.Name = "Root";
         this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.Root.Size = new System.Drawing.Size(1249, 853);
         this.Root.TextVisible = false;
         // 
         // importerLayoutControlItem
         // 
         this.importerLayoutControlItem.Control = this.importerTabControl;
         this.importerLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.importerLayoutControlItem.Name = "importerLayoutControlItem";
         this.importerLayoutControlItem.Size = new System.Drawing.Size(1249, 827);
         this.importerLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.importerLayoutControlItem.TextVisible = false;
         // 
         // layoutItemImportAll
         // 
         this.layoutItemImportAll.Control = this.btnImportAll;
         this.layoutItemImportAll.Location = new System.Drawing.Point(1159, 827);
         this.layoutItemImportAll.Name = "layoutItemImportAll";
         this.layoutItemImportAll.Size = new System.Drawing.Size(90, 26);
         this.layoutItemImportAll.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemImportAll.TextVisible = false;
         // 
         // layoutItemImportCurrent
         // 
         this.layoutItemImportCurrent.Control = this.btnImport;
         this.layoutItemImportCurrent.Location = new System.Drawing.Point(1035, 827);
         this.layoutItemImportCurrent.Name = "layoutItemImportCurrent";
         this.layoutItemImportCurrent.Size = new System.Drawing.Size(124, 26);
         this.layoutItemImportCurrent.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemImportCurrent.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 827);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(1035, 26);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
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
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.rootLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(8);
         this.Name = "ImporterDataView";
         this.Size = new System.Drawing.Size(1249, 853);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.importerTabControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportAll)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemImportCurrent)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
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
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraGrid.GridControl dataViewingGridControl;
      private DevExpress.XtraGrid.Views.Grid.GridView dataViewingGridView;
      private DevExpress.XtraTab.XtraTabControl importerTabControl;
      private DevExpress.XtraLayout.LayoutControlItem importerLayoutControlItem;
   }
}
