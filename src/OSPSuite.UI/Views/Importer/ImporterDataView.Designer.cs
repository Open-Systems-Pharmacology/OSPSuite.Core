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
         this.importerTabControl = new DevExpress.XtraTab.XtraTabControl();
         this.sourceFilePanelControl = new DevExpress.XtraEditors.PanelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.importerLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnImportAll = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnImport = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.importerTabControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this.btnImport);
         this.rootLayoutControl.Controls.Add(this.btnImportAll);
         this.rootLayoutControl.Controls.Add(this.importerTabControl);
         this.rootLayoutControl.Controls.Add(this.sourceFilePanelControl);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(1249, 853);
         this.rootLayoutControl.TabIndex = 0;
         this.rootLayoutControl.Text = "rootLayoutControl";
         // 
         // importerTabControl
         // 
         this.importerTabControl.Location = new System.Drawing.Point(12, 27);
         this.importerTabControl.Name = "importerTabControl";
         this.importerTabControl.Size = new System.Drawing.Size(1225, 768);
         this.importerTabControl.TabIndex = 5;
         // 
         // sourceFilePanelControl
         // 
         this.sourceFilePanelControl.Location = new System.Drawing.Point(12, 12);
         this.sourceFilePanelControl.Name = "sourceFilePanelControl";
         this.sourceFilePanelControl.Size = new System.Drawing.Size(1225, 1);
         this.sourceFilePanelControl.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.importerLayoutControlItem,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.emptySpaceItem3,
            this.emptySpaceItem2,
            this.emptySpaceItem4});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1249, 853);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.sourceFilePanelControl;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(1229, 5);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 5);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(1229, 10);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // importerLayoutControlItem
         // 
         this.importerLayoutControlItem.Control = this.importerTabControl;
         this.importerLayoutControlItem.Location = new System.Drawing.Point(0, 15);
         this.importerLayoutControlItem.Name = "importerLayoutControlItem";
         this.importerLayoutControlItem.Size = new System.Drawing.Size(1229, 772);
         this.importerLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.importerLayoutControlItem.TextVisible = false;
         // 
         // btnImportAll
         // 
         this.btnImportAll.Location = new System.Drawing.Point(13254, 809);
         this.btnImportAll.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.btnImportAll.Name = "btnImportAll";
         this.btnImportAll.Size = new System.Drawing.Size(1836, 22);
         this.btnImportAll.StyleController = this.rootLayoutControl;
         this.btnImportAll.TabIndex = 9;
         this.btnImportAll.Text = "Load all sheets";
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.btnImportAll;
         this.layoutControlItem2.Location = new System.Drawing.Point(1151, 807);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(78, 26);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // btnImport
         // 
         this.btnImport.Location = new System.Drawing.Point(10985, 809);
         this.btnImport.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.btnImport.Name = "btnImport";
         this.btnImport.Size = new System.Drawing.Size(1886, 22);
         this.btnImport.StyleController = this.rootLayoutControl;
         this.btnImport.TabIndex = 10;
         this.btnImport.Text = "Load current sheet";
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.btnImport;
         this.layoutControlItem3.Location = new System.Drawing.Point(1044, 807);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(97, 26);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(0, 787);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(1229, 20);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 807);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(1044, 26);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem4
         // 
         this.emptySpaceItem4.AllowHotTrack = false;
         this.emptySpaceItem4.Location = new System.Drawing.Point(1141, 807);
         this.emptySpaceItem4.Name = "emptySpaceItem4";
         this.emptySpaceItem4.Size = new System.Drawing.Size(10, 26);
         this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ImporterDataDataView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.rootLayoutControl);
         this.Name = "ImporterDataDataView";
         this.Size = new System.Drawing.Size(1249, 853);
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.importerTabControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.PanelControl sourceFilePanelControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraTab.XtraTabControl importerTabControl;
      private DevExpress.XtraLayout.LayoutControlItem importerLayoutControlItem;
      private DevExpress.XtraEditors.SimpleButton btnImport;
      private DevExpress.XtraEditors.SimpleButton btnImportAll;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
   }
}
