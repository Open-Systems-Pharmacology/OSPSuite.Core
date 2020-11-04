using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.UI.Extensions;

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
         this.nanPanelControl = new DevExpress.XtraEditors.PanelControl();
         this.btnImport = new DevExpress.XtraEditors.SimpleButton();
         this.btnImportAll = new DevExpress.XtraEditors.SimpleButton();
         this.ImporterTabControl = new DevExpress.XtraTab.XtraTabControl();
         this.sourceFilePanelControl = new DevExpress.XtraEditors.PanelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.sourceFileLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.importerTabLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.importAllBtnLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.importBtnLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem6 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.nanLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).BeginInit();
         this.rootLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nanPanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ImporterTabControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerTabLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importAllBtnLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.importBtnLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanLayoutControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // rootLayoutControl
         // 
         this.rootLayoutControl.Controls.Add(this.nanPanelControl);
         this.rootLayoutControl.Controls.Add(this.btnImport);
         this.rootLayoutControl.Controls.Add(this.btnImportAll);
         this.rootLayoutControl.Controls.Add(this.ImporterTabControl);
         this.rootLayoutControl.Controls.Add(this.sourceFilePanelControl);
         this.rootLayoutControl.Dock = System.Windows.Forms.DockStyle.Right;
         this.rootLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.rootLayoutControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.rootLayoutControl.Name = "rootLayoutControl";
         this.rootLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(593, 430, 1625, 1000);
         this.rootLayoutControl.Root = this.Root;
         this.rootLayoutControl.Size = new System.Drawing.Size(1127, 1029);
         this.rootLayoutControl.TabIndex = 0;
         this.rootLayoutControl.Text = "rootLayoutControl";
         // 
         // nanPanelControl
         // 
         this.nanPanelControl.Location = new System.Drawing.Point(6, 421);
         this.nanPanelControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.nanPanelControl.Name = "nanPanelControl";
         this.nanPanelControl.Size = new System.Drawing.Size(1115, 540);
         this.nanPanelControl.TabIndex = 11;
         // 
         // btnImport
         // 
         this.btnImport.Location = new System.Drawing.Point(819, 996);
         this.btnImport.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.btnImport.Name = "btnImport";
         this.btnImport.Size = new System.Drawing.Size(138, 27);
         this.btnImport.StyleController = this.rootLayoutControl;
         this.btnImport.TabIndex = 10;
         this.btnImport.Text = "Load current sheet";
         // 
         // btnImportAll
         // 
         this.btnImportAll.Location = new System.Drawing.Point(987, 996);
         this.btnImportAll.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.btnImportAll.Name = "btnImportAll";
         this.btnImportAll.Size = new System.Drawing.Size(134, 27);
         this.btnImportAll.StyleController = this.rootLayoutControl;
         this.btnImportAll.TabIndex = 9;
         this.btnImportAll.Text = "Load all sheets";
         // 
         // ImporterTabControl
         // 
         this.ImporterTabControl.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPageHeaders;
         this.ImporterTabControl.Location = new System.Drawing.Point(6, 50);
         this.ImporterTabControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.ImporterTabControl.Name = "ImporterTabControl";
         this.ImporterTabControl.Size = new System.Drawing.Size(1115, 369);
         this.ImporterTabControl.TabIndex = 6;
         // 
         // sourceFilePanelControl
         // 
         this.sourceFilePanelControl.Location = new System.Drawing.Point(6, 6);
         this.sourceFilePanelControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.sourceFilePanelControl.Name = "sourceFilePanelControl";
         this.sourceFilePanelControl.Size = new System.Drawing.Size(1115, 29);
         this.sourceFilePanelControl.TabIndex = 5;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.sourceFileLayoutControlItem,
            this.importerTabLayoutControlItem,
            this.importAllBtnLayoutControlItem,
            this.importBtnLayoutControlItem,
            this.emptySpaceItem4,
            this.emptySpaceItem5,
            this.emptySpaceItem6,
            this.nanLayoutControlItem});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1127, 1029);
         this.Root.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 990);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(813, 29);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // sourceFileLayoutControlItem
         // 
         this.sourceFileLayoutControlItem.Control = this.sourceFilePanelControl;
         this.sourceFileLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.sourceFileLayoutControlItem.Name = "sourceFileLayoutControlItem";
         this.sourceFileLayoutControlItem.Size = new System.Drawing.Size(1117, 31);
         this.sourceFileLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.sourceFileLayoutControlItem.TextVisible = false;
         // 
         // importerTabLayoutControlItem
         // 
         this.importerTabLayoutControlItem.Control = this.ImporterTabControl;
         this.importerTabLayoutControlItem.Location = new System.Drawing.Point(0, 44);
         this.importerTabLayoutControlItem.Name = "importerTabLayoutControlItem";
         this.importerTabLayoutControlItem.Size = new System.Drawing.Size(1117, 371);
         this.importerTabLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.importerTabLayoutControlItem.TextVisible = false;
         // 
         // importAllBtnLayoutControlItem
         // 
         this.importAllBtnLayoutControlItem.Control = this.btnImportAll;
         this.importAllBtnLayoutControlItem.Location = new System.Drawing.Point(981, 990);
         this.importAllBtnLayoutControlItem.Name = "importAllBtnLayoutControlItem";
         this.importAllBtnLayoutControlItem.Size = new System.Drawing.Size(136, 29);
         this.importAllBtnLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.importAllBtnLayoutControlItem.TextVisible = false;
         // 
         // importBtnLayoutControlItem
         // 
         this.importBtnLayoutControlItem.Control = this.btnImport;
         this.importBtnLayoutControlItem.Location = new System.Drawing.Point(813, 990);
         this.importBtnLayoutControlItem.Name = "importBtnLayoutControlItem";
         this.importBtnLayoutControlItem.Size = new System.Drawing.Size(140, 29);
         this.importBtnLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.importBtnLayoutControlItem.TextVisible = false;
         // 
         // emptySpaceItem4
         // 
         this.emptySpaceItem4.AllowHotTrack = false;
         this.emptySpaceItem4.Location = new System.Drawing.Point(953, 990);
         this.emptySpaceItem4.Name = "emptySpaceItem4";
         this.emptySpaceItem4.Size = new System.Drawing.Size(28, 29);
         this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem5
         // 
         this.emptySpaceItem5.AllowHotTrack = false;
         this.emptySpaceItem5.Location = new System.Drawing.Point(0, 31);
         this.emptySpaceItem5.Name = "emptySpaceItem5";
         this.emptySpaceItem5.Size = new System.Drawing.Size(1117, 13);
         this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem6
         // 
         this.emptySpaceItem6.AllowHotTrack = false;
         this.emptySpaceItem6.Location = new System.Drawing.Point(0, 957);
         this.emptySpaceItem6.Name = "emptySpaceItem6";
         this.emptySpaceItem6.Size = new System.Drawing.Size(1117, 33);
         this.emptySpaceItem6.TextSize = new System.Drawing.Size(0, 0);
         // 
         // nanLayoutControlItem
         // 
         this.nanLayoutControlItem.Control = this.nanPanelControl;
         this.nanLayoutControlItem.Location = new System.Drawing.Point(0, 415);
         this.nanLayoutControlItem.Name = "nanLayoutControlItem";
         this.nanLayoutControlItem.Size = new System.Drawing.Size(1117, 542);
         this.nanLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.nanLayoutControlItem.TextVisible = false;
         // 
         // ImporterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ImporterView";
         this.Controls.Add(this.rootLayoutControl);
         this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
         this.Name = "ImporterView";
         this.Size = new System.Drawing.Size(1127, 1029);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rootLayoutControl)).EndInit();
         this.rootLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.nanPanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ImporterTabControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFilePanelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sourceFileLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importerTabLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importAllBtnLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.importBtnLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nanLayoutControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl sourceFilePanelControl;
      private DevExpress.XtraLayout.LayoutControl rootLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem sourceFileLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraTab.XtraTabControl ImporterTabControl;
      private DevExpress.XtraLayout.LayoutControlItem importerTabLayoutControlItem;
      private DevExpress.XtraEditors.SimpleButton btnImport;
      private DevExpress.XtraEditors.SimpleButton btnImportAll;
      private DevExpress.XtraLayout.LayoutControlItem importAllBtnLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem importBtnLayoutControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;
      private DevExpress.XtraEditors.PanelControl nanPanelControl;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem6;
      private DevExpress.XtraLayout.LayoutControlItem nanLayoutControlItem;
   }
}